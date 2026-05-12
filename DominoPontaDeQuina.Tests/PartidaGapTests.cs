using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Tests;

[Trait("Categoria", "Gap")]
public class PartidaGapTests
{
    /// <summary>
    /// <b>Objetivo:</b> Validar retorno da pontuação acumulada por time na partida.
    /// <br/><b>Critério:</b> Dicionário deve conter todos os times com seus respectivos pontos.
    /// </summary>
    [Fact(DisplayName = "Deve retornar pontuação dos times da partida (Critério: dicionário completo com valores corretos).")]
    public void GetPontuacaoTimes_DeveRetornarPontuacaoDosTimesDaPartida()
    {
        var partida = new Partida();
        var timeA = new Time("Time A");
        var timeB = new Time("Time B");

        timeA.SomarPontos(12);
        timeB.SomarPontos(7);

        partida.Times.Add(timeA);
        partida.Times.Add(timeB);

        var pontuacoes = partida.GetPontuacaoTimes();

        Assert.Equal(2, pontuacoes.Count);
        Assert.Equal(12, pontuacoes[timeA]);
        Assert.Equal(7, pontuacoes[timeB]);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar detecção de alcance da pontuação alvo por algum time.
    /// <br/><b>Critério:</b> Deve retornar true quando ao menos um time atingir o alvo.
    /// </summary>
    [Fact(DisplayName = "Deve retornar true quando algum time atingir a pontuação alvo (Critério: VerificaPontuacaoAlvoAtingida = true).")]
    public void VerificaPontuacaoAlvoAtingida_DeveRetornarTrue_QuandoAlgumTimeAtingirAlvo()
    {
        var partida = new Partida(pontuacaoAlvo: 15);
        var timeA = new Time("Time A");
        var timeB = new Time("Time B");

        timeA.SomarPontos(10);
        timeB.SomarPontos(15);

        partida.Times.Add(timeA);
        partida.Times.Add(timeB);

        var pontuacaoAtingida = partida.VerificaPontuacaoAlvoAtingida();

        Assert.True(pontuacaoAtingida);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar seleção do time vencedor com base na pontuação alvo.
    /// <br/><b>Critério:</b> Deve retornar o time que atingiu primeiro ou corretamente o alvo.
    /// </summary>
    [Fact(DisplayName = "Deve retornar o time vencedor ao atingir pontuação alvo (Critério: time retornado corresponde ao vencedor esperado).")]
    public void GetTimeVencedor_DeveRetornarTimeQueAtingiuPontuacaoAlvo()
    {
        var partida = new Partida(pontuacaoAlvo: 20);
        var timeA = new Time("Time A");
        var timeB = new Time("Time B");

        timeA.SomarPontos(20);
        timeB.SomarPontos(18);

        partida.Times.Add(timeA);
        partida.Times.Add(timeB);

        var vencedor = partida.GetTimeVencedor();

        Assert.Same(timeA, vencedor);
    }
}
