using SistemaPedidos.Application.DTOs;

namespace SistemaPedidos.Application.Interfaces;

public interface IClienteService
{
    Task<IReadOnlyList<ClienteDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClienteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ClienteDto> CreateAsync(ClienteInputDto input, CancellationToken cancellationToken = default);
    Task<ClienteDto> UpdateAsync(int id, ClienteInputDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IProductoService
{
    Task<IReadOnlyList<ProductoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductoDto> CreateAsync(ProductoInputDto input, CancellationToken cancellationToken = default);
    Task<ProductoDto> UpdateAsync(int id, ProductoInputDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IPedidoService
{
    Task<IReadOnlyList<PedidoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PedidoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PedidoDto> CreateAsync(PedidoInputDto input, CancellationToken cancellationToken = default);
    Task<PedidoDto> UpdateAsync(int id, PedidoInputDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
