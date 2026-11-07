# TradeXpress

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/olaiyamukthar/tradexpress) 
[![Build](https://github.com/olaiyamukthar/tradexpress/actions/workflows/build.yml/badge.svg)](https://github.com/olaiyamukthar/tradexpress/actions/workflows/build.yml)

A simulated NGN-RMB (and multi-currency) cross-border trading and
settlement platform — a personal, continuously-developed project built to
learn new backend technologies hands-on, with real trading/settlement
mechanics (fake money, real engineering).

> ⚠️ **This is a simulation.** No real money moves. Built for learning and
> demonstration purposes.

## What this demonstrates

- Clean Architecture (.NET 10) with strict dependency boundaries
- CQRS via MediatR, with cross-cutting concerns (validation, error
  handling) implemented as pipeline behaviors rather than per-handler code
- EF Core Code-First against PostgreSQL, Fluent API configuration
- Kafka (KRaft mode) for event-driven domain events; RabbitMQ (planned)
  for background job processing
- Double-entry ledger, T+2-style delayed settlement with batch netting —
  real fintech mechanics, not just CRUD
- KYC/compliance modeling, RBAC, MFA (planned)

## Tech stack

.NET 10 · PostgreSQL (Neon/Supabase) · EF Core · MediatR · FluentValidation
· Kafka · RabbitMQ (planned) · Redis · Docker · Angular (planned, later
phase)

## Architecture & decisions

For the full design, reasoning, and current status:
- [`ARCHITECTURE.md`](./ARCHITECTURE.md) — how it's built, and why
- [`DECISIONS.md`](./DECISIONS.md) — dated log of significant decisions
- [`ROADMAP.md`](./ROADMAP.md) — what's built, what's next
- [`DeepWiki`](https://deepwiki.com/olaiyamukthar/tradexpress) — auto-generated
  snapshot of the code structure (may lag the latest commit; the docs above
  are the source of truth)
- [`SYSTEM_DESIGN_NOTES.md`](./SYSTEM_DESIGN_NOTES.md) — the system-design
  concepts behind what's built

## Running locally

Prerequisites: .NET 10 SDK, Docker Desktop, `dotnet-ef` global tool.

```bash
# start local infrastructure (Postgres, Redis, Kafka, Kafdrop)
docker compose up

# apply database migrations
dotnet ef database update --project src/TradeXpress.Data --startup-project src/TradeXpress.Api

# run the API
dotnet run --project src/TradeXpress.Api
```

Swagger UI available at `http://localhost:<port>/swagger` once running.

## Status

Actively in development. See [`ROADMAP.md`](./ROADMAP.md) for current
progress.