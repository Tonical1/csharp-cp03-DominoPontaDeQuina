using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Interfaces;

/// <summary>
/// Define o contrato do nivel de jogada na hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam a decisao do jogador, a peca escolhida, o lado do tabuleiro e o resultado
/// da tentativa de executar a jogada dentro da rodada.
/// </summary>
internal interface IJogada
{
    /// <summary>
    /// Obtem o jogador responsavel pela jogada.
    /// Cada jogada deve estar associada a um unico jogador da rodada atual.
    /// </summary>
    public Jogador Jogador { get; }

    /// <summary>
    /// Obtem a peca escolhida para a jogada.
    /// Quando a jogada representar passagem de vez, a expectativa e que essa propriedade possa estar nula.
    /// </summary>
    public Peca? Peca { get; }

    /// <summary>
    /// Obtem o lado do tabuleiro escolhido para a jogada.
    /// Quando a jogada representar passagem de vez, a expectativa e que essa propriedade possa estar nula.
    /// </summary>
    public LadoTabuleiro? Lado { get; }

    /// <summary>
    /// Obtem o status atual da jogada.
    /// O status indica se a jogada ainda esta pendente, se foi considerada valida, invalida,
    /// se ja foi aplicada ao tabuleiro ou se representou passagem de vez.
    /// </summary>
    public StatusJogada Status { get; }

    /// <summary>
    /// Determina se a jogada representa passagem de vez.
    /// Essa situacao e esperada quando o jogador nao possui peca compativel para jogar.
    /// </summary>
    /// <returns><see langword="true"/> quando a jogada representar passagem de vez; caso contrario, <see langword="false"/>.</returns>
    public bool EhPassarVez();

    /// <summary>
    /// Marca a jogada como aplicada.
    /// Isso deve ocorrer apos a jogada ser efetivamente executada no tabuleiro.
    /// </summary>
    public void MarcarComoAplicada();

    /// <summary>
    /// Marca a jogada como invalida.
    /// Isso deve ocorrer quando a jogada nao puder ser executada de acordo com as regras da rodada.
    /// </summary>
    public void MarcarComoInvalida();
}