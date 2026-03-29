# Running the Project

This guide covers all the ways to run the TicTacToe.Web application.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Option 1: Docker Compose (recommended)](#option-1-docker-compose-recommended)
- [Option 2: Local Development](#option-2-local-development)
- [Environment Configuration](#environment-configuration)
- [Useful Commands](#useful-commands)
- [Troubleshooting](#troubleshooting)

---

## Prerequisites

### For Docker (Option 1)

| Tool | Version | Download |
| --- | --- | --- |
| Docker Desktop | 4.x+ | [docker.com](https://www.docker.com/products/docker-desktop/) |

### For Local Development (Option 2)

| Tool | Version | Download |
| --- | --- | --- |
| .NET SDK | 9.0 | [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/9.0) |
| Node.js | 22.x LTS | [nodejs.org](https://nodejs.org/) |
| Angular CLI | 19.x | `npm install -g @angular/cli` |
| SQL Server | 2022 | Via Docker (see below) or [local install](https://www.microsoft.com/sql-server) |

---

## Option 1: Docker Compose (recommended)

This is the easiest way to run the full stack. One command starts the database, backend, frontend, and monitoring.

### 1. Start all services

```bash
cd docker
docker compose up -d
```

### 2. Wait for containers to be healthy

```bash
docker compose ps
```

Expected output:

```
NAME                 STATUS
sql-server           Up (healthy)
tictactoe-backend    Up
tictactoe-frontend   Up
portainer            Up
```

### 3. Access the application

| Service | URL | Description |
| --- | --- | --- |
| **Frontend** | http://localhost:4200 | Angular application |
| **Backend API** | http://localhost:5200 | ASP.NET Core Web API |
| **API Docs** | http://localhost:5200/scalar/v1 | Interactive API documentation (Scalar) |
| **Portainer** | http://localhost:4242 | Docker container management UI |
| **SQL Server** | `localhost:1433` | Database (user: `sa`, password: `P@ssw0rd123`) |

### 4. Stop all services

```bash
docker compose down
```

To also remove persisted data (database, volumes):

```bash
docker compose down -v
```

### 5. Rebuild after code changes

```bash
# Rebuild everything
docker compose build
docker compose up -d

# Rebuild only the backend
docker compose build backend
docker compose up -d backend

# Rebuild only the frontend
docker compose build frontend
docker compose up -d frontend
```

---

## Option 2: Local Development

Use this when you need hot-reload, breakpoints, and a faster feedback loop during development.

### 1. Start the database

You still need SQL Server. The easiest way is via Docker:

```bash
cd docker
docker compose up -d sql-server
```

### 2. Run the backend

**Using Visual Studio:**

1. Open `src/server/TicTacToe.WebAPI.sln` in Visual Studio
2. Set `TicTacToe.WebAPI` as the startup project
3. Select the `https` launch profile
4. Press `F5` or `Ctrl+F5`

The API will be available at `https://localhost:7199`.

**Using the CLI:**

```bash
cd src/server/TicTacToe.WebAPI
dotnet run --launch-profile https
```

### 3. Apply database migrations

On first run (or after pulling new migrations):

```bash
cd src/server/TicTacToe.WebAPI
dotnet ef database update --project "../TicTacToe.Infra,Data/TicTacToe.Infra.Data.csproj"
```

### 4. Run the frontend

```bash
cd src/client/TicTacToe.Web.Client
npm install
ng serve
```

The app will be available at `http://localhost:4200`.

> **Note:** In local development mode, the frontend points to `https://host.docker.internal:7199/` as the API URL. If you are running the backend directly on your machine (not in Docker), you may need to update `src/client/TicTacToe.Web.Client/src/environments/environment.development.ts`:

```typescript
export const environment = {
    envName: 'development',
    apiUrl: 'https://localhost:7199/'
};
```

---

## Environment Configuration

### Backend

| File | Environment | SQL Server Host |
| --- | --- | --- |
| `appsettings.json` | Default | `host.docker.internal:1433` |
| `appsettings.Development.json` | Local dev (VS/CLI) | Inherits from default |
| `appsettings.Docker.json` | Docker Compose | `sql-server:1433` (container network) |

### Frontend

| File | Environment | API URL |
| --- | --- | --- |
| `environment.ts` | Production (Azure) | Backend Azure URL |
| `environment.development.ts` | Local dev (ng serve) | `https://localhost:7199/` |
| `environment.docker.ts` | Docker Compose | `/` (proxied by Nginx) |

### Docker Compose Services

| Service | Internal Port | Exposed Port | Image |
| --- | --- | --- | --- |
| sql-server | 1433 | 1433 | `mcr.microsoft.com/mssql/server:2022-latest` |
| backend | 8080 | 5200 | Custom (.NET 9 multi-stage) |
| frontend | 80 | 4200 | Custom (Angular build + Nginx) |
| portainer | 9000 | 4242 | `portainer/portainer-ce:latest` |

---

## Useful Commands

```bash
# View logs from a specific container
docker logs tictactoe-backend -f
docker logs tictactoe-frontend -f

# Open a shell inside a container
docker exec -it tictactoe-backend bash

# Run backend tests
cd src/server
dotnet test

# Run frontend tests
cd src/client/TicTacToe.Web.Client
ng test

# Check container status
docker compose -f docker/docker-compose.yaml ps
```

---

## Troubleshooting

### SQL Server container is not healthy

The SQL Server container may take up to 30 seconds to initialize. Check logs:

```bash
docker logs sql-server
```

If it keeps restarting, ensure you have at least **2 GB of RAM** allocated to Docker.

### Frontend shows a blank page

Check the browser console for errors. Common causes:
- Backend is not running (API calls fail)
- CORS misconfiguration — verify `CORSConfig.cs` includes the frontend origin
- SignalR connection failed — check that the `/Ticmatchhub` route is accessible

### CORS errors

CORS origins are configured per environment in `appsettings.*.json` under `Cors:AllowedOrigins`. If you see CORS errors:
- Check that the frontend URL is listed in the correct `appsettings` file
- In Azure, you can override via Application Settings: `Cors__AllowedOrigins__0`, `Cors__AllowedOrigins__1`, etc.

### `npm ci` fails with peer dependency errors

The project has a known peer dependency conflict between `@angular/animations` v20 and `@angular/common` v19. In Docker, this is handled automatically with `--legacy-peer-deps`. For local development:

```bash
npm install --legacy-peer-deps
```

### EF Core migrations fail

Ensure the SQL Server is running and the connection string in `appsettings.json` matches your setup. The default expects SQL Server at `host.docker.internal:1433`.
