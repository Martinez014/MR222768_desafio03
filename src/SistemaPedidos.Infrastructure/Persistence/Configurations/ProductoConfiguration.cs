using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Infrastructure.Persistence.Configurations;

public sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos", table =>
        {
            table.HasCheckConstraint("CK_Productos_Precio", "[Precio] > 0");
            table.HasCheckConstraint("CK_Productos_Stock", "[Stock] >= 0");
        });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Descripcion).HasMaxLength(500);
        builder.Property(x => x.Precio).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Version).IsRowVersion();
    }
}
