using EcoRoute.Supports.Context;
using EcoRoute.Supports.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5001";
        options.Audience = "ResourceSupports";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = "http://localhost",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("dsopkjfewosspjwe12+fqpjrfqepqjasd123x.@ewrkj3241kld"))
        };

        // SaveToken ekleyin - bu, token'ları HttpContext'te saklar
        options.SaveToken = true;
    });
var options = new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
};

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SupportsFullAccess", policy =>
    {
        policy.RequireClaim("scope", "SupportsFullPermission");
        policy.RequireRole("SuperAdmin", "Manager", "Customer");
    });
    options.AddPolicy("SupportsReadAccess", policy =>
    {
        policy.RequireClaim("scope", "SupportsReadPermission");
        policy.RequireRole("SuperAdmin", "Manager", "Customer");
    });
});

// HttpContextAccessor'ı ekle
builder.Services.AddHttpContextAccessor();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorUI", policy =>
    {
        policy.WithOrigins("http://localhost:5054")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// HttpClient for notification service
builder.Services.AddHttpClient<ISupportNotificationService, SupportNotificationService>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SupportsContext>();
builder.Services.AddScoped<ISupportService, SupportService>();
builder.Services.AddScoped<ISupportNotificationService, SupportNotificationService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

var uploadsFolder = Path.Combine(builder.Environment.ContentRootPath, "Uploads");

if (!Directory.Exists(uploadsFolder))
{
    Directory.CreateDirectory(uploadsFolder);
}

var attachmentsFolder = Path.Combine(uploadsFolder, "Attachments");
if (!Directory.Exists(attachmentsFolder))
{
    Directory.CreateDirectory(attachmentsFolder);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// No HTTPS redirection, matching DataCollection service
app.UseCors("AllowBlazorUI");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsFolder),
    RequestPath = "/Uploads"
});
app.MapControllers();

app.Run();