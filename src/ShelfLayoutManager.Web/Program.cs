using ShelfLayoutManager.Infrastructure;

WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder(args);

webAppBuilder.Services
    .AddInfrastructure(webAppBuilder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCors(options =>
    {
        options.AddPolicy("AllowBlazorLocalhost",
            policy =>
            {
                policy.WithOrigins("https://localhost:7152")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    })
    .AddControllers();

WebApplication webApp = webAppBuilder.Build();

if (webApp.Environment.IsDevelopment())
{
    webApp.UseSwagger();
    webApp.UseSwaggerUI(options => options.EnableTryItOutByDefault());
}

webApp.UseHttpsRedirection().UseCors("AllowBlazorLocalhost").UseAuthorization();

webApp.MapControllers();

webApp.Run();
