using ApiRefactor.Models;
using ApiRefactor.Repositories.Interfaces;
using ApiRefactor.Repositories;
using ApiRefactor.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureRepositoryManager(config);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
