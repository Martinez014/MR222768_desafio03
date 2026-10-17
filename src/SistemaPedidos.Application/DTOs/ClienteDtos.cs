using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Application.DTOs;

public sealed record ClienteDto(int Id, string Nombre, string Apellido, string Email);

public sealed class ClienteInputDto
{
    [Required(ErrorMessage = "El nombre es obligatorio."), StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio."), StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio."), EmailAddress(ErrorMessage = "El email no tiene un formato válido."), StringLength(254)]
    public string Email { get; set; } = string.Empty;
}
