using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Infrastructure.Persistence.Configurations;

public sealed class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos", table =>
            table.HasCheckConstraint("CK_Pedidos_Cantidad", "[Cantidad] > 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FechaPedido).HasColumnType("datetime2").IsRequired();
        builder.HasIndex(x => x.ClienteId);
        builder.HasIndex(x => x.ProductoId);
        builder.HasOne(x => x.Cliente)
            .WithMany(x => x.Pedidos)
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Producto)
            .WithMany(x => x.Pedidos)
            .HasForeignKey(x => x.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
