using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o nivel de partida na hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam o estado global, os times participantes e o historico das rodadas.
/// </summary>
/// <param name="pontuacaoAlvo">
/// Define a pontuacao minima que um time deve atingir para encerrar a partida como vencedor.
/// </param>
public class Partida(int pontuacaoAlvo = 50) : IPartida
{
    /// <summary>
    /// Armazena as rodadas registradas neste nivel da hierarquia.
    /// </summary>
    Stack<Rodada> _rodadas = [];

    /// <inheritdoc />
    public int PontuacaoAlvo { get; } = pontuacaoAlvo;

    /// <inheritdoc />
    public StatusPartida Status { get; protected set; } = StatusPartida.NaoIniciada;

    /// <inheritdoc />
    public List<Time> Times { get; } = [];

    /// <inheritdoc />
    public ReadOnlyCollection<Rodada> HistoricoRodadas => _rodadas.ToList().AsReadOnly();

    /// <inheritdoc />
    public Rodada? RodadaAtual => _rodadas?.Peek();

    /// <inheritdoc />
    public Dictionary<Time, int> GetPontuacaoTimes()
    {
        return Times.ToDictionary(time => time, time => time.Pontuacao);
    }

    /// <inheritdoc />
    public Time? GetTimeVencedor()
    {
        var pontuacaoTimes = GetPontuacaoTimes();

        return pontuacaoTimes
            .Where(kvp => kvp.Value >= PontuacaoAlvo)
            .OrderByDescending(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .FirstOrDefault();
    }

    /// <inheritdoc />
    public bool VerificaPontuacaoAlvoAtingida()
    {
        return GetPontuacaoTimes().Values.Any(pontos => pontos >= PontuacaoAlvo);
    }

    /// <inheritdoc />
    public void IniciarNovaRodada()
    {
        if (Status is StatusPartida.Finalizada)
            throw new DominioException("Nao e possivel iniciar uma nova rodada em uma partida finalizada.");
        Status = StatusPartida.EmAndamento;
        _rodadas.Push(new());
    }

    /// <inheritdoc />
    public void FinalizarPartida()
    {
        if (Status is not StatusPartida.EmAndamento)
            throw new DominioException("Nao e possivel finalizar uma partida que nao esta em andamento.");
        if(VerificaPontuacaoAlvoAtingida())
            Status = StatusPartida.Finalizada;
    }
}