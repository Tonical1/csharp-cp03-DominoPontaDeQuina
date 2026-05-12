using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Tests.Models;

/// <summary>
/// Testes da classe Peca - a unidade básica do domínio do jogo.
/// </summary>
[Trait("Categoria", "Basico")]
public class PecaTests
{
    #region SomaValores

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 2)]
    [InlineData(3, 5, 8)]
    [InlineData(6, 6, 12)]
    public void Peca_SomaValores_DeveRetornarSomaCorreta(int valorA, int valorB, int somaEsperada)
    {
        // Arrange
        var peca = new Peca(valorA, valorB);

        // Act
        var soma = peca.SomaValores;

        // Assert
        Assert.Equal(somaEsperada, soma);
    }

    #endregion

    #region EhSena

    /// <summary>
    /// <b>Objetivo:</b> Garantir que a peça [6|6] seja reconhecida como sena.
    /// <br/><b>Critério:</b> EhSena deve retornar true para [6|6].
    /// </summary>
    [Fact(DisplayName = "Deve retornar true para EhSena quando a peça for [6|6] (Critério: EhSena = true).")]
    public void Peca_EhSena_DeveRetornarTrueParaSena()
    {
        // Arrange
        var peca = new Peca(6, 6);

        // Act & Assert
        Assert.True(peca.EhSena);
    }

    [Theory]
    [InlineData(6, 5)]
    [InlineData(5, 6)]
    [InlineData(0, 0)]
    [InlineData(3, 3)]
    public void Peca_EhSena_DeveRetornarFalsePraOutrasPecas(int valorA, int valorB)
    {
        // Arrange
        var peca = new Peca(valorA, valorB);

        // Act & Assert
        Assert.False(peca.EhSena);
    }

    #endregion

    #region PossuiValor

    [Theory]
    [InlineData(2, 5, 2, true)]
    [InlineData(2, 5, 5, true)]
    [InlineData(2, 5, 3, false)]
    [InlineData(3, 3, 3, true)]
    public void Peca_PossuiValor_DeveRetornarValorCorreto(int valorA, int valorB, int valorBuscado, bool esperado)
    {
        // Arrange
        var peca = new Peca(valorA, valorB);

        // Act
        var possuiValor = peca.PossuiValor(valorBuscado);

        // Assert
        Assert.Equal(esperado, possuiValor);
    }

    [Theory]
    [InlineData(6, 6, 6)]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 1)]
    public void Peca_PossuiValor_DeveRetornarTrueParaDoublesPerfeitosDoValor(int valorA, int valorB, int valorBuscado)
    {
        // Arrange
        var peca = new Peca(valorA, valorB);

        // Act & Assert
        Assert.True(peca.PossuiValor(valorBuscado));
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    [InlineData(5, 6)]
    public void Peca_PossuiValor_DeveRetornarTrueParaAmbosOsLados(int valorA, int valorB)
    {
        // Arrange
        var peca = new Peca(valorA, valorB);

        // Act & Assert
        Assert.True(peca.PossuiValor(valorA));
        Assert.True(peca.PossuiValor(valorB));
    }

    #endregion

    #region Inverter

    [Theory]
    [InlineData(2, 5)]
    [InlineData(1, 3)]
    [InlineData(0, 6)]
    public void Peca_Inverter_DeveRetornarPecaComValoresInvertidos(int valorA, int valorB)
    {
        // Arrange
        var peca = new Peca(valorA, valorB);

        // Act
        var pecaInvertida = peca.Inverter();

        // Assert
        Assert.Equal(valorB, pecaInvertida.ValorA);
        Assert.Equal(valorA, pecaInvertida.ValorB);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar que inverter uma peça dupla mantém os valores originais.
    /// <br/><b>Critério:</b> ValorA e ValorB devem permanecer inalterados após inversão.
    /// </summary>
    [Fact(DisplayName = "Deve manter os mesmos valores ao inverter uma peça dupla (Critério: ValorA e ValorB inalterados).")]
    public void Peca_Inverter_DeveRetornarAPecaMesmaQuandoEhDouble()
    {
        // Arrange
        var peca = new Peca(3, 3);

        // Act
        var pecaInvertida = peca.Inverter();

        // Assert
        Assert.Equal(peca.ValorA, pecaInvertida.ValorA);
        Assert.Equal(peca.ValorB, pecaInvertida.ValorB);
    }

    /// <summary>
    /// <b>Objetivo:</b> Garantir reversibilidade da operação de inversão.
    /// <br/><b>Critério:</b> Inverter duas vezes deve retornar à peça original.
    /// </summary>
    [Fact(DisplayName = "Deve retornar à peça original após duas inversões (Critério: inversão dupla reversível).")]
    public void Peca_Inverter_DoubleInversaoDevolveAoPecaOriginal()
    {
        // Arrange
        var peca = new Peca(2, 5);

        // Act
        var pecaInvertidaDuasVezes = peca.Inverter().Inverter();

        // Assert
        Assert.Equal(peca.ValorA, pecaInvertidaDuasVezes.ValorA);
        Assert.Equal(peca.ValorB, pecaInvertidaDuasVezes.ValorB);
    }

    #endregion

    #region ToString

    [Theory]
    [InlineData(2, 5, "[2|5]")]
    [InlineData(0, 0, "[0|0]")]
    [InlineData(6, 6, "[6|6]")]
    public void Peca_ToString_DeveRetornarFormatoCorreto(int valorA, int valorB, string esperado)
    {
        // Arrange
        var peca = new Peca(valorA, valorB);

        // Act
        var toString = peca.ToString();

        // Assert
        Assert.Equal(esperado, toString);
    }

    #endregion

    #region Cenários Relevantes

    /// <summary>
    /// <b>Objetivo:</b> Validar coerência entre soma e inversão em múltiplas peças de uma mesma sequência.
    /// <br/><b>Critério:</b> Somas e valores invertidos devem refletir corretamente o estado esperado.
    /// </summary>
    [Fact(DisplayName = "Deve calcular somas e inversão corretamente em múltiplas peças (Critério: valores consistentes em sequência).")]
    public void Peca_MultiplasPecas_DeveCalcularValoresCorretos()
    {
        // Arrange
        var peca1 = new Peca(3, 5);
        var peca2 = new Peca(5, 2);
        var peca3 = new Peca(2, 4);

        // Act
        var soma1 = peca1.SomaValores;
        var soma2 = peca2.SomaValores;
        var soma3 = peca3.SomaValores;
        var invertida1 = peca1.Inverter();

        // Assert
        Assert.Equal(8, soma1);
        Assert.Equal(7, soma2);
        Assert.Equal(6, soma3);
        Assert.Equal(5, invertida1.ValorA);
        Assert.Equal(3, invertida1.ValorB);
    }

    #endregion
}
