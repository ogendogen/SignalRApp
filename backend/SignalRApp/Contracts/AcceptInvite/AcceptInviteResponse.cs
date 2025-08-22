namespace SignalRApp.Contracts.AcceptInvite;

public record AcceptInviteResponse(bool AcceptInviteResult, string? ErrorMessage = null);
