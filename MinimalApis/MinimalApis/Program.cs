
using Microsoft.AspNetCore.Mvc;
using MinimalApis;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSingleton<ArticleService>();
var app = builder.Build();


app.MapGet("/get", () => "Hello Abdou Welcom To Home! =:)");
app.MapPost("/post", () => "Hello Post Would you add?");
app.MapPut("/put", () => "hello Put would you put");
app.MapDelete("/delete", () => "hello delete would you delete Something");

app.MapMethods("method", new[] { "Get", "Post" }, () => "Hello methode");
app.MapGet("/articles/{id:int}", ( [FromServices] ArticleService service, int id) =>
{
    var article = service.GetArticles().Find(x => x.Id == id);
    if (article is not null) return Results.Ok(article);
    else return Results.NotFound();
});
app.MapPost("/articles",  ([FromServices] ArticleService service, Article a) =>
{
    var result = service.AddArticle(a.title);
     return Results.Ok(result);
});
app.MapGet("/personne/{nom}", 
                (
                    [FromRoute]String nom,
                    [FromQuery] String? prenom,
                    [FromHeader( Name = "Connection")] String connexion) => Results.Ok($"{nom},{prenom}, connexion est: {connexion}"));
app.Run();