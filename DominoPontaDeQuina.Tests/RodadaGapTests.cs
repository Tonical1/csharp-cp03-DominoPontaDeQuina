using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using System.Collections.ObjectModel;

namespace DominoPontaDeQuina.Core.Tests;

public class RodadaGapTests
{
    /// <summary>
    /// <b>Objetivo:</b> Validar transição inicial de estado ao iniciar a rodada.
    /// <br/><b>Critério:</b> Status deve ser EmAndamento e JogadorAtual deve ser definido.
    /// </summary>
    [Fact(DisplayName = "Deve iniciar a rodada em andamento e definir jogador atual (Critério: Status = EmAndamento e JogadorAtual não nulo).")]
    public void Iniciar_DeveColocarRodadaEmAndamentoEDefinirJogadorAtual()
    {
        var rodada = new Rodada();
        var jogadores = new List<Jogador>
        {
            new("Alice"),
            new("Bob"),
            new("Carol"),
            new("Dave")
        }.AsReadOnly();

        rodada.Iniciar(jogadores);

        Assert.Equal(StatusRodada.EmAndamento, rodada.Status);
        Assert.NotNull(rodada.JogadorAtual);
    }

    /// <summary>
    /// <b>Objetivo:</b> Garantir que registrar jogada aplique estado e histórico corretamente.
    /// <br/><b>Critério:</b> Jogada deve ficar como Aplicada e entrar no histórico da rodada.
    /// </summary>
    [Fact(DisplayName = "Deve marcar jogada como aplicada e adicionar ao histórico (Critério: StatusJogada = Aplicada e histórico atualizado).")]
    public void RegistrarJogada_DeveMarcarJogadaComoAplicadaEAdicionarAoHistorico()
    {
        var rodada = new Rodada();
        var jogada = new Jogada(new Jogador("Alice"));

        rodada.RegistrarJogada(jogada);

        Assert.Equal(StatusJogada.Aplicada, jogada.Status);
        Assert.Single(rodada.HistoricoJogadas);
        Assert.Same(jogada, rodada.HistoricoJogadas[0]);
    }
}
