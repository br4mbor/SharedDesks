using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;
using Abb.Euopc.SharedDesks.Infrastructure.Options;
using Abb.Euopc.SharedDesks.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedDesksInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedDesksApplication()
            .AddSharedDesksDataAccess(configuration);

        services.AddOptions<EmailNotificationServiceOptions>()
            .Configure<IConfiguration>((options, configuration) => configuration.GetSection(EmailNotificationServiceOptions.ConfigurationSectionName)
                .Bind(options)
            );

        services.AddOptions<AzureCdnServiceOptions>()
            .Configure<IConfiguration>((options, configuration) => configuration.GetSection(AzureCdnServiceOptions.ConfigurationSectionName)
                .Bind(options)
            );

        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        services.AddScoped<IImageUploadService, AzureCdnService>();

        return services;
    }
}

