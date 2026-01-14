using Bookstore.Domain;
using Bookstore.Infrastructure.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Service;

public class BookstoreService
{
    private readonly BookstoreContext _db = new();

    public void AddInventory(Inventory inventory) => _db.Inventories.Add(inventory);
    public void RemoveInventory(Inventory inventory) => _db.Inventories.Remove(inventory);

    public async Task<Store> GetStoreByNameAsync(string selectedStore)
    {
        return await _db.Stores
              .FirstAsync(s => s.Name == selectedStore);
    }

    public async Task<List<string>> GetStoresAsync()
    {
        return await _db.Stores
               .Select(s => s.Name)
               .ToListAsync();
    }

    public async Task<List<Inventory>> GetInventoriesAsync(string selectedStore)
    {
        return await _db.Inventories
                 .Include(i => i.Isbn13Navigation)
                 .ThenInclude(b => b.Author)
                 .Where(i => i.Store.Name == selectedStore)
                 .ToListAsync();
    }

    public async Task<List<Book>> GetBooksWithAuthorsAsync()
    {
        return await _db.Books
                    .Include(b => b.Author)
                    .ToListAsync();
    }

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();

    public bool HasChanges() => _db.ChangeTracker.HasChanges();

}


