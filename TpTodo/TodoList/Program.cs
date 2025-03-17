
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoList;
using TodoList.Dto;
var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton<TaskServices>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.MapGet("/todos", (TaskServices taskServices) => taskServices.GetAll());

app.MapGet("/todos/{id:int}", (int id,[FromServices] TaskServices taskServices) =>
{
            var task = taskServices.GetbyId(id);

            if( task is not null ) return Results.Ok(task);
            return Results.NotFound();
});

app.MapGet("/todos/active", ([FromServices]TaskServices taskServices) =>
{
    var todoactives = taskServices.ActivesTask();
    return Results.Ok(todoactives);
});

app.MapPost("/todos", (
                       [FromBody] TaskInputModel task,
                      [FromServices] IValidator<TaskInputModel> validator,
                      [FromServices] TaskServices taskServices
                ) =>
{
    var result = validator.Validate(task);
    if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
    {
        e.ErrorMessage,
        e.PropertyName

    }));
    var mytak = taskServices.AddTask(task.Title);      
    return Results.Ok(mytak);  

});

app.MapPut("/todos/{id:int}", (int id, TaskInputModel task,[FromServices] TaskServices taskServices) =>
{
    var taskodl = taskServices.UpdateTask(id, task);
    return Results.Ok(taskodl);
});

app.Run();