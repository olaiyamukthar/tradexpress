# TradeXpress — System Design Notes

Short explanations of the system-design concepts behind what has been
built, written while building so they stay tied to real code. Each note
names the concept, where it shows up in TradeXpress, and the tradeoff.
Notes for upcoming phases are listed at the bottom.

## Dependency inversion (ports and adapters)
Where: `IApplicationDbContext` is defined in Business and implemented by
`TradeXpressDbContext` in Data.
Idea: business logic depends on an abstraction it owns, and infrastructure
depends on the business layer, not the other way around.
Why it matters: handlers can be tested with a fake context, and swapping
the database or ORM touches one project instead of every handler.
Tradeoff: one more interface to maintain. The interface exposes
`DbSet<T>`, so Business still depends on EF Core types. That is a
pragmatic shortcut over a full repository layer.

## Cross-cutting concerns as a pipeline
Where: `ValidationBehavior` wraps every MediatR command, and
`ExceptionHandlingMiddleware` wraps every HTTP request.
Idea: chain of responsibility. Shared behavior is applied around handlers
instead of inside them, so no handler can forget it.
Tradeoff: control flow is less visible when reading a single handler.
Tracing a request means knowing the pipeline order.

## Check-then-insert is a race; the database constraint is the guarantee
Where: the email-uniqueness validator (`AnyAsync`) plus the unique index
on `Users.Email`.
Idea: two simultaneous requests can both pass the validator before either
inserts. Only the unique index makes duplicates impossible. The validator
exists for a good error message, and the constraint exists for
correctness.
Gap: the losing request currently surfaces as a generic 500. The correct
response is 409 Conflict (tracked in the roadmap).

## Atomicity through a single transaction (unit of work)
Where: registration adds `User`, `Account` and `AccountUser`, then calls
`SaveChangesAsync` once. EF Core wraps that in one transaction.
Idea: either all three rows exist or none do, so a user can never exist
without an account.
Later: this local-transaction guarantee does not extend to Kafka. That gap
is what the outbox pattern solves in Phase 5.

## Error responses are a disclosure surface
Where: the exception middleware returns a stable ProblemDetails contract
and logs the real exception server-side.
Idea: ASP.NET Core's Development exception page returned a raw Postgres
exception, file paths included. That page is Development-only, and the
Production default is an empty 500 that gives clients nothing to act on.
Either way, the response contract should be a deliberate design, not
whatever the framework defaults to in each environment.

## Configuration layering and secrets
Where: `.env` for Docker, User Secrets for local app config, environment
variables in production.
Idea: the same code reads configuration from different sources per
environment (twelve-factor config), so secrets never live in committed
files.

## Migrations and deployment
Where: EF Core migrations applied as an explicit deploy step, not on app
startup.
Idea: with multiple app instances, startup-time migration can race, and a
bad migration would take down a live app. A separate step fails the deploy
instead.

## Continuous integration: fast feedback from a clean machine
Where: the GitHub Actions build workflow.
Idea: every push is built automatically on a fresh machine that has only
what's committed. The longer the gap between a change and the feedback
about it, the more expensive the bug is to find. A clean machine also
removes everything that's only true on your laptop: files you forgot to
commit, tools you installed by hand, settings in your environment.
Tradeoff: a build-only pipeline proves compilation and nothing else. Its
value grows as tests are added to it, and later as a branch rule that
requires a green run before a pull request can merge.

## Coming up
- Phase 2: authentication vs authorization, JWT tradeoffs (stateless but
  hard to revoke), refresh-token rotation, rate-limiting algorithms
- Phase 3: money representation and rounding, idempotency keys,
  double-entry invariants, optimistic vs pessimistic concurrency on wallets
- Phase 4: webhooks, at-least-once delivery, retry with backoff
- Phase 5: outbox and inbox patterns, eventual consistency and CAP in
  practice, Kafka partition ordering, why "exactly once" needs idempotent
  consumers
- Phase 6: object storage and signed URLs, human-in-the-loop review
- Phase 7: matching engines and the single-writer principle, recovering
  in-memory state
- Phase 8: where non-deterministic components belong in a system,
  guardrails for agents
- Phase 9: logs, metrics and traces, deployment strategies