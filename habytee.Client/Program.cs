using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using habytee.Client;
using Radzen;
using habytee.Client.Services;
using habytee.Client.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<AnimationService>();
builder.Services.AddScoped<BrowserDetectThemeService>();
builder.Services.AddScoped<MainViewModel>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}/api") });
builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
