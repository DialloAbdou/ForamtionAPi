using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TodoList.Dto;
using TodoList.Repositories;
using TodoList.Services;

namespace TodoList.Endpoints
{
    public static class UserEndPoint
    {
        public static IServiceCollection AddUSerService(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped< IUserServices, UserServices>();
            return services;
        }
        public static RouteGroupBuilder GetUserEndPoint(this RouteGroupBuilder groupe)
        {

            groupe.MapPost("", AddUserAsync)
                .Produces<UserOutputModel>(statusCode: 200, contentType: "applicaton/json")
                .WithTags("Gestionnaire des Utilisateurs");

            return groupe;
        }


        private static async Task<IResult> AddUserAsync(
            [FromBody] UserInputModel userInput,
            [FromServices] UserServices userService,
            [FromServices] IValidator<UserInputModel> validator
            )
        {
            var result = validator.Validate(userInput);
            if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage,
            }));
            var user = await userService.CreateUserAsync(userInput);
            return Results.Ok(user);
        }
    }
}
