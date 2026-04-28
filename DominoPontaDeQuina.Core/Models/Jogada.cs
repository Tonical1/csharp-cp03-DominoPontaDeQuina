using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IJogada"/>
/// <param name="jogador">O jogador da jogada.</param>
/// <param name="peca">A peca jogada, quando houver.</param>
/// <param name="valorColado">O valor colado na jogada, quando houver.</param>
/// <param name="lado">O lado escolhido, quando houver.</param>
public class Jogada(Jogador jogador, Peca? peca = null, int? valorColado = null, LadoTabuleiro? lado = null) : IJogada
{
    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador;

    /// <inheritdoc />
    public Peca? Peca { get; } = peca;

    /// <inheritdoc />
    public LadoTabuleiro? Lado { get; } = lado;

    /// <inheritdoc />
    public StatusJogada Status { get; private set; } = StatusJogada.Pendente;

    /// <inheritdoc />
    public bool EhPassarVez() =>
        Peca is null && Lado is null;

    /// <inheritdoc />
    public void MarcarComoAplicada() =>
        Status = StatusJogada.Aplicada;

    /// <inheritdoc />
    public void MarcarComoInvalida() =>
        Status = StatusJogada.Invalida;
}