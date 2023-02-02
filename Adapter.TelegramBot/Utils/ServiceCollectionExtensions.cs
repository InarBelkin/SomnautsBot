using System.Reflection;
using System.Text.Json;
using Adapter.TelegramBot.Handlers;
using Adapter.TelegramBot.Interfaces;
using Adapter.TelegramBot.Services;
using Core.Interfaces.Driven;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Adapter.TelegramBot.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(provider =>
            new TelegramBotClient(provider.GetRequiredService<IOptions<TelegramOptions>>().Value.Token));
        services.AddHostedService<TgBotHostedService>();
        services.AddScoped<ITgUpdateHandler, TgUpdateHandler>()
            .AddScoped<ITgCommandsHandler, TgCommandsHandler>()
            .AddScoped<ITgButtonsHandler, TgButtonsHandler>()
            .AddScoped<ITgMessagesHandler, TgMessagesHandler>();

        services.AddLocalizations();
        services.AddScoped<ISendErrorToUserMiddleware, SendErrorToUserMiddleware>();

        services.AddScoped<ITelegramUserProvider, TelegramUserProvider>();
        services.AddScoped<IUserProvider>(provider => provider.GetRequiredService<ITelegramUserProvider>());

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