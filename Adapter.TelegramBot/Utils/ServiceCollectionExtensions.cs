using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.TelegramBot.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramOptions>(configuration.GetSection("TelegramOptions"));
        services.AddLocalizations();
        return services;
    }

    private static IServiceCollection AddLocalizations(this IServiceCollection services)
    {
        var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            "UiLocalizations.json");
        using var fs = new FileStream(filePath, FileMode.Open);
        var localization = JsonSerializer.Deserialize<UiLocalization>(fs,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        services.AddSingleton(localization!);

        return services;
    }
}