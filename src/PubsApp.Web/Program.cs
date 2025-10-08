using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PubsApp.Web;
using PubsApp.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient to point to the API
var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7080";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

// Register API services
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<TitleService>();
builder.Services.AddScoped<PublisherService>();

await builder.Build().RunAsync();
