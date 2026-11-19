# tsu_web_back

Back-end web labs at TSU (Tomsk State University, 2020; on .NET 10 since 2026) in **ASP.NET Core**, built with a
teammate. Each lab lives on its own branch; `main` holds only the empty starting project.

| Branch | Lab |
|---|---|
| `lab1` | calculator service: controller, view model |
| `lab2` | forms: model binding and an error model |
| `lab3` | operations with server-side validation |
| `lab4` | sign-in, password reset with a code, private user info |
| `lab5` | hospital database: hospitals, doctors, labs, wards, beds, staff (layered, EF Core + SQLite, tests) |
| `lab6` | blog, forum and private drive with accounts and roles (layered, Identity + SQLite, tests) |

## Run a lab

Needs the .NET 10 SDK or newer.

```sh
git switch lab1                   # lab1 … lab4: one MVC project
dotnet run --project Web          # http://localhost:5000

git switch lab5                   # lab5, lab6: layered solutions with tests
make test && make run             # SQLite database, created and migrated on start
```

Labs 5 and 6 are split into `src/BackendN.Core` (entities and rules), `src/BackendN.Application`
(ports and services), `src/BackendN.Infrastructure` (EF Core on SQLite) and `Web` (thin
controllers), with tests in `tests/`. Each branch's README has the details; lab 6 explains who
may do what and how to configure an admin.

Decisions: [docs/adr](docs/adr/README.md).
