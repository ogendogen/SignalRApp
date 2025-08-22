using SignalRApp.Hubs.Models;
using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models;
using SignalRApp.Services.Models.Enums;
using System.Numerics;

namespace SignalRApp.Services;

public class TicTacToeService : ITicTacToeService
{
    private readonly HashSet<Game> _games = new();

    public void EndGame(string player1, string player2)
    {
        throw new NotImplementedException();
        //var groupName = GetPlayersGroupName(player1, player2);
        //var isGameFound = _games.TryGetValue(groupName, out Game? game);
        //if (isGameFound)
        //{
        //    _games.Remove(groupName);
        //}
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

    public Guid StartGame(Player player1, Player player2)
    {
        var game = new Game(player1, player2);
        _games.Add(game);
        return game.Id;
    }

    private string GetPlayersGroupName(string player1, string player2)
    {
        return $"{player1} {player2}";
    }
}
