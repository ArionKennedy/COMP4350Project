# COMP4350 Project - Palette

Palette is a media tracking and discovery platform that allows users to track, review, organize, and discover movies, books, games, music, and other forms of entertainment.

The platform is designed around different ways people engage with media. Some users enjoy reviewing and sharing their opinions, some are looking for recommendations, and others want to maintain a personal archive of what they have experienced.

Palette brings these activities together in one place and helps users build a picture of their own media taste, or their personal “palette.”

## Core Features

Palette currently plans to include the following six core features:

- **[Authentication](https://github.com/ArionKennedy/COMP4350Project/issues/25)**
  - User account creation and login
  - Secure access to personal data

- **[Search & Discovery](https://github.com/ArionKennedy/COMP4350Project/issues/26)**
  - Search for movies, books, games, music, and other supported media
  - Browse and discover media through relevant categories, creators, series, or other relationships

- **[Logging & Archiving](https://github.com/ArionKennedy/COMP4350Project/issues/27)**
  - Save watched, read, played, or listened-to media
  - Maintain a personal history of media activity
  - Organize saved media into collections or records

- **[Recommendations](https://github.com/ArionKennedy/COMP4350Project/issues/28)**
  - Recommend what users may want to watch, read, play, or listen to next
  - Use user activity, community data, and relationships between media to support recommendations
  - Support specialized recommendations such as canonical or release-order viewing

- **[Sharing & Social Features](https://github.com/ArionKennedy/COMP4350Project/issues/29)**
  - Rate and review media
  - Participate in discussions
  - Share media activity, opinions, and recommendations with other users

- **[Analytics & Visualizations](https://github.com/ArionKennedy/COMP4350Project/issues/30)**
  - View statistics about personal media consumption
  - Track information such as total activity time and unique directors, authors, or game studios
  - Generate graphical summaries and visualizations similar to Spotify Wrapped or Receiptify


## Technology Stack

The current planned technology stack is:

### Frontend

- React Native
- Expo

### Backend

- ASP.NET
- Docker

### Data

- Primary database: PostgreSQL
- Redis for caching recent queries
- Docker

For the database schema, ER diagram, relationships, and data storage strategy, see [Database Design](Docs/database.md).

### Authentication and External Services

- Auth0 for user authentication
- External Media APIs to reduce storage requirements

### Hosting

- Cloud hosting provider: TBD
- AWS is currently being considered

React Native with Expo will be used for cross-platform mobile development, while ASP.NET will provide the backend application logic and APIs. PostgreSQL will serve as the primary database, with Redis used as a local server cache for frequently accessed or recent queries. Auth0 will handle user authentication, while external media APIs will be used where appropriate to reduce storage requirements. Docker will be used to containerize the different components of the system. The final cloud hosting provider is still to be determined, with AWS currently under consideration.  

Technology choices are preliminary and may evolve during development.  
For more details and future updates, see [System Architecture](Docs/architecture.md).

## Project Documentation

Important project documentation:

- [Team Working Agreement](Docs/Team%20Working%20Agreement.pdf)
- [Sprint 0](Docs/sprint0.md)
- [Sprint 1](Docs/sprint1.md)
- [System Architecture](Docs/architecture.md)
- [Database Design](Docs/database.md)
- [Process and Protocols](Docs/Process%20and%20Protocols.md)
- [Local Setup](Docs/Local%20Setup.md)
- [Database Migrations](Docs/db-migration.md)

## Team

- Junyi Hu
- William Gore
- Arion Kennedy
- Israel Ijiekhuamen
- Alexandre Brakel
- Binal Chhodavadiya

## Project Status

Palette is currently in Sprint 1, with active development underway. The initial project structure, database setup, and frontend-backend communication have been established. Development of the core features is ongoing.

Technology choices and implementation details may evolve as development progresses.

## Development Setup

For complete setup instructions, including frontend, backend, Docker, PostgreSQL, environment variables, and database migrations, see the [Local Setup Guide](Docs/Local%20Setup.md).

### Prerequisites

- Node.js and npm
- .NET SDK 10
- Docker Desktop (or Docker Engine with Compose)
- Expo Go (optional, for testing on a physical device)

### Quick Start

**1. Install dependencies**

From the project root:

```bash
npm install
cd frontend
npm ci
cd ..
dotnet restore backend/Palette.Api.csproj
```

**2. Configure the environment**

Copy the frontend environment template:

```bash
cp frontend/.env.example frontend/.env.local
```

Configure the PostgreSQL database and backend connection string according to the [Local Setup Guide](Docs/Local%20Setup.md).

Make sure the database is running before starting the backend.

**3. Start the application**

From the project root:

```bash
npm start
```

This starts both the Expo frontend and ASP.NET Core backend.

To stop both services, press `Ctrl+C`.

### Running Services Separately

The frontend and backend can also be started independently in separate terminals.

**Frontend:**

```bash
cd frontend
npx expo start
```

**Backend:**

```bash
cd backend
dotnet run
```

### Repository Structure

```text
COMP4350Project/
├── backend/             # ASP.NET Core API
├── frontend/            # React Native / Expo application
├── Docs/                # Project documentation
├── package.json         # Development startup scripts
├── package-lock.json
└── README.md
```

The frontend and backend are maintained as separate projects and can be run together using npm start from the repository root, or independently.

The structure may evolve as development continues.
