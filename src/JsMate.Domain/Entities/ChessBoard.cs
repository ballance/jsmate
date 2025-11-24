using JsMate.Domain.Entities.Pieces;
using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities;

/// <summary>
/// Represents a chess board with all pieces and game state.
/// </summary>
public class ChessBoard : IChessBoard
{
    public Guid Id { get; init; }
    public PieceTeam CurrentTurn { get; set; } = PieceTeam.White;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public BoardPosition? EnPassantTarget { get; private set; }

    private readonly List<ChessPiece> _pieces = [];
    private readonly Dictionary<BoardPosition, ChessPiece> _positionIndex = [];
    private readonly Stack<MoveRecord> _moveHistory = new();

    public IReadOnlyList<ChessPiece> Pieces => _pieces.AsReadOnly();
    public IReadOnlyList<MoveRecord> MoveHistory => _moveHistory.ToList();
    public bool CanUndo => _moveHistory.Count > 0;

    public ChessBoard(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
    }

    // For serialization
    public ChessBoard() : this(Guid.NewGuid()) { }

    /// <summary>
    /// Creates a new chess board with the standard initial piece setup.
    /// </summary>
    public static ChessBoard CreateWithInitialSetup(Guid? id = null)
    {
        var board = new ChessBoard(id);
        board.SetupInitialPieces();
        return board;
    }

    private void SetupInitialPieces()
    {
        // Add pawns
        for (var col = 0; col < 8; col++)
        {
            AddPiece(new Pawn(PieceTeam.White, new BoardPosition(6, col), col + 1));
            AddPiece(new Pawn(PieceTeam.Black, new BoardPosition(1, col), col + 1));
        }

        // Add rooks
        AddPiece(new Rook(PieceTeam.White, new BoardPosition(7, 0), 1));
        AddPiece(new Rook(PieceTeam.White, new BoardPosition(7, 7), 2));
        AddPiece(new Rook(PieceTeam.Black, new BoardPosition(0, 0), 1));
        AddPiece(new Rook(PieceTeam.Black, new BoardPosition(0, 7), 2));

        // Add knights
        AddPiece(new Knight(PieceTeam.White, new BoardPosition(7, 1), 1));
        AddPiece(new Knight(PieceTeam.White, new BoardPosition(7, 6), 2));
        AddPiece(new Knight(PieceTeam.Black, new BoardPosition(0, 1), 1));
        AddPiece(new Knight(PieceTeam.Black, new BoardPosition(0, 6), 2));

        // Add bishops
        AddPiece(new Bishop(PieceTeam.White, new BoardPosition(7, 2), 1));
        AddPiece(new Bishop(PieceTeam.White, new BoardPosition(7, 5), 2));
        AddPiece(new Bishop(PieceTeam.Black, new BoardPosition(0, 2), 1));
        AddPiece(new Bishop(PieceTeam.Black, new BoardPosition(0, 5), 2));

        // Add queens
        AddPiece(new Queen(PieceTeam.White, new BoardPosition(7, 3), 1));
        AddPiece(new Queen(PieceTeam.Black, new BoardPosition(0, 3), 1));

        // Add kings
        AddPiece(new King(PieceTeam.White, new BoardPosition(7, 4), 1));
        AddPiece(new King(PieceTeam.Black, new BoardPosition(0, 4), 1));
    }

    public void AddPiece(ChessPiece piece)
    {
        _pieces.Add(piece);
        if (piece.IsActive)
        {
            _positionIndex[piece.Position] = piece;
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPieces(IEnumerable<ChessPiece> pieces)
    {
        _pieces.Clear();
        _positionIndex.Clear();
        foreach (var piece in pieces)
        {
            AddPiece(piece);
        }
    }

    public ChessPiece? GetPieceAt(BoardPosition position)
    {
        return _positionIndex.TryGetValue(position, out var piece) ? piece : null;
    }

    public IReadOnlyList<ChessPiece> GetActivePieces()
    {
        return _pieces.Where(p => p.IsActive).ToList();
    }

    public IReadOnlyList<ChessPiece> GetActivePieces(PieceTeam team)
    {
        return _pieces.Where(p => p.IsActive && p.Team == team).ToList();
    }

    public bool IsEmpty(BoardPosition position)
    {
        return !_positionIndex.ContainsKey(position);
    }

    public bool IsInCheck(PieceTeam team)
    {
        var king = _pieces.FirstOrDefault(p => p.IsActive && p.Type == PieceType.King && p.Team == team);
        if (king == null)
            return false;

        var enemyTeam = team == PieceTeam.White ? PieceTeam.Black : PieceTeam.White;
        var enemyPieces = GetActivePieces(enemyTeam);

        return enemyPieces.Any(p => p.GetAttackPositions(this).Contains(king.Position));
    }

    /// <summary>
    /// Executes a move on the board.
    /// </summary>
    public MoveResult ExecuteMove(BoardPosition from, BoardPosition to)
    {
        var piece = GetPieceAt(from);
        if (piece == null)
            return MoveResult.Failure("No piece at the starting position.");

        if (piece.Team != CurrentTurn)
            return MoveResult.Failure("It's not this team's turn.");

        var validMoves = piece.GetValidMoves(this);
        if (!validMoves.Contains(to))
            return MoveResult.Failure("Invalid move for this piece.");

        // Record state for undo
        var previousEnPassantTarget = EnPassantTarget;
        var pieceHadMoved = GetPieceHadMoved(piece);

        // Handle capture
        var capturedPiece = GetPieceAt(to);
        ChessPiece? enPassantCapturedPawn = null;
        BoardPosition? enPassantCapturePos = null;

        // Check for en passant capture
        if (piece is Pawn && to == EnPassantTarget)
        {
            var capturedPawnRow = piece.Team == PieceTeam.White ? to.Row + 1 : to.Row - 1;
            enPassantCapturePos = new BoardPosition(capturedPawnRow, to.Col);
            enPassantCapturedPawn = GetPieceAt(enPassantCapturePos.Value);
            if (enPassantCapturedPawn != null)
            {
                enPassantCapturedPawn.Capture();
                _positionIndex.Remove(enPassantCapturePos.Value);
            }
        }

        if (capturedPiece != null)
        {
            capturedPiece.Capture();
            _positionIndex.Remove(to);
        }

        // Move the piece
        _positionIndex.Remove(from);
        piece.MoveTo(to);
        _positionIndex[to] = piece;

        // Handle castling - move the rook
        BoardPosition? rookFrom = null;
        BoardPosition? rookTo = null;
        if (piece is King && Math.Abs(from.Col - to.Col) == 2)
        {
            (rookFrom, rookTo) = HandleCastlingRookMove(from, to);
        }

        // Check if move puts own king in check (invalid)
        if (IsInCheck(CurrentTurn))
        {
            // Undo the move
            piece.MoveTo(from);
            _positionIndex.Remove(to);
            _positionIndex[from] = piece;

            if (capturedPiece != null)
            {
                capturedPiece.Restore();
                _positionIndex[to] = capturedPiece;
            }

            if (enPassantCapturedPawn != null && enPassantCapturePos != null)
            {
                enPassantCapturedPawn.Restore();
                _positionIndex[enPassantCapturePos.Value] = enPassantCapturedPawn;
            }

            return MoveResult.Failure("Move would leave king in check.");
        }

        // Determine captured piece info
        var actualCaptured = capturedPiece ?? enPassantCapturedPawn;

        // Record move for undo
        var moveRecord = new MoveRecord
        {
            From = from,
            To = to,
            MovedPieceType = piece.Type,
            MovedPieceTeam = piece.Team,
            MovedPieceNumber = piece.PieceNumber,
            CapturedPieceType = actualCaptured?.Type,
            CapturedPieceTeam = actualCaptured?.Team,
            CapturedPieceNumber = actualCaptured?.PieceNumber,
            CapturedPiecePosition = enPassantCapturePos ?? (capturedPiece != null ? to : null),
            WasCastling = rookFrom != null,
            RookFrom = rookFrom,
            RookTo = rookTo,
            PreviousEnPassantTarget = previousEnPassantTarget,
            PieceHadMoved = pieceHadMoved
        };
        _moveHistory.Push(moveRecord);

        // Set en passant target if pawn moved two squares
        EnPassantTarget = null;
        if (piece is Pawn && Math.Abs(from.Row - to.Row) == 2)
        {
            var targetRow = (from.Row + to.Row) / 2;
            EnPassantTarget = new BoardPosition(targetRow, from.Col);
        }

        // Switch turns
        CurrentTurn = CurrentTurn == PieceTeam.White ? PieceTeam.Black : PieceTeam.White;
        UpdatedAt = DateTime.UtcNow;

        return MoveResult.Success(new Move(from, to, actualCaptured != null));
    }

    private bool GetPieceHadMoved(ChessPiece piece)
    {
        return piece switch
        {
            Pawn p => p.HasMoved,
            Rook r => r.HasMoved,
            King k => k.HasMoved,
            _ => false
        };
    }

    private (BoardPosition? RookFrom, BoardPosition? RookTo) HandleCastlingRookMove(BoardPosition kingFrom, BoardPosition kingTo)
    {
        var backRank = kingFrom.Row;

        if (kingTo.Col == 6) // Kingside
        {
            var rookFrom = new BoardPosition(backRank, 7);
            var rookTo = new BoardPosition(backRank, 5);
            var rook = GetPieceAt(rookFrom);
            if (rook != null)
            {
                _positionIndex.Remove(rookFrom);
                rook.MoveTo(rookTo);
                _positionIndex[rookTo] = rook;
            }
            return (rookFrom, rookTo);
        }
        else if (kingTo.Col == 2) // Queenside
        {
            var rookFrom = new BoardPosition(backRank, 0);
            var rookTo = new BoardPosition(backRank, 3);
            var rook = GetPieceAt(rookFrom);
            if (rook != null)
            {
                _positionIndex.Remove(rookFrom);
                rook.MoveTo(rookTo);
                _positionIndex[rookTo] = rook;
            }
            return (rookFrom, rookTo);
        }

        return (null, null);
    }

    /// <summary>
    /// Checks if the given team is in checkmate (in check with no legal moves).
    /// </summary>
    public bool IsCheckmate(PieceTeam team)
    {
        if (!IsInCheck(team))
            return false;

        return !HasAnyLegalMoves(team);
    }

    /// <summary>
    /// Checks if the given team is in stalemate (not in check but no legal moves).
    /// </summary>
    public bool IsStalemate(PieceTeam team)
    {
        if (IsInCheck(team))
            return false;

        return !HasAnyLegalMoves(team);
    }

    /// <summary>
    /// Checks if the given team has any legal moves available.
    /// </summary>
    private bool HasAnyLegalMoves(PieceTeam team)
    {
        var teamPieces = GetActivePieces(team);

        foreach (var piece in teamPieces)
        {
            var legalMoves = GetLegalMoves(piece.Position);
            if (legalMoves.Count > 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the current game state for the team whose turn it is.
    /// </summary>
    public GameState GetGameState()
    {
        if (IsCheckmate(CurrentTurn))
            return GameState.Checkmate;

        if (IsStalemate(CurrentTurn))
            return GameState.Stalemate;

        if (IsInCheck(CurrentTurn))
            return GameState.Check;

        return GameState.InProgress;
    }

    /// <summary>
    /// Gets valid moves for the piece at the given position, filtering out moves that would leave king in check.
    /// </summary>
    public IReadOnlyList<BoardPosition> GetLegalMoves(BoardPosition position)
    {
        var piece = GetPieceAt(position);
        if (piece == null || !piece.IsActive)
            return [];

        var candidateMoves = piece.GetValidMoves(this);
        var legalMoves = new List<BoardPosition>();

        foreach (var move in candidateMoves)
        {
            // Simulate the move to see if it leaves king in check
            var capturedPiece = GetPieceAt(move);
            var originalPosition = piece.Position;

            // Temporarily make the move
            _positionIndex.Remove(originalPosition);
            if (capturedPiece != null)
            {
                _positionIndex.Remove(move);
                capturedPiece.Capture(); // Mark as inactive so IsInCheck ignores it
            }

            piece.MoveTo(move);
            _positionIndex[move] = piece;

            // Check if king is in check
            var wouldBeInCheck = IsInCheck(piece.Team);

            // Undo the move
            _positionIndex.Remove(move);
            piece.MoveTo(originalPosition);
            _positionIndex[originalPosition] = piece;

            if (capturedPiece != null)
            {
                _positionIndex[move] = capturedPiece;
                capturedPiece.Restore(); // Restore active state
            }

            if (!wouldBeInCheck)
                legalMoves.Add(move);
        }

        return legalMoves;
    }

    /// <summary>
    /// Undoes the last move, restoring the board to its previous state.
    /// </summary>
    public bool UndoMove()
    {
        if (!CanUndo)
            return false;

        var record = _moveHistory.Pop();

        // Find the moved piece at its current position
        var piece = GetPieceAt(record.To);
        if (piece == null)
            return false;

        // Move the piece back to its original position
        _positionIndex.Remove(record.To);
        piece.MoveTo(record.From);
        _positionIndex[record.From] = piece;

        // Restore the piece's HasMoved state
        RestorePieceHasMoved(piece, record.PieceHadMoved);

        // Restore captured piece if any
        if (record.CapturedPieceType.HasValue && record.CapturedPiecePosition.HasValue)
        {
            var capturedPiece = _pieces.FirstOrDefault(p =>
                !p.IsActive &&
                p.Type == record.CapturedPieceType.Value &&
                p.Team == record.CapturedPieceTeam!.Value &&
                p.PieceNumber == record.CapturedPieceNumber);

            if (capturedPiece != null)
            {
                capturedPiece.Restore();
                capturedPiece.MoveTo(record.CapturedPiecePosition.Value);
                _positionIndex[record.CapturedPiecePosition.Value] = capturedPiece;
            }
        }

        // Undo castling rook move if applicable
        if (record.WasCastling && record.RookFrom.HasValue && record.RookTo.HasValue)
        {
            var rook = GetPieceAt(record.RookTo.Value);
            if (rook is Rook castlingRook)
            {
                _positionIndex.Remove(record.RookTo.Value);
                castlingRook.MoveTo(record.RookFrom.Value);
                _positionIndex[record.RookFrom.Value] = castlingRook;
                castlingRook.ResetHasMoved(false);
            }
        }

        // Restore en passant target
        EnPassantTarget = record.PreviousEnPassantTarget;

        // Switch turn back
        CurrentTurn = CurrentTurn == PieceTeam.White ? PieceTeam.Black : PieceTeam.White;
        UpdatedAt = DateTime.UtcNow;

        return true;
    }

    private void RestorePieceHasMoved(ChessPiece piece, bool hadMoved)
    {
        switch (piece)
        {
            case Pawn pawn:
                pawn.ResetHasMoved(hadMoved);
                break;
            case Rook rook:
                rook.ResetHasMoved(hadMoved);
                break;
            case King king:
                king.ResetHasMoved(hadMoved);
                break;
        }
    }

    /// <summary>
    /// Clears the move history stack.
    /// </summary>
    public void ClearMoveHistory()
    {
        _moveHistory.Clear();
    }
}

/// <summary>
/// Result of executing a move.
/// </summary>
public record MoveResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public Move? ExecutedMove { get; init; }

    public static MoveResult Success(Move move) => new() { IsSuccess = true, ExecutedMove = move };
    public static MoveResult Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };
}
