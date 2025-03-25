using Blazored.LocalStorage;
using EcoRoute.UI.Auth;
using EcoRoute.UI.Components;
using EcoRoute.UI.MessageHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Razor Components (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// CORS – gerekirse API’ye erişiyorsan
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5001", "http://localhost:5005")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/accessdenied";
        options.Cookie.Name = "EcoRoute.Cookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;  // veya None
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // çünkü http
    });

// Authorization
builder.Services.AddAuthorization();

// Auth State Provider
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// HttpClient + JWT mesaj handler
builder.Services.AddScoped<JwtAuthorizationMessageHandler>();
builder.Services.AddHttpClient("EcoRouteApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
}).AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("EcoRouteApiClient"));

var app = builder.Build();

// Error pages
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Authentication & Authorization
app.UseCors(); // CORS middleware'i doğru yerde mi?
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// --- LOGIN ENDPOINT ---
app.MapPost("/login", async (HttpContext httpContext, [FromBody] string token) =>
{
    try
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claims = jwt.Claims.ToList();

        // Eğer "Username" claim'i varsa onu Name claim'ine eşle
        if (!claims.Any(c => c.Type == ClaimTypes.Name))
        {
            var usernameClaim = claims.FirstOrDefault(c => c.Type == "Username");
            if (usernameClaim != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, usernameClaim.Value));
            }
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Results.Ok();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Login error: {ex.Message}");
        return Results.Unauthorized();
    }
});

// --- AUTH-CHECK ---
app.MapGet("/auth-check", (HttpContext httpContext) =>
{
    var isAuth = httpContext.User.Identity?.IsAuthenticated ?? false;
    return Results.Json(new { isAuthenticated = isAuth });
});

// --- LOGOUT ---
app.MapPost("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok();
});

// Razor components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
