using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Interfaces;

/// <summary>
/// Define o contrato do nivel de rodada na hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam o estado da rodada, o historico das jogadas e as regras de negocio que determinam
/// como a rodada comeca, como ela evolui e em quais condicoes ela termina.
/// </summary>
internal interface IRodada
{
    /// <summary>
    /// Mantem o tabuleiro associado a rodada atual.
    /// </summary>
    public Tabuleiro Tabuleiro { get; }

    /// <summary>
    /// Obtem o historico das jogadas registradas nesta rodada.
    /// Esse historico representa a sequencia das jogadas executadas desde o inicio da rodada,
    /// incluindo jogadas validas, invalidas e situacoes em que o jogador passa a vez.
    /// </summary>
    public ReadOnlyCollection<Jogada> HistoricoJogadas { get; }

    /// <summary>
    /// Obtem a mao do jogador atual na rodada.
    /// O jogador atual e aquele que possui a vez no momento e, portanto, pode realizar uma jogada
    /// ou passar a vez caso nao tenha peca compativel com o tabuleiro.
    /// </summary>
    public MaoJogador JogadorAtual { get; }

    /// <summary>
    /// Obtem o status atual da rodada.
    /// O status indica se a rodada ainda nao foi iniciada, se esta em andamento ou se ja foi finalizada.
    /// </summary>
    public StatusRodada Status { get; }

    /// <summary>
    /// Obtem o tipo de finalizacao da rodada.
    /// A rodada pode ser finalizada quando um jogador bate ou quando o tabuleiro trava.
    /// </summary>
    public TipoFinalizacaoRodada? TipoFinalizacao { get; }

    /// <summary>
    /// Inicia a rodada com os jogadores informados.
    /// No inicio da rodada, espera-se que as pecas sejam distribuidas e que seja definido quem comeca.
    /// Na primeira rodada, a expectativa e iniciar com quem possuir a sena; nas rodadas seguintes,
    /// a expectativa e considerar o resultado da rodada anterior para reaproveitar o fluxo do jogo.
    /// </summary>
    /// <param name="jogadores">Os jogadores participantes da rodada.</param>
    /// <param name="rodadaAnterior">A rodada anterior, quando houver, para reaproveitar informacoes de fluxo.</param>
    public void Iniciar(ReadOnlyCollection<Jogador> jogadores, Rodada rodadaAnterior = null);

    /// <summary>
    /// Registra uma jogada na rodada.
    /// Cada jogada registrada passa a compor o historico da rodada e influencia o andamento do jogo,
    /// inclusive a avaliacao de pontuacao, batida, travamento do tabuleiro e troca de vez.
    /// </summary>
    /// <param name="jogada">A jogada a ser registrada.</param>
    public void RegistrarJogada(Jogada jogada);

    /// <summary>
    /// Verifica se a rodada foi encerrada por batida.
    /// A regra esperada de batida ocorre quando um jogador consegue ficar sem pecas na mao,
    /// encerrando imediatamente a rodada.
    /// </summary>
    /// <returns><see langword="true"/> quando houver batida; caso contrario, <see langword="false"/>.</returns>
    public bool VerificarBatida();

    /// <summary>
    /// Verifica se a rodada foi encerrada por travamento do tabuleiro.
    /// O travamento e esperado quando nenhum jogador ainda na rodada possui peca compativel
    /// com as pontas externas do tabuleiro, tornando impossivel continuar jogando.
    /// </summary>
    /// <returns><see langword="true"/> quando o tabuleiro estiver travado; caso contrario, <see langword="false"/>.</returns>
    public bool VerificarTabuleiroTravado();

    /// <summary>
    /// Obtem o vencedor da rodada.
    /// O vencedor esperado depende da forma de encerramento: em caso de batida, vence quem ficou sem pecas;
    /// em caso de travamento, a expectativa e vencer quem tiver a menor soma de valores na mao.
    /// </summary>
    /// <returns>O jogador vencedor, ou <see langword="null"/> quando a rodada ainda nao tiver vencedor definido.</returns>
    public Jogador? GetVencedor();
}