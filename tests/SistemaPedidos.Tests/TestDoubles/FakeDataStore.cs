using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Tests.TestDoubles;

internal sealed class FakeDataStore : IClienteRepository, IProductoRepository, IPedidoRepository, IUnitOfWork
{
    public List<Cliente> Clientes { get; } = [];
    public List<Producto> Productos { get; } = [];
    public List<Pedido> Pedidos { get; } = [];

    Task<IReadOnlyList<Cliente>> IClienteRepository.GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<Cliente>>(Clientes.ToList());

    Task<Cliente?> IClienteRepository.GetByIdAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(Clientes.FirstOrDefault(x => x.Id == id));

    Task IClienteRepository.AddAsync(Cliente cliente, CancellationToken cancellationToken)
    {
        cliente.Id = cliente.Id == 0 ? NextId(Clientes.Select(x => x.Id)) : cliente.Id;
        Clientes.Add(cliente);
        return Task.CompletedTask;
    }

    void IClienteRepository.Remove(Cliente cliente) => Clientes.Remove(cliente);

    Task<bool> IClienteRepository.HasPedidosAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(Pedidos.Any(x => x.ClienteId == id));

    Task<IReadOnlyList<Producto>> IProductoRepository.GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<Producto>>(Productos.ToList());

    Task<Producto?> IProductoRepository.GetByIdAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(Productos.FirstOrDefault(x => x.Id == id));

    Task IProductoRepository.AddAsync(Producto producto, CancellationToken cancellationToken)
    {
        producto.Id = producto.Id == 0 ? NextId(Productos.Select(x => x.Id)) : producto.Id;
        Productos.Add(producto);
        return Task.CompletedTask;
    }

    void IProductoRepository.Remove(Producto producto) => Productos.Remove(producto);

    Task<bool> IProductoRepository.HasPedidosAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(Pedidos.Any(x => x.ProductoId == id));

    Task<IReadOnlyList<Pedido>> IPedidoRepository.GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<Pedido>>(Pedidos.ToList());

    Task<Pedido?> IPedidoRepository.GetByIdAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(Pedidos.FirstOrDefault(x => x.Id == id));

    Task IPedidoRepository.AddAsync(Pedido pedido, CancellationToken cancellationToken)
    {
        pedido.Id = pedido.Id == 0 ? NextId(Pedidos.Select(x => x.Id)) : pedido.Id;
        Pedidos.Add(pedido);
        return Task.CompletedTask;
    }

    void IPedidoRepository.Remove(Pedido pedido) => Pedidos.Remove(pedido);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => Task.FromResult(1);

    public Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default) => operation();

    private static int NextId(IEnumerable<int> ids) => ids.DefaultIfEmpty(0).Max() + 1;
}
