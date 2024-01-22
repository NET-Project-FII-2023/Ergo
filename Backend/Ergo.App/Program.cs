using Blazored.LocalStorage;
using Ergo.App;
using Ergo.App.Auth;
using Ergo.App.Contracts;
using Ergo.App.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});


builder.Services.AddScoped<CustomStateProvider>();

builder.Services.AddHttpClient<ITaskDataService, TaskDataService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});
builder.Services.AddHttpClient<IUserDataService, UserService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});

builder.Services.AddHttpClient<ICommentDataService, CommentDataService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});

builder.Services.AddHttpClient<IInboxItemDataService, InboxItemService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});

builder.Services.AddHttpClient<IProjectDataService, ProjectDataService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});

builder.Services.AddHttpClient<IMachineLearningService, MachineLearningService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});


await builder.Build().RunAsync();
