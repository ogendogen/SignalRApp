using SignalRApp.Hubs.Models;
using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models;
using SignalRApp.Services.Models.Enums;
using System.Numerics;

namespace SignalRApp.Services;

public class TicTacToeService : ITicTacToeService
{
    private readonly HashSet<Game> _games = new();

    private void EndGame(Game game)
    {
        _games.Remove(game);
    }

    public MovementResult Move(Guid gameId, Player player, int x, int y, FieldStatus fieldStatus)
    {
        var game = _games.First(g => g.Id == gameId);
        var movementResult = game.Move(player.Name, x, y, fieldStatus);

        if (movementResult == MovementResult.Player1Wins || movementResult == MovementResult.Player2Wins || movementResult == MovementResult.Draw)
        {
            EndGame(game);
        }

        return movementResult;
    }

    public Guid StartGame(Player player1, Player player2)
    {
        var game = new Game(player1, player2);
        _games.Add(game);
        return game.Id;
    }

    public bool IsGameExists(Guid gameId)
    {
        var game = _games.FirstOrDefault(g => g.Id == gameId);
        return game is not null;
    }
}
