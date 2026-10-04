# TradeXpress — Roadmap

Phased milestones for an ongoing, no-fixed-end-date project. Order
reflects dependency (each phase generally needs the previous one working)
more than strict priority — some items may be reordered as new tech
interests come up, in keeping with the project's purpose.

## Phase 1 — Foundation ✅ (done)
- Local infrastructure: Postgres, Redis, Kafka (KRaft), Kafdrop via Docker
  Compose
- Solution scaffold: Domain/Business/Data/Infrastructure/Api, dependency
  graph wired
- Git + GitHub, credentials properly externalized (.env, User Secrets)

## Phase 2 — Auth & Onboarding (in progress)
- [x] Individual registration (Domain → Business → Data → Api, verified
      end-to-end)
- [x] Validation pipeline behavior (FluentValidation + MediatR)
- [x] Global error handling (ProblemDetails middleware)
- [ ] Company registration (+ `CompanyProfile` entity)
- [ ] Login + JWT issuance
- [ ] Refresh token management
- [ ] Email verification step
- [ ] MFA (TOTP, backup codes, step-up on sensitive actions)
- [ ] Serilog structured logging

## Phase 3 — Trading Core
- [ ] Currency, CurrencyPair (routing config), Wallet entities
- [ ] Rate engine (live feed + triangulation for routed pairs)
- [ ] Order/Trade entities, system-liquidity execution (no P2P yet)
- [ ] Rate snapshotting, idempotency keys on trade endpoints
- [ ] Ledger (double-entry), fee/spread calculation

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
- [ ] KycSubmission, KycDocument (Supabase/R2 storage, signed URLs)
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

## Phase 10 — Frontend
- [ ] Angular app scaffold (single app, role-based route guards)
- [ ] Auth screens (once Phase 2 fully complete)
- [ ] Trading/wallet screens (once Phase 3-4 complete)
- [ ] SignalR for live rate/trade updates

## Ongoing, no fixed phase
- New-tech experiments in `/experiments`, promoted to core once proven
- `/experiments` items tried so far: (none yet)