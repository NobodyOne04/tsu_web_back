# Lab 5 (hospitals): run `make help`.
.DEFAULT_GOAL := help
.PHONY: help build test run migration

help: ## Show this help
	@awk 'BEGIN {FS = ":.*##"} /^[a-zA-Z_-]+:.*##/ {printf "  \033[36m%-10s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)

build: ## Build the solution
	dotnet build Backend5.slnx

test: ## Domain, service and web tests
	dotnet test Backend5.slnx

run: ## Run the site on http://localhost:5000 (SQLite file Web/backend5.db, migrated on start)
	dotnet run --project Web

migration: ## Add an EF Core migration: make migration NAME=AddSomething
	dotnet tool restore
	dotnet dotnet-ef migrations add $(NAME) --project src/Backend5.Infrastructure --startup-project Web --output-dir Persistence/Migrations
