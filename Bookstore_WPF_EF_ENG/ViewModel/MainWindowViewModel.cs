using System.Collections.ObjectModel;
using Bookstore.Domain;
using Bookstore.Infrastructure.Data.Model;
using Bookstore_WPF_EF_ENG.Command;
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

                LoadInventories();

                RaisePropertyChanged();// TODO: varför har vi denna, (två st)
                RaisePropertyChanged("Inventories");

            }
        }


        public ObservableCollection<Book> Books { get; private set; }

        private string? _selectedBook;

        public string? SelectedBook
        {
            get => _selectedBook;

            set
            {
                _selectedBook = value;

                //LoadInventories();

                RaisePropertyChanged();
                ShowBookDetailsCommand.RaiseCanExecuteChanged();
                //RaisePropertyChanged("Books");

            }
        }

        public ObservableCollection<Inventory> Inventories { get; private set; }

        private string? _selectedInventory;

        public string? SelectedInventory
        {
            get => _selectedInventory;

            set
            {
                _selectedInventory = value;
                RaisePropertyChanged();


            }
        }

        public Action<object> ShowBookDetails { get; set; }
        public DelegateCommand ShowBookDetailsCommand { get; private set; }

        public MainWindowViewModel() // TODO:denna syncront, temporär- bytt till async senare
        {
            ShowBookDetailsCommand = new DelegateCommand(DoShowBookDetails, CanShowBookDetails);
            LoadStores();

        }

        private void DoShowBookDetails(object obj) => ShowBookDetails(obj);


        private bool CanShowBookDetails(object? arg) => SelectedBook is not null;

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
