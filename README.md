# GalaxyQuest

GalaxyQuest is a lightweight ASP.NET Core backend for a space strategy game prototype. It generates a procedural galaxy, exposes star systems and planets through REST endpoints, and manages users and units that can move across the galaxy.

## Overview

The project is designed as a backend foundation for a sci-fi resource-management game. It provides in-memory game state, procedural map generation, and a small API surface for interacting with users, star systems, planets, and units.

The core idea is simple: generate a galaxy, let players exist in it, and expose a clean API to fetch systems, move units, and prepare for future gameplay features such as buildings and resource production.

## Features

- Procedural galaxy generation using a shared core library
- Retrieval of star systems and planets
- User creation and lookup
- Unit creation and movement between systems and planets
- Planet resource information exposure
- Building-related endpoint scaffolding for future expansion
- Swagger/OpenAPI documentation in development mode

## Tech stack

- Language: C#
- Framework: ASP.NET Core 7
- API: REST with JSON responses
- Documentation: Swagger / OpenAPI
- Testing: xUnit + ASP.NET Core integration test support
- Shared logic: `Shard.Shared.Core`

## Project structure

```text
.
├── GalaxyQuest/                  # Main ASP.NET Core application
│   ├── Controllers/              # HTTP endpoints
│   ├── Entities/                 # Domain models
│   ├── EntitiesDTO/              # DTOs returned by the API
│   ├── Services/                 # Repositories and map generation logic
│   ├── Utilities/                # Response helpers
│   ├── Program.cs                # Application bootstrap
│   ├── appsettings.json          # Main configuration
│   ├── appsettings.Development.json
│   └── GalaxyQuest.csproj        # API project file
├── GalaxyQuest.IntegrationTests/ # Integration tests
├── Shard.Shared.Core/            # Shared game-generation and time utilities
├── tests/                       # Sample test files/resources
├── shard.sln                    # .NET solution file
├── coverlet.runsettings         # Coverage settings
├── extractTestResults.sh        # Test result extraction helper
├── .gitlab-ci.yml               # CI configuration
├── README.md                    # Project documentation
├── .dockerignore                # Docker ignore file
├── .gitignore                   # Git ignore rules
└── .vscode/                     # Editor configuration
```

## Architecture

### Main application
The main application lives in the `GalaxyQuest` project. It is an ASP.NET Core web application that registers services and exposes controllers through `Program.cs`.

### Services and repositories
The project uses a small service layer to centralize in-memory game state:

- `MapFactory`: converts generated galaxy data into DTOs
- `MapRepository`: stores systems and planets and provides lookup methods
- `UserRepository`: stores players and their units

### Domain model
The domain is composed of entities such as:

- `User`
- `Unit`
- `Planet`
- `StarSystem`
- `Location`
- `Building`

DTOs are used to expose public data in a structured way without leaking internal structures.

### Shared core generation
The `Shard.Shared.Core` project contains the procedural generation logic used to produce the galaxy. It includes map specification models, random generation utilities, and time/clock abstractions used by the gameplay logic.

## API endpoints

### Systems and planets
- `GET /Systems` — retrieves all systems
- `GET /Systems/{systemName}` — retrieves one system
- `GET /Systems/{systemName}/planets` — retrieves all planets in a system
- `GET /Systems/{systemName}/planets/{planetName}` — retrieves one planet

### Users
- `PUT /Users/{id}` — creates a user
- `GET /Users/{id}` — fetches a user

### Units
- `GET /users/{userId}/Units` — lists the user's units
- `GET /users/{userId}/Units/{unitId}` — gets one unit
- `PUT /users/{userId}/Units/{unitId}` — updates a unit movement/action
- `GET /users/{userId}/Units/{unitId}/location` — fetches the unit's location

### Buildings
- `POST /users/{userId}/Buildings` — creates a building for a user, notably for a builder unit

## Running the project

### Prerequisites

- .NET SDK 7 or newer

### Restore and run

```bash
git clone https://github.com/JeremyRbs/GalaxyQuest.git
cd GalaxyQuest
dotnet restore
dotnet build
dotnet run --project GalaxyQuest
```

In development mode, Swagger is enabled and can usually be opened from the terminal output, for example:

```text
https://localhost:<port>/swagger
```

## Testing

The repository includes integration tests in the `GalaxyQuest.IntegrationTests` project.

Run the suite with:

```bash
dotnet test
```

## Current status

GalaxyQuest is a game backend prototype rather than a full production game. It focuses on a small gameplay loop based on in-memory state, procedural map generation, and API-driven interaction. The project is a solid foundation for expanding into deeper mechanics such as:

- real resource production and consumption,
- building construction and upgrades,
- combat or exploration systems,
- persistence and database integration,
- more robust validation and security.

## Notes

The project is intentionally lightweight and easy to explore. It is especially useful as a learning project or a starting point for a multi-entity sci-fi strategy backend.
