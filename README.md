# tsu_web_back · lab 6: blog, forum, drive

A small community site in ASP.NET Core MVC with accounts: a **blog** (categories, posts,
comments, attachments), a **forum** (categories, forums, topics, messages, attachments) and a
private **drive** (folders and files). TSU back-end lab 6 (2020), redesigned in 2026 on .NET 10
with a layered architecture.

## Run

```sh
make test     # 19 tests: domain rules and permissions, the whole app on temp storage
make run      # http://localhost:5000; database Web/backend6.db, uploads Web/App_Data/files
```

To get an admin, configure one (never in code):

```sh
dotnet user-secrets --project Web set Admin:Email you@example.com
dotnet user-secrets --project Web set Admin:Password '<a strong password>'
```

It's created (and put in the Administrators role) on the next start.

## Who can do what

| | Anonymous | Signed in | Author / owner | Admin |
|---|---|---|---|---|
| Read blog and forum, view their attachments | yes | yes | yes | yes |
| Write posts, comments, topics, replies | no | yes | | |
| Edit or delete them, add attachments | | | yes | yes |
| Manage blog and forum categories, forums | | | | yes |
| Drive folders and files | | own only | | everyone's |

Someone else's drive folder or file answers 404, as if it didn't exist.

## Design

```
src/Backend6.Core/            Blog, Forum, Drive entities with their rules; Actor; no ASP.NET, no EF Core
src/Backend6.Application/     Ports.cs (repositories, IFileStore, IClock, IUnitOfWork), BlogService, ForumService, DriveService
src/Backend6.Infrastructure/  EF Core + Identity on SQLite, repositories, LocalFileStore, migrations, DI and admin seeding
Web/                          thin controllers, forms, Razor views, ActorAccessor, DomainErrorsFilter
tests/Backend6.Tests/         DomainTests, WebTests (WebApplicationFactory on a temp database and folder)
```

- **Permissions live in the domain**: entities answer `CanEdit(actor)` / `CanAccess(actor)` and
  throw `ForbiddenException`; the web layer only turns outcomes into 403 / 404 / the login page.
- **Uploads** go to `Storage:Path`, outside `wwwroot`, under random names, at most 10 MB. They
  are served only through actions that check access; raster images show inline, everything else
  (HTML, SVG, ...) downloads with `nosniff`, so an upload never runs as a page.
- **Consistency**: a file is stored before the database commit and removed if the commit fails;
  files are deleted only after their rows are. Only empty categories, forums and folders can be
  deleted, so nothing is orphaned.

Other labs live on the `lab1`-`lab6` branches; `main` documents them.
