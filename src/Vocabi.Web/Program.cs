using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application;
using Vocabi.Infrastructure;
using Vocabi.Infrastructure.Persistence.Seed;
using Vocabi.Shared.Utils;
using Vocabi.Web;
using Vocabi.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddUIServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var pronunciationSeeder = scope.ServiceProvider.GetRequiredService<PronunciationSeeder>();
    await pronunciationSeeder.SeedAsync(FileUtils.GetWwwRootPath("pronunciations.json"));
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
