using System.ComponentModel.DataAnnotations;
using SistemaPedidos.Application.DTOs;

namespace SistemaPedidos.Tests.Validation;

public sealed class DtoValidationTests
{
    [Fact]
    public void ClienteConEmailInvalido_EsRechazado()
    {
        var dto = new ClienteInputDto { Nombre = "Ana", Apellido = "López", Email = "correo-invalido" };

        var errors = Validate(dto);

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(ClienteInputDto.Email)));
    }

    [Fact]
    public void ClienteSinCamposObligatorios_EsRechazado()
    {
        var dto = new ClienteInputDto { Nombre = "", Apellido = "", Email = "" };

        var errors = Validate(dto);

        Assert.True(errors.Count >= 3);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(10, -1)]
    public void ProductoConPrecioOStockInvalido_EsRechazado(decimal precio, int stock)
    {
        var dto = new ProductoInputDto { Nombre = "Producto", Precio = precio, Stock = stock };

        Assert.NotEmpty(Validate(dto));
    }

    private static List<ValidationResult> Validate(object value)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(value, new ValidationContext(value), results, true);
        return results;
    }
}
