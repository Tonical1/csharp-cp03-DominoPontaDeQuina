namespace DominoPontaDeQuina.Core.Enums;

/// <summary>
/// Representa o motivo que levou ao encerramento da rodada.
/// Esse enum permite distinguir se a rodada terminou por batida ou por travamento do tabuleiro.
/// </summary>
public enum TipoFinalizacaoRodada
{
    /// <summary>
    /// Indica que a rodada terminou porque um jogador ficou sem pecas na mao.
    /// </summary>
    JogadorBateu,

    /// <summary>
    /// Indica que a rodada terminou porque nenhum jogador conseguiu continuar jogando.
    /// </summary>
    TabuleiroTravado
}