# Lab 6 (blog, forum, drive): run `make help`.
.DEFAULT_GOAL := help
.PHONY: help build test run migration

help: ## Show this help
	@awk 'BEGIN {FS = ":.*##"} /^[a-zA-Z_-]+:.*##/ {printf "  \033[36m%-10s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)

build: ## Build the solution
	dotnet build Backend6.slnx

test: ## Domain, service and web tests
	dotnet test Backend6.slnx

run: ## Run the site on http://localhost:5000 (SQLite file Web/backend6.db, migrated on start)
	dotnet run --project Web

migration: ## Add an EF Core migration: make migration NAME=AddSomething
	dotnet tool restore
	dotnet dotnet-ef migrations add $(NAME) --project src/Backend6.Infrastructure --startup-project Web --output-dir Migrations
