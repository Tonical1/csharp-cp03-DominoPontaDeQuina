# 📋 GAPS DE IMPLEMENTAÇÃO - RELATÓRIO

## Métodos Não Implementados (TODO ALUNO)

### 1. **Tabuleiro.cs**

#### `PodeColar(Peca peca, LadoTabuleiro lado) → bool`
- **Descrição**: Determina se uma peça pode ser colada no lado informado
- **Regra**: Validar se a peça possui valor compatível com a ponta externa do lado
- **Lógica**:
  - Se tabuleiro vazio: sempre pode colar (true)
  - Se lado == Esquerda: peça deve ter ValorA == PontaEsquerda
  - Se lado == Direita: peça deve ter ValorB == PontaDireita
  - A peça pode ter o valor em ValorA ou ValorB (invertida)

#### `Colar(Peca peca, LadoTabuleiro lado) → void`
- **Descrição**: Cola uma peça no lado informado do tabuleiro
- **Regra**: Posicionar a peça no lado correto, invertendo valores quando necessário
- **Lógica**:
  - Se tabuleiro vazio: adiciona peça no início
  - Se lado == Esquerda: adiciona no início, invertendo se necessário
  - Se lado == Direita: adiciona no final, invertendo se necessário
  - Verificar compatibilidade com PodeColar antes de chamar

#### `EstaTravado(IEnumerable<MaoJogador> maosJogadores) → bool`
- **Descrição**: Determina se o tabuleiro está travado
- **Regra**: Nenhuma mão de jogador possui peça compatível com as pontas
- **Lógica**:
  - Se tabuleiro vazio: false (nunca está travado)
  - Para cada mão do jogador: verificar se existe peça que pode colar em qualquer lado
  - Se nenhuma mão conseguir jogar em nenhum lado: true

---

### 2. **MaoJogador.cs**

#### `GetJogada(Tabuleiro tabuleiro) → Jogada`
- **Descrição**: Define como a mão escolhe a jogada
- **Regra**: Baseada nas peças disponíveis e estado do tabuleiro
- **Lógica**:
  - Procurar primeira peça que pode colar no lado direito
  - Se não encontrar, procurar no lado esquerdo
  - Se não encontrar, retornar Jogada de passagem (null)
  - Retornar Jogada com a peça, lado e valor colado

#### `DefazerJogada(Jogada jogada) → void`
- **Descrição**: Restaura a mão ao estado anterior
- **Regra**: Devolver a peça à mão se ela foi usada
- **Lógica**:
  - Se jogada.Peca não é null: adicionar novamente à mão
  - Se jogada.EhPassarVez(): não fazer nada

---

### 3. **Rodada.cs**

#### `DistribuirPecas(ReadOnlyCollection<Jogador> jogadores) → List<MaoJogador>`
- **Descrição**: Distribui as 28 peças entre os jogadores
- **Regra**: Cada jogador recebe 7 peças do dominó
- **Lógica**:
  - Criar lista com todas as 28 peças [0|0] até [6|6]
  - Embaralhar a lista
  - Distribuir 7 peças para cada jogador
  - Retornar lista de MaoJogador com peças distribuídas

#### `GetPrimeiroJogador(List<MaoJogador> jogadores, Rodada? rodadaAnterior) → Jogador`
- **Descrição**: Determina quem começa a rodada
- **Regra**: Primeira rodada: quem tem a sena | Próximas: vencedor da anterior
- **Lógica**:
  - Se rodadaAnterior != null: retornar GetVencedor()
  - Senão: procurar jogador que possui sena [6|6]
  - Se ninguém tem sena: retornar primeiro jogador

#### `OrganizaJogadores(List<MaoJogador> jogadores, Jogador primeiroJogador) → void`
- **Descrição**: Monta a fila de execução a partir do primeiro
- **Regra**: Queue começa no primeiro e segue circular
- **Lógica**:
  - Limpar fila _jogadores
  - Encontrar índice do primeiro jogador
  - Enfileirar jogadores começando do primeiro em ordem circular

#### `VerificarBatida() → bool`
- **Descrição**: Verifica se alguém bateu (sem peças na mão)
- **Regra**: Batida = mão vazia
- **Lógica**:
  - Verificar se JogadorAtual.EstaSemPecas()
  - Se sim: TipoFinalizacao = JogadorBateu, Status = Finalizada

#### `VerificarTabuleiroTravado() → bool`
- **Descrição**: Verifica se tabuleiro travou (ninguém pode jogar)
- **Regra**: Nenhuma mão pode colar em nenhum lado
- **Lógica**:
  - Coletar todas as mãos dos jogadores
  - Chamar Tabuleiro.EstaTravado(maosJogadores)
  - Se travado: TipoFinalizacao = TabuleiroTravado, Status = Finalizada

#### `GetVencedor() → Jogador?`
- **Descrição**: Retorna o vencedor da rodada
- **Regra**: Quem ficou sem peças (batida) ou quem tem menor soma (travamento)
- **Lógica**:
  - Se TipoFinalizacao == JogadorBateu: retornar JogadorAtual
  - Se TipoFinalizacao == TabuleiroTravado: retornar jogador com menor soma
  - Senão: null

#### `CalcularPontuacao() → void`
- **Descrição**: Calcula pontos após registrar jogada
- **Regra**: Pontos em múltiplos de 5 da soma das pontas
- **Lógica**:
  - Se TipoFinalizacao não é null: rodada não terminou
  - Calcular soma = Tabuleiro.SomarPontasExternas()
  - Se soma % 5 == 0: atribuir pontos ao vencedor (será implementado depois)
  - Verificar batida e travamento
  - Se rodada terminou: atualizar Status

---

### 4. **Partida.cs**

#### `GetPontuacaoTimes() → Dictionary<Time, int>`
- **Descrição**: Retorna pontuação acumulada de cada time
- **Regra**: Somar pontos de todos os jogadores de cada time
- **Lógica**:
  - Criar dicionário Time → int
  - Iterar cada rodada no histórico
  - Somar pontos dos jogadores de cada time
  - Retornar dicionário

#### `GetTimeVencedor() → Time?`
- **Descrição**: Retorna o time vencedor
- **Regra**: Time que atingiu PontuacaoAlvo
- **Lógica**:
  - Obter pontuação de cada time
  - Procurar time com pontuação >= PontuacaoAlvo
  - Retornar time ou null

#### `VerificaPontuacaoAlvoAtingida() → bool`
- **Descrição**: Verifica se algum time atingiu pontuação alvo
- **Regra**: Time >= PontuacaoAlvo encerra partida
- **Lógica**:
  - Obter pontuação de cada time
  - Verificar se algum time >= PontuacaoAlvo
  - Retornar true/false

---

## Resumo dos Gaps

| Classe | Método | Tipo | Complexidade |
|--------|--------|------|--------------|
| Tabuleiro | PodeColar | public | Média |
| Tabuleiro | Colar | public | Média |
| Tabuleiro | EstaTravado | public | Alta |
| MaoJogador | GetJogada | public | Média |
| MaoJogador | DefazerJogada | public | Baixa |
| Rodada | DistribuirPecas | private | Média |
| Rodada | GetPrimeiroJogador | private | Baixa |
| Rodada | OrganizaJogadores | private | Média |
| Rodada | VerificarBatida | public | Baixa |
| Rodada | VerificarTabuleiroTravado | public | Média |
| Rodada | GetVencedor | public | Média |
| Rodada | CalcularPontuacao | private | Alta |
| Partida | GetPontuacaoTimes | public | Alta |
| Partida | GetTimeVencedor | public | Média |
| Partida | VerificaPontuacaoAlvoAtingida | public | Baixa |

**Total: 15 métodos para implementar**

---

Status: Pronto para implementação
