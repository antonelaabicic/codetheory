using Brism;
using codetheory.Web.Components;
using codetheory.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddBrism();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://codetheory-api.onrender.com/");
});

builder.Services.AddSingleton<AuthStateService>();
builder.Services.AddScoped<AuthorizedHttpClient>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
