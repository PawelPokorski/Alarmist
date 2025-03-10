using Alarmist.API;
using Alarmist.Application;
using Alarmist.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add MediatR
builder.Services.AddApplication();
// Add Repositories and DbContext
builder.Services.AddInfrastructure(builder.Configuration);
// Add Controllers and Swagger
builder.Services.AddApi();

var app = builder.Build();

// Configure the API middleware pipeline
app.UseApi();

app.Run();