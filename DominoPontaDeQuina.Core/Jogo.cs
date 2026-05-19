using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core;

/// <summary>
/// Controla o fluxo principal no topo da hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel fica a orquestracao da partida atual, da sequencia de rodadas e da execucao das jogadas.
/// </summary>
public class Jogo()
{
    /// <summary>
    /// Mantem internamente o historico das partidas iniciadas pelo jogo.
    /// </summary>
    Stack<Partida> _partidas = [];

    /// <summary>
    /// Obtem o historico das partidas controladas por esta instancia.
    /// </summary>
    public ReadOnlyCollection<Partida> HistoricoPartidas => _partidas.ToList().AsReadOnly();

    /// <summary>
    /// Obtem a partida atual controlada pelo jogo.
    /// </summary>
    public Partida? PartidaAtual => _partidas.TryPeek(out var partidaAtual) ? partidaAtual : null;

    /// <summary>
    /// Registra os times da partida atual.
    /// </summary>
    public Task RegistrarTimesAsync()
    {
        // TODO ALUNO: registrar os times e jogadores da partida antes do inicio da primeira rodada.

        if (PartidaAtual is null)
            throw new DominioException("Nao ha partida atual.");

        Jogador jogador1 = new Jogador("Pedro Gaspar");
        Jogador jogador2 = new Jogador("Enrico Ricarte");
        Jogador jogador3 = new Jogador("Victor Freire");
        Jogador jogador4 = new Jogador("Bruno Biletsky");
        Jogador coach = new Jogador("Eduardo Vicentini");

        Time time1 = new Time("Time BitWise");
        time1.Jogadores.Add(jogador1);
        time1.Jogadores.Add(jogador3);

        Time time2 = new Time("Time EvilBit");
        time2.Jogadores.Add(jogador2);
        time2.Jogadores.Add(jogador4);

        PartidaAtual.Times.Add(time1);
        PartidaAtual.Times.Add(time2);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Inicia uma nova partida e executa suas rodadas ate a finalizacao.
    /// </summary>
    public async Task IniciarNovaPartida()
    {
        if (PartidaAtual?.Status is StatusPartida.EmAndamento)
            throw new DominioException("Nao e possivel iniciar uma nova partida enquanto a partida atual estiver em andamento.");

        _partidas.Push(new());

        await RegistrarTimesAsync();

        PartidaAtual.IniciarNovaRodada();
        PartidaAtual.RodadaAtual?.Iniciar(ObterJogadoresDaPartida(), ObterRodadaAnterior());

        while (PartidaAtual.Status is StatusPartida.EmAndamento)
        {
            await ExecutarRodadaPartidaAsync();

            if (PartidaAtual.VerificaPontuacaoAlvoAtingida())
            {
                PartidaAtual.FinalizarPartida();
            }
            else
            {
                PartidaAtual.IniciarNovaRodada();
                PartidaAtual.RodadaAtual?.Iniciar(ObterJogadoresDaPartida(), ObterRodadaAnterior());
            }
        }
    }

    /// <summary>
    /// Executa o fluxo da rodada atual enquanto ela estiver em andamento.
    /// </summary>
    public async Task ExecutarRodadaPartidaAsync()
    {
        if (PartidaAtual?.Status is not StatusPartida.EmAndamento)
            return;
        if (PartidaAtual.RodadaAtual?.Status is not StatusRodada.EmAndamento)
            return;

        var rodadaAtual = PartidaAtual.RodadaAtual;

        while (rodadaAtual.Status is StatusRodada.EmAndamento)
        {
            await ExecutarJogadaAsync();
            rodadaAtual.VerificarBatida();
            rodadaAtual.VerificarTabuleiroTravado();
        }
    }

    /// <summary>
    /// Executa a jogada do jogador atual na rodada em andamento.
    /// </summary>
    public async Task ExecutarJogadaAsync()
    {
        if (PartidaAtual?.Status is not StatusPartida.EmAndamento)
            throw new DominioException("Nao e possivel executar uma jogada em uma partida que nao esta em andamento.");
        if (PartidaAtual.RodadaAtual?.Status is not StatusRodada.EmAndamento)
            throw new DominioException("Nao e possivel executar uma jogada em uma rodada que nao esta em andamento.");

        var jogadorAtual = PartidaAtual.RodadaAtual.JogadorAtual;
        var jogada = await GetJogadaAsync();

        if (!ValidarJogada(jogada))
        {
            jogadorAtual.DefazerJogada(jogada);
            jogada.MarcarComoInvalida();
            throw new DominioException("A jogada realizada e invalida.");
        }

        PartidaAtual.RodadaAtual.RegistrarJogada(jogada);
    }

    /// <summary>
    /// Obtem a jogada definida pelo jogador atual com base no estado do tabuleiro.
    /// </summary>
    /// <returns>A jogada escolhida pelo jogador atual.</returns>
    public Task<Jogada> GetJogadaAsync()
    {
        if (PartidaAtual?.RodadaAtual is null)
            throw new DominioException("Nao ha rodada atual para obter jogada.");

        var jogadorAtual = PartidaAtual.RodadaAtual.JogadorAtual;
        return Task.FromResult(jogadorAtual.GetJogada(PartidaAtual.RodadaAtual.Tabuleiro));
    }

    /// <summary>
    /// Valida a jogada no contexto da rodada atual.
    /// </summary>
    /// <param name="jogada">A jogada a ser validada.</param>
    /// <returns><see langword="true"/> quando a jogada for valida; caso contrario, <see langword="false"/>.</returns>
    public bool ValidarJogada(Jogada jogada)
    {
        if (PartidaAtual?.RodadaAtual is null)
            return false;

        var rodada = PartidaAtual.RodadaAtual;
        var tabuleiro = rodada.Tabuleiro;

        if (jogada.EhPassarVez())
        {
            if (tabuleiro.EstaVazio)
                return false;

            var pontaEsq = tabuleiro.PontaEsquerda;
            var pontaDir = tabuleiro.PontaDireita;

            var pecas = rodada.JogadorAtual._pecas;

            bool temPecaCompativel = pecas != null &&
                pecas.Any(p =>
                    (pontaEsq.HasValue && p.PossuiValor(pontaEsq.Value)) ||
                    (pontaDir.HasValue && p.PossuiValor(pontaDir.Value)));

            return !temPecaCompativel;
        }

        if (jogada.Peca is null || jogada.Lado is null)
            return false;

        return tabuleiro.EstaVazio ||
               tabuleiro.PodeColar(jogada.Peca.Value, jogada.Lado.Value);
    }
    /// <summary>
    /// Obtem os jogadores registrados nos times da partida atual.
    /// </summary>
    /// <returns>A colecao somente leitura dos jogadores da partida.</returns>
    private ReadOnlyCollection<Jogador> ObterJogadoresDaPartida()
    {
        if (PartidaAtual is null)
            throw new DominioException("Nao ha partida atual para obter jogadores.");

        return PartidaAtual.Times
            .SelectMany(time => time.Jogadores)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Obtem a rodada anterior a rodada atual, quando houver.
    /// </summary>
    /// <returns>A rodada anterior, ou <see langword="null"/> quando a rodada atual for a primeira da partida.</returns>
    private Rodada? ObterRodadaAnterior()
    {
        if (PartidaAtual is null || PartidaAtual.HistoricoRodadas.Count < 2)
            return null;

        return PartidaAtual.HistoricoRodadas[1];
    }
}