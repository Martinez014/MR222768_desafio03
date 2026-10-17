using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Domain.Entities;

public sealed class Pedido
{
    public int Id { get; set; }

    [Required]
    public DateTime FechaPedido { get; set; }

    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Range(1, int.MaxValue)]
    public int ClienteId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductoId { get; set; }

    public Cliente Cliente { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
