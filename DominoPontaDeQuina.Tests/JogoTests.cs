using DominoPontaDeQuina.Core;

namespace DominoPontaDeQuina.Core.Tests;

/// <summary>
/// Testes da classe Jogo para cenários de exceção e fluxo no nível de orquestração.
/// </summary>
public class JogoTests
{
    /// <summary>
    /// <b>Objetivo:</b> Garantir exceção ao executar jogada sem partida em andamento.
    /// <br/><b>Critério:</b> Deve lançar exceção e o tipo deve pertencer ao namespace do projeto.
    /// </summary>
    [Trait("Categoria", "Excecao")]
    [Fact(DisplayName = "Deve lançar exceção do projeto ao executar jogada sem partida em andamento.")]
    public async Task ExecutarJogadaAsync_DeveLancarExcecao_QuandoPartidaNaoEmAndamento()
    {
        var jogo = new Jogo();

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => jogo.ExecutarJogadaAsync());

        Assert.NotNull(ex.GetType().Namespace);
        Assert.StartsWith("DominoPontaDeQuina", ex.GetType().Namespace);
    }

    /// <summary>
    /// <b>Objetivo:</b> Garantir exceção ao obter jogada quando não existe rodada atual.
    /// <br/><b>Critério:</b> Deve lançar exceção e o tipo deve pertencer ao namespace do projeto.
    /// </summary>
    [Trait("Categoria", "Excecao")]
    [Fact(DisplayName = "Deve lançar exceção do projeto ao obter jogada sem rodada atual.")]
    public async Task GetJogadaAsync_DeveLancarExcecao_QuandoNaoHaRodadaAtual()
    {
        var jogo = new Jogo();

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => jogo.GetJogadaAsync());

        Assert.NotNull(ex.GetType().Namespace);
        Assert.StartsWith("DominoPontaDeQuina", ex.GetType().Namespace);
    }
}
