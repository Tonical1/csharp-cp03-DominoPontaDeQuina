namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o time no nivel da partida dentro da hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam os jogadores agrupados e a pontuacao acumulada usada para definir o vencedor da partida.
/// </summary>
/// <param name="nome">O nome do time.</param>
public class Time(string nome)
{
    /// <summary>
    /// Obtem o identificador unico do time.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Obtem o nome exibido do time.
    /// </summary>
    public string Nome { get; } = nome;

    /// <summary>
    /// Obtem os jogadores atualmente associados ao time.
    /// </summary>
    public List<Jogador> Jogadores { get; } = [];

    /// <summary>
    /// Obtem a pontuacao acumulada do time na partida.
    /// Quando essa pontuacao atingir a pontuacao alvo da partida, o time pode se tornar vencedor.
    /// </summary>
    public int Pontuacao { get; private set; }

    /// <summary>
    /// Adiciona um jogador ao time.
    /// Essa associacao define a participacao do jogador no grupo que acumulara pontos em conjunto.
    /// </summary>
    /// <param name="jogador">O jogador a ser adicionado.</param>
    public void AdicionarJogador(Jogador jogador)
    {
        ArgumentNullException.ThrowIfNull(jogador);

        Jogadores.Add(jogador);
    }

    /// <summary>
    /// Soma pontos ao total acumulado do time.
    /// Essa operacao e esperada quando uma jogada gerar pontuacao valida para o time.
    /// </summary>
    /// <param name="pontos">Os pontos a serem somados.</param>
    public void SomarPontos(int pontos) =>
        Pontuacao += pontos;

    /// <summary>
    /// Determina se o time possui o jogador informado entre seus integrantes.
    /// </summary>
    /// <param name="jogador">O jogador a ser verificado.</param>
    /// <returns><see langword="true"/> quando o time possuir o jogador; caso contrario, <see langword="false"/>.</returns>
    public bool PossuiJogador(Jogador jogador) =>
        Jogadores.Contains(jogador);
}