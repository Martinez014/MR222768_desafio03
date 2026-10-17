using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Repositories;

public sealed class PedidoRepository(SistemaPedidosDbContext context) : IPedidoRepository
{
    public async Task<IReadOnlyList<Pedido>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Pedidos.AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Producto)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public Task<Pedido?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        context.Pedidos
            .Include(x => x.Cliente)
            .Include(x => x.Producto)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task AddAsync(Pedido pedido, CancellationToken cancellationToken = default) =>
        context.Pedidos.AddAsync(pedido, cancellationToken).AsTask();

    public void Remove(Pedido pedido) => context.Pedidos.Remove(pedido);
}
