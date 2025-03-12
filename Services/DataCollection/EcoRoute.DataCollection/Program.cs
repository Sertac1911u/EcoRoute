using EcoRoute.DataCollection.Context;
using EcoRoute.DataCollection.Services.BinLogServices;
using EcoRoute.DataCollection.Services.EnvLogServices;
using EcoRoute.DataCollection.Services.ProcessDataServices;
using EcoRoute.DataCollection.Services.SensorServices;
using EcoRoute.DataCollection.Services.WasteBinServices;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBinLogService, BinLogService>();
builder.Services.AddScoped<IEnvLogService, EnvLogServices>();
builder.Services.AddScoped<IProcessDataService, ProcessDataService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IWasteBinService, WasteBinService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<DataCollectionContext>();
//    (opt =>
//opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
