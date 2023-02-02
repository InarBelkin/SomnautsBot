using Adapter.JsExecutor.Cache;
using Adapter.JsExecutor.Executors;
using Core.Interfaces.Driven;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.JsExecutor.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJsExecutor(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBooksModulesPool, BooksModulesPool>();
        services.AddSingleton<INeedCleanAfterBooksRescan>(provider => provider.GetRequiredService<IBooksModulesPool>());
        services.AddScoped<IBookExecutor, BookExecutorJs>();
        return services;
    }
}