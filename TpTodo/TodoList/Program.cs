
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoList.Data;
using TodoList.Endpoints;
using TodoList.Repositories;
using TodoList.Services;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();

var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console();

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddTodoServices();
builder.Services.AddUSerService();

builder.Services.AddDbContext<TaskDbContext>(
    op => op.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
//builder.Services.AddScoped<ITaskService, TaskServices>();
//builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddEndpointsApiExplorer(); // permet d'explorer les differents endointd dont on dispose
builder.Services.AddSwaggerGen(); // support de swagger

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();
#region Migration
//app.Services
//    .CreateScope().ServiceProvider
//    .GetRequiredService<TaskDbContext>().Database
//    .EnsureCreated(); // creer db sans utiliser les lignes de commandes

//await app.Services
//   .CreateScope().ServiceProvider
//   .GetRequiredService<TaskDbContext>().Database
//   .MigrateAsync(); // mise a jour de DB
#endregion!

app.MapGroup("/todos").GetTaskEndpoints();
app.MapGroup("/users").GetUserEndPoint();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();// permet de generer le fichier swagger
    app.UseSwaggerUI(); //permet de generer l'interface graphique 

}

app.Run();