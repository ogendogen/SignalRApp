using SignalRApp.Hubs.Models;
using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Services.Models;

public class Game : IEquatable<Game?>
{
    public Guid Id { get; }

    private readonly FieldStatus[,] _board = new FieldStatus[2, 2];
    private bool _player1Turn = true;
    private Player _player1 = null!;
    private Player _player2 = null!;
    private GameStatus _gameStatus = GameStatus.Unknown;

    public Game(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        Id = Guid.NewGuid();
        //https://github.com/damienbod/AspNetCoreAngularSignalRSecurity?tab=readme-ov-file
    }
    public GameStatus GameStatus
    {
        get
        {
            return _gameStatus;
        }
        set
        {
            if ((_gameStatus == GameStatus.Unknown && (value == GameStatus.Player1Ready || value == GameStatus.Player2Ready)) ||
                (_gameStatus == GameStatus.Player1Ready && value == GameStatus.Player2Ready) ||
                (_gameStatus == GameStatus.Player2Ready && value == GameStatus.Player1Ready) ||
                (_gameStatus == GameStatus.Player1Ready || _gameStatus == GameStatus.Player2Ready && value == GameStatus.InProgress))
            {
                _gameStatus = value;
            }
        }
    }
    public MovementResult Move(string player, int x, int y, FieldStatus fieldStatus)
    {
        if (x < 0 || y < 0 || x > 2 || y > 2)
        {
            return MovementResult.NotAllowed;
        }

        if (_board[x, y] != FieldStatus.Empty)
        {
            return MovementResult.NotAllowed;
        }
        _board[x, y] = fieldStatus;

        var winner = CheckWinner();
        var isDraw = CheckDraw();

        if (winner == Winner.Player1)
        {
            return MovementResult.Player1Wins;
        }

        if (winner == Winner.Player2)
        {
            return MovementResult.Player2Wins;
        }

        if (isDraw)
        {
            return MovementResult.Draw;
        }

        return MovementResult.Allowed;
    }

    private bool CheckDraw()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (_board[x, y] == FieldStatus.Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private Winner CheckWinner()
    {
        if ((_board[0,0] == FieldStatus.X && _board[0,1] == FieldStatus.X && _board[0,2] == FieldStatus.X) ||
            (_board[1,0] == FieldStatus.X && _board[1,1] == FieldStatus.X && _board[1,2] == FieldStatus.X) ||
            (_board[2, 0] == FieldStatus.X && _board[2, 1] == FieldStatus.X && _board[2, 2] == FieldStatus.X) ||
            (_board[0, 0] == FieldStatus.X && _board[1, 0] == FieldStatus.X && _board[2, 0] == FieldStatus.X) ||
            (_board[0, 1] == FieldStatus.X && _board[1, 1] == FieldStatus.X && _board[2, 1] == FieldStatus.X) ||
            (_board[0, 2] == FieldStatus.X && _board[1, 2] == FieldStatus.X && _board[2, 2] == FieldStatus.X) ||
            (_board[0, 0] == FieldStatus.X && _board[1, 1] == FieldStatus.X && _board[2, 2] == FieldStatus.X) ||
            (_board[0, 2] == FieldStatus.X && _board[1, 1] == FieldStatus.X && _board[2, 0] == FieldStatus.X))
        {
            return Winner.Player1;
        }

        if ((_board[0, 0] == FieldStatus.Circle && _board[0, 1] == FieldStatus.Circle && _board[0, 2] == FieldStatus.Circle) ||
            (_board[1, 0] == FieldStatus.Circle && _board[1, 1] == FieldStatus.Circle && _board[1, 2] == FieldStatus.Circle) ||
            (_board[2, 0] == FieldStatus.Circle && _board[2, 1] == FieldStatus.Circle && _board[2, 2] == FieldStatus.Circle) ||
            (_board[0, 0] == FieldStatus.Circle && _board[1, 0] == FieldStatus.Circle && _board[2, 0] == FieldStatus.Circle) ||
            (_board[0, 1] == FieldStatus.Circle && _board[1, 1] == FieldStatus.Circle && _board[2, 1] == FieldStatus.Circle) ||
            (_board[0, 2] == FieldStatus.Circle && _board[1, 2] == FieldStatus.Circle && _board[2, 2] == FieldStatus.Circle) ||
            (_board[0, 0] == FieldStatus.Circle && _board[1, 1] == FieldStatus.Circle && _board[2, 2] == FieldStatus.Circle) ||
            (_board[0, 2] == FieldStatus.Circle && _board[1, 1] == FieldStatus.Circle && _board[2, 0] == FieldStatus.Circle))
        {
            return Winner.Player2;
        }

        return Winner.None;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Game);
    }

    public bool Equals(Game? other)
    {
        return other is not null &&
               Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public static bool operator ==(Game? left, Game? right)
    {
        return EqualityComparer<Game>.Default.Equals(left, right);
    }

    public static bool operator !=(Game? left, Game? right)
    {
        return !(left == right);
    }
}
