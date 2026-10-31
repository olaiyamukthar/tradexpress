# TradeXpress — Roadmap

Phased milestones for an ongoing, no-fixed-end-date project. Order reflects
dependency (each phase generally needs the previous one working) more than
strict priority.

## How target dates work

Dates are soft. They exist to keep a no-deadline project moving, not to
create pressure.
- Dates get fuzzier the further out they are (months, then quarters).
- At the end of each phase, remaining dates are re-baselined from the
  actual pace.
- A slipping date moves later phases. It never means skipping the learning
  or the verification steps.
- Last re-baselined: 2026-10-29.

| Phase | Soft target |
|---|---|
| 1. Foundation | Done |
| 2. Auth & Onboarding | Jan 2027 |
| F1. Frontend: auth screens | Feb 2027 |
| 3. Trading Core | May 2027 |
| 4. Funding | Jul 2027 |
| F2. Frontend: trading and wallet screens | Aug 2027 |
| 5. Settlement & Reconciliation | Q4 2027 |
| 6. KYC/Compliance | Q4 2027 – Q1 2028 |
| 7. P2P Matching | Q1 2028 |
| 8. AI Agents | Q1 2028 |
| 9. Observability & CI/CD | Q2 2028 |
| F3. Frontend: ops/admin screens, live updates | Q2 2028 |

## Phase 1 — Foundation (done)
- [x] Local infrastructure: Postgres, Redis, Kafka (KRaft), Kafdrop via
      Docker Compose
- [x] Solution scaffold with dependency graph wired
- [x] Git + GitHub, credentials externalized (.env, User Secrets)

## Phase 2 — Auth & Onboarding (in progress)
- [x] Individual registration, verified end-to-end
- [x] Validation pipeline behavior (FluentValidation + MediatR)
- [x] Global error handling (ProblemDetails middleware)
- [ ] Company registration (`CompanyProfile` entity done; Data layer,
      command/validator/handler and endpoint next)
- [ ] Map unique-constraint violations from concurrent duplicate requests
      to 409 Conflict (currently a generic 500)
- [ ] Login + JWT issuance
- [ ] Refresh token management
- [ ] Serilog structured logging
- [ ] Email verification step
- [ ] MFA (TOTP, backup codes, step-up on sensitive actions)

## Phase 3 — Trading Core
- [ ] Currency, CurrencyPair (routing config), Wallet entities
- [ ] Rate engine (live feed + triangulation for routed pairs)
- [ ] Order/Trade entities, system-liquidity execution (no P2P yet)
- [ ] Rate snapshotting, idempotency keys on trade endpoints
- [ ] Double-entry ledger, fee/spread calculation

## Phase 4 — Funding
- [ ] FundingTransaction (deposit/withdrawal), simulated async
      payment-processor flow
- [ ] Reserved-balance concept for withdrawals
- [ ] Webhook-style confirmation + idempotency (RabbitMQ background job)

## Phase 5 — Settlement & Reconciliation
- [ ] SettlementBatch, netting logic
- [ ] Outbox pattern → Kafka for domain events
- [ ] Inbox/dedup pattern for consumers
- [ ] Reconciliation job + agent-generated incident summaries

## Phase 6 — KYC/Compliance
- [ ] KycSubmission, KycDocument (object storage, signed URLs)
- [ ] KYC pre-screening agent (vision-model extraction, human approval)
- [ ] Compliance status, KYC tiers, trading/withdrawal limits
- [ ] Trading circuit breaker (per currency pair)

## Phase 7 — P2P Matching
- [ ] Order book (price-time priority), partial fills
- [ ] System-liquidity fallback for unmatched market orders

## Phase 8 — AI Agents
- [ ] Audit-log investigator
- [ ] Progress-report agent
- [ ] Vulnerability-scanner orchestrator
- [ ] Support agent (RAG over own API docs)

## Phase 9 — Observability & CI/CD
- [ ] Health checks, OpenTelemetry (stretch)
- [ ] GitHub Actions: build/test/migrate-then-deploy pipeline
- [ ] Unit tests + Testcontainers integration tests
- [ ] Demo seed data + reset script

## Frontend blocks (Angular, single app, role-based route guards)
- F1: auth screens, once Phase 2 is complete
- F2: trading and wallet screens, once Phases 3–4 are complete
- F3: ops/admin screens, SignalR live updates

## Follow-ups (tech debt)
- [ ] Database foreign keys on `AccountUser` and `CompanyProfile`
      (currently none)
- [ ] Max lengths on text columns in existing tables (new tables get them
      from the start)

## Working agreements

**Learning:** when a feature touches a system-design concept, it is
explained while building and recorded in `SYSTEM_DESIGN_NOTES.md`.

**Visibility (X):**
- 1–2 posts per week, tied to something real: a shipped slice, a bug found
  and fixed, a design decision and its tradeoff, or a diagram.
- Always say the project is simulated (fake money).
- Use accurate vocabulary. TradeXpress is event-driven with an outbox. It
  is not event-sourced.
- Crop screenshots so they never show connection strings, passwords or
  secrets.
- Keep anything from your day job out of posts and out of the repo.
- Link the repo.

## Ongoing
- New-tech experiments in `/experiments`, promoted to core once proven
- Experiments tried so far: none