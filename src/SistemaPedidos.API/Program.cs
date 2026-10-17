using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SistemaPedidos.API.Middleware;
using SistemaPedidos.Application;
using SistemaPedidos.Infrastructure;
using SistemaPedidos.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Pedidos API",
        Version = "v1",
        Description = "API REST para administrar clientes, productos, inventario y pedidos."
    });
});
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema de Pedidos v1"));
app.MapControllers();

if (builder.Configuration.GetValue<bool>("ApplyMigrations"))
{
    await ApplyMigrationsWithRetryAsync(app.Services, app.Logger);
}

app.Run();

static async Task ApplyMigrationsWithRetryAsync(IServiceProvider services, ILogger logger)
{
    const int attempts = 12;
    for (var attempt = 1; attempt <= attempts; attempt++)
    {
        try
        {
            await using var scope = services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<SistemaPedidosDbContext>();
            await db.Database.MigrateAsync();
            logger.LogInformation("Migraciones aplicadas correctamente.");
            return;
        }
        catch (Exception exception) when (attempt < attempts)
        {
            logger.LogWarning("SQL Server aún no está disponible (intento {Attempt}/{Attempts}): {Message}", attempt, attempts, exception.Message);
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}

public partial class Program;
