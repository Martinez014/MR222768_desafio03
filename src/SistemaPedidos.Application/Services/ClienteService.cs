using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;

namespace SistemaPedidos.Application.Services;

public sealed class ClienteService(IClienteRepository repository, IUnitOfWork unitOfWork) : IClienteService
{
    public async Task<IReadOnlyList<ClienteDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await repository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<ClienteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<ClienteDto> CreateAsync(ClienteInputDto input, CancellationToken cancellationToken = default)
    {
        var entity = new Cliente { Nombre = input.Nombre.Trim(), Apellido = input.Apellido.Trim(), Email = input.Email.Trim() };
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<ClienteDto> UpdateAsync(int id, ClienteInputDto input, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException($"No existe el cliente con id {id}.");
        entity.Nombre = input.Nombre.Trim();
        entity.Apellido = input.Apellido.Trim();
        entity.Email = input.Email.Trim();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException($"No existe el cliente con id {id}.");
        if (await repository.HasPedidosAsync(id, cancellationToken))
            throw new BusinessConflictException("No se puede eliminar el cliente porque tiene pedidos asociados.");
        repository.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static ClienteDto Map(Cliente value) => new(value.Id, value.Nombre, value.Apellido, value.Email);
}
