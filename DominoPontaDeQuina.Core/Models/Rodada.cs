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
        // TODO ALUNO: implementar a logica para verificar se houve batida.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool VerificarTabuleiroTravado()
    {
        // TODO ALUNO: implementar a logica para verificar se houve travamento.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Jogador? GetVencedor()
    {
        // TODO ALUNO: implementar a logica para obter o vencedor da rodada.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Distribui as pecas entre os jogadores da rodada e retorna as maos correspondentes.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <returns>A lista de maos distribuidas para os jogadores.</returns>
    private List<MaoJogador> DistribuirPecas(ReadOnlyCollection<Jogador> jogadores)
    {
        // TODO ALUNO: implementar a distribuicao das pecas entre os jogadores.
        throw new NotImplementedException();
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
        else
        {
            // TODO ALUNO: implementar a logica para obter o primeiro jogador da rodada.
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Organiza a fila de jogadores da rodada a partir do primeiro jogador definido.
    /// </summary>
    /// <param name="jogadores">As maos dos jogadores da rodada.</param>
    /// <param name="primeiroJogador">O jogador que iniciara a rodada.</param>
    private void OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador)
    {
        // TODO ALUNO: montar a fila de jogadores da rodada a partir do primeiro jogador definido.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calcula a pontuação obtida após uma jogada ser registrada, considerando o estado atual do tabuleiro e as maos dos jogadores.
    /// </summary>
    private void CalcularPontuacao()
    {
        // TODO ALUNO: implementar a logica para calcular a pontuacao dos jogadores ao final da rodada.
        throw new NotImplementedException();
    }
}