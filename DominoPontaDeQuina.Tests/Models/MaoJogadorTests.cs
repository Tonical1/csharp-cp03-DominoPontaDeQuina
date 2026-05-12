using DominoPontaDeQuina.Core.Models;
using System.Reflection;

namespace DominoPontaDeQuina.Tests.Models;

/// <summary>
/// Testes da classe MaoJogador - gerencia as peças na mão de um jogador.
/// </summary>
public class MaoJogadorTests
{
    #region SomarPecasNaMao

    /// <summary>
    /// <b>Objetivo:</b> Garantir cálculo correto da soma total de valores com múltiplas peças distintas.
    /// <br/><b>Critério:</b> A soma deve ser igual à soma individual de cada peça da mão.
    /// </summary>
    [Trait("Categoria", "Basico")]
    [Fact(DisplayName = "Deve retornar soma correta com múltiplas peças na mão (Critério: soma total equivalente às peças adicionadas).")]
    public void MaoJogador_SomarPecasNaMao_DeveRetornarSomaCorretaComMultiplasPecas()
    {
        // Arrange
        var jogador = new Jogador("Carlos");
        var mao = new MaoJogador(jogador);
        mao.AdicionarPeca(new Peca(1, 2)); // soma: 3
        mao.AdicionarPeca(new Peca(3, 4)); // soma: 7
        mao.AdicionarPeca(new Peca(5, 6)); // soma: 11

        // Act
        var soma = mao.SomarPecasNaMao();

        // Assert
        Assert.Equal(21, soma); // 3 + 7 + 11
    }

    /// <summary>
    /// <b>Objetivo:</b> Garantir cálculo correto da soma quando a mão contém apenas senas.
    /// <br/><b>Critério:</b> Cada sena [6|6] deve contribuir com 12 pontos para a soma final.
    /// </summary>
    [Trait("Categoria", "Basico")]
    [Fact(DisplayName = "Deve calcular corretamente soma com senas na mão (Critério: cada [6|6] soma 12 pontos).")]
    public void MaoJogador_SomarPecasNaMao_DeveCalcularCorretamenteSenas()
    {
        // Arrange
        var jogador = new Jogador("Diana");
        var mao = new MaoJogador(jogador);
        mao.AdicionarPeca(new Peca(6, 6)); // soma: 12
        mao.AdicionarPeca(new Peca(6, 6)); // soma: 12

        // Act
        var soma = mao.SomarPecasNaMao();

        // Assert
        Assert.Equal(24, soma);
    }

    #endregion

    #region PossuiSena

    /// <summary>
    /// <b>Objetivo:</b> Verificar detecção de sena na mão do jogador.
    /// <br/><b>Critério:</b> PossuiSena deve retornar true quando existir peça [6|6].
    /// </summary>
    [Trait("Categoria", "Basico")]
    [Fact(DisplayName = "Deve retornar true em PossuiSena quando a mão contém [6|6] (Critério: detecção de sena).")]
    public void MaoJogador_PossuiSena_DeveRetornarTrueComSena()
    {
        // Arrange
        var jogador = new Jogador("Fernanda");
        var mao = new MaoJogador(jogador);
        mao.AdicionarPeca(new Peca(6, 6));

        // Act
        var possuiSena = mao.PossuiSena();

        // Assert
        Assert.True(possuiSena);
    }

    #endregion

    #region Cenários Relevantes

    /// <summary>
    /// <b>Objetivo:</b> Validar consistência de estado ao distribuir 7 peças, incluindo soma e presença de sena.
    /// <br/><b>Critério:</b> Quantidade, soma e indicador de sena devem refletir corretamente a distribuição.
    /// </summary>
    [Trait("Categoria", "Basico")]
    [Fact(DisplayName = "Deve manter estado consistente ao distribuir 7 peças (Critério: quantidade, soma e presença de sena corretas).")]
    public void MaoJogador_AoDistribuir7Pecas_DeveArmazenarSomaeSena()
    {
        // Arrange
        var jogador = new Jogador("Julia");
        var mao = new MaoJogador(jogador);

        // Distribuir 7 peças (distribuição típica do dominó)
        var pecas = new[]
        {
            new Peca(0, 1),
            new Peca(1, 2),
            new Peca(2, 3),
            new Peca(3, 4),
            new Peca(4, 5),
            new Peca(5, 6),
            new Peca(6, 6),
        };

        foreach (var peca in pecas)
        {
            mao.AdicionarPeca(peca);
        }

        // Act
        var soma = mao.SomarPecasNaMao();
        var temSena = mao.PossuiSena();
        var pecasCount = GetPecas(mao).Count;

        // Assert
        // (0+1) + (1+2) + (2+3) + (3+4) + (4+5) + (5+6) + (6+6)
        // = 1 + 3 + 5 + 7 + 9 + 11 + 12 = 48
        Assert.Equal(48, soma);
        Assert.True(temSena);
        Assert.Equal(7, pecasCount);
    }

    #endregion

    #region Gaps de Implementação

    /// <summary>
    /// <b>Objetivo:</b> Validar que uma jogada válida seja retornada quando houver peça compatível.
    /// <br/><b>Critério:</b> Jogada não nula, com jogador correto e peça definida.
    /// </summary>
    [Trait("Categoria", "Gap")]
    [Fact(DisplayName = "Deve retornar jogada válida quando houver possibilidade de jogar (Critério: jogada, jogador e peça definidos).")]
    public void MaoJogador_GetJogada_DeveRetornarJogadaValidaQuandoPossivel()
    {
        var mao = new MaoJogador(new Jogador("Teste"));
        mao.AdicionarPeca(new Peca(2, 5));
        var tabuleiro = new Tabuleiro();
        var jogada = mao.GetJogada(tabuleiro);
        Assert.NotNull(jogada);
        Assert.Equal(mao.Jogador, jogada.Jogador);
        Assert.NotNull(jogada.Peca);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar retorno de passar a vez quando nenhuma jogada for possível.
    /// <br/><b>Critério:</b> A jogada retornada deve representar passar vez.
    /// </summary>
    [Trait("Categoria", "Gap")]
    [Fact(DisplayName = "Deve passar a vez quando não houver jogada possível (Critério: EhPassarVez = true).")]
    public void MaoJogador_GetJogada_DeveRetornarPassarVezQuandoNaoPossivel()
    {
        var mao = new MaoJogador(new Jogador("Teste"));
        mao.AdicionarPeca(new Peca(1, 1));
        var tabuleiro = new Tabuleiro();
        tabuleiro.Pecas.Add(new Peca(2, 2));
        var jogada = mao.GetJogada(tabuleiro);
        Assert.True(jogada.EhPassarVez());
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar restauração de estado ao desfazer jogada.
    /// <br/><b>Critério:</b> A peça utilizada deve voltar para a mão após desfazer.
    /// </summary>
    [Trait("Categoria", "Gap")]
    [Fact(DisplayName = "Deve restaurar a peça na mão ao desfazer jogada (Critério: peça volta ao conjunto da mão).")]
    public void MaoJogador_DefazerJogada_DeveRestaurarPecaNaMao()
    {
        var mao = new MaoJogador(new Jogador("Teste"));
        var peca = new Peca(2, 5);
        mao.AdicionarPeca(peca);
        var tabuleiro = new Tabuleiro();
        var jogada = mao.GetJogada(tabuleiro);
        mao.DefazerJogada(jogada);
        Assert.Contains(peca, GetPecas(mao));
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar transição de estado completa: peça sai na jogada e retorna no desfazer.
    /// <br/><b>Critério:</b> A peça deve ser removida após jogar e restaurada após desfazer.
    /// </summary>
    [Trait("Categoria", "Gap")]
    [Fact(DisplayName = "Deve remover a peça após jogada e restaurar após desfazer (Critério: transição de estado ida e volta).")]
    public void MaoJogador_GetJogada_DeveMudarEstadoDaMaoAposJogadaEReverterComDefazer()
    {
        var mao = new MaoJogador(new Jogador("Teste"));
        var peca = new Peca(2, 5);
        mao.AdicionarPeca(peca);
        var tabuleiro = new Tabuleiro();
        var jogada = mao.GetJogada(tabuleiro);
        Assert.DoesNotContain(peca, GetPecas(mao));
        mao.DefazerJogada(jogada);
        Assert.Contains(peca, GetPecas(mao));
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar que passar vez não altera o estado da mão quando não há jogada possível.
    /// <br/><b>Critério:</b> Deve passar a vez e manter a quantidade de peças inalterada.
    /// </summary>
    [Trait("Categoria", "Gap")]
    [Fact(DisplayName = "Deve passar a vez sem alterar a mão quando não houver jogadas (Critério: estado da mão preservado).")]
    public void MaoJogador_GetJogada_DevePassarVezQuandoNaoHaMaisJogadasPossiveis()
    {
        var mao = new MaoJogador(new Jogador("Teste"));
        mao.AdicionarPeca(new Peca(1, 1));
        var tabuleiro = new Tabuleiro();
        tabuleiro.Pecas.Add(new Peca(2, 2));
        var jogada = mao.GetJogada(tabuleiro);
        Assert.True(jogada.EhPassarVez());
        Assert.Single(GetPecas(mao));
    }

    #endregion

    #region Helper Methods

    private List<Peca> GetPecas(MaoJogador mao)
    {
        var field = typeof(MaoJogador).GetField("_pecas", BindingFlags.NonPublic | BindingFlags.Instance);
        return field?.GetValue(mao) as List<Peca> ?? [];
    }

    #endregion
}
