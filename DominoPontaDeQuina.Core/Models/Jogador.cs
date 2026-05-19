namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o jogador na hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam apenas a identidade e os dados persistentes do participante.
/// As pecas usadas durante a partida ficam separadas em <see cref="MaoJogador"/>.
/// </summary>
/// <param name="nome">O nome do jogador.</param>
public class Jogador(string nome)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Nome { get; } = nome;

    private int _pontuacao;

    public int Pontuacao => _pontuacao;

    public void AdicionarPontos(int pontos)
    {
        if (pontos < 0)
            throw new ArgumentOutOfRangeException(nameof(pontos), "A pontuação não pode ser negativa.");

        _pontuacao += pontos;
    }
}