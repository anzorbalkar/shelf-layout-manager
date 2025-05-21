using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShelfLayoutManager.Client;

WebAssemblyHostBuilder webAppBuilder = WebAssemblyHostBuilder.CreateDefault(args);
webAppBuilder.RootComponents.Add<App>("#app");
webAppBuilder.RootComponents.Add<HeadOutlet>("head::after");

webAppBuilder.Services.AddScoped(
    _ => new HttpClient { BaseAddress = new Uri(webAppBuilder.Configuration["apiBaseUrl"] ?? "") });

await webAppBuilder.Build().RunAsync();
