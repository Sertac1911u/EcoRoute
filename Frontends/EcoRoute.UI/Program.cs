using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EcoRoute.UI;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using EcoRoute.UI.Auth;
using EcoRoute.UI.Services;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Token ekleyici handler servisini ekle
builder.Services.AddScoped<AuthorizedHandler>();

// HttpClient'i handler ile birlikte kaydet (AuthorizedClient ismiyle)
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/");
}).AddHttpMessageHandler<AuthorizedHandler>();

// Default HttpClient olarak AuthorizedClient'ı kullan
builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("AuthorizedClient");
});

// Uygulama servislerini ekle
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<CustomAuthStateProvider>();

// Yetkilendirme için local storage + auth state provider
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Blazor Toast Entegrasyonu
builder.Services.AddBlazoredToast();
await builder.Build().RunAsync();
