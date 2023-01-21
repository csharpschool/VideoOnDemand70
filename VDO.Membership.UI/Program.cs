var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthenticationService>());
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddHttpClient<AuthenticationHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5001/api/"));
builder.Services.AddHttpClient<UserHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5501/api/"));
builder.Services.AddHttpClient<MembershipHttpClient>(client => client.BaseAddress = new Uri("https://localhost:6001/api/"));

await builder.Build().RunAsync();
