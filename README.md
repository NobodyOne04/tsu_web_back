# tsu_web_back · lab 5: hospitals

A hospital directory in ASP.NET Core MVC: hospitals with phones, wards, beds (placements) and
ward staff; labs and doctors linked to hospitals. TSU back-end lab 5 (2020), redesigned in
2026 on .NET 10 with a layered architecture.

## Run

```sh
make test     # 22 tests: domain, services (fakes), whole app on a temp SQLite file
make run      # http://localhost:5000, database Web/backend5.db (created and migrated on start)
```

Needs the .NET 10 SDK or newer (`RollForward=Major` lets it run on newer runtimes).

## Design

```
src/Backend5.Core/            entities with their rules; no ASP.NET, no EF Core
src/Backend5.Application/     Ports/ (repositories, unit of work), Services/ (use cases)
src/Backend5.Infrastructure/  EF Core on SQLite: DbContext, repositories, migrations, DI
Web/                          thin controllers, forms, Razor views
tests/Backend5.Tests/         DomainTests, ServiceTests (in-memory fakes), WebTests
```

- **Rules live in the entities**: phones are parsed once (`"111, 222"`), a lab or doctor is
  linked to a hospital once, beds are positive and unique per ward, names are required.
  Breaking one throws `DomainException`, which the form shows as a validation error.
- **Aggregates**: a hospital owns its phones and lab/doctor links; a ward owns its beds and
  staff. Services load an aggregate through its repository, call the entity, and commit through
  `IUnitOfWork`.
- **Errors**: `NotFoundException` anywhere becomes a 404 (`NotFoundFilter`).
- **Database**: SQLite (`ConnectionStrings:Default`), schema in EF Core migrations;
  `make migration NAME=...` adds one.

Other labs live on the `lab1`-`lab6` branches; `main` documents them.
