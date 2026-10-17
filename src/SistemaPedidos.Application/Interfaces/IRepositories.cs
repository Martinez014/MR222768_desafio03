using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Application.Interfaces;

public interface IClienteRepository
{
    Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default);
    void Remove(Cliente cliente);
    Task<bool> HasPedidosAsync(int id, CancellationToken cancellationToken = default);
}

public interface IProductoRepository
{
    Task<IReadOnlyList<Producto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Producto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Producto producto, CancellationToken cancellationToken = default);
    void Remove(Producto producto);
    Task<bool> HasPedidosAsync(int id, CancellationToken cancellationToken = default);
}

public interface IPedidoRepository
{
    Task<IReadOnlyList<Pedido>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Pedido?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Pedido pedido, CancellationToken cancellationToken = default);
    void Remove(Pedido pedido);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
}
