using AspWebApi.Data.Models;
using AspWebApi.Dto;
using AspWebApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AspWebApi.Endpoints
{
    public static class PersonEndpoints
    {
        public static RouteGroupBuilder MaPersonEndpoints(this RouteGroupBuilder group)
        {

            group.MapGet("", GetAll)
                 .WithTags("Application Personnel");

            group.MapPost("", GetPost

                       ).Produces(200)
                         .WithTags("Application Personnel");

            group.MapGet("/{id:int}", GetById).WithTags("Application Personnel");


            group.MapPut("/{id:int}", GetPut
            ).Produces(204)
             .Produces(404)
             .WithTags("Application Personnel");

            group.MapDelete("/{id:int}",GetDeleted

            ).WithTags("Application Personnel");

            return group;
        }
        private static async Task<IResult> GetAll(
            [FromServices] IPersonneService service)
        {
            var personnes = await service.GetAllPersonneAsync();
            if (personnes is not null) return Results.Ok(personnes);
            return Results.NoContent();
        }



        private static async Task<IResult> GetPost(
                        [FromBody] PersonInputModel personne,
                        [FromServices] IValidator<PersonInputModel> validator,
                        [FromServices] IPersonneService service,
                        CancellationToken token)
        {
            var result = validator.Validate(personne);
            if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
            {
                Message = e.ErrorMessage,
                e.PropertyName,
            }));
            await service.AddPersonne(personne);
            return Results.Ok(personne);

        }

        private static async Task<IResult> GetById(

                    [FromRoute] int id,
                   [FromServices] IPersonneService service
                //[FromServices] IDistributedCache cache
                /*[FromServices] IMemoryCache cache*/)
        {

            //var personne = await cache.GetAsync<Personne>($"personne_{id}");
            //if (personne is null)
            //{
            //    personne = await service.GetPersonneById(id);
            //    if (personne is null) return Results.NotFound();
            //    await cache.SetAsync($"personne_{id}", personne);
            //    return Results.Ok(personne);
            //}
            var personne = await service.GetPersonneById(id);
            if (personne is null) return Results.NotFound();
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

        }

        private static async Task<IResult> GetPut(
                 [FromRoute] int id,
                 [FromBody] PersonInputModel personne,
                // [FromServices] ApiDbContext context,
                [FromServices] IPersonneService service
                // [FromServices] IDistributedCache cache
                /*[FromServices] IMemoryCache cache*/)
        {
            // var resulta = await context.Personnes
            //.Where(p => p.Id == id)
            //.ExecuteUpdateAsync(pe => pe.SetProperty(p => p.Nom, personne.Nom)
            //                                         .SetProperty(p => p.Prenom, personne.Prenom));
            var result = await service.UpdatePersonne(id, personne);
            if (result)
            {
                // await cache.RemoveAsync($"personne_{id}");
                return Results.NoContent();
            }
            return Results.NotFound();
        }


        private static async Task<IResult> GetDeleted(
                  [FromRoute] int id,
                  [FromServices] IPersonneService service
                  //[FromServices] ApiDbContext context
                  )
        {
            var result = await service.DeletePersonne(id);
            if (result) return Results.NoContent();
            // var resulta = await context.Personnes
            //var result = await service.DeletePersnne(i)
            //.Where(p => p.Id == id).ExecuteDeleteAsync();
            // if (resulta > 0) return Results.NoContent();
            return Results.NotFound();

        }














    }
}
