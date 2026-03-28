# TicTacToe.Web — Copilot Instructions

## Propósito do projeto

API back-end em tempo real para um jogo de **Jogo da Velha (Tic-Tac-Toe)** multiplayer.
O front-end (em outro local neste repositório) se comunica via **REST API** e **SignalR**.

## Stack tecnológica

- **.NET 9** (ASP.NET Core Web API)
- **Entity Framework Core** com SQL Server (migrations code-first)
- **MediatR** para padrão CQRS (Commands, Queries, Handlers)
- **SignalR** para comunicação em tempo real entre jogadores
- **Newtonsoft.Json** para serialização (inclusive no SignalR)
- **Scalar** para documentação da API (substitui Swagger UI)
- **xUnit** + **FluentAssertions** para testes unitários

## Arquitetura

O projeto segue **Clean Architecture** com 4 camadas:

```
TicTacToe.WebAPI          → Presentation (Controllers, Hubs, Config)
TicTacToe.Application     → Use Cases (Commands, Queries, Handlers via MediatR)
TicTacToe.Domain          → Entidades, Enums, Interfaces de repositório, Exceções
TicTacToe.Infra.Data      → EF Core (DbContext, Repositories, Migrations, Configurations)
TicTacToe.Domain.UnitTests → Testes unitários da camada de domínio
```

**Regra de dependência:** as camadas internas (Domain) não conhecem as externas. Application depende apenas de Domain. Infra.Data e WebAPI dependem de Application e Domain.

## Entidades de domínio

- **TicMatch** — Representa uma partida. Contém jogadores, tabuleiro, estado e placar.
- **TicPlayer** — Jogador com nome, apelido e símbolo (X ou O).
- **TicBoard** — Tabuleiro 3×3. Armazenado serializado como JSON no banco via `SerializedBoard`.
- **TicBoardCell** — Célula individual do tabuleiro.
- **TicScore** — Placar da partida (vencedor ou empate).
- **TicMatchState** — Enum: `NOT_STARTED`, `IN_PROGRESS`, `FINISHED`.
- **PlayModeType** — Enum: `PlayerVsPlayer`, `PlayerVsComputer`.

## Fluxo de jogo

1. Criar jogadores via `POST /api/Player`
2. Criar uma partida via `POST /api/Match`
3. Adicionar jogadores à partida via `POST /api/Match/{matchId}/add-player`
4. Jogadores se conectam ao Hub SignalR (`/Ticmatchhub`) e entram no grupo da partida via `JoinMatchAsync`
5. Jogadas são feitas em tempo real via `MakePlayerMoveAsync` no Hub
6. O Hub notifica todos os jogadores do grupo sobre cada jogada

## Endpoints REST

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/Player` | Cria um novo jogador |
| POST | `/api/Match` | Cria uma nova partida |
| POST | `/api/Match/{matchId}/add-player` | Adiciona jogador a uma partida |
| GET | `/api/Match/{matchId}` | Busca partida por ID |

## Métodos SignalR (Hub: `/Ticmatchhub`)

| Método | Direção | Descrição |
|--------|---------|-----------|
| `JoinMatchAsync` | Client → Server | Jogador entra na partida e no grupo SignalR |
| `MakePlayerMoveAsync` | Client → Server | Jogador faz uma jogada |
| `TicPlayerJoined` | Server → Client | Notifica que um jogador entrou |
| `TicPlayerMadeMove` | Server → Client | Notifica jogada realizada |

## Convenções de código

- Idioma do código: **inglês** (nomes de classes, métodos, propriedades)
- Idioma de comunicação com o desenvolvedor: **português brasileiro**
- Entidades herdam de `BaseEntity` (Id, CreatedAt, UpdatedAt)
- Propriedades de domínio usam `private set` para proteger invariantes
- Exceções de domínio usam `DomainException`
- Use Cases seguem o padrão: `Command/Query` + `Handler` + `Response`
- Nomenclatura dos Use Cases: pasta com nome da ação (ex: `CreateMatch`, `MakeMove`)
- Repositórios são interfaces no Domain, implementados no Infra.Data
- Unit of Work pattern via `IUnitOfWork`
- Testes unitários ficam em `TicTacToe.Domain.UnitTests` organizados por módulo
