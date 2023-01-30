namespace Adapter.WebApi;

public class AdapterApi
{
    private readonly WebApplicationBuilder _builder;
    private readonly Action<IServiceCollection, IConfiguration> _options;

    public AdapterApi(string[] args, Action<IServiceCollection, IConfiguration> options)
    {
        _options = options;
        _builder = WebApplication.CreateBuilder(args);
    }

    public Task RunAsync()
    {
        _builder.Services.AddControllers();
        _builder.Services.AddEndpointsApiExplorer();
        _builder.Services.AddSwaggerGen();

        _options.Invoke(_builder.Services, _builder.Configuration);

        var app = _builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app.RunAsync();
    }
}