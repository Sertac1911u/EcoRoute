using EcoRoute.DataProcessing.Context;
using EcoRoute.DataProcessing.Services.DataProcessingLogServices;
using EcoRoute.DataProcessing.Services.ProcessedDataServices;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IDataProcessingLogService, DataProcessingLogService>();
builder.Services.AddScoped<IProcessedDataService , ProcessedDataService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<DataProcessingContext>();

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
