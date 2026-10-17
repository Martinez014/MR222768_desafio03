using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Domain.Entities;

public sealed class Producto
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Range(typeof(decimal), "0.01", "999999999999.99")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Timestamp]
    public byte[] Version { get; set; } = [];

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
