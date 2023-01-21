namespace Abb.Euopc.SharedDesks.Infrastructure.Options;

public sealed class AzureCdnServiceOptions
{
    public const string ConfigurationSectionName = "AzureCdnService";

    public string ConnectionString { get; set; } = default!;

    public string RootUrl { get; set; } = default!;
}
