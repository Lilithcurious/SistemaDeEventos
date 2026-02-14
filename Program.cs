using System.Diagnostics;
using Microsoft.OpenApi.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Repositories;
using SistemaDeEventos.Services;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal class Program
{
    public Program()
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuração do DbContext (a connection string vem de appsettings.json)
        builder.Services.AddDbContext<EventosContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Registrar Repositories
        builder.Services.AddScoped<IEventRepository, EventsRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<IRatingRepository, RatingRepository>();
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();

        // Registrar Services
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<ITicketService, TicketService>();
        builder.Services.AddScoped<IRatingService, RatingService>();
        builder.Services.AddScoped<ILocationService, LocationService>();

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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private string GetDebuggerDisplay()
    {
        return ToString()!;
    }
}