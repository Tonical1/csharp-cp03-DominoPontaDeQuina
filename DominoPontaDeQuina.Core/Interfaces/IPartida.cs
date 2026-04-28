using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Interfaces;

/// <summary>
/// Define o contrato interno de uma partida.
/// Uma partida agrupa varias rodadas e termina quando um dos times atinge a pontuacao alvo definida para o jogo.
/// </summary>
internal interface IPartida
{
    /// <summary>
    /// Obtem a pontuacao alvo da partida.
    /// A pontuacao alvo e a quantidade minima de pontos que um time precisa alcancar para vencer a partida.
    /// </summary>
    public int PontuacaoAlvo { get; }

    /// <summary>
    /// Obtem o status atual da partida.
    /// O status indica se a partida ainda nao foi iniciada, se esta em andamento ou se ja foi finalizada.
    /// </summary>
    public StatusPartida Status { get; }

    /// <summary>
    /// Obtem os times participantes da partida.
    /// Um dos times se torna vencedor quando sua pontuacao total atingir ou ultrapassar a pontuacao alvo.
    /// </summary>
    public List<Time> Times { get; }

    /// <summary>
    /// Obtem o historico de rodadas da partida.
    /// Cada rodada representa um set da partida e permanece registrada para consulta posterior.
    /// </summary>
    public ReadOnlyCollection<Rodada> HistoricoRodadas { get; }

    /// <summary>
    /// Obtem a rodada atual da partida.
    /// Quando a partida estiver em andamento, esta propriedade representa a rodada ativa.
    /// </summary>
    public Rodada? RodadaAtual { get; }

    /// <summary>
    /// Obtem a pontuacao atual dos times.
    /// </summary>
    /// <returns>Um dicionario com a pontuacao acumulada de cada time ao longo da partida.</returns>
    public Dictionary<Time, int> GetPontuacaoTimes();

    /// <summary>
    /// Obtem o time vencedor da partida.
    /// Um time vencedor e aquele que atingiu ou superou a pontuacao alvo antes dos demais.
    /// </summary>
    /// <returns>O time vencedor, ou <see langword="null"/> quando a partida ainda nao tiver vencedor definido.</returns>
    public Time? GetTimeVencedor();

    /// <summary>
    /// Inicia uma nova rodada.
    /// Novas rodadas podem ser iniciadas enquanto a partida nao tiver sido finalizada.
    /// </summary>
    public void IniciarNovaRodada();

    /// <summary>
    /// Finaliza a partida.
    /// A partida deve ser finalizada quando um time atingir ou ultrapassar a pontuacao alvo estabelecida.
    /// </summary>
    public void FinalizarPartida();

    /// <summary>
    /// Verifica se a pontuacao alvo foi atingida.
    /// Este e o criterio principal para finalizar a partida e definir um time vencedor.
    /// </summary>
    /// <returns><see langword="true"/> quando algum time atingir ou ultrapassar a pontuacao alvo; caso contrario, <see langword="false"/>.</returns>
    public bool VerificaPontuacaoAlvoAtingida();
}