using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Application.DTOs;

public sealed record ProductoDto(int Id, string Nombre, string? Descripcion, decimal Precio, int Stock);

public sealed class ProductoInputDto
{
    [Required(ErrorMessage = "El nombre es obligatorio."), StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Range(typeof(decimal), "0.01", "999999999999.99", ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }
}
