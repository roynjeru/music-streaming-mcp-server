using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ModelContextProtocol.AspNetCore.Authentication;
using ProtectedMcpServer.Tools.Spotify;
using System.Net.Http.Headers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddHttpContextAccessor();
string serverUrl = config.GetValue<string>("ASPNETCORE_URLS");
builder.Services.AddMcpServer()
    .WithTools<SpotifyPlaylistTools>()
    .WithHttpTransport();

var app = builder.Build();

app.MapMcp();

Console.WriteLine($"Starting MCP server with authorization at: {serverUrl}");
Console.WriteLine("Press Ctrl+C to stop the server");

app.Run();