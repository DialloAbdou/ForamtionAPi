using Microsoft.AspNetCore.Mvc;
using Serilog;
using FluentValidation;
using AspWebApi.Data.Models;
using AspWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using AspWebApi;
using AspWebApi.Services;
using AspWebApi.Dto;
using AspWebApi.Endpoints;

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

builder.Services.AddScoped<IPersonneService, PersonneService>();

// ======AJout de documentation=====

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region AideCaheMemory
//builder.Services.AddMemoryCache();
//builder.Services.AddOutputCache(opt =>
//{
//    opt.AddBasePolicy(b => b.Expire(TimeSpan.FromMinutes(1)));
//    opt.AddPolicy("Expire2min", b=>b.Expire(TimeSpan.FromMinutes(2)));
//    opt.AddPolicy("Expire10sec", b=>b.Expire(TimeSpan.FromSeconds(10)));
//    opt.AddPolicy("ById", b => b.SetVaryByRouteValue("id"));
//});
//builder.Services.AddDistributedMemoryCache();
#endregion
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = "localhost:6379";
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
 await app.Services.
    CreateScope().
    ServiceProvider.
    GetRequiredService<ApiDbContext>().Database.MigrateAsync();
//app.UseOutputCache();

app.MapGroup("/personne")
   .MaPersonEndpoints();

#region hello

//app.MapGet("/hello/{nom}",(
//   [FromRoute] String nom,
//   [FromServices] ILogger<Program> logger) =>
//{
//    logger.LogInformation("On a dis Bonjour {nom}", nom);
//    return Results.Ok($"Bonjour {nom}");
//} );
#endregion
#region appEndpoint
//app.MapPost("/personne", async (
//    [FromBody] PersonInputModel personne,
//    [FromServices] IValidator<PersonInputModel> validator,
//    [FromServices] IPersonneService service,
//    CancellationToken token) =>
//{
//    var result = validator.Validate(personne);
//    if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
//    {
//        Message = e.ErrorMessage,
//        e.PropertyName,
//    }));
//    await service.AddPersonne(personne);
//    return Results.Ok(personne);
//}).Produces(200)
// .WithTags("Application Personnel");

//app.MapGet("/personne", async (
//    /*[FromServices] ApiDbContext context*/
//    [FromServices] IPersonneService service) =>
//{
//    var personnes = await service.GetAllPersonneAsync();
//    if (personnes is not null) return Results.Ok(personnes);
//    return Results.NoContent();
//}).WithTags("Application Personnel");

//app.MapGet("/personne/{id:int}", async (
//    [FromRoute] int id,
//    [FromServices] IPersonneService service
//    //[FromServices] IDistributedCache cache
//    /*[FromServices] IMemoryCache cache*/) =>
//{
//    //var personne = await cache.GetAsync<Personne>($"personne_{id}");
//    //if (personne is null)
//    //{
//    //    personne = await service.GetPersonneById(id);
//    //    if (personne is null) return Results.NotFound();
//    //    await cache.SetAsync($"personne_{id}", personne);
//    //    return Results.Ok(personne);
//    //}
//    var personne = await service.GetPersonneById(id);
//    if (personne is null) return Results.NotFound();
//    return Results.Ok(personne);
//    //if (!cache.GetAsync<Personne>($"personne_{id}", out var personne))
//    //{
//    //    personne = await context.Personnes.FirstOrDefaultAsync(p => p.Id == id);
//    //    if (personne is not null)
//    //    {
//    //        cache.SetAsync($"personne{id}", personne);
//    //        return Results.Ok(personne);
//    //    }
//    //    return Results.NotFound();
//    //}
//    //var personne = await context.Personnes.FirstOrDefaultAsync(x => x.Id == id);
//    //if (personne is not null) return Results.Ok(personne);
//    //return Results.NotFound();

//}).WithTags("Application Personnel");

//app.MapPut("/personne/{id:int}", async (
//    [FromRoute] int id,
//    [FromBody] PersonInputModel personne,
//   // [FromServices] ApiDbContext context,
//   [FromServices] IPersonneService service
//   // [FromServices] IDistributedCache cache
//   /*[FromServices] IMemoryCache cache*/) =>
//{
//    // var resulta = await context.Personnes
//    //.Where(p => p.Id == id)
//    //.ExecuteUpdateAsync(pe => pe.SetProperty(p => p.Nom, personne.Nom)
//    //                                         .SetProperty(p => p.Prenom, personne.Prenom));
//    var result = await service.UpdatePersonne(id, personne);
//    if (result)
//    {
//        // await cache.RemoveAsync($"personne_{id}");
//        return Results.NoContent();
//    }
//    return Results.NotFound();

//}).Produces(204)
// .Produces(404)
// .WithTags("Application Personnel");


//app.MapDelete("/personne/{id:int}", async (
//    [FromRoute] int id,
//    [FromServices] IPersonneService service
//    //[FromServices] ApiDbContext context
//    ) =>
//{
//   // var resulta = await context.Personnes
//   //var result = await service.DeletePersnne(i)
//   //.Where(p => p.Id == id).ExecuteDeleteAsync();
//   // if (resulta > 0) return Results.NoContent();
//    return Results.NotFound();

//}).WithTags("Application Personnel");

#endregion

app.Run();
