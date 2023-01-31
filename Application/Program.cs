using Adapter.PostgreSQL.Utils;
using Adapter.TelegramBot.Utils;
using Adapter.WebApi;
using Core.Utils;

var api = new AdapterApi(args, (services, configuration) =>
{
    services.AddCore(configuration);
    services.AddTelegramBot(configuration);
    services.AddPostgreSql(configuration);
});

await api.RunAsync();