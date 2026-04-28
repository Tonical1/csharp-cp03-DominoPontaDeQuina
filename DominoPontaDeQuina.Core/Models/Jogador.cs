namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o jogador na hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam apenas a identidade e os dados persistentes do participante.
/// As pecas usadas durante a partida ficam separadas em <see cref="MaoJogador"/>.
/// </summary>
/// <param name="nome">O nome do jogador.</param>
public class Jogador(string nome)
{
    /// <summary>
    /// Obtem o identificador unico do jogador.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Obtem o nome exibido do jogador na partida.
    /// </summary>
    public string Nome { get; } = nome;
}