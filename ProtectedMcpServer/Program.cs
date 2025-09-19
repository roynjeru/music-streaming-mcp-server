using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ModelContextProtocol.AspNetCore.Authentication;
using ProtectedMcpServer.Tools;
using System.Net.Http.Headers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var serverUrl = "http://localhost:7071/";
var inMemoryOAuthServerUrl = "https://localhost:7029";

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = McpAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddMcp(options =>
{
    options.ResourceMetadata = new()
    {
        Resource = new Uri(serverUrl),
        ResourceDocumentation = new Uri("https://docs.example.com/api/weather"),
        AuthorizationServers = { new Uri(inMemoryOAuthServerUrl) },
        ScopesSupported = ["mcp:tools"],
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMcpServer()
    .WithTools<WeatherTools>()
    .WithHttpTransport();

// Configure HttpClientFactory for weather.gov API
builder.Services.AddHttpClient("WeatherApi", client =>
{
    client.BaseAddress = new Uri("https://api.weather.gov");
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("weather-tool", "1.0"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Use the default MCP policy name that we've configured
app.MapMcp().RequireAuthorization();

Console.WriteLine($"Starting MCP server with authorization at {serverUrl}");
Console.WriteLine($"Using in-memory OAuth server at {inMemoryOAuthServerUrl}");
Console.WriteLine($"Protected Resource Metadata URL: {serverUrl}.well-known/oauth-protected-resource");
Console.WriteLine("Press Ctrl+C to stop the server");

app.Run(serverUrl);