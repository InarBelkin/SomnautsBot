using Adapter.TelegramBot.Utils;
using Adapter.WebApi;

var api = new AdapterApi(args, (services, configuration) => { services.AddTelegramBot(configuration); });

await api.RunAsync();