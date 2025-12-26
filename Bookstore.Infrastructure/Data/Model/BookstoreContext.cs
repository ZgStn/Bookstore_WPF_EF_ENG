using Bookstore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infrastructure.Data.Model;

public partial class BookstoreContext : DbContext
{
    public BookstoreContext()
    {
    }

    public BookstoreContext(DbContextOptions<BookstoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(" Data Source=localhost;Database = BookstoreDb;Integrated Security=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AuthorEntityTypeConfiguration().Configure(modelBuilder.Entity<Author>());
        new BookEntityTypeConfiguration().Configure(modelBuilder.Entity<Book>());
        new GenreEntityTypeConfiguration().Configure(modelBuilder.Entity<Genre>());
        new InventoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Inventory>());
        new StoreEntityTypeConfiguration().Configure(modelBuilder.Entity<Store>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(e => e.FirstName).HasMaxLength(20);
        builder.Property(e => e.LastName).HasMaxLength(20);
    }
}

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

public class GenreEntityTypeConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {

        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Name).HasMaxLength(100);

    }
}

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
