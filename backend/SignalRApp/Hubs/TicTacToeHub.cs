using Microsoft.AspNetCore.SignalR;
using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models.Enums;
using System.Threading.Tasks;

namespace SignalRApp.Hubs;

public class TicTacToeHub : Hub
{
    private readonly IGroupsService _groupsService;
    private readonly ITicTacToeService _ticTacToeService;

    public TicTacToeHub(IGroupsService groupsService, ITicTacToeService ticTacToeService)
    {
        _groupsService = groupsService;
        _ticTacToeService = ticTacToeService;
    }

    public async Task StartGame(string player1, string player2)
    {
        var groupName = GetPlayersGroupName(player1, player2);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("GameStarted", player1, player2);
    }

    public async Task Move(string player, int row, int col)
    {
        // which player -> field status
        var groupName = GetPlayersActiveGroupName(player);
        var fieldStatus = GetFieldStatus(groupName, player);
        var movementResult = _ticTacToeService.Move(groupName, player, row, col, fieldStatus);
        await Clients.Group(groupName).SendAsync("MoveMade", player, row, col, movementResult);
    }

    private FieldStatus GetFieldStatus(string groupName, string player)
    {
        if (groupName.StartsWith(player))
        {
            return FieldStatus.X;
        }

        return FieldStatus.Circle;
    }

    private string GetPlayersGroupName(string player1, string player2)
    {
        return $"{player1} {player2}";
    }

    private string GetPlayersActiveGroupName(string player)
    {
        return _groupsService.FindPlayersGroup(player);
    }
}
