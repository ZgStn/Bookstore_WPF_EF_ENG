using System.Collections.ObjectModel;
using Bookstore.Domain;
using Bookstore.Infrastructure.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Presentation.ViewModel
{
    internal class BookDetailsViewModel : ViewModelBase // TODO: use or discard?
    {
        public ObservableCollection<Inventory> Details { get; set; }
        public BookDetailsViewModel(Inventory inventory)
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
