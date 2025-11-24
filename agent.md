# JsMate - Chess Application

A web-based chess application with Clean Architecture .NET 8 backend and vanilla JavaScript frontend.

## Quick Reference

```bash
# Build & Run Backend
cd src && dotnet build && dotnet run --project JsMate.Api

# Run Tests
cd src && dotnet test

# Serve Frontend (requires npm install first)
npm start  # http://localhost:9995

# API: http://localhost:5096
# Swagger: http://localhost:5096/swagger
```

## Architecture

Clean Architecture with five layers:

```
src/
├── JsMate.Domain/          # Domain entities, value objects (no dependencies)
├── JsMate.Application/     # Services, interfaces, DTOs
├── JsMate.Infrastructure/  # LiteDB persistence
├── JsMate.Api/             # ASP.NET Core Minimal APIs
└── JsMate.Tests/           # xUnit + FluentAssertions
```

**Key patterns:**
- Domain layer has zero external dependencies
- Repository pattern for persistence abstraction
- Value objects as record structs (BoardPosition, Move)
- Factory methods (ChessBoard.CreateWithInitialSetup)

## Tech Stack

- **Backend:** .NET 8, ASP.NET Core Minimal APIs, LiteDB, Serilog
- **Frontend:** Vanilla JS (ES6+), jQuery 1.12.2, CSS3
- **Testing:** xUnit, FluentAssertions

## Code Conventions

- **C#:** Nullable enabled, implicit usings, records for DTOs
- **Fields:** `_camelCase` prefix
- **Methods/Properties:** PascalCase
- **Async-first:** All service/repository methods are async

## Domain Model

**Core entities:**
- `ChessBoard` - Aggregate root with game state, move execution, undo
- `ChessPiece` - Abstract base for King, Queen, Rook, Bishop, Knight, Pawn

**Value objects:**
- `BoardPosition` - Coordinates (0-7, 0-7), algebraic notation support
- `Move` / `MoveRecord` - Move data and undo state

**Key methods:**
- `ExecuteMove(from, to)` - Validates and executes
- `GetLegalMoves(position)` - Moves that don't leave king in check
- `UndoMove()` - Full state restoration

## API Endpoints

```
GET    /api/board                        # New board
GET    /api/board/{id}                   # Get/create by ID
GET    /api/board/{id}/moves/{row}/{col} # Valid moves
POST   /api/board/{id}/move              # Execute move
POST   /api/board/{id}/undo              # Undo last move
GET    /health                           # Health check
```

## Testing

```bash
dotnet test                                    # All tests
dotnet test --filter "ClassName=ChessBoardTests"  # Specific class
```

Tests cover domain logic, piece movement, and full game scenarios via PGN parsing.
