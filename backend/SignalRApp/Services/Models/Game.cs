using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Services.Models;

public class Game
{
    private readonly FieldStatus[,] _board = new FieldStatus[2, 2];
    private bool _player1Turn = true;
    private string _player1Name = null!;
    private string _player2Name = null!;
    public Game(string player1, string player2)
    {
        _player1Name = player1;
        _player2Name = player2;
        Name = $"{player1} {player2}";
        //https://github.com/damienbod/AspNetCoreAngularSignalRSecurity?tab=readme-ov-file
    }
    public string Name { get; set; }
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
}
