using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using System.Reflection;

namespace DominoPontaDeQuina.Tests.Models;

/// <summary>
/// Testes de comportamento esperado da classe Rodada para métodos ainda não implementados.
/// </summary>
public class RodadaGapTests
{
    /// <summary>
    /// <b>Objetivo:</b> Validar que batida finaliza a rodada e define vencedor corretamente.
    /// <br/><b>Critério:</b> Ao detectar jogador atual sem peças, deve finalizar com tipo JogadorBateu.
    /// </summary>
    [Fact(DisplayName = "Deve finalizar a rodada por batida quando o jogador atual ficar sem peças.")]
    public void Rodada_VerificarBatida_DeveFinalizarRodadaQuandoJogadorAtualSemPecas()
    {
        var rodada = new Rodada();
        var semPecas = new MaoJogador(new Jogador("A"));
        var comPeca = new MaoJogador(new Jogador("B"));
        comPeca.AdicionarPeca(new Peca(1, 1));

        ConfigurarFilaJogadores(rodada, [semPecas, comPeca]);
        ConfigurarStatus(rodada, StatusRodada.EmAndamento);

        var houveBatida = rodada.VerificarBatida();

        Assert.True(houveBatida);
        Assert.Equal(StatusRodada.Finalizada, rodada.Status);
        Assert.Equal(TipoFinalizacaoRodada.JogadorBateu, rodada.TipoFinalizacao);
        Assert.Same(semPecas.Jogador, rodada.GetVencedor());
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar que travamento finaliza a rodada e escolhe vencedor por menor soma na mão.
    /// <br/><b>Critério:</b> Tabuleiro sem jogadas possíveis deve finalizar com tipo TabuleiroTravado.
    /// </summary>
    [Fact(DisplayName = "Deve finalizar a rodada por tabuleiro travado e definir vencedor por menor soma.")]
    public void Rodada_VerificarTabuleiroTravado_DeveFinalizarComVencedorPorMenorSoma()
    {
        var rodada = new Rodada();
        rodada.Tabuleiro.Colar(new Peca(1, 2), LadoTabuleiro.Direita);

        var maoA = new MaoJogador(new Jogador("A"));
        maoA.AdicionarPeca(new Peca(3, 4)); // soma 7

        var maoB = new MaoJogador(new Jogador("B"));
        maoB.AdicionarPeca(new Peca(6, 6)); // soma 12

        ConfigurarFilaJogadores(rodada, [maoA, maoB]);
        ConfigurarStatus(rodada, StatusRodada.EmAndamento);

        var travou = rodada.VerificarTabuleiroTravado();

        Assert.True(travou);
        Assert.Equal(StatusRodada.Finalizada, rodada.Status);
        Assert.Equal(TipoFinalizacaoRodada.TabuleiroTravado, rodada.TipoFinalizacao);
        Assert.Same(maoA.Jogador, rodada.GetVencedor());
    }

    private static void ConfigurarFilaJogadores(Rodada rodada, IEnumerable<MaoJogador> maos)
    {
        var filaField = typeof(Rodada).GetField("_jogadores", BindingFlags.NonPublic | BindingFlags.Instance)!;
        filaField.SetValue(rodada, new Queue<MaoJogador>(maos));
    }

    private static void ConfigurarStatus(Rodada rodada, StatusRodada status)
    {
        var statusField = typeof(Rodada).GetField("<Status>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!;
        statusField.SetValue(rodada, status);
    }
}
