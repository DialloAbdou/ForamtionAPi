using Microsoft.AspNetCore.Mvc;
using Serilog;
using FluentValidation;
using AspWebApi.Data.Models;
using AspWebApi.Data;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();

var loggerConfigguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt",rollingInterval:RollingInterval.Day);

var logger = loggerConfigguration.CreateLogger();

 builder.Logging.AddSerilog(logger);

builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // fluentvalidation

builder.Services.AddDbContext<ApiDbContext>();

var app=builder.Build();

app.Services.
    CreateScope().
    ServiceProvider.
    GetRequiredService<ApiDbContext>().Database.EnsureCreated();
#region hello
//app.MapGet("/hello/{nom}",(
//   [FromRoute] String nom,
//   [FromServices] ILogger<Program> logger) =>
//{
//    logger.LogInformation("On a dis Bonjour {nom}", nom);
//    return Results.Ok($"Bonjour {nom}");
//} );
#endregion
app.MapPost("/personne", (
    [FromBody]Personne personne,
    [FromServices] IValidator<Personne> validator,
    [FromServices] ApiDbContext context) =>
{
    var result = validator.Validate(personne);
    if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
    {
        Message = e.ErrorMessage,
        e.PropertyName,
    }));
    context.Personnes.Add(personne);
    context.SaveChanges();
    return Results.Ok(personne);
});
app.Run();  
