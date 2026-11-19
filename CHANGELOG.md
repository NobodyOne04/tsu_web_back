# Changelog

## Unreleased

### Changed
- Every lab on .NET 10 (ADR 0003); labs 5 and 6 rebuilt as layered solutions (Core, Application, Infrastructure, Web) with tests and SQLite

### Fixed
- Lab 6: unauthenticated forum management, forgeable message authors, public uploads and drive files, a hard-coded admin password, and a seeding bug that crashed every start after the first
- Labs 1-2: division by zero returned Infinity; lab 5: crashes on duplicate links and repeated deletes

### Added
- README with the lab list, CLAUDE.md, changelog and ADRs

### Changed
- Default branch renamed from `master` to `main`

### Removed
- Build output (`Web/bin`, `Web/obj`) from `main`; now git-ignored
