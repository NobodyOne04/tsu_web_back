# tsu_web_back

TSU back-end web labs (2020), ASP.NET Core 3.1 MVC. One branch per lab; `main` is the empty skeleton.

## Commands
```sh
git switch labN                    # lab1 … lab6
dotnet build Web.sln
dotnet run --project Web
dotnet ef database update --project Web    # labs 5-6 (EF Core, SQL Server)
```
No tests.

## Layout
- `Web.sln`, `Web/Web.csproj` (netcoreapp3.1), `Web/Program.cs`, `Web/Startup.cs`, `Web/appsettings*.json`.
- On lab branches: `Web/Controllers/`, `Web/Models/`, `Web/Views/` (and `Web/Data/`, migrations in 5-6).
- `docs/adr/`: decision records (`.adr`), index in `docs/adr/README.md`.

## Conventions
- Read the ADR before changing a recorded decision; commit subjects carry its number.
- The `labN` branches are the content: never delete them or merge them into `main`.
- `bin/` and `obj/` are build output and git-ignored (the lab branches still contain old copies).
- Archived coursework: docs or small fixes only unless asked.
