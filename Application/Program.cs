using Adapter.PostgreSQL.Utils;
using Adapter.TelegramBot.Utils;
using Adapter.WebApi;
using Core.Utils;

var api = new AdapterApi(args, (services, configuration) =>
{
    services.Configure<TelegramOptions>(configuration.GetSection("TelegramOptions"));
    services.Configure<BooksOptions>(configuration.GetSection("BooksOptions"));

    services.AddCore(configuration);
    services.AddTelegramBot(configuration);
    services.AddPostgreSql(configuration);
});

await api.RunAsync();