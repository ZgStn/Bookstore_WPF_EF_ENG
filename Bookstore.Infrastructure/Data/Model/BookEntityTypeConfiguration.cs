using Bookstore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infrastructure.Data.Model;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {

        builder.HasKey(e => e.Isbn13);

        builder.Property(e => e.Isbn13)
            .HasMaxLength(13)
            .HasColumnName("ISBN13");
        builder.Property(e => e.Language).HasMaxLength(50);
        builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.Title).HasMaxLength(200);

        builder.HasOne(d => d.Author).WithMany(p => p.Books)
            .HasForeignKey(d => d.AuthorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Books_Authors");

        builder.HasOne(d => d.Genre).WithMany(p => p.Books)
            .HasForeignKey(d => d.GenreId)
            .HasConstraintName("FK_Books_Genres");

    }
}
