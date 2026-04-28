namespace DominoPontaDeQuina.Core.Enums;

/// <summary>
/// Representa o estado da rodada dentro da hierarquia Partida -> Rodadas -> Jogadas.
/// Esse enum indica se a rodada ainda nao comecou, se esta ativa ou se ja foi encerrada.
/// </summary>
public enum StatusRodada
{
    /// <summary>
    /// Indica que a rodada ainda nao foi iniciada.
    /// Nesse estado ainda e esperado distribuir pecas e definir quem comeca.
    /// </summary>
    NaoIniciada,

    /// <summary>
    /// Indica que a rodada esta em andamento.
    /// Nesse estado os jogadores podem executar jogadas, pontuar, bater ou travar o tabuleiro.
    /// </summary>
    EmAndamento,

    /// <summary>
    /// Indica que a rodada foi finalizada.
    /// Esse estado e esperado apos batida ou travamento do tabuleiro.
    /// </summary>
    Finalizada
}