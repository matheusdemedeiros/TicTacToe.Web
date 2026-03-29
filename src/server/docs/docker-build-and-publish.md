# Como buildar, versionar e publicar imagens Docker

## Requisitos
- Docker Desktop (Windows/Mac) ou Docker Engine (Linux)
- Conta no Docker Hub
- Terminal (PowerShell, bash, etc.)

---

## 1. Backend (.NET)

### Build da imagem

```
docker build -t <usuario_dockerhub>/tictactoe-api:<versao> -f src/server/Dockerfile src/server
```
Exemplo:
```
docker build -t matheusdemedeiros/tictactoe-api:1.0.0 -f src/server/Dockerfile src/server
```

### Teste local (opcional)
```
docker run --rm -it -p 8080:8080 matheusdemedeiros/tictactoe-api:1.0.0
```

### Publicar no Docker Hub
```
docker push matheusdemedeiros/tictactoe-api:1.0.0
```
Se for a primeira vez, rode `docker login` e entre com seu usuário/senha do Docker Hub.

---

## 2. Frontend (Angular)

### Build da imagem

```
docker build -t <usuario_dockerhub>/tictactoe-frontend:<versao> \
  --build-arg BUILD_CONFIGURATION=vps \
  --build-arg BASE_HREF=/tictactoe/ \
  -f src/client/TicTacToe.Web.Client/Dockerfile src/client/TicTacToe.Web.Client
```
Exemplo:
```
docker build -t matheusdemedeiros/tictactoe-frontend:1.0.0 --build-arg BUILD_CONFIGURATION=vps --build-arg BASE_HREF=/tictactoe/ -f src/client/TicTacToe.Web.Client/Dockerfile src/client/TicTacToe.Web.Client
```

### Teste local (opcional)
```
docker run --rm -it -p 8081:80 matheusdemedeiros/tictactoe-frontend:1.0.0
```

### Publicar no Docker Hub
```
docker push matheusdemedeiros/tictactoe-frontend:1.0.0
```

---

## 3. Dicas de versionamento
- Use tags semânticas: `1.0.0`, `1.0.1`, etc.
- Para atualizar, repita o build com nova tag e faça o push.
- Nunca use `latest` em produção, prefira tags explícitas.

---

## 4. Atualização na VPS
- Atualize o arquivo `.env` com as novas tags.
- Rode na VPS:
```
docker compose -f docker-compose.prod.yaml pull
docker compose -f docker-compose.prod.yaml up -d
```
