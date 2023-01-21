using Abb.Euopc.SharedDesks.Application.Options;
using Abb.Euopc.SharedDesks.Application.Services;
using Abb.Euopc.SharedDesks.Application.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Validators;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedDesksApplication(this IServiceCollection services)
    {
        services.AddScoped<IAreaService, AreaService>();
        services.AddScoped<IDeskService, DeskService>();
        services.AddScoped<IDeskItemService, DeskItemService>();
        services.AddScoped<IDeskItemTypeService, DeskItemTypeService>();
        services.AddScoped<IReservationService, ReservationService>();

        services.AddScoped<AreaValidator>();
        services.AddScoped<DeskValidator>();
        services.AddScoped<DeskItemValidator>();
        services.AddScoped<DeskItemTypeValidator>();
        services.AddScoped<ReservationValidator>();

        services.AddOptions<AppOptions>()
            .Configure<IConfiguration>((options, configuration) => configuration.GetSection(AppOptions.ConfigurationSectionName)
                .Bind(options)
            );

        services.AddScoped<IAppService, AppService>();

        return services;
    }
}
