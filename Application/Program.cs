using Adapter.WebApi;

var api = new AdapterApi(args, (services, configuration) => { });

await api.RunAsync();