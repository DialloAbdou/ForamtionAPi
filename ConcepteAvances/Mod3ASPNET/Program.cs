using Mod3ASPNET;
using Mod3ASPNET.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/file", async (HttpContext context) =>
{
    using var memorySream = new MemoryStream();
    await context.Request.Body.CopyToAsync(memorySream);
    memorySream.Seek(0, SeekOrigin.Begin);
    File.WriteAllBytes("Todo.PNG", memorySream.ToArray());
    return Results.Ok("ça marche");

}).AddEndpointFilter<LogginFilter>();


// => appelle formulaire 
app.MapPost("/formfile", async (IFormFile file) =>
{
    using var memorySream = new MemoryStream();
    await file.CopyToAsync(memorySream);
    memorySream.Seek(0, SeekOrigin.Begin);
    File.WriteAllBytes(file.FileName, memorySream.ToArray());
    return Results.Ok();

}).DisableAntiforgery();


// renvoie d'un fichier html ou autre fichiers

app.MapGet("/hello-html", () =>
{
    return new HtmlResult("""
                    <html lang="en">
                <head>
                  <meta charset="UTF-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1.0">
                  <title>Formation minimal api </title>
                </head>
                <body>
                    <h2>Bienvenu sur minimal api</h2>
                </body>
                </html>
                """);

});


app.Run();

