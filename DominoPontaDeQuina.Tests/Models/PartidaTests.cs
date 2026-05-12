using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Tests.Models;

/// <summary>
/// Testes da classe Partida - gerencia o nível mais alto da hierarquia do jogo.
/// </summary>
[Trait("Categoria", "Basico")]
public class PartidaTests
{
    #region Status da Partida

    /// <summary>
    /// <b>Objetivo:</b> Validar transição de estado da partida ao iniciar nova rodada.
    /// <br/><b>Critério:</b> Status deve ficar EmAndamento após IniciarNovaRodada.
    /// </summary>
    [Fact(DisplayName = "Deve deixar a partida em andamento ao iniciar nova rodada (Critério: Status = EmAndamento).")]
    public void Partida_Status_DeveEstarEmAndamentoAposCriarRodada()
    {
        // Arrange
        var partida = new Partida();

        // Act
        partida.IniciarNovaRodada();

        // Assert
        Assert.Equal(StatusPartida.EmAndamento, partida.Status);
    }

    #endregion

    #region Rodadas

    /// <summary>
    /// <b>Objetivo:</b> Validar criação de rodada e atualização do histórico da partida.
    /// <br/><b>Critério:</b> RodadaAtual deve ser definida e histórico deve conter a nova rodada.
    /// </summary>
    [Fact(DisplayName = "Deve adicionar rodada ao histórico ao iniciar nova rodada (Critério: RodadaAtual definida e histórico incrementado).")]
    public void Partida_IniciarNovaRodada_DeveAdicionarRodadaAoHistorico()
    {
        // Arrange
        var partida = new Partida();

        // Act
        partida.IniciarNovaRodada();

        // Assert
        Assert.NotNull(partida.RodadaAtual);
        Assert.Single(partida.HistoricoRodadas);
    }

    #endregion

    #region Cenários Relevantes

    /// <summary>
    /// <b>Objetivo:</b> Validar consistência do histórico ao iniciar rodadas consecutivas.
    /// <br/><b>Critério:</b> Histórico deve refletir corretamente a quantidade de rodadas iniciadas.
    /// </summary>
    [Fact(DisplayName = "Deve manter histórico consistente ao iniciar múltiplas rodadas (Critério: quantidade correta de rodadas).")]
    public void Partida_AoIniciarMultiplasRodadas_DeveManterHistorico()
    {
        // Arrange
        var partida = new Partida(50);
        var time1 = new Time("Time A");
        var time2 = new Time("Time B");
        partida.Times.Add(time1);
        partida.Times.Add(time2);

        // Act
        partida.IniciarNovaRodada();
        partida.IniciarNovaRodada();

        // Assert
        Assert.Equal(2, partida.HistoricoRodadas.Count);
        Assert.Equal(50, partida.PontuacaoAlvo);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar armazenamento da pontuação alvo em diferentes instâncias de partida.
    /// <br/><b>Critério:</b> Cada partida deve manter seu valor de alvo configurado.
    /// </summary>
    [Fact(DisplayName = "Deve armazenar corretamente pontuações alvo diferentes entre partidas (Critério: cada instância preserva seu alvo).")]
    public void Partida_ComPontuacoesAlvoDiferentes_DeveArmazenarCadaUma()
    {
        // Arrange & Act
        var partida50 = new Partida(50);
        var partida100 = new Partida(100);
        var partida200 = new Partida(200);

        // Assert
        Assert.Equal(50, partida50.PontuacaoAlvo);
        Assert.Equal(100, partida100.PontuacaoAlvo);
        Assert.Equal(200, partida200.PontuacaoAlvo);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar estabilidade de estado da partida após várias rodadas consecutivas.
    /// <br/><b>Critério:</b> Histórico, status, alvo e quantidade de times devem permanecer consistentes.
    /// </summary>
    [Fact(DisplayName = "Deve manter estado consistente após cinco rodadas consecutivas (Critério: histórico, status, alvo e times corretos).")]
    public void Partida_AoCriar5RodasConsecutivas_DeveManterTudoCorreto()
    {
        // Arrange
        var partida = new Partida(100);
        var jogadores = new List<Jogador>
        {
            new Jogador("João"),
            new Jogador("Maria"),
            new Jogador("Pedro"),
            new Jogador("Ana"),
        };
        var time1 = new Time("Time A");
        var time2 = new Time("Time B");
        partida.Times.Add(time1);
        partida.Times.Add(time2);

        // Act
        for (int i = 0; i < 5; i++)
        {
            partida.IniciarNovaRodada();
        }

        var rodadas = partida.HistoricoRodadas.Count;

        // Assert
        Assert.Equal(5, rodadas);
        Assert.Equal(StatusPartida.EmAndamento, partida.Status);
        Assert.Equal(100, partida.PontuacaoAlvo);
        Assert.Equal(2, partida.Times.Count);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar transição completa de estado ao iniciar rodada.
    /// <br/><b>Critério:</b> Status em andamento, rodada atual definida e histórico com uma entrada.
    /// </summary>
    [Fact(DisplayName = "Deve transicionar status corretamente ao iniciar rodada (Critério: em andamento, rodada atual definida e histórico atualizado).")]
    public void Partida_AoIniciarRodada_DeveTransicionarStatusCorretamente()
    {
        // Arrange
        var partida = new Partida();

        // Act
        partida.IniciarNovaRodada();

        // Assert
        Assert.Equal(StatusPartida.EmAndamento, partida.Status);
        Assert.NotNull(partida.RodadaAtual);
        Assert.Single(partida.HistoricoRodadas);
    }

    #endregion
}
