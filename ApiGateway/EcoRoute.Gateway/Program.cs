using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Config dosyalarını oku (ocelot.json + appsettings.json)
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Authentication (JWT)
builder.Services.AddAuthentication("OcelotAuthenticationScheme")
    .AddJwtBearer("OcelotAuthenticationScheme", options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"]; // Örnek: http://localhost:5001
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost",
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("dsopkjfewosspjwe12+fqpjrfqepqjasd123x.@ewrkj3241kld")
            ),
            ClockSkew = TimeSpan.Zero
        };
    });

// Ocelot middleware ekle
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Middlewares
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// (İsteğe bağlı: Root endpoint eklemek istersen Ocelot'tan önce olmalı)
app.MapGet("/", () => "EcoRoute Gateway Running...");

// Ocelot en sonda!
await app.UseOcelot();

app.Run();
