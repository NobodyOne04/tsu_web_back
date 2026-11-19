# tsu_web_back

TSU back-end web labs (2020), ASP.NET Core MVC on .NET 10. One branch per lab; `main` holds the
starting skeleton and the docs.

## Commands
```sh
git switch labN                         # lab1 … lab6
dotnet run --project Web                # labs 1-4
make test / make run / make migration NAME=X   # labs 5-6 (layered solutions, BackendN.slnx)
```

## Layout
- `main`: `Web.sln` skeleton, README (lab list), this file, `docs/adr/`.
- `lab1`-`lab4`: one `Web/` MVC project each (controllers, models, views).
- `lab5`, `lab6`: `src/BackendN.Core` (entities + rules), `src/BackendN.Application` (ports +
  services), `src/BackendN.Infrastructure` (EF Core / SQLite, migrations, DI), `Web/` (thin
  controllers, forms, views), `tests/BackendN.Tests`; `dotnet-tools.json` pins `dotnet-ef`.
- `docs/adr/`: decision records (`.adr`), index in `docs/adr/README.md`.

## Conventions
- Read the ADR before changing a recorded decision; commit subjects carry its number.
- The `labN` branches are the content: never delete them or merge them into `main`; a lab's PR
  targets its own branch.
- Labs 5-6: rules and permissions belong in the Core entities; services orchestrate; controllers
  only map HTTP. New rules get a domain test; new pages a WebApplicationFactory test.
- Lab 6: uploads only through `IFileStore` (outside `wwwroot`); never serve uploads as HTML;
  never put credentials in code (admin comes from configuration).
- `bin/`, `obj/`, `.idea/`, `*.db`, `App_Data/` are git-ignored.
