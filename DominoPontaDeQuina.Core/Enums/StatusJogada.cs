namespace DominoPontaDeQuina.Core.Enums;

/// <summary>
/// Representa o estado de uma jogada dentro da rodada.
/// Esse enum indica se a jogada ainda aguarda avaliacao, se foi rejeitada ou se ja produziu efeito no tabuleiro.
/// </summary>
public enum StatusJogada
{
    /// <summary>
    /// Indica que a jogada foi criada, mas ainda nao teve seu resultado consolidado.
    /// </summary>
    Pendente,

    /// <summary>
    /// Indica que a jogada nao pode ser executada de acordo com as regras da rodada.
    /// </summary>
    Invalida,

    /// <summary>
    /// Indica que a jogada foi executada e aplicada ao fluxo da rodada.
    /// </summary>
    Aplicada
}