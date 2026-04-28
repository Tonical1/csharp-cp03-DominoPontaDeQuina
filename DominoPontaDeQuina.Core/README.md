# DominoPontaDeQuina.Core

Este projeto contém o esqueleto básico da regra do jogo Dominó Ponta de Quina.

## Objetivo do trabalho

O objetivo deste trabalho é implementar os gaps deixados no core do projeto.

O foco do aluno deve estar em completar a regra do jogo, a validação das jogadas, o controle de fluxo e a organização da lógica do software com boa qualidade de código.

Este core já fornece a base do domínio. Portanto, o aluno não deve se preocupar em criar novas interfaces ou novos componentes, a não ser que isso seja realmente necessário para a solução proposta.

## Regras do jogo

### Estrutura geral

O jogo segue a hierarquia:

- Partida
- Rodadas
- Jogadas

Uma partida é composta por várias rodadas.

Cada rodada funciona como um set:
- começa com a distribuição das peças
- termina quando um jogador bate
- ou termina quando o tabuleiro trava

O jogo pode ser disputado:
- por times com 1 jogador
- por times com 2 jogadores

Não existe enum de modo de jogo. O formato da partida deve ser determinado pela composição dos times cadastrados.

### Regras esperadas da partida

- Registrar os times e os jogadores.
- Iniciar a primeira rodada.
- Iniciar novas rodadas enquanto nenhum time atingir a pontuação alvo.
- Encerrar a partida quando um time atingir ou ultrapassar a pontuação alvo.

### Regras esperadas da rodada

- Distribuir peças aos jogadores.
- Na primeira rodada, iniciar com quem possuir a sena `[6|6]`.
- Nas rodadas seguintes, iniciar com quem venceu a rodada anterior.
- Executar as jogadas em sentido horário.
- Finalizar a rodada quando um jogador bater ou quando o tabuleiro travar.
- Em caso de travamento, definir o vencedor pela menor soma dos valores das peças restantes na mão.

### Regras esperadas da jogada

- O jogador escolhe uma peça e um lado do tabuleiro.
- A peça só pode ser colada se for compatível com a ponta escolhida.
- Depois da jogada, deve ser verificada a soma das pontas externas do tabuleiro.
- Quando a soma das pontas externas for múltiplo de 5, o time do jogador pontua.
- A pontuação da jogada deve ser calculada por `somaDasPontas / 5`.
- Se o jogador não tiver peça compatível, ele deve passar a vez.

## O que deve ser implementado pelos alunos

Os principais gaps deixados no core e que devem ser implementados são:

- distribuição de peças
- definição do jogador inicial da rodada
- validação das jogadas
- posicionamento de peças no tabuleiro
- controle de turno
- regra de pontuação
- verificação de batida
- verificação de tabuleiro travado
- definição do vencedor da rodada
- finalização da partida
- implementação das exceções customizadas que fizerem sentido no fluxo
- criação de serviços e validators para organizar a lógica do software

## Limites do escopo do aluno

Durante o desenvolvimento deste trabalho:

- a classe `Jogo` não deve ser alterada
- as interfaces existentes não devem ser alteradas
- o aluno deve evitar criar novas interfaces ou novos componentes, a não ser que considere isso realmente necessário
- o foco deve estar em completar e organizar a implementação dos gaps do core

## Fluxo de trabalho no repositório

- O aluno deve trabalhar em uma branch do próprio repositório.
- Não deve ser criado fork para a entrega.
- A branch desenvolvida será submetida a uma pipeline automatizada de testes.

## Critérios de avaliação

A nota será composta pelos seguintes critérios:

- Pipeline de testes: 50%
- Documentação do código: 10%
- Implementação de exceções customizadas: 10%
- Criação de serviços e validators organizando a lógica do software: 20%
- Aderência às convenções do C#: 10%

## Convenções esperadas

- Seguir as convenções de nomenclatura do C#.
- Escrever código limpo e organizado.
- Documentar os principais tipos e membros públicos.
- Utilizar exceções customizadas quando fizer sentido para a regra do domínio.
- Organizar a lógica em serviços e validators quando isso melhorar a separação de responsabilidades.

## Observação final

O objetivo não é reinventar a arquitetura inteira do projeto, mas completar com qualidade os pontos que foram deixados em aberto no core.
