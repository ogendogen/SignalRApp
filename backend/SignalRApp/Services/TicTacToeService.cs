using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models;
using SignalRApp.Services.Models.Enums;
using System.Numerics;

namespace SignalRApp.Services;

public class TicTacToeService : ITicTacToeService
{
    private readonly Dictionary<string, Game> _games = new Dictionary<string, Game>();

    public void EndGame(string player1, string player2)
    {
        var groupName = GetPlayersGroupName(player1, player2);
        var isGameFound = _games.TryGetValue(groupName, out Game? game);
        if (isGameFound)
        {
            _games.Remove(groupName);
        }
    }

    public MovementResult Move(string groupName, string player, int x, int y, FieldStatus fieldStatus)
    {
        var isGameFound = _games.TryGetValue(groupName, out Game? game);
        if (!isGameFound)
        {
            return MovementResult.GameNotFound;
        }

        return game!.Move(player, x, y, fieldStatus);
    }

    public void StartGame(string player1, string player2)
    {
        if (_games.TryGetValue(player1, out Game? game))
        {
            // do sth
        }

        _games.Add(GetPlayersGroupName(player1, player2), new Game(player1, player2));
    }

    private string GetPlayersGroupName(string player1, string player2)
    {
        return $"{player1} {player2}";
    }
}
