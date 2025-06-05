using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Services;
using EcoRoute.DataCollection.Services.SensorServices;
using EcoRoute.DataCollection.Services.WasteBinServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5001";
        options.Audience = "ResourceDataCollection";
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
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DataCollectionFullAccess", policy =>
    {
        policy.RequireClaim("scope", "DataCollectionFullPermission");
        policy.RequireRole("SuperAdmin", "Manager");
    });

    options.AddPolicy("DataCollectionReadAccess", policy =>
    {
        policy.RequireClaim("scope", "DataCollectionReadPermission");
        policy.RequireRole("SuperAdmin", "Manager", "Driver"); 
    });

    options.AddPolicy("DataProcessingFullAccess", policy =>
    {
        policy.RequireClaim("scope", "DataProcessingFullPermission");
        policy.RequireRole("SuperAdmin", "Manager");
    });

    options.AddPolicy("RouteOptimizationFullAccess", policy =>
    {
        policy.RequireClaim("scope", "RouteOptimizationFullPermission");
        policy.RequireRole("SuperAdmin", "Manager");
    });
});

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


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataCollectionContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IWasteBinService, WasteBinService>();
builder.Services.AddHttpClient<IDataCollectionNotificationService, DataCollectionNotificationService>();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorUI");


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
