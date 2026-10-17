using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Application.Services;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Tests.TestDoubles;

namespace SistemaPedidos.Tests.Services;

public sealed class PedidoServiceTests
{
    [Fact]
    public async Task CrearPedido_Valido_CreaPedidoYDescuentaStock()
    {
        var (store, service) = CreateService(stock: 10);

        var result = await service.CreateAsync(Input(cantidad: 3));

        Assert.Equal(1, result.Id);
        Assert.Equal(3, result.Cantidad);
        Assert.Equal(7, store.Productos[0].Stock);
        Assert.Single(store.Pedidos);
    }

    [Fact]
    public async Task ConsultarPedidoExistente_DevuelveClienteYProducto()
    {
        var (store, service) = CreateService(stock: 10);
        var created = await service.CreateAsync(Input(2));

        var result = await service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal("Ana", result.Cliente.Nombre);
        Assert.Equal("Teclado", result.Producto.Nombre);
    }

    [Fact]
    public async Task ConsultarPedidoInexistente_DevuelveNull()
    {
        var (_, service) = CreateService(stock: 10);

        Assert.Null(await service.GetByIdAsync(999));
    }

    [Fact]
    public async Task CrearPedido_SinStockSuficiente_LanzaConflictoYNoModificaStock()
    {
        var (store, service) = CreateService(stock: 2);

        await Assert.ThrowsAsync<BusinessConflictException>(() => service.CreateAsync(Input(3)));

        Assert.Equal(2, store.Productos[0].Stock);
        Assert.Empty(store.Pedidos);
    }

    [Fact]
    public async Task EliminarPedido_RestauraStock()
    {
        var (store, service) = CreateService(stock: 10);
        var created = await service.CreateAsync(Input(4));

        await service.DeleteAsync(created.Id);

        Assert.Equal(10, store.Productos[0].Stock);
        Assert.Empty(store.Pedidos);
    }

    [Fact]
    public async Task ActualizarCantidad_AjustaStockPorDiferencia()
    {
        var (store, service) = CreateService(stock: 10);
        var created = await service.CreateAsync(Input(2));

        var result = await service.UpdateAsync(created.Id, Input(5));

        Assert.Equal(5, result.Cantidad);
        Assert.Equal(5, store.Productos[0].Stock);
    }

    [Fact]
    public async Task ActualizarCambiandoProducto_RestauraAnteriorYDescuentaNuevo()
    {
        var (store, service) = CreateService(stock: 10);
        store.Productos.Add(new Producto { Id = 2, Nombre = "Mouse", Precio = 20, Stock = 6 });
        var created = await service.CreateAsync(Input(2));
        var update = Input(3);
        update.ProductoId = 2;

        await service.UpdateAsync(created.Id, update);

        Assert.Equal(10, store.Productos.Single(x => x.Id == 1).Stock);
        Assert.Equal(3, store.Productos.Single(x => x.Id == 2).Stock);
    }

    [Theory]
    [InlineData(999, 1)]
    [InlineData(1, 999)]
    public async Task CrearPedido_ConRelacionInexistente_LanzaNoEncontrado(int clienteId, int productoId)
    {
        var (_, service) = CreateService(stock: 10);
        var input = Input(1);
        input.ClienteId = clienteId;
        input.ProductoId = productoId;

        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.CreateAsync(input));
    }

    private static (FakeDataStore Store, PedidoService Service) CreateService(int stock)
    {
        var store = new FakeDataStore();
        store.Clientes.Add(new Cliente { Id = 1, Nombre = "Ana", Apellido = "López", Email = "ana@example.com" });
        store.Productos.Add(new Producto { Id = 1, Nombre = "Teclado", Precio = 50, Stock = stock });
        return (store, new PedidoService(store, store, store, store));
    }

    private static PedidoInputDto Input(int cantidad) => new()
    {
        ClienteId = 1,
        ProductoId = 1,
        Cantidad = cantidad,
        FechaPedido = DateTime.UtcNow
    };
}
