# Testes do DominoPontaDeQuina

Este diretório contém os testes automatizados para o projeto DominoPontaDeQuina, cobrindo as classes principais do domínio com cenários complexos e completos.

## Estrutura dos Testes

### Testes Adicionados

#### 1. **PecaTests** (`Models/PecaTests.cs`)
Testes para a classe `Peca` - a unidade básica do domínio do jogo de dominó.

**Cobertura (21 testes):**
- ✅ Construção e armazenamento de valores
- ✅ Cálculo de soma de valores
- ✅ Identificação de senas (peça 6|6)
- ✅ Verificação de valores específicos
- ✅ Inversão de peças
- ✅ Representação em string
- ✅ Cenários complexos com múltiplas peças

**Exemplos de testes:**
```csharp
Peca_DeveArmazenarValoresCorretamente
Peca_SomaValores_DeveRetornarSomaCorreta
Peca_EhSena_DeveRetornarTrueParaSena
Peca_Inverter_DeveRetornarPecaComValoresInvertidos
Peca_CenarioComplexo_DistribuicaoCompleteDeTodasAsPecas
```

#### 2. **TabuleiroTests** (`Models/TabuleiroTests.cs`)
Testes para a classe `Tabuleiro` - gerencia o estado das peças coladas durante a rodada.

**Cobertura (16 testes):**
- ✅ Construção e estado inicial (vazio)
- ✅ Obtenção de pontas (esquerda e direita)
- ✅ Cálculo de soma de pontas externas
- ✅ Limpeza do tabuleiro
- ✅ Comportamento com múltiplas peças
- ✅ Cenários de partida simulada

**Exemplos de testes:**
```csharp
Tabuleiro_Construcao_DeveEstarVazioNoInicio
Tabuleiro_Pontas_DeveRetornarPontasExtremosComMultiplasPecas
Tabuleiro_SomarPontasExternas_DeveRetornarSomaApenasDosPontosExtremosComMultiplasPecas
Tabuleiro_Limpar_DeveRemoverTodasAsPecas
Tabuleiro_CenarioComplexo_SimulacaoDePartidaComMultiplasPecas
```

#### 3. **MaoJogadorTests** (`Models/MaoJogadorTests.cs`)
Testes para a classe `MaoJogador` - gerencia as peças na mão de um jogador.

**Cobertura (17 testes):**
- ✅ Construção e validação de jogador
- ✅ Adição de peças à mão
- ✅ Cálculo de soma de peças
- ✅ Verificação de sena
- ✅ Verificação se mão está vazia
- ✅ Cenários complexos com distribuição completa

**Exemplos de testes:**
```csharp
MaoJogador_AdicionarPeca_DeveArmazenarUmaPecaNaMao
MaoJogador_SomarPecasNaMao_DeveRetornarSomaCorretaComMultiplasPecas
MaoJogador_PossuiSena_DeveRetornarTrueComSena
MaoJogador_EstaSemPecas_DeveRetornarTrueParaMaoVazia
MaoJogador_CenarioComplexo_JogadorComDistribuicaoCompleta
MaoJogador_CenarioComplexo_MultiplicarPecasAteOLimiteDoJogo
```

#### 4. **PartidaTests** (`Models/PartidaTests.cs`)
Testes para a classe `Partida` - gerencia o nível mais alto da hierarquia do jogo.

**Cobertura (18 testes):**
- ✅ Construção com pontuação alvo customizável
- ✅ Gerenciamento de status da partida
- ✅ Adição de times
- ✅ Gerenciamento de rodadas
- ✅ Transições de estado
- ✅ Histórico de rodadas como ReadOnlyCollection
- ✅ Cenários complexos com múltiplas rodadas

**Exemplos de testes:**
```csharp
Partida_Construcao_DeveInicializarComPontuacaoAlvoPersonalizada
Partida_Status_DeveEstarEmAndamentoAposCriarRodada
Partida_Times_DevePermitirAdicionarMultiplosTimes
Partida_IniciarNovaRodada_DeveAdicionarRodadaAoHistorico
Partida_CenarioComplexo_PartidaCompleteComMultiplasRodadas
```

## Padrões Utilizados

### Nomenclatura dos Testes
Todos os testes seguem o padrão: **`Classe_Metodo_ComportamentoEsperado`**

Exemplo: `Peca_Inverter_DeveRetornarPecaComValoresInvertidos`

### Padrão AAA (Arrange-Act-Assert)
```csharp
[Fact]
public void Peca_DeveArmazenarValoresCorretamente()
{
    // Arrange - Prepara os dados de teste
    var peca = new Peca(3, 5);

    // Act - Executa a ação
    var valorA = peca.ValorA;

    // Assert - Verifica o resultado
    Assert.Equal(3, valorA);
}
```

### Uso de Theory com InlineData
Para testes parametrizados com múltiplos cenários:
```csharp
[Theory]
[InlineData(0, 0, 0)]
[InlineData(1, 1, 2)]
[InlineData(3, 5, 8)]
public void Peca_SomaValores_DeveRetornarSomaCorreta(int valorA, int valorB, int somaEsperada)
{
    // Testa múltiplos casos em um único método
}
```

## Executando os Testes

### Visual Studio
1. Abra o **Test Explorer** (View → Test Explorer)
2. Clique em "Run All Tests"
3. Ou selecione uma classe específica e execute

### Linha de Comando
```bash
# Executar todos os testes
dotnet test

# Executar apenas testes de uma classe
dotnet test --filter Namespace=DominoPontaDeQuina.Tests.Models.PecaTests

# Com saída detalhada
dotnet test --verbosity detailed

# Gerar relatório de cobertura
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## Cobertura de Testes

### Resumo Geral
- **Total de testes**: 72 testes
- **Classes cobertas**: 4 (Peca, Tabuleiro, MaoJogador, Partida)
- **Tipo de testes**: Fact (específicos), Theory (parametrizados)
- **Cenários**: Normais, extremos e complexos

### Distribuição por Classe
- **PecaTests**: 21 testes
- **TabuleiroTests**: 16 testes
- **MaoJogadorTests**: 17 testes
- **PartidaTests**: 18 testes

## Cenários Complexos Inclusos

### 1. Distribuição Completa do Dominó
- Testa a adição de todas as 28 peças possíveis
- Valida somas, contagens e estados

### 2. Simulação de Partida Completa
- Inicia múltiplas rodadas
- Testa transições de estado
- Valida histórico de rodadas

### 3. Fluxo de Jogo do Jogador
- Recebe 7 peças iniciais
- Verifica se possui sena
- Calcula soma de peças na mão
- Simula remoção de peças

### 4. Operações em Tabuleiro
- Coloca peças e monitora pontas
- Verifica mudanças de pontas extremas
- Calcula soma de pontos a cada jogada
- Limpa tabuleiro após rodada

## Métodos Auxiliares

### GetPecas (em MaoJogadorTests)
Usa Reflection para acessar o campo privado `_pecas` da classe `MaoJogador`:
```csharp
private List<Peca> GetPecas(MaoJogador mao)
{
    var field = typeof(MaoJogador).GetField("_pecas", 
        BindingFlags.NonPublic | BindingFlags.Instance);
    return field?.GetValue(mao) as List<Peca> ?? [];
}
```

## Requisitos

- .NET 8.0
- xUnit 2.9.2
- Microsoft.NET.Test.Sdk 17.11.1

## Integração Contínua

Os testes devem ser executados automaticamente em:
- ✅ Pull requests
- ✅ Commits na branch `main`
- ✅ Builds de release

## Roadmap Futuro

- [ ] Testes para `Rodada`
- [ ] Testes para `RodadaEmpate` (quando definida)
- [ ] Testes para `Time`
- [ ] Testes para `Jogador`
- [ ] Testes de integração para fluxos completos
- [ ] Testes de performance
