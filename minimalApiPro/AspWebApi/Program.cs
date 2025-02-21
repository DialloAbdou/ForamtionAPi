using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder();
builder.Logging.ClearProviders();
var loggerConfigguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt",rollingInterval:RollingInterval.Day);
var logger = loggerConfigguration.CreateLogger();
 builder.Logging.AddSerilog(logger);
var app=builder.Build();

app.MapGet("/hello/{nom}",(
   [FromRoute] String nom,
   [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("On a dis Bonjour {nom}", nom);
    return Results.Ok($"Bonjour {nom}");
} );
app.Run();  
