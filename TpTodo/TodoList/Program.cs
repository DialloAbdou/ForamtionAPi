
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoList;
using TodoList.Data;
using TodoList.Dto;
using TodoList.Endpoints;
var builder = WebApplication.CreateBuilder();
builder.Logging.ClearProviders();
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console();
var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<TaskDbContext>(
    op => op.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

builder.Services.AddSingleton<TaskServices>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

//app.MapGroup("/todos").GetTaskEndpoints();

app.MapGet("/todos", (TaskServices taskServices) => taskServices.GetAll());

app.MapGet("/todos/{id:int}", (
    int id, 
    [FromServices] TaskServices taskServices,
    [FromServices] ILogger<Program> logger) =>
{
    var task = taskServices.GetbyId(id);

    logger.LogInformation("le nom de l'identifiant est {id},{title} ", id, task.Title);
    if (task is not null) return Results.Ok(task);
    return Results.NotFound();
});

app.MapGet("/todos/active", (
       [FromServices] TaskServices taskServices,
       ILogger<Program> logger) =>
{
    var todoactives = taskServices.ActivesTask();

    return Results.Ok(todoactives);
});

app.MapPost("/todos", (
                       [FromBody] TaskInputModel task,
                      [FromServices] IValidator<TaskInputModel> validator,
                      [FromServices] TaskServices taskServices,
                      [FromServices] ILogger<Program> logger
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

app.MapPut("/todos/{id:int}", (
      [FromRoute] int id,
      [FromBody] TaskInputModel task,
      [FromServices] TaskServices taskServices,
       [FromServices] ILogger<Program> logger) =>
{
    var taskodl = taskServices.UpdateTask(id, task);
    logger.LogInformation("voici indentifiant modifier {id}", id);
    return Results.Ok(taskodl);
});

app.Run();