using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Repositories;

public sealed class ProductoRepository(SistemaPedidosDbContext context) : IProductoRepository
{
    public async Task<IReadOnlyList<Producto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Productos.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<Producto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        context.Productos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task AddAsync(Producto producto, CancellationToken cancellationToken = default) =>
        context.Productos.AddAsync(producto, cancellationToken).AsTask();

    public void Remove(Producto producto) => context.Productos.Remove(producto);

    public Task<bool> HasPedidosAsync(int id, CancellationToken cancellationToken = default) =>
        context.Pedidos.AnyAsync(x => x.ProductoId == id, cancellationToken);
}
