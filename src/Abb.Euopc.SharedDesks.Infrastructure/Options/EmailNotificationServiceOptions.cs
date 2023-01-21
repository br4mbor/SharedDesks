namespace Abb.Euopc.SharedDesks.Infrastructure.Options;

public sealed class EmailNotificationServiceOptions
{
    public const string ConfigurationSectionName = "EmailNotificationService";

    public string FromAddress { get; set; } = default!;
}
