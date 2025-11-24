# JsMate

A web-based chess application with a modern Clean Architecture backend and vanilla JavaScript frontend.

![jsMate screenshot](https://github.com/ballance/jsmate/blob/master/project_resources/jsmate-screenshot_v_0_5.png "jsMate screenshot")

## Architecture

```
src/
├── JsMate.Api/            # ASP.NET Core Minimal API (presentation layer)
├── JsMate.Application/    # Application services and interfaces
├── JsMate.Domain/         # Domain entities, value objects, and chess rules
├── JsMate.Infrastructure/ # LiteDB persistence implementation
└── JsMate.Tests/          # Unit and integration tests
```

## Features

- Full chess rules implementation (castling, en passant, pawn promotion)
- Check and checkmate detection
- Move validation per piece type
- Undo move functionality
- Visual move highlighting (valid moves vs attacks)
- Click-to-select, click-to-move UX
- Persistent game state via LiteDB

## Tech Stack

| Layer          | Technology                    |
| -------------- | ----------------------------- |
| Frontend       | Vanilla JavaScript (ES6+)     |
| API            | ASP.NET Core 8 Minimal API    |
| Persistence    | LiteDB                        |
| Testing        | xUnit                         |

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/en/download/) (for serving static files)

### Running the Application

#### 1. Start the API

```bash
cd src/JsMate.Api
dotnet run
```

The API will be available at `http://localhost:5096`

#### 2. Serve the Frontend

```bash
npm install
npm start
```

Or use any static file server to serve the `public/` directory.

#### 3. Open in Browser

Navigate to `http://localhost:8080` (or wherever your static server is running).

### Running Tests

```bash
cd src
dotnet test
```

## API Endpoints

| Method | Endpoint                           | Description                          |
| ------ | ---------------------------------- | ------------------------------------ |
| GET    | `/api/board`                       | Create a new board                   |
| GET    | `/api/board/{id}`                  | Get or create board by ID            |
| GET    | `/api/board/{id}/moves/{row}/{col}`| Get valid moves for piece at position|
| POST   | `/api/board/{id}/move`             | Execute a move                       |
| POST   | `/api/board/{id}/undo`             | Undo the last move                   |

## Project History

Originally built over a [time-boxed weekend](https://github.com/ballance/jsmate/graphs/punch-card), now modernized with Clean Architecture and a proper domain model.

## Planning

[Trello Board](https://trello.com/b/rEdr94uM/jsmate-kanban-board)
