using Microsoft.AspNetCore.SignalR;
using SignalRApp.Contracts.AcceptInvite;
using SignalRApp.Contracts.Invite;
using SignalRApp.Contracts.Login;
using SignalRApp.Contracts.Move;
using SignalRApp.Contracts.RejectInvite;
using SignalRApp.Hubs.Constants;
using SignalRApp.Hubs.Models;
using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models.Enums;

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
        if (string.IsNullOrEmpty(request.Player) || string.IsNullOrWhiteSpace(request.Player))
        {
            await Clients.Caller.SendAsync(MethodsNames.Login, new LoginResponse(false));
            return;
        }

        var user = _connectedPlayers.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        if (user is null)
        {
            await Clients.Caller.SendAsync(MethodsNames.Login, new LoginResponse(false));
            return;
        }

        if (_connectedPlayers.Any(p => p.Name == request.Player))
        {
            await Clients.Caller.SendAsync(MethodsNames.Login, new LoginResponse(false));
            return;
        }

        user.Name = request.Player;

        await Clients.Caller.SendAsync(MethodsNames.Login, new LoginResponse(true));
    }

    public async Task Invite(InviteRequest request)
    {
        var invitingPlayer = _connectedPlayers.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        var invitedPlayerConnectionId = _connectedPlayers.FirstOrDefault(p => p.Name == request.InvitedPlayer)?.ConnectionId; // todo: optimize with reversed dictionary
        if (invitedPlayerConnectionId is not null)
        {
            var invitation = new Invitation(Guid.NewGuid(), invitingPlayer!.Name, request.InvitedPlayer); // todo: fix exclamation mark
            _invitations.Add(invitation);
            await Clients.Client(invitedPlayerConnectionId).SendAsync(MethodsNames.Invitation, invitation);
            await Clients.Caller.SendAsync(MethodsNames.InviteSent, new InviteResponse(true));
        }
        else
        {
            await Clients.Caller.SendAsync(MethodsNames.Login, new InviteResponse(false, "User does not exist"));
        }
    }

    public async Task AcceptInvite(AcceptInviteRequest request)
    {
        var invitation = _invitations.FirstOrDefault(x => x.InviteId == request.InviteId);
        if (invitation is null)
        {
            await Clients.Caller.SendAsync(MethodsNames.InviteAccepted, new AcceptInviteResponse(false, "Invitation does not exist"));
            return;
        }

        var player1 = new Player() { ConnectionId = _connectedPlayers.FirstOrDefault(p => p.Name == invitation.From)!.ConnectionId, Name = invitation.From };
        var player2 = new Player() { ConnectionId = Context.ConnectionId, Name = invitation.To };

        var gameId = _ticTacToeService.StartGame(player1, player2);
        var groupName = gameId.ToString();
        _groupsService.AddGroup(gameId, player1, player2);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.OthersInGroup(groupName).SendAsync(MethodsNames.AcceptInvite, invitation.From, invitation.To);

        await Clients.Group(groupName).SendAsync(MethodsNames.GameStarted, gameId, player1, player2);
    }

    public async Task RejectInvite(RejectInviteRequest request)
    {
        var invitation = _invitations.FirstOrDefault(x => x.InviteId == request.InviteId);
        if (invitation is null)
        {
            await Clients.Caller.SendAsync(MethodsNames.InviteRejected, new RejectInviteResponse(false, "Invitation does not exist"));
            return;
        }

        _invitations.Remove(invitation);
        await Clients.Caller.SendAsync(MethodsNames.InviteRejected, new RejectInviteResponse(true));
    }

    public async Task Move(MoveRequest moveRequest)
    {
        if (! _ticTacToeService.IsGameExists(moveRequest.GameId))
        {
            await Clients.Caller.SendAsync(MethodsNames.Move, new MoveResponse() { MovementResult = MovementResult.Error, Error = "Game does not exist" });
            return;
        }

        var group = _groupsService.GetGroupByGameId(moveRequest.GameId);
        if (group is null)
        {
            await Clients.Caller.SendAsync(MethodsNames.Move, new MoveResponse() { MovementResult = MovementResult.Error, Error = "Group does not exist" });
            return;
        }

        var player = group.Player1.Name == moveRequest.PlayerName ? group.Player1 : (group.Player2.Name == moveRequest.PlayerName ? group.Player2 : null);
        if (player is null)
        {
            await Clients.Caller.SendAsync(MethodsNames.Move, new MoveResponse() { MovementResult = MovementResult.Error, Error = "Player does not exist" });
            return;
        }

        var movementResult = _ticTacToeService.Move(moveRequest.GameId, player, moveRequest.X, moveRequest.Y, moveRequest.FieldStatus);

        await Clients.Group(group.GroupId.ToString()).SendAsync(MethodsNames.Move, new MoveResponse() { MovementResult = movementResult });
    }
}
