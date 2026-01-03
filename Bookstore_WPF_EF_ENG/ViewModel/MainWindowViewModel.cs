using System.Collections.ObjectModel;
using Bookstore.Domain;
using Bookstore.Infrastructure.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_WPF_EF_ENG.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> Stores { get; private set; }

        private string? _selectedStore;

        public string? SelectedStore
        {
            get => _selectedStore;

            set
            {
                _selectedStore = value;
                RaisePropertyChanged();
                LoadInventories();

            }
        }
        public ObservableCollection<Inventory> Inventories { get; private set; }


        public MainWindowViewModel() // TODO:denna syncront, temporär- bytt till async senare
        {
            LoadStores();

        }
        private void LoadStores()
        {
            using var db = new BookstoreContext();

            Stores = new ObservableCollection<string>(
                db.Stores.Select(s => s.Name).ToList() //TODO: Behövs det .Distinct() här?

            );

            SelectedStore = Stores.FirstOrDefault();
        }

        private void LoadInventories()
        {
            using var db = new BookstoreContext();

            Inventories = new ObservableCollection<Inventory>(
                 db.Inventories.Include(i => i.Isbn13Navigation)
                 .Where(i => i.Store.Name == SelectedStore).ToList()






             );


        }
    }
}
