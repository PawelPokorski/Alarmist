using Alarmist.API;
using Alarmist.Application;
using Alarmist.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add MediatR
builder.Services.AddApplication();
// Add Repositories and DbContext
builder.Services.AddInfrastructure(builder.Configuration);
// Add Controllers and Swagger
builder.Services.AddApi();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Alarmist API", 
        Version = "v1",
        Description = "API do zarządzania alarmami i powiadomieniami"
    });
});

var app = builder.Build();

// Configure the API middleware pipeline
app.UseApi();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alarmist API V1");
    });
}

app.Run();