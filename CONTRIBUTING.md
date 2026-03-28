# Contribuição

Guia de contribuição para o projeto **TicTacToe.Web**.

---

## Padrão de Commits

Este projeto segue o [Conventional Commits](https://www.conventionalcommits.org/pt-br/).

### Formato

```
<tipo>(<escopo>): <descrição>

[corpo opcional]

[rodapé opcional]
```

- **Descrição**: sempre em português, iniciando com verbo no **presente do indicativo** e sem letra maiúscula no início.
- **Escopo**: opcional, mas recomendado quando a mudança é específica de uma camada.
- **Corpo**: explica o "porquê" quando a descrição não for suficiente (wrap a 72 caracteres).
- **Rodapé**: referencia issues quando aplicável (`Closes #42`).

### Tipos

| Tipo | Quando usar |
|------|------------|
| `feat` | Nova funcionalidade ou comportamento |
| `fix` | Correção de bug |
| `refactor` | Refatoração sem alterar comportamento externo |
| `docs` | Apenas documentação |
| `chore` | Configuração, tooling, infra, dependências |
| `test` | Adição ou correção de testes |
| `style` | Formatação, espaçamento, sem mudança de lógica |
| `perf` | Melhoria de performance |
| `ci` | Pipelines de CI/CD |

### Escopos

#### Backend

| Escopo | Camada / Pasta |
|--------|---------------|
| `domain` | Entidades, enums, value objects, `DomainException` |
| `application` | Use Cases, Handlers, Commands, Responses |
| `infra` | Repositórios, DbContext, Migrations, EF configs |
| `api` | Controllers, Hubs, Middlewares, `Program.cs` |

#### Frontend

| Escopo | Camada / Pasta |
|--------|---------------|
| `ui` | Componentes visuais, templates HTML, SCSS |
| `match` | Lógica de partida (tic-match, tic-board, hub service) |
| `home` | Lobby, register flow, criação de jogador/partida |
| `core` | Services cross-cutting (notification, interceptor, session) |

#### Infraestrutura

| Escopo | Camada / Pasta |
|--------|---------------|
| `docker` | Dockerfile, docker-compose, nginx |
| `deps` | Pacotes NuGet ou npm |
| `ci` | GitHub Actions, workflows |

### Regra do presente do indicativo

As mensagens de commit descrevem **o que o commit faz**, não o que foi feito. Use sempre o verbo no **presente do indicativo**.

#### ✅ Exemplos válidos

```
feat(domain): adiciona validação de empate no tabuleiro
fix(api): corrige middleware para tratar FormatException
feat(match): exibe tela de game over ao finalizar partida
refactor(application): unifica response com TicMatchStateResponse
chore(docker): configura nginx como proxy reverso do backend
test(domain): cobre cenário de vitória na diagonal secundária
docs: atualiza arquitetura com novo contrato do SignalR
feat(core): persiste sessão do jogo no localStorage
```

#### ❌ Exemplos inválidos

```
feat(domain): adicionada validação de empate no tabuleiro       # ← particípio (adicionada)
fix(api): corrigiu middleware para tratar FormatException        # ← pretérito (corrigiu)
feat(match): exibindo tela de game over ao finalizar partida    # ← gerúndio (exibindo)
refactor: Unifica response com TicMatchStateResponse            # ← letra maiúscula no início
chore(docker): configurei nginx como proxy reverso do backend   # ← primeira pessoa (configurei)
test(domain): foram adicionados testes de vitória diagonal      # ← voz passiva (foram adicionados)
docs: atualizando arquitetura com novo contrato do SignalR      # ← gerúndio (atualizando)
feat(core): Adicionou persistência de sessão no localStorage    # ← pretérito + maiúscula (Adicionou)
```

### Breaking Changes

Quando um commit introduz uma mudança que quebra compatibilidade, adicionar `!` após o tipo:

```
feat(api)!: altera contrato do SignalR para response unificada
```

---

## Regras de Commits

1. Cada commit deve ser **atômico** — uma mudança lógica por commit.
2. Não misturar mudanças de backend e frontend no mesmo commit (quando possível).
3. Commits de documentação (`docs`) devem ser separados de commits de código.
4. Commits de testes (`test`) podem acompanhar o commit de código se forem do mesmo escopo.

---

## Branches

### Nomenclatura

| Prefixo | Quando usar | Exemplo |
|---------|------------|---------|
| `feature/` | Nova funcionalidade | `feature/add-game-over-screen` |
| `fix/` | Correção de bug | `fix/switch-player-order` |
| `chore/` | Infraestrutura, config | `chore/setup-ci-pipeline` |
| `refactor/` | Refatoração | `refactor/unify-signalr-response` |

### Fluxo

1. Criar branch a partir de `master`
2. Desenvolver com commits seguindo o padrão acima
3. Abrir Pull Request para `master`

---

## Boas Práticas

- Rodar `dotnet test` antes de commitar mudanças no backend.
- Rodar `ng build` antes de commitar mudanças no frontend.
- Manter a documentação (`docs/`, `.instructions.md`) atualizada junto com mudanças de contrato.
