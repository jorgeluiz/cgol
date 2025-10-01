
using AutoMapper;
using DL.GameOfLife.Api.Mappers;
using DL.GameOfLife.Extensions.Registers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Mapper
builder.Services.AddSingleton(
    new MapperConfiguration(config =>
    {
        config.MainApplicationMap();
    }).CreateMapper()
);
#endregion

#region Registers
builder.Services.AddServices();
#endregion

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
