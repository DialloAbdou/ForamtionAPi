
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoList;
using TodoList.Data;
using TodoList.Dto;
using TodoList.Endpoints;
using TodoList.Services;
var builder = WebApplication.CreateBuilder();
builder.Logging.ClearProviders();
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console();
var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<TaskDbContext>(
    op => op.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

builder.Services.AddScoped<ITaskService, TaskServices>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

//app.Services
//    .CreateScope().ServiceProvider
//    .GetRequiredService<TaskDbContext>().Database
//    .EnsureCreated(); // creer db sans utiliser les lignes de commandes

 //await app.Services
 //   .CreateScope().ServiceProvider
 //   .GetRequiredService<TaskDbContext>().Database
 //   .MigrateAsync(); // mise a jour de DB


//app.MapGroup("/todos").GetTaskEndpoints();

app.MapGet("/todos",  async(
    [FromServices] ITaskService taskServices) =>  await taskServices.GetAllTasks());

app.MapGet("/todos/{id:int}",  async(
    int id, 
    [FromServices] ITaskService taskServices,
    [FromServices] ILogger<Program> logger) =>
{
    var task = await  taskServices.GetTaskById(id);

    logger.LogInformation("le nom de l'identifiant est {id},{title} ", id, task.Title);
    if (task is not null) return Results.Ok(task);
    return Results.NotFound();
});

app.MapGet("/todos/active", async (
       [FromServices] ITaskService taskServices,
       ILogger<Program> logger) =>
{
    var todoactives = await taskServices.GetTaskActive();

    return Results.Ok(todoactives);
});

app.MapPost("/todos",  async (
                       [FromBody] TaskInputModel taskImput,
                      [FromServices] IValidator<TaskInputModel> validator,
                      [FromServices] ITaskService taskServices,
                      [FromServices] ILogger<Program> logger
                ) =>
{
    var result = validator.Validate(taskImput);
    if (!result.IsValid) return Results.BadRequest(result.Errors.Select(e => new
    {
        e.ErrorMessage,
        e.PropertyName

    }));
    var mytak =   await taskServices.AddTask(taskImput);
    return Results.Ok(mytak);

});

app.MapPut("/todos/{id:int}",async (
      [FromRoute] int id,
      [FromBody] TaskInputModel task,
      [FromServices] ITaskService taskServices,
       [FromServices] ILogger<Program> logger) =>
{
    var taskodl =  await taskServices.UpdateTask(id, task);
    logger.LogInformation("voici indentifiant modifier {id}", id);
    return Results.Ok(taskodl);
});

app.Run();