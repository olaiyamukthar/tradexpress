# TradeXpress — Decisions Log

A dated record of significant decisions and the reasoning behind them.
Purpose: so returning to this project after a gap doesn't mean
reconstructing "why is it built this way" from scratch.

## Planning (pre-code)

- **Project concept**: simulated NGN-RMB (and multi-currency) cross-border
  trading/settlement platform, chosen over generic CRUD portfolio ideas
  because it ties into a long-term venture idea and gives every planned
  technology (Kafka, RBAC, audit trails) a real business reason to exist.
- **Currency scope**: generalized multi-currency model with configurable
  direct-trade vs. vehicle-currency routing per pair, rather than
  hardcoding NGN-RMB only — NGN-RMB kept as the flagship/demo corridor.
  Routing configuration is admin-controlled, not user-facing.
- **Trading model**: both P2P order-book matching and system-liquidity
  market-maker fallback, price-time priority. Sequencing: build
  system-only execution first, layer in P2P matching later — full
  matching-engine complexity isn't worth tackling before the simpler path
  works end-to-end.
- **Settlement model**: trade execution (match) and settlement (fund
  movement) are separated, simulating real FX T+2-style delayed
  settlement rather than instant finalization. Trades reserve funds on
  match, sit pending, then a periodic settlement batch nets exposures and
  finalizes ledger entries. Settlement delay is configurable (including a
  0-delay fast-demo mode). Funding transactions (deposits/withdrawals)
  flow through the same batch mechanism against ExternalClearing wallets.
- **Account model**: introduced `Account` (Individual/Company) above
  `User`, with `AccountUser` as a join entity, so Company accounts can
  have multiple authorized users with different roles (Owner/Trader/
  Viewer). Wallets/orders/trades belong to `Account`, not `User`.
- **KYC/KYB**: approval is account-level (not per-user within a company
  account) — matches how real business banking verifies once, not
  per-employee. Documents can be uploaded before/without verification
  (test data). The KYC pre-screening agent extracts/flags issues but
  never auto-approves — a human Ops reviewer always makes the final call.
- **Registration**: account-type-specific from the start (separate
  Individual and Company registration commands/endpoints), not a single
  generic flow with a later upgrade step, since Company registration
  needs materially different data (company name, registration number,
  tax ID).
- **Architecture**: Clean Architecture, CQRS via MediatR throughout (not a
  hybrid with plain services) — chosen so cross-cutting concerns
  (validation, later caching/rate-limiting/idempotency) apply uniformly
  via pipeline behaviors, rather than each service method needing to
  remember to invoke them.
- **Solution structure naming**: Api/Business/Data/Infrastructure, mapped
  onto the familiar Controller/Service/Repository/DB mental model, rather
  than introducing unfamiliar Clean Architecture terminology.
- **Response shape**: `ProblemDetails`/`ValidationProblemDetails` (RFC
  7807) for all errors; no fixed success envelope. Explicitly considered
  and rejected adopting a `ResponseCode`/`ResponseMessage`/`Data` envelope
  (Mukthar's usual convention elsewhere) for success responses, since it
  would duplicate what the status code already conveys.
- **Messaging split**: Kafka for outbox-driven domain events (trades,
  settlement, KYC status changes, audit); RabbitMQ for background
  job/task processing (KYC pre-screening agent calls, webhook/
  notification retries) — chosen for RabbitMQ's native dead-lettering and
  per-message TTL, better suited to "consumed once and done" work than
  replicating the same on Kafka.
- **Vertical-slice development**: build one feature end-to-end (Domain →
  Business → Data → Api, tested working) before starting the next,
  instead of building each layer horizontally in full first.
- **Frontend phasing**: Angular/TypeScript, deferred until after major
  completed backend modules (e.g. all of Auth), not built alongside every
  individual backend slice — keeps focus on backend depth first.
- **Documentation ownership**: Claude maintains `ARCHITECTURE.md`,
  `DECISIONS.md`, `ROADMAP.md` going forward; Mukthar doesn't draft these
  first.

## 2026-09-09 / 2026-09-10 — Local infrastructure

- Kafka: KRaft mode over Zookeeper (modern approach, one container instead
  of two, Zookeeper mode being phased out industry-wide).
- Kafka dual-listener setup (`PLAINTEXT` for host clients, `INTERNAL` for
  container-to-container) — required after hitting a real reconnect-loop
  bug where Kafdrop was told to reconnect to `localhost`, which inside its
  own container meant itself, not the Kafka container.
- Postgres: explicit `scram-sha-256` password auth (image defaults to
  "trust" otherwise) plus a named volume for persistence.
- Credentials moved out of `docker-compose.yml` into a gitignored `.env`
  file with variable substitution, once the repo was about to go public.

## 2026-09-15 — Data layer foundations

- EF Core Code-First, Fluent API only (no data annotations) — deliberate
  departure from the stored-procedure pattern used elsewhere, to build
  range.
- Enums stored as strings (`HasConversion<string>()`) rather than default
  integers, to avoid silent meaning-shifts if enum values are ever
  reordered or inserted mid-list later in the project's long lifespan.
- `AccountUser` uses a composite key (`UserId`, `AccountId`) rather than a
  dedicated `Id` — makes a duplicate user-account link structurally
  impossible at the database level, rather than relying on application
  code to check for it.
- Connection strings kept out of `appsettings.Development.json` (which is
  committed) via .NET User Secrets — mirrors the same "don't commit
  credentials" principle already applied to Docker.

## 2026-09-16 to 2026-09-20 — Business/Api layer, first vertical slice

- `IApplicationDbContext` introduced in Business, implemented by
  `TradeXpressDbContext` in Data — corrected an early handler mistake
  where a handler referenced `TradeXpressDbContext` directly, violating
  the Business-never-depends-on-Data rule.
- FluentValidation wired as a generic MediatR pipeline behavior
  (`ValidationBehavior<TRequest,TResponse>`) rather than called manually
  per-handler, so every current and future command gets validation
  automatically.
- Global exception-handling middleware added after unhandled exceptions (a
  Postgres duplicate-key `DbUpdateException`, then a FluentValidation
  `ValidationException`) came back as 500s with the Development exception
  page. That page is Development-only, and the Production default is an
  empty 500 that tells clients nothing. The middleware gives callers a
  stable contract (400 with field errors for validation failures, generic
  500 otherwise) and logs the real exception server-side.
- Unique index added on `User.Email` — first attempt silently generated
  an empty migration (stale build), caught by reading the generated
  `Up()` method before applying it; now a standing habit, not a one-off
  check.
- Swashbuckle.AspNetCore updated to fix a real Swagger UI rendering bug
  (newer-generated `"openapi": "3.0.4"` specs rejected by older bundled
  swagger-ui-dist); a stale browser cache masked the fix on first reload
  — resolved via a cache-disabled hard reload.

  ## 2026-10-31 — Planning process

- Soft target dates added per roadmap phase (see ROADMAP.md). Soft by
  design: they keep a no-deadline project moving and are re-baselined at
  each phase boundary from the actual pace.
- System-design notes added (SYSTEM_DESIGN_NOTES.md). Each concept is
  explained while building the feature that needs it, so the knowledge
  stays tied to real code.
- Building in public on X, 1–2 posts per week tied to real milestones.
- Not added to the plan (can be revisited): a fixed 12-week schedule (it
  conflicts with the learning-first, no-fixed-end-date goal and ignores
  dependency order), an API gateway (this is a modular monolith with no
  second service to front), Semantic Kernel (agents will call the
  Anthropic API directly), and Native AOT benchmarks.
- Vocabulary rule: the system is event-driven with an outbox. It is not
  event-sourced, and docs and posts must not claim it is.

  ## 2026-10-31 — CI build workflow

- Added a GitHub Actions workflow (`.github/workflows/build.yml`) that
  restores and builds the solution in Release configuration on every push
  and pull request to `main`, ahead of the full Phase 9 pipeline. Chosen
  because it costs about an hour, builds from a clean checkout on a fresh
  Linux machine (catching forgotten `git add` files and "works on my
  machine" problems), and puts a build badge on the README.
- Known limit: it only proves the solution compiles. There are no tests
  yet, so it will not catch behavior bugs. Tests and the migrate-then-
  deploy step attach to this same workflow in Phase 9.
- Settings: `ubuntu-latest` runner, .NET `8.0.x`, `actions/checkout@v7`,
  `actions/setup-dotnet@v6`. The action versions are major-version tags
  and should be rechecked against each action's releases page when the
  workflow is next edited.

  ## 2026-10-31 — Company registration

- Company registration is its own command, validator, handler and endpoint
  (`POST /api/auth/register/company`). It needed no change to
  `Program.cs`, because MediatR and FluentValidation register by scanning
  the Business assembly.
- `CompanyProfile` is a separate table, one-to-one with `Account`, enforced
  by a unique index on `AccountId`. `RegistrationNumber` is also unique.
  Individual accounts simply have no profile row, instead of carrying empty
  company columns.
- Text columns have max lengths from the start, and validator limits match
  the column limits, so oversized input becomes a clean 400 instead of a
  database exception at `SaveChanges`.
- Company text fields are trimmed in both the uniqueness check and the
  handler, so the check and the insert see the same value. Case-insensitive
  uniqueness is deferred (roadmap follow-up).
- Validator rules use `CascadeMode.Stop` where a later rule assumes an
  earlier one passed. Found with a null registration number: the
  uniqueness rule's `Trim()` ran after `NotEmpty()` failed and threw a
  `NullReferenceException`.
- Swagger: enabled `SupportNonNullableReferenceTypes()`, so schemas show
  non-nullable strings. The validators remain the enforcement; the schema
  is documentation.