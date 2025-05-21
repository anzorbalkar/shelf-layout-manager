using ShelfLayoutManager.Infrastructure;

WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder(args);

string allowClientAccessCorsPolicyName = "AllowClientAccess";
string? clientBaseUrl = Environment.GetEnvironmentVariable("CLIENT_BASE_URL");

if (string.IsNullOrEmpty(clientBaseUrl))
{
    // Local client; source:
    // src/ShelfLayoutManager.Client/Properties/launchSettings.json, "https".
    clientBaseUrl = "https://localhost:7152";
}

webAppBuilder.Services
    .AddInfrastructure(webAppBuilder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCors(options =>
        options.AddPolicy(
            allowClientAccessCorsPolicyName,
            policy => policy.WithOrigins(clientBaseUrl).AllowAnyMethod().AllowAnyHeader()))
    .AddControllers();

WebApplication webApp = webAppBuilder.Build();

webApp.UseSwagger();
webApp.UseSwaggerUI(options => options.EnableTryItOutByDefault());
webApp.UseHttpsRedirection();
webApp.UseCors(allowClientAccessCorsPolicyName);
webApp.MapControllers();

webApp.Run();