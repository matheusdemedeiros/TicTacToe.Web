# Architecture

Technical reference for the TicTacToe.Web application architecture, API contracts, and domain model.

---

## Table of Contents

- [Overview](#overview)
- [Backend Architecture](#backend-architecture)
- [Frontend Architecture](#frontend-architecture)
- [API Reference](#api-reference)
- [SignalR Hub Reference](#signalr-hub-reference)
- [Domain Model](#domain-model)
- [Game Flow Sequence](#game-flow-sequence)

---

## Overview

```
+-----------+        REST API         +-------------+       EF Core       +-----------+
| Angular   | ---------------------> | ASP.NET Core| -----------------> | SQL Server|
| Frontend  | <----- SignalR -------> | Backend     | <----------------- |           |
+-----------+        WebSocket        +-------------+                     +-----------+
     :4200                                 :5200                             :1433
```

---

## Backend Architecture

The backend follows **Clean Architecture** with strict dependency rules:

```
                  +------------------+
                  |     WebAPI       |  Controllers, Hubs, Extensions
                  +--------+---------+
                           |
                  +--------v---------+
                  |   Application    |  Use Cases (Commands, Queries, Handlers)
                  +--------+---------+
                           |
            +--------------v--------------+
            |          Domain             |  Entities, Enums, Interfaces
            +-----------------------------+
                           ^
                  +--------+---------+
                  |   Infra.Data     |  EF Core, Repositories, Migrations
                  +------------------+
```

### Layer Responsibilities

| Layer | Project | Responsibility |
| --- | --- | --- |
| **Domain** | `TicTacToe.Domain` | Entities, enums, repository interfaces, `DomainException` |
| **Application** | `TicTacToe.Application` | Use Cases via MediatR (CQRS): Command/Query + Handler + Response |
| **Infrastructure** | `TicTacToe.Infra.Data` | EF Core `DbContext`, repository implementations, migrations |
| **Presentation** | `TicTacToe.WebAPI` | REST Controllers, SignalR Hub, DI configuration, CORS |
| **Tests** | `TicTacToe.Domain.UnitTests` | Unit tests for domain entities (xUnit + FluentAssertions) |

### Dependency Rule

Inner layers **never** depend on outer layers:
- Domain depends on nothing
- Application depends only on Domain
- Infra.Data depends on Domain (implements interfaces)
- WebAPI depends on Application and registers Infra.Data

### Key Patterns

| Pattern | Implementation |
| --- | --- |
| CQRS | MediatR `IRequest<T>` / `IRequestHandler<T,R>` |
| Repository | Interfaces in Domain, implementations in Infra.Data |
| Unit of Work | `IUnitOfWork` wrapping `DbContext.SaveChangesAsync` |
| Domain Exceptions | `DomainException` for business rule violations |

---

## Frontend Architecture

Angular 19 application with standalone components and feature-based structure:

```
src/app/
  components/
    home/                         # Lobby: create player, create/join match
      shared/
        models/                   # Interfaces and enums
        services/                 # MatchService, PlayerService (HTTP)
    match/                        # Gameplay
      shared/
        models/                   # TicMatch, TicBoardCell interfaces
        services/                 # TicMatchHubService (SignalR)
      tic-match/                  # Match orchestrator component
      tic-board/                  # 3x3 board renderer
      tic-board-cell/             # Individual cell component
  core/                           # Cross-cutting (NotificationService)
```

### Key Patterns

| Pattern | Implementation |
| --- | --- |
| Standalone Components | No `NgModules`, components declare their own imports |
| Dependency Injection | `inject()` function (not constructor injection) |
| Reactive Streams | SignalR events wrapped in RxJS `Observable` |
| HTTP Communication | Angular `HttpClient` for REST endpoints |
| Real-time Communication | `@microsoft/signalr` `HubConnection` |

---

## Error Handling

### Backend Flow

```
Exception thrown in Handler/Domain
        |
        v
GlobalExceptionHandlerMiddleware
        |
        +--> DomainException  --> 400 { message, errorCode: ""DOMAIN_ERROR"", statusCode: 400 }
        +--> FormatException  --> 400 { message, errorCode: ""VALIDATION_ERROR"", statusCode: 400 }
        +--> Exception        --> 500 { message, errorCode: ""INTERNAL_ERROR"", statusCode: 500 }
```

### Frontend Flow

```
HTTP Error Response from Backend
        |
        v
httpErrorInterceptor (global)
        |
        +--> Extracts ApiErrorResponse.message
        +--> Shows toastr notification (red toast)
        +--> Re-throws error for component-level handling if needed
```

### Error Response Format

```json
{
  ""message"": ""Player with this nickname already exists."",
  ""errorCode"": ""DOMAIN_ERROR"",
  ""statusCode"": 400
}
```

### SignalR Error Handling

Hub method errors are caught via `.catch()` in `TicMatchHubService` and displayed as toastr notifications.
Connection drops trigger automatic reconnection with user feedback.

---
## API Reference

### REST Endpoints

#### `POST /api/Player`

Get-or-create a player. Returns existing player if nickname already exists.

```json
// Request
{ "name": "John", "nickName": "john123" }

// Response 201 Created
{ "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
```

#### `POST /api/Match`

Create a new match, optionally adding an initial player.

```json
// Request
{ "playMode": 0, "initialPlayerId": "3fa85f64-..."  }

// Response 201 Created
{ "matchId": "..., "ticPlayerWithXSymbolId": "...", "ticPlayerWithOSymbolId": "..." }
```

`playMode`: `0` = PlayerVsPlayer, `1` = PlayerVsComputer

#### `POST /api/Match/{matchId}/add-player`

Add a player to an existing match.

```json
// Request
{ "playerId": "...", "matchId": "..." }

// Response 200 OK
{
  "matchId": "...",
  "playerId": "...",
  "playerName": "John",
  "nickname": "john123",
  "ticPlayerWithXSymbolId": "...",
  "ticPlayerWithOSymbolId": "..."
}
```

#### `GET /api/Match/{matchId}`

Retrieve match information.

```json
// Response 200 OK
{
  "found": true,
  "ticMatchState": 1,
  "playerNumbers": 2,
  "matchId": "..."
}
```

---

## SignalR Hub Reference

**Hub URL:** `/Ticmatchhub`

### Client to Server (invoke)

#### `JoinMatchAsync`

Player joins the match group to receive real-time updates.

```json
{ "matchId": "3fa85f64-..." }
```

When both players have joined, the match starts automatically.

#### `MakePlayerMoveAsync`

Player makes a move on the board.

```json
{ "matchId": "...", "playerId": "...", "cellRow": 0, "cellCol": 1 }
```

### Server to Client (on)

#### `TicPlayerJoined`

Fired when a player joins the match.

```json
{
  "matchId": "...",
  "board": [[{"symbol": null, "state": 0}, ...], ...],
  "state": 1,
  "currentPlayerId": "...",
  "currentPlayerSymbol": "X",
  "ticPlayerWithXSymbolId": "...",
  "ticPlayerWithOSymbolId": "..."
}
```

#### `TicPlayerMadeMove`

Fired after each move.

```json
{
  "matchId": "...",
  "board": [[{"symbol": "X", "state": 1}, ...], ...],
  "state": 1,
  "currentPlayerId": "...",
  "currentPlayerSymbol": "O"
}
```

---

## Domain Model

```
TicMatch
  |-- Players: List<TicPlayer>
  |-- Board: TicBoard
  |     |-- Board: TicBoardCell[3][3]
  |     |-- WinningSimbol: string?
  |     |-- SerializedBoard: string (JSON in DB)
  |-- State: TicMatchState (NOT_STARTED | IN_PROGRESS | FINISHED)
  |-- TicScore
  |     |-- WinningPlayer: TicPlayer?
  |     |-- WinningSymbol: string
  |     |-- Tie: bool
  |-- CurrentPlayer: TicPlayer?
  |-- PlayMode: PlayModeType (PlayerVsPlayer | PlayerVsComputer)

TicPlayer
  |-- Name: string
  |-- NickName: string
  |-- Symbol: string? (X or O, assigned when joining a match)

TicBoardCell
  |-- Symbol: string
  |-- State: TicBoardCellState (BLANK | MARKED)
```

### Shared Enums

| Enum | Values |
| --- | --- |
| `TicMatchState` | `NOT_STARTED = 0`, `IN_PROGRESS = 1`, `FINISHED = 2` |
| `PlayModeType` | `PlayerVsPlayer = 0`, `PlayerVsComputer = 1` |
| `TicBoardCellState` | `BLANK = 0`, `MARKED = 1` |

---

## Game Flow Sequence

```
Player A (Browser)          Frontend            Backend             Player B (Browser)
     |                         |                   |                        |
     |-- Create Player ------->|-- POST /Player -->|                        |
     |<-- { id: A } -----------|<-- 201 ---------- |                        |
     |                         |                   |                        |
     |-- Create Match -------->|-- POST /Match --->|                        |
     |<-- { matchId } ---------|<-- 201 ---------- |                        |
     |                         |                   |                        |
     |                         |                   |    Create Player ------|
     |                         |                   |<-- POST /Player -------|
     |                         |                   |--- 201 { id: B } ---->|
     |                         |                   |                        |
     |                         |                   |    Add Player to Match |
     |                         |                   |<-- POST /add-player --|
     |                         |                   |--- 200 -------------->|
     |                         |                   |                        |
     |-- Connect SignalR ----->|== WebSocket =====>|<== WebSocket ========= |
     |-- JoinMatchAsync ------>|----------------->|                        |
     |                         |                   |                        |
     |                         |                   |<--- JoinMatchAsync ----|
     |                         |                   |--- StartMatch() ----->|
     |                         |                   |                        |
     |<-- TicPlayerJoined -----|<-- broadcast -----|---- TicPlayerJoined -->|
     |                         |                   |                        |
     |-- MakePlayerMoveAsync ->|----------------->|                        |
     |<-- TicPlayerMadeMove ---|<-- broadcast -----|---- TicPlayerMadeMove >|
     |                         |                   |                        |
     |                         |   (turns alternate until win or tie)       |
```
