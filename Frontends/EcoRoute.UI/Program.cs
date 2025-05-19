using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EcoRoute.UI;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using EcoRoute.UI.Auth;
using EcoRoute.UI.Services;
using Blazored.Toast;
using EcoRoute.UI.Services.WasteBinServices;
using EcoRoute.UI.Services.SupportsServices;
using EcoRoute.UI.Services.SettingsServices;
using EcoRoute.UI.Services.NotificationServices;
using EcoRoute.UI.Services.RouteOptimizationServices;
using EcoRoute.UI.Services.ReportsServices;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthorizedHandler>();

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
}).AddHttpMessageHandler<AuthorizedHandler>();

builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("AuthorizedClient");
});

// Servisler
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WasteBinService>();
builder.Services.AddScoped<SensorService>();
builder.Services.AddScoped<SupportTicketService>();
builder.Services.AddScoped<SettingsService>();
builder.Services.AddScoped<NotificationService>(); 
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IReportService, ReportService>();


// LocalStorage ve Authentication
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Toast Notification
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();