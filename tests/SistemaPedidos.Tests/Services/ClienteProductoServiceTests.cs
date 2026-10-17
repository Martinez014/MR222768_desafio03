using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Application.Services;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Tests.TestDoubles;

namespace SistemaPedidos.Tests.Services;

public sealed class ClienteProductoServiceTests
{
    [Fact]
    public async Task CrearCliente_ConDatosValidos_CreaRegistro()
    {
        var store = new FakeDataStore();
        var service = new ClienteService(store, store);

        var result = await service.CreateAsync(new ClienteInputDto
        {
            Nombre = " Ana ", Apellido = " López ", Email = " ana@example.com "
        });

        Assert.Equal(1, result.Id);
        Assert.Equal("Ana", result.Nombre);
        Assert.Single(store.Clientes);
    }

    [Fact]
    public async Task CrearProducto_ConDatosValidos_CreaRegistro()
    {
        var store = new FakeDataStore();
        var service = new ProductoService(store, store);

        var result = await service.CreateAsync(new ProductoInputDto
        {
            Nombre = "Teclado", Descripcion = "Mecánico", Precio = 49.99m, Stock = 10
        });

        Assert.Equal(49.99m, result.Precio);
        Assert.Equal(10, result.Stock);
        Assert.Single(store.Productos);
    }

    [Fact]
    public async Task ConsultarClienteInexistente_DevuelveNull()
    {
        var store = new FakeDataStore();
        var service = new ClienteService(store, store);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task EliminarClienteConPedidos_LanzaConflicto()
    {
        var store = SeedStore();
        var service = new ClienteService(store, store);

        await Assert.ThrowsAsync<BusinessConflictException>(() => service.DeleteAsync(1));
    }

    [Fact]
    public async Task EliminarProductoConPedidos_LanzaConflicto()
    {
        var store = SeedStore();
        var service = new ProductoService(store, store);

        await Assert.ThrowsAsync<BusinessConflictException>(() => service.DeleteAsync(1));
    }

    private static FakeDataStore SeedStore()
    {
        var store = new FakeDataStore();
        var cliente = new Cliente { Id = 1, Nombre = "Ana", Apellido = "López", Email = "ana@example.com" };
        var producto = new Producto { Id = 1, Nombre = "Teclado", Precio = 50, Stock = 8 };
        store.Clientes.Add(cliente);
        store.Productos.Add(producto);
        store.Pedidos.Add(new Pedido
        {
            Id = 1, ClienteId = 1, ProductoId = 1, Cliente = cliente, Producto = producto,
            Cantidad = 2, FechaPedido = DateTime.UtcNow
        });
        return store;
    }
}
