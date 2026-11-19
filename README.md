# tsu_web_back

Back-end web labs at TSU (Tomsk State University, 2020) in **ASP.NET Core 3.1**, built with a
teammate. Each lab lives on its own branch; `main` holds only the empty starting project.

| Branch | Lab |
|---|---|
| `lab1` | calculator service: controller, view model |
| `lab2` | forms: model binding and an error model |
| `lab3` | operations with server-side validation |
| `lab4` | sign-in, password reset with a code, private user info |
| `lab5` | hospital database: hospitals, doctors, labs, wards, staff, placements (EF Core, SQL Server) |
| `lab6` | forum and blog: posts, comments, categories, attachments, files and folders, accounts and roles (ASP.NET Identity) |

## Run a lab

```sh
git switch lab6
dotnet run --project Web          # https://localhost:5001
```

Needs the .NET Core 3.1 SDK (out of support since December 2022; a newer SDK can build it with
a `global.json` roll-forward or a retarget). Labs 5-6 use SQL Server LocalDB from
`appsettings.json` (Windows), so on macOS or Linux point `DefaultConnection` at a SQL Server
container and run `dotnet ef database update`.

Decisions: [docs/adr](docs/adr/README.md).
