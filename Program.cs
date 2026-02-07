using Microsoft.OpenApi.Models;
using Npgsql; // Mantenha esta referência
using SistemaDeEventos.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<EventosContext>(options => 
    options.UseNpgsql("Host=localhost;Port=5432;Database=eventos;Username=postgres"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
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