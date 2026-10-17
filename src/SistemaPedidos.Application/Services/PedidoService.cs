using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;

namespace SistemaPedidos.Application.Services;

public sealed class PedidoService(
    IPedidoRepository pedidos,
    IClienteRepository clientes,
    IProductoRepository productos,
    IUnitOfWork unitOfWork) : IPedidoService
{
    public async Task<IReadOnlyList<PedidoDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await pedidos.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<PedidoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await pedidos.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public Task<PedidoDto> CreateAsync(PedidoInputDto input, CancellationToken cancellationToken = default)
    {
        Validate(input);
        return unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var cliente = await clientes.GetByIdAsync(input.ClienteId, cancellationToken)
                ?? throw new EntityNotFoundException($"No existe el cliente con id {input.ClienteId}.");
            var producto = await productos.GetByIdAsync(input.ProductoId, cancellationToken)
                ?? throw new EntityNotFoundException($"No existe el producto con id {input.ProductoId}.");
            EnsureStock(producto, input.Cantidad);
            producto.Stock -= input.Cantidad;
            var entity = new Pedido
            {
                FechaPedido = input.FechaPedido,
                Cantidad = input.Cantidad,
                ClienteId = cliente.Id,
                ProductoId = producto.Id,
                Cliente = cliente,
                Producto = producto
            };
            await pedidos.AddAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Map(entity);
        }, cancellationToken);
    }

    public Task<PedidoDto> UpdateAsync(int id, PedidoInputDto input, CancellationToken cancellationToken = default)
    {
        Validate(input);
        return unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var pedido = await pedidos.GetByIdAsync(id, cancellationToken)
                ?? throw new EntityNotFoundException($"No existe el pedido con id {id}.");
            var cliente = await clientes.GetByIdAsync(input.ClienteId, cancellationToken)
                ?? throw new EntityNotFoundException($"No existe el cliente con id {input.ClienteId}.");

            if (pedido.ProductoId == input.ProductoId)
            {
                var diferencia = input.Cantidad - pedido.Cantidad;
                if (diferencia > 0) EnsureStock(pedido.Producto, diferencia);
                pedido.Producto.Stock -= diferencia;
            }
            else
            {
                var nuevoProducto = await productos.GetByIdAsync(input.ProductoId, cancellationToken)
                    ?? throw new EntityNotFoundException($"No existe el producto con id {input.ProductoId}.");
                EnsureStock(nuevoProducto, input.Cantidad);
                pedido.Producto.Stock += pedido.Cantidad;
                nuevoProducto.Stock -= input.Cantidad;
                pedido.Producto = nuevoProducto;
                pedido.ProductoId = nuevoProducto.Id;
            }

            pedido.Cliente = cliente;
            pedido.ClienteId = cliente.Id;
            pedido.FechaPedido = input.FechaPedido;
            pedido.Cantidad = input.Cantidad;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Map(pedido);
        }, cancellationToken);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var pedido = await pedidos.GetByIdAsync(id, cancellationToken)
                ?? throw new EntityNotFoundException($"No existe el pedido con id {id}.");
            pedido.Producto.Stock += pedido.Cantidad;
            pedidos.Remove(pedido);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }, cancellationToken);

    private static void Validate(PedidoInputDto input)
    {
        if (input.Cantidad <= 0) throw new BusinessConflictException("La cantidad debe ser mayor que cero.");
        if (input.FechaPedido == default) throw new BusinessConflictException("La fecha del pedido es obligatoria.");
    }

    private static void EnsureStock(Producto producto, int cantidad)
    {
        if (producto.Stock < cantidad)
            throw new BusinessConflictException($"Stock insuficiente para el producto {producto.Id}. Disponible: {producto.Stock}.");
    }

    private static PedidoDto Map(Pedido value) => new(
        value.Id, value.FechaPedido, value.Cantidad, value.ClienteId, value.ProductoId,
        new ClienteResumenDto(value.Cliente.Id, value.Cliente.Nombre, value.Cliente.Apellido, value.Cliente.Email),
        new ProductoResumenDto(value.Producto.Id, value.Producto.Nombre, value.Producto.Precio));
}
