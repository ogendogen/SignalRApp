using Microsoft.AspNetCore.SignalR;
using SignalRApp.Contracts.AcceptInvite;
using SignalRApp.Contracts.Invite;
using SignalRApp.Contracts.Login;
using SignalRApp.Contracts.RejectInvite;
using SignalRApp.Contracts.StartGame;
using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Hubs;

public class TicTacToeHub : Hub
{
    private readonly IGroupsService _groupsService;
    private readonly ITicTacToeService _ticTacToeService;
    private static readonly Dictionary<string, string> _connectionUsers = new();
    private static readonly Dictionary<Guid, string> _invitations = new();

    public TicTacToeHub(IGroupsService groupsService, ITicTacToeService ticTacToeService)
    {
        _groupsService = groupsService;
        _ticTacToeService = ticTacToeService;
    }

    public override async Task OnConnectedAsync()
    {
        _connectionUsers.Add(Context.ConnectionId, string.Empty);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionUsers.Remove(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Login(LoginRequest request)
    {
        _connectionUsers[Context.ConnectionId] = request.Player;
        await Clients.Caller.SendAsync("Login", new LoginResponse(true));
    }

    public async Task Invite(InviteRequest request)
    {
        var login = _connectionUsers[Context.ConnectionId];
        var invitedPlayerConnectionId = _connectionUsers.FirstOrDefault(x => x.Value == request.InvitedPlayer).Key; // todo: optimize with reversed dictionary
        if (invitedPlayerConnectionId is not null)
        {
            var inviteId = Guid.NewGuid();
            _invitations.Add(inviteId, login);
            await Clients.Client(invitedPlayerConnectionId).SendAsync("Invitation", new Invitation(inviteId, request.InvitedPlayer));
            await Clients.Caller.SendAsync("InviteSent", new InviteResponse(true));
        }
        else
        {
            await Clients.Caller.SendAsync("InviteSent", new InviteResponse(false, "User does not exist"));
        }
    }

    public async Task AcceptInvite(AcceptInviteRequest request)
    {
        //var player1 = _connectionUsers[Context.ConnectionId];
        //var player2 = request.Player;
        //var groupName = GetPlayersGroupName(player1, player2);
        //_groupsService.AddGroup(groupName);
        //await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //await Clients.Group(groupName).SendAsync("InviteAccepted", player1, player2);
    }

    public async Task RejectInvite(RejectInviteRequest request)
    {
    }

    public async Task StartGame(StartGameRequest request)
    {
        // todo: rethink

        //var gameId = _ticTacToeService.StartGame(request.Player);
        //await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        //await Clients.Group(groupName).SendAsync("GameStarted", player1, player2);
    }

    public async Task Move(string player, int row, int col)
    {
        // which player -> field status
        var groupName = GetPlayersActiveGroupName(player);
        if (String.IsNullOrEmpty(groupName))
        {
            await Clients.Caller.SendAsync("MoveMade", "Error", "Group not found");
            return;
        }

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
