using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Tests;

[Trait("Categoria", "Gap")]
public class TabuleiroGapTests
{
    /// <summary>
    /// <b>Objetivo:</b> Garantir a regra de abertura do tabuleiro vazio.
    /// <br/><b>Critério:</b> A primeira peça deve ser aceita em ambos os lados.
    /// </summary>
    [Fact(DisplayName = "Deve permitir colar a primeira peça em qualquer lado quando tabuleiro estiver vazio (Critério: true para esquerda e direita).")]
    public void PodeColar_DevePermitirPrimeiraPeca_QuandoTabuleiroEstiverVazio()
    {
        var tabuleiro = new Tabuleiro();

        var podeNaEsquerda = tabuleiro.PodeColar(new Peca(1, 4), LadoTabuleiro.Esquerda);
        var podeNaDireita = tabuleiro.PodeColar(new Peca(1, 4), LadoTabuleiro.Direita);

        Assert.True(podeNaEsquerda);
        Assert.True(podeNaDireita);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar compatibilidade de peça com a ponta escolhida.
    /// <br/><b>Critério:</b> Deve aceitar peça compatível e rejeitar peça incompatível.
    /// </summary>
    [Fact(DisplayName = "Deve validar compatibilidade da peça com a ponta escolhida (Critério: aceita compatível e rejeita incompatível).")]
    public void PodeColar_DeveValidarCompatibilidadeComPontaEscolhida()
    {
        var tabuleiro = new Tabuleiro();
        tabuleiro.Colar(new Peca(2, 5), LadoTabuleiro.Direita);

        var pecaCompativelEsquerda = tabuleiro.PodeColar(new Peca(2, 1), LadoTabuleiro.Esquerda);
        var pecaIncompativelEsquerda = tabuleiro.PodeColar(new Peca(6, 6), LadoTabuleiro.Esquerda);

        Assert.True(pecaCompativelEsquerda);
        Assert.False(pecaIncompativelEsquerda);
    }

    /// <summary>
    /// <b>Objetivo:</b> Garantir posicionamento correto da peça ao colar no lado esquerdo.
    /// <br/><b>Critério:</b> Pontas e quantidade de peças devem refletir o novo estado do tabuleiro.
    /// </summary>
    [Fact(DisplayName = "Deve posicionar corretamente peça no lado esquerdo (Critério: pontas e quantidade atualizadas).")]
    public void Colar_DevePosicionarPecaCorretamenteNoLadoEsquerdo()
    {
        var tabuleiro = new Tabuleiro();
        tabuleiro.Colar(new Peca(2, 5), LadoTabuleiro.Direita);

        tabuleiro.Colar(new Peca(3, 2), LadoTabuleiro.Esquerda);

        Assert.Equal(3, tabuleiro.PontaEsquerda);
        Assert.Equal(5, tabuleiro.PontaDireita);
        Assert.Equal(2, tabuleiro.Pecas.Count);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar detecção de travamento quando nenhum jogador possui peça compatível.
    /// <br/><b>Critério:</b> EstaTravado deve retornar true.
    /// </summary>
    [Fact(DisplayName = "Deve retornar true em EstaTravado quando nenhum jogador possuir peça compatível.")]
    public void EstaTravado_DeveRetornarTrue_QuandoNenhumJogadorPossuirPecaCompativel()
    {
        var tabuleiro = new Tabuleiro();
        tabuleiro.Colar(new Peca(1, 2), LadoTabuleiro.Direita);

        var maoJogadorA = new MaoJogador(new Jogador("Alice"));
        var maoJogadorB = new MaoJogador(new Jogador("Bob"));

        maoJogadorA.AdicionarPeca(new Peca(3, 4));
        maoJogadorB.AdicionarPeca(new Peca(5, 6));

        var travado = tabuleiro.EstaTravado([maoJogadorA, maoJogadorB]);

        Assert.True(travado);
    }

    /// <summary>
    /// <b>Objetivo:</b> Validar não travamento quando algum jogador possui peça compatível.
    /// <br/><b>Critério:</b> EstaTravado deve retornar false.
    /// </summary>
    [Fact(DisplayName = "Deve retornar false em EstaTravado quando algum jogador possuir peça compatível.")]
    public void EstaTravado_DeveRetornarFalse_QuandoAlgumJogadorPossuirPecaCompativel()
    {
        var tabuleiro = new Tabuleiro();
        tabuleiro.Colar(new Peca(1, 2), LadoTabuleiro.Direita);

        var maoJogadorA = new MaoJogador(new Jogador("Alice"));
        var maoJogadorB = new MaoJogador(new Jogador("Bob"));

        maoJogadorA.AdicionarPeca(new Peca(3, 4));
        maoJogadorB.AdicionarPeca(new Peca(2, 6));

        var travado = tabuleiro.EstaTravado([maoJogadorA, maoJogadorB]);

        Assert.False(travado);
    }
}
