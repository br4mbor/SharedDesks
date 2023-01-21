namespace Abb.Euopc.SharedDesks.Domain.Common;

public sealed class EmailMessage
{
    public string To { get; set; } = default!;

    public string? Cc { get; set; }

    public string Subject { get; set; } = default!;

    public string Message { get; set; } = default!;
}
