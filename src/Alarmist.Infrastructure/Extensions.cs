using Alarmist.Domain.Interfaces;
using Alarmist.Infrastructure.Persistence.Data;
using Alarmist.Infrastructure.Persistence.Repositories;
using Alarmist.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Alarmist.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<AlarmistContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AlarmistConnection"));
        });

        return services;
    }
}
