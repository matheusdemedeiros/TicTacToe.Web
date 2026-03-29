# TicTacToe.Web.Client

Angular 19 frontend for the TicTacToe multiplayer game.

## Prerequisites

- Node.js 22.x LTS
- Angular CLI 19.x (`npm install -g @angular/cli`)

## Setup

```bash
npm install --legacy-peer-deps
```

## Development Server

```bash
ng serve
```

Open http://localhost:4200. The app reloads automatically on file changes.

## Build

```bash
# Development
ng build --configuration=development

# Docker (used by Dockerfile)
ng build --configuration=docker
```

## Tests

```bash
ng test
```

## Environment Files

| File | Used When | API URL |
| --- | --- | --- |
| `environment.development.ts` | `ng serve` | `https://host.docker.internal:7199/` |
| `environment.docker.ts` | Docker Compose | `/` (proxied by Nginx) |
| `environment.ts` | Production build | Configure as needed |

## Project Structure

```
src/app/
  components/
    home/                   # Lobby: player creation, match creation/join
      shared/
        models/             # TypeScript interfaces and enums
        services/           # MatchService, PlayerService (HTTP)
    match/                  # Gameplay components
      shared/
        models/             # TicMatch, TicBoardCell interfaces
        services/           # TicMatchHubService (SignalR), hub message models
      tic-match/            # Match orchestrator component
      tic-board/            # 3x3 board component
      tic-board-cell/       # Individual cell component
  core/                     # Cross-cutting services (notifications)
```
