# Sumário dos Testes

## Visão geral
A suíte de testes está organizada para validar comportamento de domínio, fluxo da partida e cenários de gaps de implementação (abordagem *red*, com resultados esperados de negócio).

## Arquivos e cobertura funcional

### `Models/PecaTests.cs`
- Valida regras da entidade `Peca`:
  - `SomaValores`
  - `EhSena`
  - `PossuiValor`
  - `Inverter`
  - `ToString`
- Inclui cenário composto para coerência entre soma e inversão.

### `Models/TabuleiroTests.cs`
- Valida estado e regras do `Tabuleiro`:
  - pontas esquerda/direita
  - soma de pontas externas
  - limpeza do tabuleiro
  - sequência de jogadas com mudança de estado
- Inclui cenários para gaps:
  - `PodeColar`
  - `Colar`
  - `EstaTravado`

### `Models/MaoJogadorTests.cs`
- Valida regras da `MaoJogador`:
  - soma de peças
  - identificação de sena
  - cenário de mão distribuída com consistência de estado
- Inclui cenários de gap:
  - seleção de jogada (`GetJogada`)
  - passar vez
  - desfazer jogada (`DefazerJogada`)
  - transição de estado (jogar e desfazer)

### `Models/PartidaTests.cs`
- Valida comportamento base da `Partida`:
  - mudança de status ao iniciar rodada
  - criação e histórico de rodadas
  - manutenção de estado em múltiplas rodadas
  - persistência de pontuação alvo

### `Models/RodadaGapTests.cs`
- Testa resultados esperados de implementação futura em `Rodada`:
  - finalização por batida com vencedor correto
  - finalização por travamento com vencedor por menor soma
- Esses testes exercitam comportamento de negócio esperado para métodos ainda não implementados.

### `PartidaFluxoTests.cs`
- Valida fluxo de finalização da `Partida`:
  - finaliza ao atingir pontuação alvo
  - não finaliza sem atingir alvo
  - lança exceção ao finalizar em estado inválido
- Critério de exceção: `Assert.Throws<Exception>` com validação de namespace `DominoPontaDeQuina`.

### `PartidaGapTests.cs`
- Testa resultados esperados para gaps da `Partida`:
  - pontuação por time
  - verificação de pontuação alvo atingida
  - obtenção de time vencedor

### `MaoJogadorGapTests.cs`
- Testa resultados esperados para gaps de `MaoJogador`:
  - passar vez quando não há peça compatível
  - restauração de estado ao desfazer jogada

### `TabuleiroGapTests.cs`
- Testa resultados esperados para gaps de `Tabuleiro`:
  - validação de jogada em tabuleiro vazio
  - compatibilidade com pontas
  - colagem no lado correto
  - detecção de travamento/destravamento

### `RodadaGapTests.cs` (raiz)
- Valida aspectos de fluxo da `Rodada`:
  - início em andamento com jogador atual definido
  - registro de jogada no histórico

### `RodadaFinalizacaoGapTests.cs`
- Valida finalização de rodada em cenários de:
  - batida
  - tabuleiro travado
- Verifica status final, tipo de finalização e vencedor.

## Diretrizes aplicadas na suíte
- Evitar testes triviais que apenas confirmam dados criados no próprio setup.
- Evitar testes que apenas validem `NotImplementedException` como estado atual.
- Priorizar testes de comportamento esperado da implementação futura (gaps em modo *red*).
- Para disparo de exceções, usar `Assert.Throws<Exception>` e validar namespace do tipo lançado (`DominoPontaDeQuina...`).
