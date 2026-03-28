# TicTacToe.Web - Copilot Instructions

## Project Purpose

Multiplayer real-time **Tic-Tac-Toe (Jogo da Velha)** web application.
Players interact through an **Angular frontend** that communicates with an **ASP.NET Core backend** via REST API and SignalR.

## Developer Communication Language: Brazilian Portuguese (pt-BR)

## Repository Structure

```
TicTacToe.Web/                              (Git root)
+-- .github/
|   +-- copilot-instructions.md             (this file - global instructions)
+-- docker/
+-- src/
    +-- client/
    |   +-- TicTacToe.Web.Client/           (Angular 19 frontend)
    +-- server/
        +-- TicTacToe.WebAPI/               (Presentation: Controllers, Hubs)
        +-- TicTacToe.Application/          (Use Cases: Commands, Queries, Handlers)
        +-- TicTacToe.Domain/               (Entities, Enums, Repository Interfaces)
        +-- TicTacToe.Infra,Data/           (EF Core: DbContext, Repositories, Migrations)
        +-- TicTacToe.Domain.UnitTests/     (xUnit + FluentAssertions)
```

## Tech Stack

### Backend (.NET 9)
- ASP.NET Core Web API
- Entity Framework Core with SQL Server (code-first migrations)
- MediatR for CQRS pattern (Commands, Queries, Handlers)
- SignalR for real-time player communication
- Newtonsoft.Json for serialization (including SignalR)
- Scalar for API documentation

### Frontend (Angular 19)
- Angular standalone components
- @microsoft/signalr client for real-time communication
- RxJS for reactive patterns
- SCSS for styling

## Architecture

### Backend - Clean Architecture
Dependency rule: inner layers do not know about outer layers.
- **Domain**: Entities, Enums, Repository Interfaces, DomainException
- **Application**: Use Cases (Command/Query + Handler + Response) via MediatR
- **Infra.Data**: EF Core DbContext, Repositories, Configurations, Migrations
- **WebAPI**: Controllers (REST), Hubs (SignalR), Extensions

### Frontend - Component-based
- **components/home/**: Lobby/home page, match creation, player creation
- **components/match/**: Game board, cells, match logic
- **core/**: Cross-cutting services (notifications)
- Services use Angular HttpClient for REST and @microsoft/signalr for WebSocket

## Game Flow (Frontend <-> Backend)

1. `POST /api/Player` - Create players (frontend: PlayerService.create)
2. `POST /api/Match` - Create a match (frontend: MatchService.create)
3. `POST /api/Match/{matchId}/add-player` - Add player to match (frontend: MatchService.addPlayer)
4. `GET /api/Match/{matchId}` - Retrieve match info (frontend: MatchService.retrieveById)
5. SignalR `JoinMatchAsync` - Player joins match group (frontend: TicMatchHubService.joinMatch)
6. SignalR `MakePlayerMoveAsync` - Player makes a move (frontend: TicMatchHubService.makePlayerMove)
7. SignalR `TicPlayerJoined` event - Server notifies all players (frontend: TicMatchHubService.onPlayerJoined)
8. SignalR `TicPlayerMadeMove` event - Server notifies move result (frontend: TicMatchHubService.onPlayerMadeMove)

## API Contracts (Backend <-> Frontend)

### REST Endpoints

| Method | Route | Request | Response |
|--------|-------|---------|----------|
| POST | /api/Player | { name, nickName } | { id } |
| POST | /api/Match | { playMode, initialPlayerId } | { matchId, ticPlayerWithXSymbolId, ticPlayerWithOSymbolId } |
| POST | /api/Match/{id}/add-player | { playerId, matchId } | { matchId, playerId, playerName, nickname, ticPlayerWithXSymbolId, ticPlayerWithOSymbolId } |
| GET | /api/Match/{id} | - | { found, ticMatchState, playerNumbers, matchId } |

### SignalR Hub: /Ticmatchhub

| Method | Direction | Payload |
|--------|-----------|---------|
| JoinMatchAsync | Client->Server | { matchId } |
| MakePlayerMoveAsync | Client->Server | { matchId, playerId, cellRow, cellCol } |
| TicPlayerJoined | Server->Client | { matchId, board[][], state, currentPlayerId, currentPlayerSymbol, ticPlayerWithXSymbolId, ticPlayerWithOSymbolId } |
| TicPlayerMadeMove | Server->Client | { matchId, board[][], state, currentPlayerId, currentPlayerSymbol } |

### Shared Enums

- TicMatchState: NOT_STARTED=0, IN_PROGRESS=1, FINISHED=2
- PlayModeType: PlayerVsPlayer=0, PlayerVsComputer=1
- TicBoardCellState: BLANK=0, MARKED=1

## Domain Entities (Backend)

- **TicMatch**: Match with Players, Board, State, Score, CurrentPlayer, PlayMode
- **TicPlayer**: Player with Name, NickName, Symbol (X or O)
- **TicBoard**: 3x3 board, serialized as JSON in DB via SerializedBoard
- **TicBoardCell**: Individual cell with Symbol and State
- **TicScore**: Score (WinningPlayer, WinningSymbol, Tie)

## Error Handling

### Backend - Global Exception Middleware
All exceptions are caught by `GlobalExceptionHandlerMiddleware` and returned as structured JSON:

```json
{ "message": "Player with this nickname already exists.", "errorCode": "DOMAIN_ERROR", "statusCode": 400 }
```

| Exception Type | HTTP Status | errorCode |
|----------------|-------------|-----------|
| `DomainException` | 400 Bad Request | `DOMAIN_ERROR` |
| `FormatException` | 400 Bad Request | `VALIDATION_ERROR` |
| `Exception` (unhandled) | 500 Internal Server Error | `INTERNAL_ERROR` |

- Handlers MUST throw `DomainException` for business rule violations (never return null or throw generic Exception)
- Controllers do NOT handle errors manually — the middleware handles everything
- Response model: `ApiErrorResponse` (message, errorCode, statusCode)

### Frontend - HTTP Error Interceptor
- `httpErrorInterceptor` (functional interceptor) catches all HTTP errors globally
- Extracts `message` from the backend `ApiErrorResponse` and shows it via `NotificationService` (ngx-toastr)
- No need for `error:` callbacks in `subscribe()` for API calls — the interceptor handles display automatically
- SignalR errors are caught in `TicMatchHubService` via `.catch()` on `invoke()` calls

### Frontend - Notification Service
`NotificationService` provides: `showSuccess()`, `showError()`, `showWarning()`, `showInfo()`

## Code Conventions

### Backend (.NET)
- Code language: English (class names, methods, properties)
- Entities inherit from BaseEntity (Id, CreatedAt, UpdatedAt)
- Domain properties use `private set` to protect invariants
- Domain exceptions use DomainException
- Use Cases follow: Command/Query + Handler + Response pattern
- Use Case folders named after the action (e.g., CreateMatch, MakeMove)
- Repository interfaces in Domain, implementations in Infra.Data
- Unit of Work pattern via IUnitOfWork

### Frontend (Angular)
- Code language: English
- Standalone components (no NgModules)
- Services use `inject()` function pattern
- Models defined as TypeScript interfaces
- Hub service wraps SignalR in RxJS Observables
- Feature-based folder structure under components/
