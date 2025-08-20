using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application;
using Vocabi.Infrastructure;
using Vocabi.Infrastructure.External.Flashcards;
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
    var configurator = scope.ServiceProvider.GetRequiredService<IAnkiTemplateConfigurator>();
    await configurator.EnsureVocabiDeckAsync();
    await configurator.EnsureVocabiNoteModelAsync();
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
