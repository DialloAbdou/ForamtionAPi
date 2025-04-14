using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoList.Dto;
using TodoList.Repositories;
using TodoList.Services;

namespace TodoList.Endpoints
{
    public static class TaskEndpoints
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskServices>();
            return services;
        }
        public static RouteGroupBuilder GetTaskEndpoints(this RouteGroupBuilder groupe)
        {
            groupe.MapGet("", GetAllTaskAsync)
                .Produces<TaskOutPutModel>(statusCode: 200, contentType: "applicaton/json")
                .WithTags("Gestionnaire des Taches");

            groupe.MapGet("/{id:int}", GetTaskByIdAsync)
                .Produces<TaskOutPutModel>(statusCode: 200, contentType: "applicaton/json")
                .WithTags("Gestionnaire des Taches");

            groupe.MapGet("/active", GetAllTaskActivesAsync)
                .Produces<TaskOutPutModel>(statusCode: 200, contentType: "applicaton/json")
                .WithTags("Gestion des Taches");

            groupe.MapPost("", AddTaskAsync)
                .Produces<TaskOutPutModel>(statusCode:200,contentType:"applicaton/json")
                .WithTags("Gestionnaire des Taches");

            groupe.MapPut("/{id:int}", UpdateTaskAsync)
                .Produces(204)
                .Produces(404)
                .WithTags("Gestionnaire des Taches");
            return groupe;
        }

        /// <summary>
        /// Elle renvoie la liste des taches
        /// </summary>
        /// <param name="taskServices"></param>
        /// <returns></returns>
        private static async Task<IResult> GetAllTaskAsync
            (
                 [FromServices] ITaskService taskServices
            )
        {
            var taks = await taskServices.GetAllTasks();
            if (taks is null) return Results.NoContent();
            return Results.Ok(taks);
        }

        /// <summary>
        /// elle renvoie un element de taches
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskServices"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private static async Task<IResult> GetTaskByIdAsync
            (
              [FromRoute] int id,
              [FromServices] ITaskService taskServices,
              [FromServices] ILogger<Program> logger
            )
        {
            var task = await taskServices.GetTaskById(id);
            logger.LogInformation("le nom de l'identifiant est {id},{title} ", id, task.Title);
            if (task is not null) return Results.Ok(task);
            return Results.NotFound();
        }

        /// <summary>
        /// liste des Actives
        /// </summary>
        /// <param name="taskServices"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private static async Task<IResult> GetAllTaskActivesAsync
            (
                [FromServices] ITaskService taskServices,
                [FromServices] ILogger<Program> logger
            )
        {
            var todoactives = await taskServices.GetTaskActive();
            if (todoactives is null) return Results.NoContent();
            return Results.Ok(todoactives);
        }

        /// <summary>
        /// Ajout de taches 
        /// </summary>
        /// <param name="taskImput"></param>
        /// <param name="validator"></param>
        /// <param name="taskServices"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        private static async Task<IResult> AddTaskAsync
            (
                [FromBody] TaskInputModel taskImput,
                [FromServices] IValidator<TaskInputModel> validator,
                 String userToken,
                [FromServices] ITaskService taskServices,
                [FromServices] ILogger<Program> logger
            )
        {
           
            var usr =  await taskServices.GetUserAsync (userToken);

            var result = validator.Validate(taskImput);
            if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
            {
                e.ErrorMessage,
                e.PropertyName

            }));
            var mytak = await taskServices.AddTask(taskImput);
            return Results.Ok(mytak);
        }

        /// <summary>
        /// /Mise a jour
        /// </summary>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <param name="taskServices"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private static async Task<IResult> UpdateTaskAsync
            (
              [FromRoute] int id,
              [FromBody] TaskInputModel task,
              [FromServices] ITaskService taskServices,
              [FromServices] ILogger<Program> logger)
        {
            var result = await taskServices.UpdateTask(id, task);
            if (result)
            {
                return Results.Ok($"it's {result} ");
                logger.LogInformation("voici indentifiant modifier {id}", id);
            }
            return Results.NotFound();

        }





    }
}
