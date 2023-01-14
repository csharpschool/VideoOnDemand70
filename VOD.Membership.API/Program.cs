using Microsoft.EntityFrameworkCore;
using VOD.Application.Common.DTOs;
using VOD.Membership.Database.Contexts;
using VOD.Membership.Database.Entities;
using VOD.Membership.Database.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices();
ConfigureAutoMapper();

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

    builder.Services.AddDbContext<VODContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("VODConnection")));

    builder.Services.AddScoped<IDbService, DbService>();
}

void ConfigureAutoMapper()
{
    var config = new AutoMapper.MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Video, VideoDTO>().ReverseMap();

        cfg.CreateMap<Instructor, InstructorDTO>()
        .ReverseMap()
        .ForMember(dest => dest.Courses, src => src.Ignore());

        cfg.CreateMap<Course, CourseDTO>()
           .ReverseMap()
           // Only needed for seeding data.
           .ForMember(dest => dest.Instructor, src => src.Ignore());

        cfg.CreateMap<Section, SectionDTO>()
            .ForMember(dest => dest.Course, src => src.MapFrom(s => s.Course.Title))
            .ReverseMap()
            .ForMember(dest => dest.Course, src => src.Ignore());
    });
    var mapper = config.CreateMapper();
    builder.Services.AddSingleton(mapper);
}
