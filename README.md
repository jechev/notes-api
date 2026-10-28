# Notes API

A small ASP.NET Core Web API backed by PostgreSQL, containerized with Docker and Docker Compose.
Built as a hands-on exercise for containers: multi-stage build, Compose networking, health checks and persistent volumes.

## Stack

- ASP.NET Core (controllers), .NET 10
- Entity Framework Core with PostgreSQL (Npgsql)
- Docker multi-stage build (SDK image to build, ASP.NET runtime image to run)
- Docker Compose with a database health check and a named volume

## Run

```bash
docker compose up --build
```

The API listens on http://localhost:8080.

## Endpoints

| Method | Path      | Description              |
| ------ | --------- | ------------------------ |
| GET    | `/health` | Liveness check           |
| GET    | `/notes`  | List notes, newest first |
| POST   | `/notes`  | Create a note            |

```bash
curl -X POST http://localhost:8080/notes \
  -H "Content-Type: application/json" \
  -d '{"text": "My first note"}'

curl http://localhost:8080/notes
```

## Architecture

```
client --> api (ASP.NET Core, :8080) --> db (PostgreSQL 16, volume: dbdata)
```

- `api` waits for the `db` health check before starting (`depends_on: condition: service_healthy`).
- The API also retries the first database connection, so it survives slow database starts.
- Data survives `docker compose down`; use `docker compose down -v` to delete it.

## Possible next steps

- EF Core migrations instead of `EnsureCreated`
- GitHub Actions workflow to build, test and push the image
- Integration tests with Testcontainers
- Kubernetes manifests (Deployment, Service, Secret)