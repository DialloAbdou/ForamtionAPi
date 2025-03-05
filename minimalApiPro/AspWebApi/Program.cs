using Microsoft.AspNetCore.Mvc;
using Serilog;
using FluentValidation;
using AspWebApi.Data.Models;
using AspWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using AspWebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

var loggerConfigguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);

var logger = loggerConfigguration.CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // fluentvalidation

builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlite(
    builder.Configuration.GetConnectionString("sqlite")));

//builder.Services.AddMemoryCache();
//builder.Services.AddOutputCache(opt =>
//{
//    opt.AddBasePolicy(b => b.Expire(TimeSpan.FromMinutes(1)));
//    opt.AddPolicy("Expire2min", b=>b.Expire(TimeSpan.FromMinutes(2)));
//    opt.AddPolicy("Expire10sec", b=>b.Expire(TimeSpan.FromSeconds(10)));
//    opt.AddPolicy("ById", b => b.SetVaryByRouteValue("id"));
//});
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = "localhost:6379";
});

var app = builder.Build();

app.Services.
    CreateScope().
    ServiceProvider.
    GetRequiredService<ApiDbContext>().Database.EnsureCreated();
//app.UseOutputCache();
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
    [FromServices] IDistributedCache cache
    /*[FromServices] IMemoryCache cache*/) =>
{
    var personne = await cache.GetAsync<Personne>($"personne_{id}");
    if (personne is null)
    {
        personne = await context.Personnes.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (personne is null) return Results.NotFound();
        await cache.SetAsync($"personne_{id}", personne);
        return Results.Ok(personne);
    }
    return Results.Ok(personne);
    //if (!cache.GetAsync<Personne>($"personne_{id}", out var personne))
    //{
    //    personne = await context.Personnes.FirstOrDefaultAsync(p => p.Id == id);
    //    if (personne is not null)
    //    {
    //        cache.SetAsync($"personne{id}", personne);
    //        return Results.Ok(personne);
    //    }
    //    return Results.NotFound();
    //}
    //var personne = await context.Personnes.FirstOrDefaultAsync(x => x.Id == id);
    //if (personne is not null) return Results.Ok(personne);
    //return Results.NotFound();

});

app.MapPut("/personne/{id:int}", async (
    [FromRoute] int id,
    [FromBody] Personne personne,
    [FromServices] ApiDbContext context,
      [FromServices] IDistributedCache cache
   /*[FromServices] IMemoryCache cache*/) =>
{
    var resulta = await context.Personnes
   .Where(p => p.Id == id)
   .ExecuteUpdateAsync(pe => pe.SetProperty(p => p.Nom, personne.Nom)
                                            .SetProperty(p => p.Prenom, personne.Prenom));
    if (resulta > 0)
    {
        await cache.RemoveAsync($"personne_{id}");
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
