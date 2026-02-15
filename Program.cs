using System.Diagnostics;
using Microsoft.OpenApi.Models;
using Npgsql;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Repositories;
using SistemaDeEventos;
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

        // friendly JSON responses for certain status codes
        app.UseStatusCodePages(async context =>
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            switch (response.StatusCode)
            {
                case 400:
                    await response.WriteAsJsonAsync(new { error = "Dados inválidos" });
                    break;
                case 401:
                    await response.WriteAsJsonAsync(new { error = "Login errado" });
                    break;
                case 404:
                    await response.WriteAsJsonAsync(new { error = "Não encontrado" });
                    break;
                case 500:
                    await response.WriteAsJsonAsync(new { error = "Erro interno" });
                    break;
            }
        });

        // catch unhandled exceptions and return a 500 payload
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                if (feature != null)
                {
                    await context.Response.WriteAsJsonAsync(new { error = "Erro interno", detail = feature.Error.Message });
                }
            });
        });

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private string GetDebuggerDisplay()
    {
        return ToString()!;
    }
}