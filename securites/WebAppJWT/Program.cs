using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

RSA rsa = RSA.Create();
if (!File.Exists("key.bin"))
{
    var key = rsa.ExportRSAPrivateKey();
    File.WriteAllBytes("key.bin", key);
}
rsa.ImportRSAPrivateKey(File.ReadAllBytes("key.bin"), out var _);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false
        };
        options.Configuration = new OpenIdConnectConfiguration
        {
            SigningKeys = { new RsaSecurityKey(rsa) }
        };
        options.MapInboundClaims = false;
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/username", [Authorize] (HttpContext ctx) =>
{
    var username = ctx.User.FindFirstValue("name");
    var sub = ctx.User.FindFirstValue("sub");
    return Results.Ok(sub + " " + username);
});

app.MapGet("/login", (HttpContext ctx) =>
{
    var jwtHandler = new JsonWebTokenHandler();
    var key = new RsaSecurityKey(rsa);
    var token = jwtHandler.CreateToken(new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim("sub", "1"),
            new Claim("name", "Abdou Diallo")
        }),
        Issuer = "http://localhost:5265",        
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
    });
    return Results.Ok(token);
});

app.Run();