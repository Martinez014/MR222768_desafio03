using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaPedidos.Infrastructure.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SistemaPedidosDbContext>
{
    public SistemaPedidosDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Server=localhost,1433;Database=SistemaPedidosDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True";
        var options = new DbContextOptionsBuilder<SistemaPedidosDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        return new SistemaPedidosDbContext(options);
    }
}
