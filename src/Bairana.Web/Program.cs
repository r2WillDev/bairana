using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Bairana.Web;
using Bairana.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// A versão local usa exclusivamente fixtures sem contato. O piloto exige validação estrita.
builder.Services.AddScoped(sp => new EmpreendedorDataService(sp.GetRequiredService<HttpClient>(), demonstracao: true));

await builder.Build().RunAsync();
