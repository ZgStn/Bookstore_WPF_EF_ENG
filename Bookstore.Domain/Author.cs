namespace Bookstore.Domain;

public partial class Author
{
    public int AuthorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? DateOfDeath { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
