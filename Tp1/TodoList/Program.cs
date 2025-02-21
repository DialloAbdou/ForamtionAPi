
using Microsoft.AspNetCore.Mvc;
using TodoList;
var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton<TaskServices>();

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

app.MapPost("/todos", ([FromServices]TaskServices taskServices, MyTask task) =>
{
    var result = taskServices.AddTask(task.Title);      

    return Results.Ok(result);  

});

app.MapPut("/todos/{id:int}", (int id, MyTask task,[FromServices] TaskServices taskServices) =>
{
    var taskodl = taskServices.UpdateTask(id, task);

    return Results.Ok(taskodl);
});

app.Run();