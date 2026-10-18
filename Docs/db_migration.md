# Database Migration Guide

This document explains how to create, apply, revert, and manage Entity Framework Core migrations in Palette.

Palette uses PostgreSQL with Entity Framework Core to manage database schema changes.

## 1. Overview

EF Core migrations allow developers to update the database schema as entity models change without manually recreating tables.

Each migration represents a set of database changes that can be applied through the `Up()` method and reversed through the `Down()` method.

**Important:** Always review generated migration code before applying it, especially when modifying existing tables containing data.

### Prerequisites

- PostgreSQL is running through Docker Compose.
- The ASP.NET Core backend has a valid PostgreSQL connection string.
- The .NET SDK and EF Core CLI tools are installed.

Make sure you are in the `backend/` directory before running the following commands.

```bash
dotnet restore
dotnet ef --version
```

If the EF Core CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef --version 10.0.4
```

If an existing installation needs updating:

```bash
dotnet tool update --global dotnet-ef --version 10.0.4
```

### Running EF Core Commands

All EF Core commands in this guide should be run from the `backend/` directory.

From the repository root:

```bash
cd backend
```

For example:

```bash
dotnet ef migrations add MigrationName --context PaletteDbContext
dotnet ef database update --context PaletteDbContext
```

Palette uses `PaletteDbContext` to manage its database schema. The `--context` option specifies which DbContext to use.  

The PostgreSQL host, database name, and credentials are provided through `ConnectionStrings:DefaultConnection`, typically using local user secrets or environment variables. They do not need to be specified in each command.

**Note:** Docker Compose commands should be run from the repository root, where `compose.yaml` is located.

### EF Core Command Options

| Option | Purpose |
|---|---|
| `--context` / `-c` | Specifies the DbContext to use |
| `--project` / `-p` | Specifies the project containing the migrations |
| `--startup-project` / `-s` | Specifies the project used to load application configuration |
| `--connection` | Overrides the configured database connection for supported commands |

Palette currently uses `PaletteDbContext`.

For consistency, the examples in this guide explicitly specify:

```bash
--context PaletteDbContext
```

The PostgreSQL host, database name, and credentials are normally provided through `ConnectionStrings:DefaultConnection`.

There is no need to specify the server or database name in each migration command.

---

## 2. When Should You Create a Migration?

Create a migration when changes to entity models or EF Core configurations require changes to the database schema.

### Migration Required

| Change | Example |
|---|---|
| Create a table | Add a `COMMENTS` entity |
| Delete a table | Remove an obsolete entity |
| Add or remove a column | Add `profile_picture` to `USERS` |
| Rename a column | Rename `username` to `display_name` |
| Change column type | Change `rating` from integer to decimal |
| Change nullability | Make `email` required |
| Modify relationships | Add a foreign key between two entities |
| Add or modify constraints | Add a unique constraint |
| Modify indexes | Add an index for frequent searches |
| Modify model-managed seed data | Change data maintained through `HasData()` |

### Migration Usually Not Required

| Change | Example |
|---|---|
| Change application logic | Modify a search algorithm |
| Change API endpoints | Add a new controller |
| Change frontend code | Update a React Native component |
| Add non-mapped properties | Add a property ignored by EF Core |
| Insert application data | A user submits a review |
| Update existing records | A user edits their review |
| Delete existing records | A user removes a media log |
| Modify runtime seed data | Change development data inserted through runtime seeding |

**Note:** Changes to EF Core model-managed data (`HasData`) may require migrations. Ordinary data modifications and runtime seeding typically do not.

---

## 3. Creating and Applying a Migration

### Step 1: Update the Entity Model

Make the necessary changes to the C# entity models or EF Core configurations.

For example:

```csharp
public string? ProfilePictureUrl { get; set; }
```

### Step 2: Create the Migration

```bash
dotnet ef migrations add AddUserProfilePicture \
  --context PaletteDbContext
```

Use descriptive migration names.

Examples:

```bash
dotnet ef migrations add InitialCreate \
  --context PaletteDbContext

dotnet ef migrations add CreateCommentsTable \
  --context PaletteDbContext

dotnet ef migrations add UpdateReviewRatingType \
  --context PaletteDbContext
```

### Step 3: Review the Generated Migration

EF Core generates files similar to:

```text
Migrations/
├── 20261017000000_AddUserProfilePicture.cs
├── 20261017000000_AddUserProfilePicture.Designer.cs
└── PaletteDbContextModelSnapshot.cs
```

The migration file contains two important methods:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<string>(
        name: "ProfilePictureUrl",
        table: "USERS",
        type: "text",
        nullable: true);
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "ProfilePictureUrl",
        table: "USERS");
}
```

- `Up()` applies schema changes.
- `Down()` reverses those changes.
- `Designer.cs` contains migration metadata.
- `PaletteDbContextModelSnapshot.cs` represents the latest model snapshot used to generate future migrations.

Always review both `Up()` and `Down()` before applying a migration.

### Step 4: Apply the Migration

```bash
dotnet ef database update \
  --context PaletteDbContext
```

This applies all pending migrations to the configured PostgreSQL database.

### Step 5: Verify the Result

Use DBeaver to inspect the updated tables, columns, and constraints.

To list migrations:

```bash
dotnet ef migrations list \
  --context PaletteDbContext
```

EF Core tracks applied migrations in the `__EFMigrationsHistory` table.

### Step 6: Commit the Changes

Commit the entity model changes, EF Core configurations, and generated migration files together.

---

## 4. Automatic Migration on Startup

Palette is configured to automatically apply pending EF Core migrations when the backend starts in the **Development** environment.

This allows developers to synchronize their local database schema after pulling changes from other team members, without manually applying migrations.

### Workflow After Pulling from `develop`

1. Pull the latest changes from `develop` into your working branch.

2. Make sure PostgreSQL is running. From the repository root:

   ```bash
   docker compose up -d
   ```

3. Navigate to the `backend/` directory and start the application:

   ```bash
   cd backend
   dotnet run
   ```

4. The backend automatically checks for and applies all pending migrations before serving requests.

### How It Works

In `Program.cs`, the application calls `dbContext.Database.Migrate()` during startup when running in the Development environment.

EF Core tracks applied migrations in the `__EFMigrationsHistory` table.

- If pending migrations exist, they are applied in order.
- If no migrations are pending, no database schema changes are made.
- If a migration fails, the application will not finish starting until the error is resolved.

### Important Notes

- Automatic migration only runs in the **Development** environment.
- PostgreSQL must be running, and `ConnectionStrings:DefaultConnection` must be configured correctly.
- Developers do not need to run `dotnet ef database update` after pulling changes when starting the backend in Development.
- Creating a new migration still requires `dotnet ef migrations add MigrationName --context PaletteDbContext`.
- Manually applying or reverting migrations is still supported when needed.
- Automatic migrations should not be relied on for production deployment. Production schema changes should use a controlled migration process.

---

## 5. Removing or Reverting a Migration

### Case A: Migration Not Yet Applied

If the latest migration has not been applied to your database:

```bash
dotnet ef migrations remove \
  --context PaletteDbContext
```

This removes the latest migration and updates the model snapshot without discarding your entity model changes.

### Case B: Migration Already Applied Locally

Suppose the migration history is:

```text
InitialCreate
AddUserProfilePicture
AddCommentsTable
```

To remove `AddCommentsTable`, first roll back the local database to the previous migration:

```bash
dotnet ef database update AddUserProfilePicture \
  --context PaletteDbContext
```

Then remove the migration:

```bash
dotnet ef migrations remove \
  --context PaletteDbContext
```

To roll back all applied migrations in a disposable local database:

```bash
dotnet ef database update 0 \
  --context PaletteDbContext
```

**Warning:** Reverting a migration may delete tables, columns, or data.

Do not remove or rewrite migrations already shared with other developers or applied to shared, staging, or production databases without coordinating with the team.

---

## 6. Resolving Migration Conflicts Between Branches

Migration conflicts may occur when multiple developers independently modify the database schema.

### Example Scenario

- Developer A creates `AddUserProfilePicture` and merges it into `develop`.
- Developer B creates `AddCommentsTable` on a feature branch based on an older version of `develop`.
- Developer B must update their branch before merging.

In this situation, the two migrations may have been generated from different model snapshots.

### Recommended Workflow

**Step 1: Save your current work.**

Commit or safely stash your changes before making migration changes.

If a conflicting merge has already started, resolve or abort the merge before following the steps below.

**Step 2: Roll back your local migration if it has been applied.**

```bash
dotnet ef database update PreviousMigration \
  --context PaletteDbContext
```

Replace `PreviousMigration` with the migration immediately preceding your own.

Skip this step if your migration has not been applied.

**Step 3: Remove your migration.**

```bash
dotnet ef migrations remove \
  --context PaletteDbContext
```

This is appropriate when your migration is still the latest local migration and has not been shared.

**Step 4: Merge the updated target branch.**

```bash
git fetch origin
git merge origin/develop
```

Resolve any model or configuration conflicts.

**Step 5: Recreate your migration.**

```bash
dotnet ef migrations add AddCommentsTable \
  --context PaletteDbContext
```

The new migration should now be based on the latest model snapshot from `develop`.

**Step 6: Review and apply.**

```bash
dotnet ef database update \
  --context PaletteDbContext
```

Verify that the new migration contains only your intended changes and that the updated database contains changes from both branches.

### Important Notes

- Do not manually rename migration files to resolve conflicts.
- Do not manually reorder migration timestamps.
- Do not blindly accept Git conflict resolutions in the model snapshot.
- Check whether other migrations were merged before generating your own.
- Only remove and recreate migrations that are safe to rewrite.
- If your migration has already been shared or is no longer the latest migration, coordinate a different resolution rather than automatically removing it.

---

## 7. Reviewing Generated Migration Code

EF Core generates migrations automatically, but the generated operations may not always represent the intended database change.

Pay special attention when modifying existing tables containing data.

### Example A: Renaming a Column

Suppose `username` is renamed to `display_name`.

EF Core might generate:

```csharp
migrationBuilder.DropColumn(
    name: "username",
    table: "USERS");

migrationBuilder.AddColumn<string>(
    name: "display_name",
    table: "USERS",
    nullable: true);
```

This could result in data loss.

If the intended change is a rename, use:

```csharp
migrationBuilder.RenameColumn(
    name: "username",
    table: "USERS",
    newName: "display_name");
```

The `Down()` method should reverse the rename.

### Example B: Changing Column Types

Suppose `REVIEWS.rating` changes from integer to decimal.

Before applying the migration, check:

- Whether existing values can be converted safely.
- Whether precision or range changes could cause data loss.
- Whether existing indexes or constraints depend on the column.
- Whether the modification affects other database objects.

Some changes may require data conversion or a staged migration instead of a direct `AlterColumn()` operation.

### Example C: Making a Nullable Column Required

Suppose `USERS.email` previously allowed NULL values.

Changing it to NOT NULL may fail if existing records contain NULL values.

The safer process is:

1. Check existing records.
2. Decide how missing values should be handled.
3. Backfill or correct the data.
4. Apply the NOT NULL constraint.

### Example D: Adding a Foreign Key

When adding a foreign key to an existing table, verify that all existing values reference valid records.

Otherwise, applying the constraint may fail.

### Example E: Database Dependencies

Schema changes may fail because of:

- Existing foreign key constraints
- Unique constraints
- Indexes
- Views or other dependent database objects
- Incompatible data types
- Existing records that violate new constraints

Review the PostgreSQL error and generated migration operations before making corrections.

### Migration Review Checklist

- Review `Up()` and `Down()`.
- Check for unexpected `DropTable()` or `DropColumn()` operations.
- Check column types, nullability, and constraints.
- Check existing data compatibility.
- Verify that indexes and foreign keys remain valid.
- Review potentially destructive operations before applying them.
- Use custom migration operations when EF Core's generated code is insufficient.

---

## 8. Resetting the Local Development Database

If a disposable local database becomes difficult to repair during development, resetting it may be easier than manually fixing migration history.

**Warning: These steps delete local database data. Never perform them against a shared, staging, or production environment.**

### Step 1: Stop PostgreSQL

From the repository root:

```bash
docker compose down
```

### Step 2: Delete the Local Database Volume

```bash
docker compose down -v
```

This removes volumes managed by the current Docker Compose project, including the PostgreSQL data volume.

Make sure the correct Compose project is selected before running this command.

### Step 3: Restart PostgreSQL

```bash
docker compose up -d
```

### Step 4: Reapply All Migrations

From `backend/`, start the backend application:

```bash
dotnet run
```

In the Development environment, the backend automatically applies all pending migrations and recreates the database schema.

Alternatively, migrations can be applied manually without starting the backend:

```bash
dotnet ef database update --context PaletteDbContext
```

### Step 5: Verify in DBeaver

Refresh the database connection and verify that the expected tables are recreated.

**Note:** Resetting the database does not delete migration files. The committed migrations remain available and are applied again to reconstruct the schema.

---

## 9. Recommended Team Workflow

When modifying the database:

1. Update your feature branch with the latest changes from `develop`.
2. Modify the required entity models and EF Core configurations.
3. Generate a migration with a descriptive name.
4. Review `Up()` and `Down()`.
5. Apply the migration locally.
6. Verify the updated schema and existing data.
7. Commit the migration files with the model changes.
8. Check for newly merged migrations before opening or merging your PR.
9. Resolve any migration divergence before merging.
10. Follow the team's existing PR and peer review process.

### Quick Command Reference

| Action | Command |
|---|---|
| Create migration | `dotnet ef migrations add MigrationName --context PaletteDbContext` |
| Apply migrations | `dotnet ef database update --context PaletteDbContext` |
| List migrations | `dotnet ef migrations list --context PaletteDbContext` |
| Remove latest unapplied migration | `dotnet ef migrations remove --context PaletteDbContext` |
| Roll back database | `dotnet ef database update PreviousMigration --context PaletteDbContext` |
| Roll back all migrations | `dotnet ef database update 0 --context PaletteDbContext` |
| Check model changes | `dotnet ef migrations has-pending-model-changes --context PaletteDbContext` |
| Start PostgreSQL | `docker compose up -d` |
| Stop PostgreSQL | `docker compose down` |
| Reset local database volumes | `docker compose down -v` |
| Start backend and auto-apply pending migrations (Development) | `dotnet run` |

All `dotnet ef` commands assume execution from `backend/`. Docker Compose commands should be run from the repository root.

## References

- [EF Core — Managing Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing)
- [EF Core — Applying Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying)
- [EF Core — Team Environments](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/teams)