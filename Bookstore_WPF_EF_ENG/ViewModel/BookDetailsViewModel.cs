using System.Collections.ObjectModel;
using Bookstore.Domain;
using Bookstore.Infrastructure.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Presentation.ViewModel
{
    internal class BookDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<Inventory> Details { get; set; } //TODO: vi har samma observablecollection i mainwindowviewmodel, blir det ett problem?
        public BookDetailsViewModel(Inventory inventory) // TODO: make awaited (not in constructor)
        {
            _ = LoadQuantityAsync(inventory.Quantity);
        }

        private async Task LoadQuantityAsync(int quantity)
        {
            using var db = new BookstoreContext();

            Details = new ObservableCollection<Inventory>(
                await db.Inventories.Where(i => i.Quantity == quantity).ToListAsync()
                );

            RaisePropertyChanged(nameof(Details));
        }
    }
}
