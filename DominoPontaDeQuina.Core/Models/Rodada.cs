using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IRodada"/>
public class Rodada() : IRodada
{
    /// <summary>
    /// Armazena internamente as jogadas registradas nesta rodada.
    /// </summary>
    Stack<Jogada> Jogadas { get; } = [];

    /// <inheritdoc />
    public Tabuleiro Tabuleiro { get; } = new();

    /// <summary>
    /// Mantem a fila de maos de jogadores na ordem de execucao da rodada.
    /// </summary>
    Queue<MaoJogador> _jogadores = [];

    /// <inheritdoc />
    public ReadOnlyCollection<Jogada> HistoricoJogadas => Jogadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public MaoJogador JogadorAtual => _jogadores.Peek();

    /// <inheritdoc />
    public StatusRodada Status { get; private set; } = StatusRodada.NaoIniciada;

    /// <inheritdoc />
    public TipoFinalizacaoRodada? TipoFinalizacao { get; private set; }

    /// <inheritdoc />
    public void Iniciar(ReadOnlyCollection<Jogador> jogadores, Rodada rodadaAnterior = null)
    {
        var maosJogadores = DistribuirPecas(jogadores);
        var primeiroJogador = GetPrimeiroJogador(maosJogadores, rodadaAnterior);
        OrganizaJogadores(maosJogadores, primeiroJogador);
        Status = StatusRodada.EmAndamento;
    }

    /// <inheritdoc />
    public void RegistrarJogada(Jogada jogada)
    {
        ArgumentNullException.ThrowIfNull(jogada);
        jogada.MarcarComoAplicada();
        Jogadas.Push(jogada);
        CalcularPontuacao();
    }

    /// <inheritdoc />
    public bool VerificarBatida()
    {
        if (JogadorAtual.EstaSemPecas())
        {
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.JogadorBateu;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        if (Tabuleiro.EstaTravado(_jogadores))
        {
            Status = StatusRodada.Finalizada;
            TipoFinalizacao = TipoFinalizacaoRodada.TabuleiroTravado;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public Jogador? GetVencedor()
    {
        if (TipoFinalizacao == TipoFinalizacaoRodada.JogadorBateu)
        {
            return JogadorAtual.Jogador;
        }
        else if (TipoFinalizacao == TipoFinalizacaoRodada.TabuleiroTravado)
        {
            return _jogadores
                .OrderBy(mao => mao.SomarPecasNaMao())
                .FirstOrDefault()?.Jogador;
        }
        return null;
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        var pecasDisponiveis = GerarTodasAsPecas();
        var random = new Random();
        pecasDisponiveis = pecasDisponiveis.OrderBy(_ => random.Next()).ToList();

        var maos = jogadores.Select(jogador => new MaoJogador(jogador)).ToList();
        int pecasPorJogador = pecasDisponiveis.Count / jogadores.Count;

        for (int i = 0; i < pecasPorJogador; i++)
        {
            foreach (var mao in maos)
            {
                mao.AdicionarPeca(pecasDisponiveis.First());
                pecasDisponiveis.RemoveAt(0);
            }
        }

        return maos;
    }

    /// <summary>
    /// Determina o primeiro jogador da rodada com base nas maos distribuidas e na rodada anterior.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores desta rodada.</param>
    /// <param name="rodadaAnterior">A rodada anterior, quando houver.</param>
    /// <returns>O jogador que deve iniciar a rodada.</returns>
    private Jogador GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior = null)
    {
        if (rodadaAnterior is not null)
        {
            return rodadaAnterior.GetVencedor();
        }

        return jogadores.FirstOrDefault(mao => mao.PossuiSena())?.Jogador
            ?? jogadores.First().Jogador;
    }

    /// <summary>
    /// Organiza a fila de jogadores da rodada a partir do primeiro jogador definido.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        while (jogadores.First().Jogador != primeiroJogador)
        {
            var mao = jogadores.First();
            jogadores.RemoveAt(0);
            jogadores.Add(mao);
        }

        _jogadores = new Queue<MaoJogador>(jogadores);
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// </summary>
    private void CalcularPontuacao()
    {
        var pontos = Tabuleiro.SomarPontasExternas();
        JogadorAtual.Jogador.AdicionarPontos(pontos);
    }
    private List<Peca> GerarTodasAsPecas()
    {
        var pecas = new List<Peca>();

        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                pecas.Add(new Peca(i, j));
            }
        }

        return pecas;
    }
}