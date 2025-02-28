using Microsoft.AspNetCore.Mvc;
using Serilog;
using FluentValidation;
using AspWebApi.Data.Models;
using AspWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();

var loggerConfigguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);

var logger = loggerConfigguration.CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // fluentvalidation

builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlite(
    builder.Configuration.GetConnectionString("sqlite")));

builder.Services.AddMemoryCache();

var app = builder.Build();

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
app.MapPost("/personne", async (
    [FromBody] Personne personne,
    [FromServices] IValidator<Personne> validator,
    [FromServices] ApiDbContext context,
    CancellationToken token) =>
{
    var result = validator.Validate(personne);
    if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
    {
        Message = e.ErrorMessage,
        e.PropertyName,
    }));
    context.Personnes.Add(personne);
    await context.SaveChangesAsync(token);
    return Results.Ok(personne);
});

app.MapGet("/personne", async (
    [FromServices] ApiDbContext context) =>
{
    var personnes = await context.Personnes.ToListAsync();
    if (personnes is not null) return Results.Ok(personnes);
    return Results.NoContent();
});

app.MapGet("/personne/{id:int}", async (
    [FromRoute] int id,
    [FromServices] ApiDbContext context,
    [FromServices] IMemoryCache cache) =>
{
    if (!cache.TryGetValue<Personne>($"personne_{id}", out var personne))
    {
        personne = await context.Personnes.FirstOrDefaultAsync(p => p.Id == id);
        if (personne is not null)
        {
            cache.Set($"personne{id}", personne);
            return Results.Ok(personne);
        }
        return Results.NotFound();
    }
    return Results.Ok(personne);
    
});

app.MapPut("/personne/{id:int}", async (
    [FromRoute] int id,
    [FromBody] Personne personne,
    [FromServices] ApiDbContext context,
    [FromServices] IMemoryCache cache) =>
{
    var resulta = await context.Personnes
   .Where(p => p.Id == id)
   .ExecuteUpdateAsync(pe => pe.SetProperty(p => p.Nom, personne.Nom)
                                            .SetProperty(p => p.Prenom, personne.Prenom));
    if (resulta > 0)
    {
        cache.Remove($"personne_{id}");
         return Results.NoContent();
    }
    return Results.NotFound();

});

app.MapDelete("/personne/{id:int}", async (
    [FromRoute] int id,
    [FromServices] ApiDbContext context
    ) =>
{
    var resulta = await context.Personnes
   .Where(p => p.Id == id).ExecuteDeleteAsync();
    if (resulta > 0) return Results.NoContent();
    return Results.NotFound();

});

app.Run();
