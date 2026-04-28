namespace DominoPontaDeQuina.Core.Enums;

/// <summary>
/// Representa os lados disponiveis do tabuleiro para colar uma peca.
/// Esse enum e usado para indicar em qual ponta externa da mesa uma jogada tentara encaixar a peca.
/// </summary>
public enum LadoTabuleiro
{
    /// <summary>
    /// Indica a ponta esquerda do tabuleiro.
    /// </summary>
    Esquerda,

    /// <summary>
    /// Indica a ponta direita do tabuleiro.
    /// </summary>
    Direita
}