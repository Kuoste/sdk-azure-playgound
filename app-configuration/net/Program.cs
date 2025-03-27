using Microsoft.Extensions.Configuration;
using Microsoft.Azure.AppConfiguration.AspNetCore;
using Azure.Identity;
using TestAppConfig;


var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string
string connectionString = builder.Configuration.GetConnectionString("AppConfiguration")
    ?? throw new InvalidOperationException("The connection string 'AppConfiguration' was not found.");

// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(connectionString);

// Add services to the container.
builder.Services.AddRazorPages();

// Bind configuration "TestApp:Settings" section to the Settings object
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
