# Sprint 1: Logging and Archiving

## What this branch implements

A viewer can mark a TV show as watched, save it to PostgreSQL, and retrieve their history.
The My Shows screen displays real saved records, with loading, empty, error, refresh, and load-more states.
Repeated saves return the existing entry. Reviews remain separate and are not required.

Scope: TV shows, watched status, one log per user/show through this endpoint.
Collections, review forms, editing, deleting, and rewatch events are not implemented.

Authentication and media search are still team dependencies. The local demo uses one
explicit shared demo profile and five sample TV choices. The picker is local testing UI,
not a replacement for Search & Discovery. Nothing is inserted into MEDIA just by browsing it.

## Files

- `backend/Logging/ShowLogService.cs`: save and read database queries.
- `backend/Logging/LoggingAuthentication.cs`: two endpoints, current-user lookup,
  and temporary local-only authentication handler.
- `backend/Program.cs`: registers the service, authentication, and endpoints.
- `frontend/src/app/(app)/(tabs)/my-shows.tsx`: screen state and API calls.
- `frontend/src/components/show-list.tsx`: list display and status messages.
- `frontend/src/services/show-logs.ts`: HTTP requests, timeouts, and errors.
- `frontend/src/types/show-log.ts`: frontend request/response types.
- `frontend/src/constants/logging.ts`: demo picker and API settings.
- `tests/Palette.Api.Tests/ShowLogTests.cs`: PostgreSQL-backed integration tests.

The existing `sample-shows.ts` file is preserved but no longer used by the screen.
The existing logout changes were preserved. The login button now calls the existing mock
signIn function, and the app stack starts at the tabs so the screen can be reached.
Navigation files add My Shows. ESLint configuration/dependencies and a CSS type declaration
allow the frontend checks to run.

## Run the local demo

The verification container is named `palette-logging-check-20261020`.
It uses a separate database, `palette_logging_tests`, bound only to local port 55435.
The password below is only for that disposable local test container, not a team credential.
Its data remains in the container until that container is removed.

Ensure Docker Desktop is running. To start the existing verification container:

```sh
docker start palette-logging-check-20261020
```

If that container has not been created on this computer:

```sh
docker run --detach --name palette-logging-check-20261020 \
  --publish 127.0.0.1:55435:5432 \
  --env POSTGRES_PASSWORD=palette-local-check \
  --env POSTGRES_DB=palette_logging_tests postgres:17
```

In a terminal at the repository root, start the backend:

```sh
ASPNETCORE_ENVIRONMENT=Development \
LoggingDemo__Enabled=true \
ConnectionStrings__DefaultConnection='Host=127.0.0.1;Port=55435;Database=palette_logging_tests;Username=postgres;Password=palette-local-check' \
dotnet run --project backend/Palette.Api.csproj --no-launch-profile --urls http://127.0.0.1:5251
```

In another terminal, from `frontend`:

```sh
EXPO_PUBLIC_API_URL=http://127.0.0.1:5251 \
EXPO_PUBLIC_LOGGING_DEMO=true \
npm run web -- --port 8081
```

Open http://localhost:8081, choose Get started, then My Shows.
Mark a sample show as watched, reload, and reopen My Shows: the entry persists.
A backend/front-end process must be stopped before starting another on the same port.
Use Ctrl+C in the server terminal to stop it. Stop the container with
`docker stop palette-logging-check-20261020` when it is no longer needed.

The local demo handler accepts only the explicit demo token from a loopback connection,
with Development environment and LoggingDemo:Enabled=true. It fails closed otherwise.
Never enable this mode for shared or production deployments. It does not provide real login.
A release frontend hides the test picker even if the environment variable was set.

## Endpoint contract

Both endpoints require an authenticated subject. Neither accepts a user ID from the client.

- `GET /api/me/shows?page=1`: returns `{ items, page, hasMore }`, at most 50 items,
  ordered by logged date and log ID descending. Items include logId, mediaId, tmdbTvId,
  title, status, and loggedDate.
- `POST /api/me/shows`: accepts `{ tmdbTvId, title }`. Saves watched status and returns
  the entry. Validates a positive ID and a nonempty title of at most 300 characters.
  Returns the existing media/log when already present. Logged dates currently use UTC.

Saving uses one database transaction and a PostgreSQL transaction advisory lock per TV ID,
so concurrent requests through this service do not produce duplicate media/log entries.
This does not add a database-wide unique constraint to logs; future writers must reuse
this service or coordinate their own constraints. Existing data is not deleted or deduplicated.
The API checks the ID format but does not contact TMDB to verify metadata; future search
integration supplies the selected show's provider ID and title.

## When authentication and search are ready

Replace the temporary authentication registration with the team's validated token scheme.
Preserve the authorized endpoints and map the authenticated subject to USERS.Auth0Id.
The team must provision the matching USERS record; an unknown real profile receives 404.
Pass the real API access token from the frontend authentication provider into the logging
service. The current mock session string is not a valid real token.

Search & Discovery should call saveWatchedShow with the selected show's TMDB TV ID and title.
Remove the local picker when that integration is complete.

## Verification

From the repository root:

```sh
PALETTE_TEST_DATABASE='Host=127.0.0.1;Port=55435;Database=palette_logging_tests;Username=postgres;Password=palette-local-check' \
dotnet test tests/Palette.Api.Tests/Palette.Api.Tests.csproj
```

The suite accepts only a local database named palette_logging_tests.
Tests create synthetic profiles/shows in that database and never clear a team database.
They cover persistence, separate user histories, unauthorized requests, concurrent duplicate
saves, invalid inputs, shared media records, pagination, and unknown profiles.

From frontend: `npx tsc --noEmit`, `npx expo lint`, and `npx expo export --platform web`.
The existing UseStorageState.ts has two hook-dependency lint warnings.
Web interaction was checked for an empty history, saving, and reloading saved entries.
Native device testing and real authentication/search integration remain outstanding.

## Review and commit

All edits are local to Israel_logging/archiving_task. Nothing was pushed or merged.
In VS Code Source Control, inspect each diff, stage the intended files, then commit.
Do not commit .env files, node_modules, bin/obj, or build output.
