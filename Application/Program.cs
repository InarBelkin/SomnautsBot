using Adapter.JsExecutor.Utils;
using Adapter.PostgreSQL.Utils;
using Adapter.TelegramBot.Utils;
using Adapter.WebApi;
using Core.Utils;

var api = new AdapterApi(args, (services, configuration) =>
{
    services.Configure<TelegramOptions>(configuration.GetSection("TelegramOptions"));
    services.Configure<BooksOptions>(configuration.GetSection("BooksOptions"));
    services.Configure<JsExecutorOptions>(configuration.GetSection("JsExecutorOptions"));

    services.AddCore(configuration);
    services.AddTelegramBot(configuration);
    services.AddPostgreSql(configuration);
    services.AddJsExecutor(configuration);
});

await api.RunAsync();