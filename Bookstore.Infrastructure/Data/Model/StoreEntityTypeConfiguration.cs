using Bookstore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infrastructure.Data.Model;

public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.Property(e => e.City).HasMaxLength(50);
        builder.Property(e => e.Country).HasMaxLength(50);
        builder.Property(e => e.Name).HasMaxLength(50);
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.Property(e => e.PostalCode).HasMaxLength(10);
        builder.Property(e => e.Street).HasMaxLength(100);

    }
}
