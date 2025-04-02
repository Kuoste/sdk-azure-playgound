using Microsoft.Azure.Cosmos;
using Microsoft.Samples.Cosmos.NoSQL.Quickstart.Services;
using Microsoft.Samples.Cosmos.NoSQL.Quickstart.Services.Interfaces;
using Microsoft.Samples.Cosmos.NoSQL.Quickstart.Web.Components;

using Settings = Microsoft.Samples.Cosmos.NoSQL.Quickstart.Models.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddOptions<Settings.Configuration>().Bind(builder.Configuration.GetSection(nameof(Settings.Configuration)));

builder.Services.AddSingleton<CosmosClient>((serviceProvider) =>
{
    string connectionString = Environment.GetEnvironmentVariable("AZURE_COSMOS_DB_CONNECTION_STRING") 
        ?? throw new InvalidOperationException("AZURE_COSMOS_DB_CONNECTION_STRING environment variable is not set.");

    CosmosClient client = new(
        connectionString: connectionString
    );
    return client;
});

builder.Services.AddTransient<IDemoService, DemoService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
