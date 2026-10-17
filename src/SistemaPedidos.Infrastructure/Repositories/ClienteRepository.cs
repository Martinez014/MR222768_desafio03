using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Repositories;

public sealed class ClienteRepository(SistemaPedidosDbContext context) : IClienteRepository
{
    public async Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Clientes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        context.Clientes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default) =>
        context.Clientes.AddAsync(cliente, cancellationToken).AsTask();

    public void Remove(Cliente cliente) => context.Clientes.Remove(cliente);

    public Task<bool> HasPedidosAsync(int id, CancellationToken cancellationToken = default) =>
        context.Pedidos.AnyAsync(x => x.ClienteId == id, cancellationToken);
}
