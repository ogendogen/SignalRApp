namespace SignalRApp.Contracts.AcceptInvite;

public record AcceptInviteResponse(bool AcceptInviteResult, string? Message = null, string? ErrorMessage = null);
