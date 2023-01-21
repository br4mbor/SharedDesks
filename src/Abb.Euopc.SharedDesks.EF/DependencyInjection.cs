using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedDesksDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<SharedDesksContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SharedDesksDB")));

        services.AddSingleton<IDomainContextFactory, DomainContextFactory>();

        return services;
    }
}
