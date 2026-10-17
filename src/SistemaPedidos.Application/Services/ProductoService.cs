using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;

namespace SistemaPedidos.Application.Services;

public sealed class ProductoService(IProductoRepository repository, IUnitOfWork unitOfWork) : IProductoService
{
    public async Task<IReadOnlyList<ProductoDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await repository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<ProductoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<ProductoDto> CreateAsync(ProductoInputDto input, CancellationToken cancellationToken = default)
    {
        Validate(input);
        var entity = new Producto { Nombre = input.Nombre.Trim(), Descripcion = input.Descripcion?.Trim(), Precio = input.Precio, Stock = input.Stock };
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<ProductoDto> UpdateAsync(int id, ProductoInputDto input, CancellationToken cancellationToken = default)
    {
        Validate(input);
        var entity = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException($"No existe el producto con id {id}.");
        entity.Nombre = input.Nombre.Trim();
        entity.Descripcion = input.Descripcion?.Trim();
        entity.Precio = input.Precio;
        entity.Stock = input.Stock;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException($"No existe el producto con id {id}.");
        if (await repository.HasPedidosAsync(id, cancellationToken))
            throw new BusinessConflictException("No se puede eliminar el producto porque tiene pedidos asociados.");
        repository.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void Validate(ProductoInputDto input)
    {
        if (input.Precio <= 0) throw new BusinessConflictException("El precio debe ser mayor que cero.");
        if (input.Stock < 0) throw new BusinessConflictException("El stock no puede ser negativo.");
    }

    private static ProductoDto Map(Producto value) => new(value.Id, value.Nombre, value.Descripcion, value.Precio, value.Stock);
}
