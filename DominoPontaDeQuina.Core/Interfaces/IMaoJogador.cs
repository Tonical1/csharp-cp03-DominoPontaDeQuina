using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Interfaces;

/// <summary>
/// Define o contrato do nivel de mao do jogador dentro da hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam as pecas disponiveis para o jogador, as operacoes sobre sua mao e as decisoes
/// relacionadas a escolha ou desfazimento de jogadas durante a rodada.
/// </summary>
internal interface IMaoJogador
{
    /// <summary>
    /// Obtem o jogador dono desta mao.
    /// A mao existe no contexto da partida, mas sempre associada a um unico jogador.
    /// </summary>
    public Jogador Jogador { get; }

    /// <summary>
    /// Adiciona uma peca a mao do jogador.
    /// Essa operacao e esperada principalmente durante a distribuicao inicial ou em fluxos que devolvam pecas ao jogador.
    /// </summary>
    /// <param name="peca">A peca a ser adicionada.</param>
    public void AdicionarPeca(Peca peca);

    /// <summary>
    /// Soma os valores das pecas atualmente na mao.
    /// Essa soma e relevante para regras como desempate ou definicao do vencedor quando o tabuleiro trava.
    /// </summary>
    /// <returns>A soma dos valores das pecas na mao.</returns>
    public int SomarPecasNaMao();

    /// <summary>
    /// Determina se a mao possui a sena.
    /// Essa verificacao e importante para regras de negocio em que a primeira rodada deve iniciar
    /// com o jogador que possui a peca [6|6].
    /// </summary>
    /// <returns><see langword="true"/> quando a mao possuir a sena; caso contrario, <see langword="false"/>.</returns>
    public bool PossuiSena();

    /// <summary>
    /// Determina se a mao esta sem pecas.
    /// Uma mao vazia normalmente indica batida e pode encerrar imediatamente a rodada.
    /// </summary>
    /// <returns><see langword="true"/> quando nao houver pecas na mao; caso contrario, <see langword="false"/>.</returns>
    public bool EstaSemPecas();

    /// <summary>
    /// Obtem a jogada escolhida a partir do estado atual do tabuleiro.
    /// A regra esperada e selecionar uma peca compativel ou representar a passagem de vez quando nao houver opcao valida.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual da rodada.</param>
    /// <returns>A jogada definida para a mao do jogador.</returns>
    public Jogada GetJogada(Tabuleiro tabuleiro);

    /// <summary>
    /// Desfaz os efeitos de uma jogada sobre a mao.
    /// Essa operacao e esperada em cenarios de rollback, correcoes de fluxo ou invalidacao de jogadas ja aplicadas.
    /// </summary>
    /// <param name="jogada">A jogada que deve ser desfeita.</param>
    public void DefazerJogada(Jogada jogada);
}