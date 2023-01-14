using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices();

// Configure the HTTP request pipeline.
ConfigureMiddleware();

void ConfigureMiddleware()
{
    var app = builder.Build();

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
}

void ConfigureServices()
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
}
