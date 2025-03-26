using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TodoList.Dto;
using TodoList.Services;

namespace TodoList.Endpoints
{
    public static class UserEndPoint 
    {
        public  static RouteGroupBuilder GetUserEndPoint( this RouteGroupBuilder groupe)
        {
            groupe.MapGet("", GetAllUserAsync);
            groupe.MapPost("", AddUserAsync);

            return groupe;
        }

        private static IResult GetAllUserAsync( [FromServices] UserServices userService)
            
        {
            var users = userService.GetAllUSerAsync();
            return Results.Ok( users ); 
            
        }

        private  static async Task<IResult> AddUserAsync(
            [FromBody] UserInputModel userInput,
            [FromServices] UserServices userService

            ) 
        {
             var user = await userService.CreateUserAsync( userInput);

            return Results.Ok(user);
        }
    }
}
