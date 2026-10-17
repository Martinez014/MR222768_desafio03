using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Infrastructure.Persistence;

public sealed class SistemaPedidosDbContext(DbContextOptions<SistemaPedidosDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SistemaPedidosDbContext).Assembly);
    }
}
