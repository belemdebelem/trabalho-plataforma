# Unity 3D Side-Scroller Platformer

## 1. Visão geral

Este projeto consiste no desenvolvimento de um jogo de plataforma 3D com movimentação lateral, utilizando Unity.

O projeto possui finalidade acadêmica e didática, tendo como principal objetivo aprender os fundamentos do desenvolvimento de jogos utilizando a Unity Engine.

O jogo terá aparência e modelagem inteiramente tridimensionais, utilizando objetos geométricos nativos da Unity. Entretanto, sua jogabilidade será limitada a um plano bidimensional, seguindo o estilo de jogos clássicos de plataforma, como Super Mario e Sonic.

O desenvolvimento será conduzido integralmente por uma LLM em ambiente CLI, responsável por criar toda a estrutura do projeto, escrever os scripts, configurar os objetos, gerar a cena e preparar os arquivos necessários para execução em outra máquina.

**Restrição importante:** a máquina utilizada para o desenvolvimento não possui Unity instalada. Portanto, não será possível abrir o Editor, executar o jogo ou realizar testes visuais durante a implementação.

A LLM deverá gerar todos os arquivos necessários para que o projeto possa ser aberto, configurado e executado posteriormente em outra máquina Windows 10 com Unity instalada.

---

## 2. Requisitos técnicos e compatibilidade

### 2.1. Engine

- Engine: Unity.
- Versão-alvo: Unity 2022.3.22f1 (linha 2022.3 LTS).
- Linguagem: C#.
- Sistema operacional de destino: Windows 10.
- Tipo de jogo: 3D Side-Scroller Platformer.
- Renderização: 3D.
- Câmera: Orthographic Camera.
- Física: Unity 3D Physics (PhysX).
- Sistema de entrada: Input Manager tradicional.

A versão exata do Editor deverá ser confirmada antes da implementação e registrada em `ProjectSettings/ProjectVersion.txt`.

Não utilizar APIs ou funcionalidades exclusivas de versões posteriores da Unity.

### 2.2. Requisitos de execução

O projeto deverá:

- Abrir em uma instalação limpa da Unity compatível com a versão especificada.
- Não depender de bibliotecas externas pagas.
- Não depender de serviços online.
- Não depender de arquivos presentes exclusivamente na máquina de desenvolvimento.
- Não exigir instalação manual de pacotes externos.
- Ser compilável para Windows 10.
- Possuir todos os scripts, cenas, configurações e recursos necessários dentro do diretório do projeto.

Priorizar funcionalidades nativas da Unity.

---

## 3. Conceito e funcionamento do jogo

O jogo será um plataforma 3D com perspectiva lateral.

O jogador controlará um personagem representado por um cilindro azul, que deverá percorrer um cenário composto por blocos de terra e grama, coletando moedas amarelas distribuídas pelo ambiente.

A movimentação será limitada a dois eixos:

- Eixo X: movimentação horizontal, para esquerda e direita.
- Eixo Y: movimentação vertical, utilizada principalmente para saltos e movimentação pela gravidade.

O eixo Z será utilizado para definir a profundidade dos objetos, mas o personagem não poderá se deslocar livremente nesse eixo.

A câmera ficará posicionada lateralmente ao cenário, mostrando a fase em uma perspectiva semelhante à de jogos tradicionais de plataforma.

### 3.1. Representação dos eixos

A organização espacial deverá seguir este padrão:

| Eixo | Função |
|---|---|
| X | Movimento horizontal |
| Y | Altura e pulo |
| Z | Profundidade do cenário |

O personagem deverá permanecer em um plano Z fixo durante toda a execução.

As plataformas poderão possuir volume e profundidade, mas deverão estar posicionadas de maneira que suas superfícies de colisão sejam acessíveis pelo personagem.

### 3.2. Referência visual

A estética deverá ser simples, utilizando geometria tridimensional e cores sólidas.

Não é necessário criar personagens detalhados, texturas complexas ou animações elaboradas.

O objetivo é construir um protótipo funcional de um jogo de plataforma 3D.

---

## 4. Elementos do jogo

Todos os objetos deverão ser criados utilizando primitivas geométricas nativas da Unity.

Não utilizar modelos 3D externos, sprites, imagens baixadas da internet ou assets de terceiros.

### 4.1. Personagem principal

O personagem será representado por um cilindro azul.

Características:

- GameObject: Cylinder.
- Aparência: cilindro azul.
- Geometria: tridimensional.
- Material: azul sólido.
- Rigidbody.
- Collider adequado.
- Script de movimentação.
- Sistema de detecção de chão.

O personagem deverá possuir:

- Controle de movimentação horizontal.
- Capacidade de pular.
- Gravidade.
- Colisões físicas com as plataformas.
- Restrição de movimento no eixo Z.
- Rotação física bloqueada para evitar tombamentos.

A implementação deverá utilizar Rigidbody 3D e componentes de física nativos da Unity.

A restrição de movimento deverá ser implementada utilizando as propriedades de constraints do Rigidbody, especialmente `Freeze Position Z` e bloqueio das rotações.

### 4.2. Plataformas

O cenário deverá possuir exatamente 12 plataformas principais.

Cada plataforma será um bloco tridimensional de terra com grama na parte superior.

Características:

- Objeto principal: Cube.
- Parte inferior: marrom, representando terra.
- Parte superior: verde, representando grama.
- A camada verde deverá representar aproximadamente 10% da altura total do bloco.
- A camada verde deverá acompanhar a largura e profundidade do bloco.
- Cada bloco deverá possuir colisão física.

A implementação deverá utilizar dois objetos tridimensionais:

1. Um cubo marrom representando o corpo de terra.
2. Um cubo verde mais fino, posicionado sobre o primeiro, representando a grama.

Os dois objetos deverão formar visualmente uma única plataforma.

A camada de grama deverá possuir um pequeno ajuste de posicionamento para evitar sobreposição visual inadequada.

As 12 plataformas deverão ser organizadas em diferentes posições horizontais e alturas.

A distribuição deverá criar um percurso que exija que o jogador utilize o pulo para avançar.

Organização sugerida:

- Plataformas iniciais próximas umas das outras.
- Plataformas intermediárias em diferentes alturas.
- Algumas plataformas com espaçamento maior, exigindo pulos mais longos.
- Uma plataforma final representando a chegada.

Os espaçamentos deverão ser calculados considerando a velocidade horizontal e a altura máxima de pulo do personagem.

Nenhuma plataforma obrigatória deverá exigir um salto impossível.

**Quantidade obrigatória: 12 plataformas principais.**

### 4.3. Moedas

As moedas serão representadas por esferas amarelas tridimensionais.

Características:

- GameObject: Sphere.
- Material: amarelo.
- Geometria 3D.
- SphereCollider configurado como Trigger.
- Posicionadas em diferentes locais da fase.

Ao entrar em contato com o personagem:

1. O sistema deverá identificar a colisão.
2. O contador de moedas deverá ser incrementado.
3. A moeda deverá desaparecer da cena.
4. A interface deverá ser atualizada.

Cada moeda deverá ser coletada somente uma vez.

As moedas deverão ser distribuídas ao longo das plataformas, incentivando o jogador a realizar saltos e explorar o cenário.

A quantidade deverá ser definida pela LLM, preferencialmente entre 8 e 12 moedas.

### 4.4. Cenário

O cenário deverá ser tridimensional, mas visualmente simples.

Características:

- Fundo azul-claro ou azul-celeste.
- Plataformas com volume tridimensional.
- Personagem cilíndrico azul.
- Moedas esféricas amarelas.
- Iluminação simples.
- Câmera lateral ortográfica.

O ambiente poderá possuir um plano de fundo ou elementos decorativos simples, desde que não interfiram na jogabilidade.

Evitar objetos desnecessários.

---

## 5. Câmera e visualização

A câmera será um dos elementos fundamentais do projeto.

Deverá ser configurada para observar o cenário lateralmente, como em um jogo tradicional de plataforma.

### 5.1. Configuração

- Tipo: Camera.
- Projection: Orthographic.
- Posição: lateral ao plano de movimentação.
- Orientação: direcionada para o plano X/Y.
- Profundidade: deverá permitir visualizar o volume tridimensional dos objetos.
- Clipping planes: configurados adequadamente para o tamanho do cenário.

A câmera deverá observar o cenário ao longo do eixo Z, de forma que o jogador e as plataformas sejam vistos lateralmente.

### 5.2. Comportamento

A câmera poderá permanecer fixa durante a primeira versão do jogo.

Entretanto, sua configuração deverá permitir uma futura implementação de acompanhamento do personagem.

Não implementar um sistema complexo de câmera nesta versão.

---

## 6. Mecânicas de gameplay

### 6.1. Movimentação horizontal

O personagem deverá se movimentar para esquerda e direita.

Controles:

| Tecla | Ação |
|---|---|
| A | Movimentar para a esquerda |
| D | Movimentar para a direita |
| Seta esquerda | Movimentar para a esquerda |
| Seta direita | Movimentar para a direita |

A movimentação deverá ocorrer exclusivamente no eixo X.

A implementação deverá utilizar Rigidbody 3D, preferencialmente com controle de velocidade horizontal.

A movimentação não poderá alterar a posição Z do personagem.

### 6.2. Pulo

O jogador poderá pular utilizando a barra de espaço.

Controle:

| Tecla | Ação |
|---|---|
| Espaço | Pular |

Requisitos:

- O personagem somente poderá pular quando estiver no chão.
- Não deverá existir pulo infinito.
- A força de pulo deverá ser configurável.
- A gravidade deverá funcionar normalmente.
- O personagem deverá conseguir aterrissar sobre as plataformas.
- Não deverá atravessar os blocos.
- O movimento vertical deverá ser controlado pela física da Unity.

A detecção de chão poderá utilizar Raycast ou SphereCast.

A verificação deverá considerar somente superfícies válidas para aterrissagem.

A implementação deverá evitar falsos positivos durante o contato lateral com as plataformas.

### 6.3. Coleta de moedas

Ao tocar em uma moeda:

- A moeda deverá desaparecer.
- O contador deverá aumentar.
- A interface deverá atualizar automaticamente.

A colisão deverá utilizar o sistema de física 3D da Unity.

### 6.4. Condição de vitória

A última plataforma deverá possuir uma área de chegada.

Ao alcançar essa área, o jogador deverá receber uma mensagem:

`FASE CONCLUÍDA!`

A mensagem deverá apresentar também a quantidade de moedas coletadas.

Não será necessário implementar múltiplas fases.

### 6.5. Condição de derrota

Caso o personagem caia para fora do cenário:

- O sistema deverá identificar que ele ultrapassou o limite inferior da fase.
- Uma mensagem de derrota deverá aparecer.
- Deverá existir uma possibilidade de reiniciar a fase.

---

## 7. Interface gráfica

A interface deverá ser simples, utilizando o sistema nativo de UI da Unity.

Elementos obrigatórios:

### Contador de moedas

Posicionado na parte superior da tela.

Exemplo:

`MOEDAS: 0`

O valor deverá ser atualizado automaticamente.

### Mensagem de vitória

Exibida quando o personagem alcançar a área final.

### Mensagem de derrota

Exibida quando o personagem cair do cenário.

### Botão de reinício

Disponibilizado após vitória ou derrota.

A interface deverá permanecer visível independentemente da posição da câmera.

---

## 8. Arquitetura e organização do projeto

O projeto deverá possuir uma estrutura organizada e modular.

Estrutura sugerida:

```text
UnityPlatformer/
│
├── Assets/
│   │
│   ├── Scenes/
│   │   └── MainScene.unity
│   │
│   ├── Scripts/
│   │   ├── Player/
│   │   │   ├── PlayerMovement.cs
│   │   │   └── GroundCheck.cs
│   │   │
│   │   ├── Collectibles/
│   │   │   └── Coin.cs
│   │   │
│   │   ├── Game/
│   │   │   ├── GameManager.cs
│   │   │   └── LevelManager.cs
│   │   │
│   │   └── UI/
│   │       └── GameUI.cs
│   │
│   ├── Materials/
│   │   ├── PlayerMaterial.mat
│   │   ├── DirtMaterial.mat
│   │   ├── GrassMaterial.mat
│   │   ├── CoinMaterial.mat
│   │   └── BackgroundMaterial.mat
│   │
│   ├── Prefabs/
│   │   ├── Player.prefab
│   │   ├── Platform.prefab
│   │   └── Coin.prefab
│   │
│   └── Editor/
│       └── ProjectSetup.cs
│
├── Packages/
│   └── manifest.json
│
├── ProjectSettings/
│
├── README.md
│
└── .gitignore
```

A estrutura poderá ser ajustada conforme as necessidades técnicas.

### 8.1. Responsabilidades dos scripts

**PlayerMovement.cs**

- Capturar comandos do jogador.
- Controlar movimentação horizontal.
- Executar o pulo.
- Respeitar a restrição de movimentação no eixo Z.

**GroundCheck.cs**

- Detectar contato com o chão.
- Verificar se o personagem pode pular.
- Evitar pulos consecutivos no ar.

**Coin.cs**

- Detectar colisão com o personagem.
- Comunicar a coleta ao GameManager.
- Evitar coletas duplicadas.
- Destruir a moeda coletada.

**GameManager.cs**

- Gerenciar o estado do jogo.
- Armazenar a quantidade de moedas.
- Controlar vitória e derrota.
- Coordenar o reinício.

**LevelManager.cs**

- Configurar a fase.
- Controlar limites do cenário.
- Gerenciar a área final.

**GameUI.cs**

- Atualizar o contador.
- Exibir mensagens.
- Controlar o botão de reinício.

**ProjectSetup.cs**

Responsável pela geração automatizada do projeto dentro da Unity.

Deverá:

- Criar os materiais.
- Criar os prefabs.
- Configurar o personagem.
- Configurar os componentes físicos.
- Gerar as 12 plataformas.
- Gerar as moedas.
- Configurar a câmera.
- Criar a interface.
- Criar a área de chegada.
- Salvar a cena principal.

---

## 9. Estratégia de desenvolvimento sem Unity instalada

A máquina de desenvolvimento não possui Unity.

Portanto, a LLM deverá gerar os arquivos necessários sem depender da execução do Editor.

A estratégia preferencial será utilizar scripts de Editor em C# para automatizar a construção do ambiente.

### 9.1. Geração automatizada

O arquivo `ProjectSetup.cs` deverá implementar um menu personalizado:

`Tools > Platformer > Generate Project`

Ao executar esse comando dentro da Unity, o sistema deverá:

1. Criar os materiais.
2. Criar os prefabs.
3. Criar o personagem.
4. Configurar Rigidbody e Collider.
5. Aplicar as restrições físicas.
6. Criar as plataformas.
7. Criar as moedas.
8. Posicionar a câmera.
9. Criar a interface.
10. Criar a área de chegada.
11. Configurar os scripts.
12. Salvar a cena principal.

A geração deverá ser idempotente, evitando duplicação de objetos e recursos.

A cena final deverá ser persistida em:

`Assets/Scenes/MainScene.unity`

Não depender exclusivamente de uma geração em memória que precise ser repetida toda vez que o projeto for aberto.

### 9.2. Arquivos de configuração

Criar os arquivos necessários:

- `ProjectVersion.txt`.
- `manifest.json`.
- Configurações de física.
- Configurações de entrada.
- Configurações de build.
- Configurações de plataforma.

Utilizar somente configurações compatíveis com Unity 2022.3.

### 9.3. Sistema de entrada

Priorizar o Input Manager tradicional.

Não utilizar o Input System como dependência obrigatória.

Mapear:

- A.
- D.
- Seta esquerda.
- Seta direita.
- Espaço.

---

## 10. Diretrizes de programação

A implementação deverá seguir boas práticas de desenvolvimento em C# e Unity.

Regras:

1. Utilizar nomes claros.
2. Separar responsabilidades.
3. Evitar scripts excessivamente grandes.
4. Utilizar componentes nativos.
5. Evitar dependências desnecessárias.
6. Implementar verificações de referências nulas.
7. Evitar operações desnecessárias em Update().
8. Utilizar FixedUpdate() para operações físicas quando apropriado.
9. Centralizar parâmetros configuráveis.
10. Documentar decisões técnicas relevantes.

O código deverá ser didático e compreensível.

Priorizar simplicidade em vez de arquiteturas complexas.

---

## 11. Etapas de implementação

A LLM deverá implementar o projeto de maneira incremental.

Não desenvolver todas as etapas de uma só vez.

### Etapa 1 — Planejamento e estrutura

- Analisar os requisitos.
- Definir a arquitetura.
- Criar diretórios.
- Criar arquivos de configuração.
- Criar `.gitignore`.
- Confirmar a versão-alvo.

Critério de conclusão: estrutura inicial pronta.

### Etapa 2 — Personagem e física

- Criar PlayerMovement.cs.
- Criar GroundCheck.cs.
- Configurar o cilindro.
- Adicionar Rigidbody.
- Adicionar Collider.
- Configurar Freeze Position Z.
- Bloquear rotações.
- Implementar movimento horizontal.
- Implementar pulo.

Critério de conclusão: personagem configurado para movimentação lateral 3D.

### Etapa 3 — Plataformas e cenário

- Criar materiais de terra e grama.
- Criar prefab de plataforma.
- Gerar exatamente 12 plataformas.
- Definir posições e alturas.
- Configurar colisores.
- Configurar câmera ortográfica.

Critério de conclusão: cenário 3D completo.

### Etapa 4 — Sistema de moedas

- Criar material amarelo.
- Criar prefab de moeda.
- Implementar Coin.cs.
- Gerar moedas.
- Configurar Trigger.
- Implementar coleta.
- Atualizar contador.

Critério de conclusão: sistema de coleta funcional em termos de implementação.

### Etapa 5 — Gerenciamento do jogo

- Implementar GameManager.cs.
- Implementar LevelManager.cs.
- Implementar vitória.
- Implementar derrota.
- Implementar reinício.

Critério de conclusão: fluxo principal do jogo implementado.

### Etapa 6 — Interface

- Criar GameUI.cs.
- Criar contador.
- Criar mensagens de vitória e derrota.
- Criar botão de reinício.
- Integrar interface ao GameManager.

Critério de conclusão: interface configurada.

### Etapa 7 — Automação

- Implementar ProjectSetup.cs.
- Criar menu personalizado.
- Automatizar materiais.
- Automatizar prefabs.
- Automatizar cena.
- Configurar referências.
- Garantir idempotência.

Critério de conclusão: cena principal gerada automaticamente pela Unity.

### Etapa 8 — Auditoria estática

Verificar:

- Sintaxe C#.
- Compatibilidade com Unity 2022.3.
- Referências entre scripts.
- Arquivos de configuração.
- Integridade do JSON.
- Componentes físicos.
- Restrições de movimento.
- Quantidade de plataformas.
- Quantidade de moedas.
- Configuração da câmera.
- Configuração de entrada.

A LLM deverá informar explicitamente que testes de execução e validação visual ainda dependem da máquina com Unity instalada.

Critério de conclusão: auditoria estática sem erros conhecidos.

### Etapa 9 — Documentação e entrega

- Finalizar README.
- Documentar os controles.
- Documentar abertura do projeto.
- Documentar geração da cena.
- Documentar execução.
- Documentar build Windows.
- Informar limitações de validação.

Critério de conclusão: projeto pronto para transferência.

---

## 12. Protocolo de trabalho da LLM

A LLM deverá seguir rigorosamente estas instruções.

### Desenvolvimento incremental

Ao concluir cada etapa:

1. Apresentar os arquivos criados.
2. Explicar suas responsabilidades.
3. Verificar consistência.
4. Informar critérios atendidos.
5. Informar pendências.
6. Aguardar autorização para avançar.

Não avançar automaticamente.

### Honestidade na validação

A LLM não poderá afirmar que o jogo foi testado quando não houve execução real na Unity.

Deverá diferenciar:

- Implementado.
- Verificado estaticamente.
- Compilado na Unity.
- Testado no Editor.
- Testado em Windows 10.

### Preservação do escopo

Não implementar recursos fora do escopo:

- Multiplayer.
- Inimigos.
- Combate.
- Inventário.
- Banco de dados.
- Salvamento de progresso.
- Múltiplas fases.
- Animações complexas.
- Sistemas avançados de áudio.
- Sistemas de partículas elaborados.

Qualquer alteração importante deverá ser justificada antes da implementação.

---

## 13. Critérios de aceitação

| Requisito | Resultado esperado |
|---|---|
| Engine | Unity 2022.3 LTS |
| Tipo | Jogo 3D |
| Gameplay | Plataforma lateral |
| Personagem | Cilindro azul 3D |
| Movimento | Eixo X |
| Pulo | Eixo Y |
| Profundidade | Eixo Z bloqueado |
| Física | Rigidbody 3D |
| Plataformas | Exatamente 12 cubos |
| Materiais | Terra marrom e grama verde |
| Grama | Aproximadamente 10% da altura |
| Moedas | Esferas amarelas 3D |
| Coleta | Desaparecem ao contato |
| Contador | Atualização automática |
| Vitória | Mensagem de conclusão |
| Derrota | Detecção de queda |
| Reinício | Funcional |
| Câmera | Ortográfica lateral |
| Cena | Gerada automaticamente |
| Plataforma | Windows 10 |

---

## 14. Instruções para a máquina de destino

A máquina de destino deverá possuir:

- Windows 10.
- Unity Hub.
- Unity Editor compatível com a versão do projeto.

### Abertura do projeto

1. Copiar a pasta completa `UnityPlatformer`.
2. Abrir Unity Hub.
3. Adicionar projeto existente.
4. Selecionar a versão correta do Editor.
5. Aguardar a importação.
6. Executar `Tools > Platformer > Generate Project`, caso a cena ainda não tenha sido gerada.
7. Abrir `Assets/Scenes/MainScene.unity`.
8. Pressionar Play.

### Geração do executável

1. Abrir as configurações de Build.
2. Selecionar Windows.
3. Adicionar MainScene à lista de cenas.
4. Configurar a arquitetura.
5. Gerar o executável.

O resultado deverá ser um jogo executável independentemente da abertura do Unity Editor.

---

## 15. Instrução inicial para a LLM

Você é responsável pela implementação integral deste projeto.

Sua responsabilidade é transformar este documento em um projeto Unity completo, organizado, funcional e compatível com a versão especificada.

Antes de escrever código:

1. Analise integralmente o README.
2. Verifique o ambiente CLI disponível.
3. Identifique os recursos disponíveis para geração e validação.
4. Confirme a versão exata da Unity.
5. Apresente a arquitetura técnica proposta.
6. Inicie exclusivamente a Etapa 1.

Não avance para as próximas etapas sem autorização.

O projeto deverá ser desenvolvido com foco em simplicidade, compatibilidade, organização e aprendizado.

**Requisito fundamental: o jogo é tridimensional em sua modelagem, geometria e física, mas possui movimentação de plataforma lateral restrita aos eixos X e Y. O eixo Z deverá permanecer bloqueado para o personagem.**

O resultado final deverá ser um projeto Unity completo, reproduzível e pronto para ser aberto em outra máquina Windows 10 com a versão compatível da Unity instalada.

---

## 16. Guia rápido de execução

### 16.1. Versão da Unity

O projeto está registrado em `ProjectSettings/ProjectVersion.txt` como:

```text
m_EditorVersion: 2022.3.22f1
```

Qualquer versão da linha Unity 2022.3 LTS pode ser usada. Caso a instalação local seja diferente, abra o projeto pelo Unity Hub e aceite a atualização de versão sugerida pelo Editor.

### 16.2. Controles

| Tecla | Ação |
|---|---|
| A ou Seta esquerda | Movimentar para a esquerda |
| D ou Seta direita | Movimentar para a direita |
| Espaço | Pular |

### 16.3. Abrir e gerar a cena

1. Copie a pasta completa do projeto.
2. Abra o Unity Hub e adicione o projeto existente.
3. Selecione o Editor Unity 2022.3 LTS.
4. Aguarde a importação dos scripts e a compilação.
5. No menu superior, execute `Tools > Platformer > Generate Project`.
6. Abra `Assets/Scenes/MainScene.unity`, caso ela não esteja aberta.
7. Pressione Play.

O comando de geração cria materiais, prefabs, personagem, 12 plataformas, 12 moedas, câmera, interface, área de chegada e a cena principal. Ele pode ser executado novamente sempre que necessário: os recursos gerados são recriados sem duplicação.

> Atenção: a geração cria uma nova cena vazia antes de montar o cenário. Salve alterações manuais antes de executar o comando.

### 16.4. Gerar o executável para Windows 10

1. Abra `File > Build Settings`.
2. Selecione a plataforma `Windows, Mac, Linux`.
3. Escolha `Windows` e a arquitetura desejada (x86_64).
4. Confirme que `Assets/Scenes/MainScene.unity` está marcada como cena habilitada. O comando de geração já registra essa cena na lista.
5. Clique em `Build` e escolha uma pasta de destino.

### 16.5. Estrutura implementada

```text
Assets/
├── Editor/
│   └── ProjectSetup.cs
├── Scripts/
│   ├── Camera/
│   │   └── CameraFollow.cs
│   ├── Collectibles/
│   │   └── Coin.cs
│   ├── Game/
│   │   ├── GameManager.cs
│   │   └── LevelManager.cs
│   ├── Player/
│   │   ├── GroundCheck.cs
│   │   └── PlayerMovement.cs
│   └── UI/
│       └── GameUI.cs
├── Scenes/
├── Materials/
└── Prefabs/
Packages/
└── manifest.json
ProjectSettings/
├── InputManager.asset
└── ProjectVersion.txt
```

### 16.6. Decisões técnicas

- O personagem usa `Rigidbody` 3D com `Freeze Position Z` e todas as rotações bloqueadas.
- A movimentação horizontal e o pulo são aplicados em `FixedUpdate`, por meio da velocidade do `Rigidbody`.
- A detecção de chão usa `Physics.SphereCast` com filtro por máscara de camadas e verificação da normal do contato.
- O jogador permanece na camada nativa `Ignore Raycast` (camada 2), o que impede que o próprio `SphereCast` detecte o colisor do personagem.
- As moedas e a área de chegada usam colliders do tipo Trigger.
- A câmera é ortográfica e faz um acompanhamento lateral simples e suavizado, mantendo o eixo Z fixo.
- Os scripts de runtime não usam o novo Input System; toda a entrada usa a API tradicional `UnityEngine.Input`.

### 16.7. Limitações de validação

A implementação foi criada e revisada estaticamente em ambiente sem Unity instalada. Portanto:

- Os scripts foram implementados.
- Os arquivos de configuração foram criados e verificados estaticamente.
- A compilação real, a execução no Editor e a validação visual ainda dependem de uma máquina com Unity 2022.3 LTS instalada.
- Testes em build Windows 10 ainda não foram executados.
