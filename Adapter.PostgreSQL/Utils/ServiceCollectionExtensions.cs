using Adapter.PostgreSQL.Stores;
using Adapter.PostgreSQL.Telegram;
using Adapter.TelegramBot.Interfaces;
using Core.Interfaces.Driven;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.PostgreSQL.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSql(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<SomnContext>(builder => builder.UseNpgsql(connectionString));

        services.AddScoped<ITelegramUserStore, TelegramUserStoreSql>();

        services.AddScoped<IUserStore, UserStoreSql>()
            .AddScoped<IBooksStore, BooksStoreSql>()
            .AddScoped<ISavesStore, SavesStoreSql>();
        return services;
    }
}