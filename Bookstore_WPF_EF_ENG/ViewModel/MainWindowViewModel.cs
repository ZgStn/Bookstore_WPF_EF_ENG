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

                _ = LoadInventoriesAsync();

                //RaisePropertyChanged(); // TODO: varför har vi denna, (två st)
                RaisePropertyChanged("Inventories");

            }
        }

        public Book? AddedBook { get; set; }
        public int NewQuantity { get; set; }

        public DelegateCommand AddBookCommand { get; }

        public ObservableCollection<Book> Books { get; set; }

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
                RaisePropertyChanged("Books");

            }
        }

        public ObservableCollection<Inventory> Inventories { get; private set; }

        private Inventory? _selectedInventory;

        public Inventory? SelectedInventory
        {
            get => _selectedInventory;

            set
            {
                _selectedInventory = value;
                RaisePropertyChanged();
                ShowBookDetailsCommand.RaiseCanExecuteChanged();

            }
        }

        public Action ShowBookDetails { get; set; }
        public DelegateCommand ShowBookDetailsCommand { get; private set; }

        public Action<string> ShowMessage { get; set; }
        public MainWindowViewModel() //TODO:denna syncront, temporär- bytt till async senare
        {
            ShowBookDetailsCommand = new DelegateCommand(DoShowBookDetails, CanShowBookDetails);
            AddBookCommand = new DelegateCommand(AddBook, CanAddBook);
            _ = InitializeAsync();
        }

        private bool CanAddBook(object? arg)
        {
            return SelectedStore != null
                   && AddedBook != null;
        }

        private void AddBook(object? obj)
        {
            var newInventory = new Inventory()
            {
               Isbn13Navigation = AddedBook,
               Quantity = NewQuantity
            };

            Inventories.Add(newInventory);

            AddedBook = null;
            NewQuantity = 0;

            RaisePropertyChanged(nameof(AddedBook));
            RaisePropertyChanged(nameof(NewQuantity));

        }

        private async Task InitializeAsync()
        {
            await LoadStoresAsync();
            await LoadBooksAsync();
            //await LoadInventoriesAsync();

        }
        private void DoShowBookDetails(object obj) => ShowBookDetails();

        //private void DoShowBookDetails(object obj) => ShowMessage?.Invoke("Button clicked!");//ShowBookDetails(obj);


        private bool CanShowBookDetails(object? arg) => SelectedInventory is not null;

        private async Task LoadStoresAsync() // TODO: make async 
        {
            using var db = new BookstoreContext();

            Stores = new ObservableCollection<string>(
               await db.Stores.Select(s => s.Name).ToListAsync() //TODO: Behövs det .Distinct() här?

            );
            RaisePropertyChanged(nameof(Stores));
        }

        private async Task LoadInventoriesAsync() // TODO: make async
        {
            using var db = new BookstoreContext();

            Inventories = new ObservableCollection<Inventory>(
                 await db.Inventories
                 .Include(i => i.Isbn13Navigation)
                 .ThenInclude(b => b.Author)
                 .Where(i => i.Store.Name == SelectedStore).ToListAsync()

            );

            RaisePropertyChanged(nameof(Inventories));

        }

        private async Task LoadBooksAsync()
        {
            using var db = new BookstoreContext();

            Books = new ObservableCollection<Book>(
                await db.Books
                    .Include(b => b.Author)
                    .ToListAsync()
                    );
            RaisePropertyChanged(nameof(Books));
        }
    }
}
