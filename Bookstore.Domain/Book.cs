namespace Bookstore.Domain;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public decimal Price { get; set; }

    public DateOnly? PublishingDate { get; set; }

    public int AuthorId { get; set; }

    public int NumberOfPages { get; set; }

    public int? GenreId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
