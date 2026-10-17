using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Application.DTOs;

public sealed record ClienteResumenDto(int Id, string Nombre, string Apellido, string Email);
public sealed record ProductoResumenDto(int Id, string Nombre, decimal Precio);
public sealed record PedidoDto(int Id, DateTime FechaPedido, int Cantidad, int ClienteId, int ProductoId,
    ClienteResumenDto Cliente, ProductoResumenDto Producto);

public sealed class PedidoInputDto
{
    [Required, Range(typeof(DateTime), "2000-01-01", "2100-12-31", ErrorMessage = "La fecha del pedido debe estar entre 2000 y 2100.")]
    public DateTime FechaPedido { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public int Cantidad { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un cliente válido.")]
    public int ClienteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un producto válido.")]
    public int ProductoId { get; set; }
}
