namespace SignalRApp.Contracts.RejectInvite;

public record RejectInviteResponse(bool RejectInviteResult, string? ErrorMessage = null);