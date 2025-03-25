using EcoRoute.UI.Components;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Servislerin eklenmesi
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

// Cookie tabanlı authentication ayarları
builder.Services.AddAuthentication("MyCookieScheme")
    .AddCookie("MyCookieScheme", options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// HTTP istek hattının yapılandırılması
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing middleware’inin eklenmesi
app.UseRouting();

// Authentication ve Authorization middleware’larının eklenmesi
app.UseAuthentication();
app.UseAuthorization();

// Blazor Server endpoint’lerinin eşlenmesi
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
