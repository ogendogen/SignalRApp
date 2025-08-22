using Microsoft.AspNetCore.SignalR;
using SignalRApp.Contracts.AcceptInvite;
using SignalRApp.Contracts.Invite;
using SignalRApp.Contracts.Login;
using SignalRApp.Contracts.RejectInvite;
using SignalRApp.Contracts.StartGame;
using SignalRApp.Hubs.Models;
using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models.Enums;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SignalRApp.Hubs;

public class TicTacToeHub : Hub
{
    private readonly IGroupsService _groupsService;
    private readonly ITicTacToeService _ticTacToeService;
    private static readonly HashSet<Player> _connectedPlayers = new();
    private static readonly HashSet<Invitation> _invitations = new();

    public TicTacToeHub(IGroupsService groupsService, ITicTacToeService ticTacToeService)
    {
        _groupsService = groupsService;
        _ticTacToeService = ticTacToeService;
    }

    public override async Task OnConnectedAsync()
    {
        _connectedPlayers.Add(new Player() { ConnectionId = Context.ConnectionId, Name = string.Empty });
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedPlayers.RemoveWhere(p => p.ConnectionId == Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Login(LoginRequest request)
    {
        var user = _connectedPlayers.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        if (user is null)
        {
            await Clients.Caller.SendAsync("LoggedIn", new LoginResponse(false));
            return;
        }

        user.Name = request.Player;

        await Clients.Caller.SendAsync("LoggedIn", new LoginResponse(true));
    }

    public async Task Invite(InviteRequest request)
    {
        var invitingPlayer = _connectedPlayers.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        var invitedPlayerConnectionId = _connectedPlayers.FirstOrDefault(p => p.Name == request.InvitedPlayer)?.ConnectionId; // todo: optimize with reversed dictionary
        if (invitedPlayerConnectionId is not null)
        {
            var invitation = new Invitation(Guid.NewGuid(), invitingPlayer!.Name, request.InvitedPlayer); // todo: fix exclamation mark
            _invitations.Add(invitation);
            await Clients.Client(invitedPlayerConnectionId).SendAsync("Invitation", invitation);
            await Clients.Caller.SendAsync("InviteSent", new InviteResponse(true));
        }
        else
        {
            await Clients.Caller.SendAsync("InviteSent", new InviteResponse(false, "User does not exist"));
        }
    }

    public async Task AcceptInvite(AcceptInviteRequest request)
    {
        var invitation = _invitations.FirstOrDefault(x => x.InviteId == request.InviteId);
        if (invitation is null)
        {
            await Clients.Caller.SendAsync("InviteAccepted", new AcceptInviteResponse(false, "Invitation does not exist"));
            return;
        }

        var player1 = new Player() { ConnectionId = _connectedPlayers.FirstOrDefault(p => p.Name == invitation.From)!.ConnectionId, Name = invitation.From };
        var player2 = new Player() { ConnectionId = Context.ConnectionId, Name = invitation.To };

        var gameId = _ticTacToeService.StartGame(player1, player2);
        var groupName = gameId.ToString();
        _groupsService.AddGroup(groupName, player1, player2);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.OthersInGroup(groupName).SendAsync("InviteAccepted", invitation.From, invitation.To);

        await Clients.Group(groupName).SendAsync("GameStarted", gameId, player1, player2);
    }

    public async Task RejectInvite(RejectInviteRequest request)
    {
        var invitation = _invitations.FirstOrDefault(x => x.InviteId == request.InviteId);
        if (invitation is null)
        {
            await Clients.Caller.SendAsync("InviteRejected", new RejectInviteResponse(false, "Invitation does not exist"));
            return;
        }

        _invitations.Remove(invitation);
        await Clients.Caller.SendAsync("InviteRejected", new RejectInviteResponse(true));
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

    private string GetPlayersActiveGroupName(string player)
    {
        return _groupsService.FindPlayersGroup(player);
    }
}
