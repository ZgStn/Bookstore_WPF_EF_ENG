using Bookstore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infrastructure.Data.Model;

public class InventoryEntityTypeConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {

        builder.HasKey(e => new { e.StoreId, e.Isbn13 });

        builder.ToTable("Inventory");

        builder.Property(e => e.Isbn13)
            .HasMaxLength(13)
            .HasColumnName("ISBN13");

        builder.HasOne(d => d.Isbn13Navigation).WithMany(p => p.Inventories)
            .HasForeignKey(d => d.Isbn13)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Inventory_Books");

        builder.HasOne(d => d.Store).WithMany(p => p.Inventories)
            .HasForeignKey(d => d.StoreId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Inventory_Stores");

    }
}
