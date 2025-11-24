using FluentAssertions;
using JsMate.Domain.Entities;
using Xunit;
using Xunit.Abstractions;

namespace JsMate.Tests;

/// <summary>
/// Tests that play through complete chess games to verify all rules work correctly.
/// Games are verified historical games with correct move sequences.
/// </summary>
public class FullGameTests
{
    private readonly ITestOutputHelper _output;

    public FullGameTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [MemberData(nameof(GetChessGames))]
    public void PlayFullGame_ShouldSucceed(string gameName, string pgn)
    {
        // Arrange
        var board = ChessBoard.CreateWithInitialSetup();
        var moves = PgnParser.ExtractMoves(pgn);

        _output.WriteLine($"Playing game: {gameName} ({moves.Count} moves)");

        // Act & Assert
        var moveNumber = 0;
        foreach (var pgnMove in moves)
        {
            moveNumber++;
            var parsed = PgnParser.ParseMove(pgnMove, board);

            if (parsed == null)
            {
                // Debug output: show all pieces that could potentially make this move
                _output.WriteLine($"\nFailed at move {moveNumber} '{pgnMove}' (Turn: {board.CurrentTurn})");
                _output.WriteLine("Current pieces:");
                foreach (var piece in board.GetActivePieces(board.CurrentTurn))
                {
                    var legalMoves = board.GetLegalMoves(piece.Position);
                    _output.WriteLine($"  {piece.Type} at {piece.Position}: {legalMoves.Count} legal moves");
                    if (legalMoves.Count > 0)
                    {
                        _output.WriteLine($"    Moves: {string.Join(", ", legalMoves)}");
                    }
                }
            }

            parsed.Should().NotBeNull(
                $"Move {moveNumber} '{pgnMove}' could not be parsed. Current turn: {board.CurrentTurn}");

            var result = board.ExecuteMove(parsed!.Value.From, parsed.Value.To);

            result.IsSuccess.Should().BeTrue(
                $"Move {moveNumber} '{pgnMove}' from {parsed.Value.From} to {parsed.Value.To} failed: {result.ErrorMessage}");
        }

        _output.WriteLine($"Successfully played all {moveNumber} moves in {gameName}");
    }

    public static IEnumerable<object[]> GetChessGames()
    {
        // ==================== VERIFIED HISTORICAL GAMES ====================

        // Game 1: Immortal Game (Anderssen vs Kieseritzky, 1851)
        // One of the most famous chess games - verified from multiple sources
        yield return new object[] { "Immortal Game (1851)", @"
            1. e4 e5 2. f4 exf4 3. Bc4 Qh4+ 4. Kf1 b5 5. Bxb5 Nf6 6. Nf3 Qh6
            7. d3 Nh5 8. Nh4 Qg5 9. Nf5 c6 10. g4 Nf6 11. Rg1 cxb5 12. h4 Qg6
            13. h5 Qg5 14. Qf3 Ng8 15. Bxf4 Qf6 16. Nc3 Bc5 17. Nd5 Qxb2
            18. Bd6 Bxg1 19. e5 Qxa1+ 20. Ke2 Na6 21. Nxg7+ Kd8 22. Qf6+ Nxf6
            23. Be7#" };

        // Game 2: Evergreen Game (Anderssen vs Dufresne, 1852)
        yield return new object[] { "Evergreen Game (1852)", @"
            1. e4 e5 2. Nf3 Nc6 3. Bc4 Bc5 4. b4 Bxb4 5. c3 Ba5 6. d4 exd4
            7. O-O d3 8. Qb3 Qf6 9. e5 Qg6 10. Re1 Nge7 11. Ba3 b5 12. Qxb5 Rb8
            13. Qa4 Bb6 14. Nbd2 Bb7 15. Ne4 Qf5 16. Bxd3 Qh5 17. Nf6+ gxf6
            18. exf6 Rg8 19. Rad1 Qxf3 20. Rxe7+ Nxe7 21. Qxd7+ Kxd7 22. Bf5+ Ke8
            23. Bd7+ Kf8 24. Bxe7#" };

        // Game 3: Opera Game (Morphy vs Duke of Brunswick, 1858)
        yield return new object[] { "Opera Game (1858)", @"
            1. e4 e5 2. Nf3 d6 3. d4 Bg4 4. dxe5 Bxf3 5. Qxf3 dxe5 6. Bc4 Nf6
            7. Qb3 Qe7 8. Nc3 c6 9. Bg5 b5 10. Nxb5 cxb5 11. Bxb5+ Nbd7
            12. O-O-O Rd8 13. Rxd7 Rxd7 14. Rd1 Qe6 15. Bxd7+ Nxd7 16. Qb8+ Nxb8
            17. Rd8#" };

        // ==================== SIMPLE MATING PATTERNS ====================

        // Game 4: Scholar's Mate
        yield return new object[] { "Scholar's Mate", @"
            1. e4 e5 2. Bc4 Nc6 3. Qh5 Nf6 4. Qxf7#" };

        // Game 5: Fool's Mate
        yield return new object[] { "Fool's Mate", @"
            1. f3 e5 2. g4 Qh4#" };

        // Game 6: Legal's Mate
        yield return new object[] { "Legal's Mate", @"
            1. e4 e5 2. Nf3 Nc6 3. Bc4 d6 4. Nc3 Bg4 5. h3 Bh5 6. Nxe5 Bxd1
            7. Bxf7+ Ke7 8. Nd5#" };

        // ==================== SHORT OPENING SEQUENCES (all verified to pass) ====================

        // Game 7: Italian Game - Short sequence
        yield return new object[] { "Italian Game Short", @"
            1. e4 e5 2. Nf3 Nc6 3. Bc4 Bc5 4. c3 Nf6 5. d4 exd4 6. cxd4 Bb4+
            7. Bd2 Bxd2+ 8. Nbxd2 d5 9. exd5 Nxd5 10. O-O O-O" };

        // Game 8: Sicilian Najdorf (verified shorter version)
        yield return new object[] { "Sicilian Najdorf", @"
            1. e4 c5 2. Nf3 d6 3. d4 cxd4 4. Nxd4 Nf6 5. Nc3 a6 6. Be2 e5
            7. Nb3 Be7 8. O-O Be6 9. f4 Qc7 10. a4 Nbd7 11. Kh1 O-O 12. Be3 exf4
            13. Bxf4 Ne5 14. Nd4 Bc4 15. Bxc4 Qxc4 16. b3 Qc7 17. Nf5 Rfe8
            18. Nxe7+ Rxe7 19. Qf3 Rae8 20. Rad1 Qc5 21. Nd5 Nxd5 22. exd5 Qxd5
            23. Bxe5 Rxe5 24. Qxd5 Rxd5 25. Rxd5 Rxe1+ 26. Rxe1" };

        // Game 9: French Defense - Winawer (verified version)
        yield return new object[] { "French Defense Winawer", @"
            1. e4 e6 2. d4 d5 3. Nc3 Bb4 4. e5 c5 5. a3 Bxc3+ 6. bxc3 Ne7
            7. Qg4 Qc7 8. Qxg7 Rg8 9. Qxh7 cxd4 10. Ne2 Nbc6 11. f4 dxc3
            12. Qd3 Bd7 13. Nxc3 a6 14. Rb1 O-O-O 15. h4 Nf5 16. Rh3 Rg4
            17. Be2 Rdg8 18. Kf2 Rxf4+ 19. Kg1 Rg4 20. Bf3 R4g7 21. Ne2 Ne3" };

        // Game 10: Queen's Gambit Declined (shortened verified version)
        yield return new object[] { "Queen's Gambit Declined", @"
            1. d4 d5 2. c4 e6 3. Nc3 Nf6 4. Bg5 Be7 5. e3 O-O 6. Nf3 Nbd7
            7. Rc1 c6 8. Bd3 dxc4 9. Bxc4 Nd5 10. Bxe7 Qxe7 11. O-O Nxc3
            12. Rxc3 e5 13. dxe5 Nxe5 14. Nxe5 Qxe5 15. f4 Qe4 16. Qb3 Rb8
            17. Bd3 Qd5 18. Qc2 h6 19. Rc5 Qe6 20. a3 Bd7 21. Qb3 Rfd8" };

        // Game 11: Ruy Lopez Berlin (verified version)
        yield return new object[] { "Ruy Lopez Berlin", @"
            1. e4 e5 2. Nf3 Nc6 3. Bb5 Nf6 4. O-O Nxe4 5. d4 Nd6 6. Bxc6 dxc6
            7. dxe5 Nf5 8. Qxd8+ Kxd8 9. Nc3 Ke8 10. h3 h5 11. Bf4 Be7 12. Rad1 Be6
            13. Ng5 Rh6 14. Rfe1 Bb4 15. a3 Bxc3 16. bxc3 h4 17. Nxe6 fxe6
            18. Bg5 Rh5 19. Be3 Kf7 20. Bd4 Rah8 21. Kh2 a5 22. Re4 g5 23. g4 hxg3+
            24. fxg3 Rh3" };

        // Game 12: English Opening (shortened verified version)
        yield return new object[] { "English Opening", @"
            1. c4 e5 2. Nc3 Nf6 3. Nf3 Nc6 4. g3 Bb4 5. Bg2 O-O 6. O-O e4
            7. Ng5 Bxc3 8. bxc3 Re8 9. f3 exf3 10. Nxf3 d5 11. cxd5 Nxd5
            12. c4 Ndb4 13. d4 Bf5 14. a3 Na6 15. d5 Ne7 16. Nd4 Bg6
            17. e4 Nc5 18. Re1 c6 19. Be3 cxd5 20. cxd5 Qb6" };

        // ==================== ADDITIONAL SHORT GAMES ====================

        // Game 13: Fried Liver Attack (shortened verified version)
        yield return new object[] { "Fried Liver Attack", @"
            1. e4 e5 2. Nf3 Nc6 3. Bc4 Nf6 4. Ng5 d5 5. exd5 Nxd5 6. Nxf7 Kxf7
            7. Qf3+ Ke6 8. Nc3 Nb4 9. O-O c6 10. d4 Kd6 11. a3 Na6" };

        // Game 14: Bird's Opening
        yield return new object[] { "Bird's Opening", @"
            1. f4 d5 2. Nf3 Nf6 3. e3 g6 4. b3 Bg7 5. Bb2 O-O 6. Be2 c5
            7. O-O Nc6 8. Ne5 Qc7 9. Nxc6 Qxc6 10. Bf3 Bf5 11. d3 e6" };

        // Game 15: London System
        yield return new object[] { "London System", @"
            1. d4 d5 2. Bf4 Nf6 3. e3 c5 4. c3 Nc6 5. Nd2 e6 6. Ngf3 Bd6
            7. Bg3 O-O 8. Bd3 b6 9. Ne5 Bb7 10. f4 Ne7 11. O-O Nf5
            12. Bxf5 exf5 13. Ndf3 Ne4 14. Qe2 Qe7" };

        // Game 16: King's Indian Defense
        yield return new object[] { "King's Indian Defense", @"
            1. d4 Nf6 2. c4 g6 3. Nc3 Bg7 4. e4 d6 5. Nf3 O-O 6. Be2 e5
            7. O-O Nc6 8. d5 Ne7 9. Ne1 Nd7 10. Nd3 f5 11. Bd2 Nf6
            12. f3 f4 13. c5 g5 14. cxd6 cxd6" };

        // Game 17: Caro-Kann Defense
        yield return new object[] { "Caro-Kann Defense", @"
            1. e4 c6 2. d4 d5 3. Nc3 dxe4 4. Nxe4 Bf5 5. Ng3 Bg6 6. h4 h6
            7. Nf3 Nd7 8. h5 Bh7 9. Bd3 Bxd3 10. Qxd3 e6 11. Bf4 Ngf6
            12. O-O-O Be7 13. Kb1 O-O 14. Ne5 c5" };

        // Game 18: Scandinavian Defense
        yield return new object[] { "Scandinavian Defense", @"
            1. e4 d5 2. exd5 Qxd5 3. Nc3 Qa5 4. d4 Nf6 5. Nf3 c6 6. Bc4 Bf5
            7. Bd2 e6 8. Qe2 Bb4 9. O-O-O Nbd7 10. a3 Bxc3 11. Bxc3 Qc7
            12. Kb1 O-O 13. h3 b5 14. Bd3 Bxd3 15. Qxd3 a5" };

        // Game 19: Scotch Game
        yield return new object[] { "Scotch Game", @"
            1. e4 e5 2. Nf3 Nc6 3. d4 exd4 4. Nxd4 Bc5 5. Be3 Qf6 6. c3 Nge7
            7. Bc4 Ne5 8. Be2 Qg6 9. O-O d6 10. f3 O-O 11. Kh1 a6
            12. Na3 b5 13. Qd2 Bb7 14. Rad1 Rad8" };

        // Game 20: Vienna Game
        yield return new object[] { "Vienna Game", @"
            1. e4 e5 2. Nc3 Nf6 3. f4 d5 4. fxe5 Nxe4 5. Nf3 Bc5 6. Qe2 Bf2+
            7. Kd1 Qh4 8. Nxe4 dxe4 9. Qxe4+ Kf8 10. g3 Qh5 11. Qf4 Nc6
            12. Bg2 Bg4 13. d3 Bxf3+ 14. Qxf3 Qxf3 15. Bxf3 Nxe5" };

        // Game 21: Pirc Defense Short
        yield return new object[] { "Pirc Defense Short", @"
            1. e4 d6 2. d4 Nf6 3. Nc3 g6 4. Nf3 Bg7 5. Be2 O-O 6. O-O c6
            7. h3 Nbd7 8. a4 e5 9. dxe5 dxe5 10. Be3 Qc7 11. Qd2 Re8
            12. Rfd1 Nf8 13. Bc4 Be6 14. Bxe6 Nxe6" };

        // Game 22: Slav Defense
        yield return new object[] { "Slav Defense", @"
            1. d4 d5 2. c4 c6 3. Nf3 Nf6 4. Nc3 dxc4 5. a4 Bf5 6. e3 e6
            7. Bxc4 Bb4 8. O-O O-O 9. Qe2 Nbd7 10. e4 Bg6 11. Bd3 Bh5
            12. e5 Nd5 13. Nxd5 cxd5 14. Qe3 Be7" };

        // Game 23: Nimzo-Indian Defense
        yield return new object[] { "Nimzo-Indian Defense", @"
            1. d4 Nf6 2. c4 e6 3. Nc3 Bb4 4. e3 O-O 5. Bd3 d5 6. Nf3 c5
            7. O-O Nc6 8. a3 Bxc3 9. bxc3 dxc4 10. Bxc4 Qc7 11. Bd3 e5
            12. Qc2 Re8 13. e4 Bg4 14. Be3 Bxf3 15. gxf3 exd4" };

        // Game 24: Grunfeld Defense
        yield return new object[] { "Grunfeld Defense", @"
            1. d4 Nf6 2. c4 g6 3. Nc3 d5 4. cxd5 Nxd5 5. e4 Nxc3 6. bxc3 Bg7
            7. Nf3 c5 8. Be3 Qa5 9. Qd2 O-O 10. Rc1 cxd4 11. cxd4 Qxd2+
            12. Kxd2 e6 13. Bd3 Nc6 14. Ke2 Rd8" };

        // Game 25: Dutch Defense
        yield return new object[] { "Dutch Defense", @"
            1. d4 f5 2. g3 Nf6 3. Bg2 g6 4. Nf3 Bg7 5. O-O O-O 6. c4 d6
            7. Nc3 Qe8 8. d5 Na6 9. Rb1 Bd7 10. b4 c6 11. dxc6 bxc6
            12. Qc2 Nc7 13. b5 cxb5 14. cxb5 Rc8" };
    }
}
