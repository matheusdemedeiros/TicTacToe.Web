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
| POST | /api/Player | { name, nickName } | { id } (get-or-create: returns existing player if nickname matches) |
| POST | /api/Match | { playMode, initialPlayerId } | { matchId, shortCode, ticPlayerWithXSymbolId, ticPlayerWithOSymbolId } |
| POST | /api/Match/{id}/add-player | { playerId, matchId } | { matchId, playerId, playerName, nickname, ticPlayerWithXSymbolId, ticPlayerWithOSymbolId } |
| GET | /api/Match/code/{code} | - | { matchId, shortCode } |
| GET | /api/Match/{id} | - | { found, ticMatchState, playerNumbers, matchId } |

### SignalR Hub: /Ticmatchhub

| Method | Direction | Payload |
|--------|-----------|---------|
| JoinMatchAsync | Client->Server | { matchId } |
| MakePlayerMoveAsync | Client->Server | { matchId, playerId, cellRow, cellCol } |
| TicPlayerJoined | Server->Client | TicMatchStateResponse (unified, see below) |
| TicPlayerMadeMove | Server->Client | TicMatchStateResponse (unified, see below) |

### Shared Enums

- TicMatchState: NOT_STARTED=0, IN_PROGRESS=1, FINISHED=2
- PlayModeType: PlayerVsPlayer=0, PlayerVsComputer=1
- TicBoardCellState: BLANK=0, MARKED=1

## Domain Entities (Backend)

- **TicMatch**: Match with Players, Board, State, Score, CurrentPlayer, PlayMode, ShortCode (6-char unique invite code)
- **TicPlayer**: Player with Name, NickName, Symbol (X or O)
- **TicBoard**: 3x3 board, serialized as JSON in DB via SerializedBoard
- **TicBoardCell**: Individual cell with Symbol and State
- **TicScore**: Score (WinningPlayer, WinningSymbol, Tie)

### TicMatchStateResponse (Unified Game State)

Both SignalR events (TicPlayerJoined, TicPlayerMadeMove) return the same complete state:
- matchId, board[][], state, currentPlayerId, currentPlayerSymbol
- ticPlayerWithXSymbolId, ticPlayerWithOSymbolId
- isFinished, isTie, winnerSymbol, winnerPlayerId

Built via `TicMatchStateResponse.FromMatch(match)` factory in `Application/UseCases/Match/Shared/`.

### Frontend Routes and Guards

| Route | Component | Guard | Purpose |
|-------|-----------|-------|---------|
| `/` | LoginComponent | - | Nickname entry (session-based identity) |
| `/lobby` | LobbyComponent | playerSessionGuard | Create or join match via ShortCode |
| `/join?code=X` | JoinComponent | - | Direct invite link (resolves code, adds player, redirects) |
| `/ticmatch` | TicMatchComponent | playerSessionGuard + gameSessionGuard | Gameplay screen |

### Player and Game Sessions

- `PlayerSessionService` persists playerId + nickName in localStorage (identity)
- `GameSessionService` persists matchId + playerId in localStorage (active match)
- `playerSessionGuard` redirects to `/` if no player session
- `gameSessionGuard` redirects to `/lobby` if no game session or query params
- On match screen load, falls back to localStorage if query params are missing (enables reconnection)
- Game session is cleared when the match finishes or is abandoned

### Match ShortCode (Invite System)

- Each match gets a unique 6-character alphanumeric code (e.g., `X7K2M9`)
- Generated with `RandomNumberGenerator` (crypto-safe), chars: `ABCDEFGHJKLMNPQRSTUVWXYZ23456789` (no ambiguous 0/O, 1/I/L)
- Indexed as unique in database
- Used for: lobby join field, invite links (`/join?code=X`), clipboard copy, Web Share API
- Endpoint: `GET /api/Match/code/{code}` resolves ShortCode to MatchId

### SignalR Hub Lifecycle

- `TicMatchHubService` is a singleton (`providedIn: 'root'`)
- Uses `BehaviorSubject<boolean>` for connection state (replays last value to new subscribers)
- `clearHandlers()` removes all `on()` listeners — called in `ngOnInit` and `ngOnDestroy`  
- Component subscribes with `filter(true), take(1)` to avoid duplicate handler registration
- All subscriptions tracked in array and unsubscribed in `ngOnDestroy`

- `GameSessionService` persists matchId + playerId in localStorage
- On match screen load, falls back to localStorage if query params are missing (enables reconnection)
- Session is cleared when the game finishes

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

## Environments and Deployment

The project has 3 execution environments with distinct configurations, build processes, and deploy targets.

### Environment: Development (local Visual Studio)

- **ASPNETCORE_ENVIRONMENT**: `Development`
- **Backend**: `dotnet run` or F5 in Visual Studio (`https://localhost:7199`)
- **Frontend**: `ng serve` (`http://localhost:4200`)
- **Database**: SQL Server in Docker (`localhost:1433`) or local instance
- **Config files**: `appsettings.json` + `appsettings.Development.json`
- **Frontend env**: `environment.development.ts` (apiUrl: `https://host.docker.internal:7199/`)
- **Migrations**: `cd src/server && dotnet ef database update --project "TicTacToe.Infra,Data" --startup-project "TicTacToe.WebAPI"`
- **CORS origins**: read from `appsettings.json` → `Cors:AllowedOrigins` → `["http://localhost:4200"]`

### Environment: Docker (local containers)

- **ASPNETCORE_ENVIRONMENT**: `Docker`
- **Orchestration**: `cd docker && docker compose up -d`
- **Build**: `docker compose build` (multi-stage Dockerfiles)
- **Backend**: container `tictactoe-backend` on port 5200 (internal 8080)
- **Frontend**: container `tictactoe-frontend` on port 4200 (Nginx on internal 80)
- **Database**: container `sql-server` on port 1433
- **Config files**: `appsettings.json` + `appsettings.Docker.json`
- **Frontend env**: `environment.docker.ts` (apiUrl: `/` — proxied by Nginx)
- **Migrations**: `cd src/server && dotnet ef database update --project "TicTacToe.Infra,Data" --startup-project "TicTacToe.WebAPI"` with connection string pointing to `localhost:1433`
- **CORS origins**: read from `appsettings.Docker.json` → `Cors:AllowedOrigins`

### Environment: Production (Azure)

- **ASPNETCORE_ENVIRONMENT**: `Production`
- **Backend host**: Azure App Service (Free F1) — `https://api-tictactoe-bvhncffjapd9f9az.canadacentral-01.azurewebsites.net`
- **Frontend host**: Azure Static Web Apps — `https://blue-rock-0559b9d0f.2.azurestaticapps.net`
- **Database**: Azure SQL Database
- **Config files**: `appsettings.json` + `appsettings.production.json` + Azure Configuration (connection strings override via env vars)
- **Frontend env**: `environment.ts` (apiUrl: backend Azure URL)
- **CORS origins**: read from `appsettings.production.json` → `Cors:AllowedOrigins` (can also be overridden via Azure env vars `Cors__AllowedOrigins__0`, etc.)

#### Production Build and Deploy Commands

**Backend:**
```
cd src/server
dotnet publish TicTacToe.WebAPI -c Release -o ../../publish/api
Compress-Archive -Path publish/api/* -DestinationPath publish/api.zip -Force
```
Deploy via Kudu: App Service → Advanced Tools → Zip Push Deploy

**Frontend:**
```
docker build --target export --output publish/frontend -f publish/Dockerfile.prod-frontend src/client/TicTacToe.Web.Client/
```
Deploy via SWA CLI (run inside node:22 Docker container due to local Node version incompatibility):
```
docker run --rm -v "publish/frontend:/app" node:22 bash -c "npm install -g @azure/static-web-apps-cli 2>/dev/null && swa deploy /app --deployment-token 'TOKEN' --env production"
```

**Database migrations (generate SQL script):**
```
cd src/server
dotnet ef migrations script --idempotent --project "TicTacToe.Infra,Data" --startup-project "TicTacToe.WebAPI" --output "../../docs/database-schema.sql"
```

### CORS Configuration

CORS origins are NOT hardcoded. They are read from `appsettings` via `configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()`.
Each environment file (`appsettings.json`, `appsettings.Docker.json`, `appsettings.production.json`) has its own list.
In Azure, origins can be overridden via Application Settings: `Cors__AllowedOrigins__0`, `Cors__AllowedOrigins__1`, etc.

### Login returnUrl Flow

When a user opens an invite link (`/join?code=X`) without being logged in:
1. `JoinComponent` redirects to `/?returnUrl=/join?code=X`
2. `LoginComponent` reads `returnUrl` from query params
3. After successful login, navigates to `returnUrl` instead of `/lobby`
This ensures Player B goes directly to the match after entering their nickname.