using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SistemaDeEventos;
using SistemaDeEventos.Models;
using SistemaDeEventos.Repository;
using SistemaDeEventos.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddDbContext<EventosContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=91549291"));


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sistema de Eventos API",
                Version = "v1"
            });
        });

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IRatingService, RatingService>();
        builder.Services.AddScoped<IRatingRepository, RatingRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // Endpoint de teste simples
        app.MapGet("/hello", () => "Olá, Sistema de Eventos está funcionando!");

        app.Run();
    }
}
