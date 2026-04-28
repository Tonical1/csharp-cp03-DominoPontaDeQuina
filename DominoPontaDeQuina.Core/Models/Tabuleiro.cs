using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o tabuleiro no nivel da rodada dentro da hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam as pecas ja coladas e as informacoes necessarias para validar jogadas,
/// calcular pontuacao pelas pontas externas e verificar situacoes de travamento.
/// </summary>
public class Tabuleiro
{
    /// <summary>
    /// Obtem as pecas posicionadas no tabuleiro na ordem em que foram coladas.
    /// </summary>
    public List<Peca> Pecas { get; } = [];

    /// <summary>
    /// Indica se o tabuleiro ainda nao possui pecas coladas.
    /// </summary>
    public bool EstaVazio => Pecas.Count == 0;

    /// <summary>
    /// Obtem a ponta esquerda atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaEsquerda => EstaVazio ? null : Pecas[0].ValorA;

    /// <summary>
    /// Obtem a ponta direita atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaDireita => EstaVazio ? null : Pecas[^1].ValorB;

    /// <summary>
    /// Determina se uma peca pode ser colada no lado informado.
    /// A regra esperada e validar se a peca possui valor compativel com a ponta externa do lado escolhido.
    /// </summary>
    /// <param name="peca">A peca a ser verificada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    /// <returns><see langword="true"/> quando a peca puder ser colada; caso contrario, <see langword="false"/>.</returns>
    public bool PodeColar(Peca peca, LadoTabuleiro lado)
    {
        // TODO ALUNO: validar se a peca pode ser colada no lado escolhido.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Cola uma peca no lado informado do tabuleiro.
    /// A regra esperada e posicionar a peca no lado correto, invertendo seus valores quando necessario.
    /// </summary>
    /// <param name="peca">A peca a ser colada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        // TODO ALUNO: posicionar a peca no lado escolhido, invertendo quando necessario.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Soma os valores das pontas externas atualmente expostas.
    /// Essa soma e a base para regras de pontuacao em que a rodada concede pontos quando o resultado for multiplo de 5.
    /// </summary>
    /// <returns>A soma das pontas externas, ou 0 quando o tabuleiro estiver vazio.</returns>
    public int SomarPontasExternas() =>
        EstaVazio ? 0 : PontaEsquerda!.Value + PontaDireita!.Value;

    /// <summary>
    /// Determina se o tabuleiro esta travado.
    /// O travamento e esperado quando nenhuma mao de jogador possuir peca compativel com as pontas externas atuais.
    /// </summary>
    /// <param name="maosJogadores">As maos dos jogadores da rodada.</param>
    /// <returns><see langword="true"/> quando o tabuleiro estiver travado; caso contrario, <see langword="false"/>.</returns>
    public bool EstaTravado(IEnumerable<MaoJogador> maosJogadores)
    {
        // TODO ALUNO: implementar a regra de travamento do tabuleiro.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Limpa o tabuleiro para preparar uma nova rodada.
    /// </summary>
    public void Limpar() =>
        Pecas.Clear();
}