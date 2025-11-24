using JsMate.Domain.Entities;
using JsMate.Domain.Entities.Pieces;
using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Infrastructure.Persistence.Models;

/// <summary>
/// Document model for persisting chess board to LiteDB.
/// </summary>
public class ChessBoardDocument
{
    public Guid Id { get; set; }
    public string CurrentTurn { get; set; } = "White";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ChessPieceDocument> Pieces { get; set; } = [];

    public static ChessBoardDocument FromDomain(ChessBoard board)
    {
        return new ChessBoardDocument
        {
            Id = board.Id,
            CurrentTurn = board.CurrentTurn.ToString(),
            CreatedAt = board.CreatedAt,
            UpdatedAt = board.UpdatedAt,
            Pieces = board.Pieces.Select(ChessPieceDocument.FromDomain).ToList()
        };
    }

    public ChessBoard ToDomain()
    {
        var turn = Enum.Parse<PieceTeam>(CurrentTurn);

        var board = new ChessBoard(Id)
        {
            CreatedAt = CreatedAt,
            CurrentTurn = turn
        };

        var pieces = Pieces.Select(p => p.ToDomain()).ToList();
        board.SetPieces(pieces);

        return board;
    }
}

/// <summary>
/// Document model for persisting chess pieces to LiteDB.
/// </summary>
public class ChessPieceDocument
{
    public Guid Id { get; set; }
    public string PieceType { get; set; } = "";
    public string Team { get; set; } = "";
    public int Row { get; set; }
    public int Col { get; set; }
    public bool IsActive { get; set; }
    public int PieceNumber { get; set; }
    public bool HasMoved { get; set; }

    public static ChessPieceDocument FromDomain(ChessPiece piece)
    {
        var doc = new ChessPieceDocument
        {
            Id = piece.Id,
            PieceType = piece.Type.ToString(),
            Team = piece.Team.ToString(),
            Row = piece.Position.Row,
            Col = piece.Position.Col,
            IsActive = piece.IsActive,
            PieceNumber = piece.PieceNumber
        };

        // Track HasMoved for pieces that need it
        doc.HasMoved = piece switch
        {
            Pawn pawn => pawn.HasMoved,
            Rook rook => rook.HasMoved,
            King king => king.HasMoved,
            _ => false
        };

        return doc;
    }

    public ChessPiece ToDomain()
    {
        var team = Enum.Parse<PieceTeam>(Team);
        var position = new BoardPosition(Row, Col);

        ChessPiece piece = PieceType switch
        {
            "Pawn" => new Pawn(team, position, PieceNumber),
            "Knight" => new Knight(team, position, PieceNumber),
            "Bishop" => new Bishop(team, position, PieceNumber),
            "Rook" => new Rook(team, position, PieceNumber),
            "Queen" => new Queen(team, position, PieceNumber),
            "King" => new King(team, position, PieceNumber),
            _ => throw new InvalidOperationException($"Unknown piece type: {PieceType}")
        };

        if (!IsActive)
        {
            piece.Capture();
        }

        return piece;
    }
}
