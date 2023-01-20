var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsAllAccessPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();

void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddCors(policy => {
        policy.AddPolicy("CorsAllAccessPolicy", opt =>
            opt.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
        );
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<VODUserContext>(
        options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("VODUserConnection")));

    builder.Services.AddIdentity<VODUser, IdentityRole>()
            .AddEntityFrameworkStores<VODUserContext>()
            .AddDefaultTokenProviders();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

}
