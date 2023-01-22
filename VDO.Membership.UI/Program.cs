var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthenticationService>());
builder.Services.AddScoped<IMembershipService, MembershipService>();

builder.Services.AddAuthorizationCore(options =>
{
    // Not a customer
    options.AddPolicy(UserPolicy.NotCustomer, policy =>
        policy.RequireAssertion(context =>
            !context.User.HasClaim(ClaimTypes.Role, UserRole.Customer)
        ));

    // Registerd, but not paying customer
    options.AddPolicy(UserPolicy.Registered, policy =>
    policy.RequireAssertion(context =>
        context.User.HasClaim(ClaimTypes.Role, UserRole.Registered) ||
        context.User.HasClaim(ClaimTypes.Role, UserRole.Customer)
    ));

    // Paying customer
    options.AddPolicy(UserPolicy.Customer, policy =>
        policy.RequireRole(UserRole.Customer));

    // Administrator (not used in this UI)
    options.AddPolicy(UserPolicy.Admin, policy =>
        policy.RequireRole(UserRole.Admin));
});

builder.Services.AddHttpClient<AuthenticationHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5001/api/"));
builder.Services.AddHttpClient<UserHttpClient>(client => client.BaseAddress = new Uri("https://localhost:5501/api/"));
builder.Services.AddHttpClient<MembershipHttpClient>(client => client.BaseAddress = new Uri("https://localhost:6001/api/"));

await builder.Build().RunAsync();
