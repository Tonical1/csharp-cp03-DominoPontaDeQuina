namespace DominoPontaDeQuina.Core.Enums;

/// <summary>
/// Representa o estado global da partida na hierarquia Partida -> Rodadas -> Jogadas.
/// Esse enum indica se a partida ainda nao comecou, se esta ativa ou se ja foi encerrada.
/// </summary>
public enum StatusPartida
{
    /// <summary>
    /// Indica que a partida ainda nao foi iniciada.
    /// Nesse estado ainda e esperado registrar times, jogadores e preparar a primeira rodada.
    /// </summary>
    NaoIniciada,

    /// <summary>
    /// Indica que a partida esta em andamento.
    /// Nesse estado a partida pode iniciar ou continuar rodadas, registrar jogadas e acumular pontuacao.
    /// </summary>
    EmAndamento,

    /// <summary>
    /// Indica que a partida foi finalizada.
    /// Esse estado e esperado quando algum time atingir ou ultrapassar a pontuacao alvo.
    /// </summary>
    Finalizada
}