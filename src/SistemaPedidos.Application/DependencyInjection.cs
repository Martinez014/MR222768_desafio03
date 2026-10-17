using Microsoft.Extensions.DependencyInjection;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Application.Services;

namespace SistemaPedidos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) => services
        .AddScoped<IClienteService, ClienteService>()
        .AddScoped<IProductoService, ProductoService>()
        .AddScoped<IPedidoService, PedidoService>();
}
