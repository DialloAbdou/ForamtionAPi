using Microsoft.AspNetCore.Mvc;
using Serilog;
using FluentValidation;
using AspWebApi.Data.Models;
using AspWebApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();

var loggerConfigguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt",rollingInterval:RollingInterval.Day);

var logger = loggerConfigguration.CreateLogger();

 builder.Logging.AddSerilog(logger);

builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // fluentvalidation

builder.Services.AddDbContext<ApiDbContext>(opt=>opt.UseSqlite(
    builder.Configuration.GetConnectionString("sqlite")));

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

app.MapGet("/personne", ([FromServices] ApiDbContext context ) =>
{
    var personnes = context.Personnes.ToList();
    if (personnes is not null) return Results.Ok(personnes);
    return Results.NoContent();
});

app.MapGet("/personne/{id:int}", ( 
    [FromRoute]int id,
    [FromServices] ApiDbContext context) =>
{
    var personne = context.Personnes.FirstOrDefault(p => p.Id == id);
    if (personne is not null) return Results.Ok(personne);
    else return Results.NotFound();
});

app.MapPut("/personne/{id:int}", (
    [FromRoute] int id,
    [FromServices] ApiDbContext context,
    [FromBody]Personne personne) =>
{
    var resulta = context.Personnes
   .Where(p => p.Id == id)
   .ExecuteUpdate(pe => pe.SetProperty(p => p.Nom, personne.Nom)
                                            .SetProperty(p => p.Prenom, personne.Prenom));

    if (resulta > 0) return Results.NoContent();
    return Results.NotFound();

});

app.MapDelete("/personne/{id:int}", (
    [FromRoute] int id,
    [FromServices] ApiDbContext context
    ) =>
{
    var resulta = context.Personnes
   .Where(p => p.Id == id).ExecuteDelete();
    if (resulta > 0) return Results.NoContent();
    return Results.NotFound();

});

app.Run();  
