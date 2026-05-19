using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    internal List<Peca> _pecas = [];

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        // TODO ALUNO: definir como a mao escolhe a jogada com base nas pecas disponiveis e no estado do tabuleiro.
        if (tabuleiro.EstaVazio)
        {
            var peca = _pecas.First();
            _pecas.Remove(peca);
            return new Jogada(Jogador, peca, peca.ValorA, Enums.LadoTabuleiro.Esquerda);
        }

        var pontaEsquerda = tabuleiro.PontaEsquerda;
        var pontaDireita = tabuleiro.PontaDireita;

        foreach (var peca in _pecas)
        {
            if (peca.PossuiValor(pontaEsquerda.Value))
            {
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, pontaEsquerda.Value, Enums.LadoTabuleiro.Esquerda);
            }
            if (peca.PossuiValor(pontaDireita.Value))
            {
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, pontaDireita.Value, Enums.LadoTabuleiro.Direita);
            }
        }

        return new Jogada(Jogador);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        // TODO ALUNO: restaurar a mao do jogador ao estado anterior a jogada desfeita.
        if (jogada.Peca is not null)
        {
            _pecas.Add(jogada.Peca.Value);
        }
    }
}