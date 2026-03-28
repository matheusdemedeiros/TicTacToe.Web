# TicTacToe.Web

Multiplayer real-time **Tic-Tac-Toe** web application built with Angular and ASP.NET Core, communicating via REST API and SignalR WebSockets.

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet) ![Angular 19](https://img.shields.io/badge/Angular-19-DD0031?logo=angular) ![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver) ![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)

## Features

- Real-time multiplayer gameplay via SignalR
- REST API for match and player management
- Clean Architecture backend with CQRS (MediatR)
- Angular standalone components frontend
- Dockerized full-stack environment
- Scalar API documentation

## Tech Stack

| Layer | Technology |
| --- | --- |
| Frontend | Angular 19, TypeScript, TailwindCSS, SignalR Client |
| Backend | ASP.NET Core (.NET 9), MediatR, Entity Framework Core |
| Database | SQL Server 2022 |
| Real-time | SignalR (WebSocket) |
| Infra | Docker Compose, Nginx (reverse proxy) |

## Repository Structure

```
TicTacToe.Web/
  .github/
    copilot-instructions.md          # AI assistant context
  docker/
    docker-compose.yaml               # Full-stack orchestration
  docs/
    running-the-project.md            # Detailed setup and run guide
    architecture.md                   # Architecture and API reference
  src/
    client/
      TicTacToe.Web.Client/           # Angular 19 frontend
    server/
      TicTacToe.WebAPI/               # ASP.NET Core Web API
      TicTacToe.Application/          # Use Cases (CQRS with MediatR)
      TicTacToe.Domain/               # Entities, Enums, Interfaces
      TicTacToe.Infra,Data/           # EF Core, Repositories, Migrations
      TicTacToe.Domain.UnitTests/     # xUnit + FluentAssertions
```

## Quick Start

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running

### Run the entire stack with one command

```bash
cd docker
docker compose up -d
```

Once all containers are healthy:

| Service | URL |
| --- | --- |
| Frontend | http://localhost:4200 |
| Backend API | http://localhost:5200 |
| API Docs (Scalar) | http://localhost:5200/scalar/v1 |
| Portainer | http://localhost:4242 |

To stop everything:

```bash
docker compose down
```

> For local development setup (without Docker), see [docs/running-the-project.md](docs/running-the-project.md).

## Game Flow

```
1. Create players .............. POST /api/Player
2. Create a match .............. POST /api/Match
3. Add players to match ........ POST /api/Match/{id}/add-player
4. Connect to SignalR hub ...... /Ticmatchhub
5. Join the match group ........ Hub: JoinMatchAsync
6. Make moves in real-time ..... Hub: MakePlayerMoveAsync
7. Receive updates ............. Hub: TicPlayerJoined, TicPlayerMadeMove
```

## Documentation

| Document | Description |
| --- | --- |
| [Running the Project](docs/running-the-project.md) | Prerequisites, Docker setup, local development, troubleshooting |
| [Architecture](docs/architecture.md) | Clean Architecture layers, API contracts, SignalR events, domain model |

## Tests

```bash
cd src/server
dotnet test
```

## License

This project is for educational and portfolio purposes.
