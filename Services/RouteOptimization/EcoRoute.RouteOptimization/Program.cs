using EcoRoute.RouteOptimization.Context;
using EcoRoute.RouteOptimization.Services.MyRouteServices;
using EcoRoute.RouteOptimization.Services.RouteOptimizationResultServices;
using EcoRoute.RouteOptimization.Services.WaypointServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.Audience = "ResourceRouteOptimization";
    opt.RequireHttpsMetadata = false;
});
builder.Services.AddScoped<IMyRouteService, MyRouteService>();
builder.Services.AddScoped<IWaypointService, WaypointService>();
builder.Services.AddScoped<IRouteOptimizationResultService, RouteOptimizationResultService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddDbContext<RouteOptimizationContext>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
