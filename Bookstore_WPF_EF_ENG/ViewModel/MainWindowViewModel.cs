using System.Collections.ObjectModel;
using Bookstore.Domain;
using Bookstore.Infrastructure.Data.Model;
using Bookstore_WPF_EF_ENG.Command;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_WPF_EF_ENG.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly BookstoreContext _db;

        public MainWindowViewModel() //TODO:denna syncront, temporär- bytt till async senare
        {
            _db = new BookstoreContext();
            ShowBookDetailsCommand = new DelegateCommand(DoShowBookDetails, CanShowBookDetails);
            AddBookCommand = new DelegateCommand(AddBook, CanAddBook);
            SaveChangesCommand = new DelegateCommand(
                async _ => await SaveChangesAsync(),
                _ => _db.ChangeTracker.HasChanges());

            _ = InitializeAsync();
        }

        private async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
            RaisePropertyChanged(nameof(AvailableBooks));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand SaveChangesCommand { get; }
        public ObservableCollection<string> Stores { get; private set; } = new();

        private string? _selectedStore;

        public string? SelectedStore
        {
            get => _selectedStore;

            set
            {
                _selectedStore = value;
                RaisePropertyChanged(); 

                _ = LoadInventoriesAsync();
                AddBookCommand.RaiseCanExecuteChanged();
            }
        }

        private Book? _addedBook;

        public Book? AddedBook
        {
            get => _addedBook;
            set
            {
                _addedBook = value;
                RaisePropertyChanged();
                AddBookCommand.RaiseCanExecuteChanged();
            }
        }

        //public int NewQuantity { get; set; }

        public DelegateCommand AddBookCommand { get; } 
        
        //public DelegateCommand AddNewBookCommand { get; } // TODO: for extra credit

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
                SaveChangesCommand.RaiseCanExecuteChanged();

            }
        }

        public Action ShowBookDetails { get; set; }
        public DelegateCommand ShowBookDetailsCommand { get; private set; }

        //public Action<string> ShowMessage { get; set; }

        public ObservableCollection<Book> AvailableBooks 
        {
            get
            {
                if (Books == null || Inventories == null)
                    return new ObservableCollection<Book>();

                var availableToAdd = Books
                                .Where(b => !Inventories.Any(i => i.Isbn13 == b.Isbn13))
                                .ToList();

                return new ObservableCollection<Book>(availableToAdd);
            } }

        

        private bool CanAddBook(object? arg)
        {
            return SelectedStore != null
                   && AddedBook != null;
        }

        private void AddBook(object? obj)
        {
            var newInventory = new Inventory()
            {
                Isbn13 = AddedBook!.Isbn13,
                Isbn13Navigation = AddedBook
                //Quantity = NewQuantity
            };

            Inventories.Add(newInventory);
            RaisePropertyChanged(nameof(AddedBook));
            SaveChangesCommand.RaiseCanExecuteChanged();


            //NewQuantity = 0;
            //AvailableBooksPlaceholder = "Available book";
            //RaisePropertyChanged(nameof(NewQuantity));

        }





        //private string _availableBooksPlaceholder;     

        //public string AvailableBooksPlaceholder
        //{
        //    get => _availableBooksPlaceholder; 
        //    set 
        //    { 
        //        _availableBooksPlaceholder = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public Book PlaceHolderBook { get; } = new Book { Title = "Available books" };

        private async Task InitializeAsync()
        {
            await LoadStoresAsync();
            await LoadBooksAsync();
            //await LoadInventoriesAsync();

        }
        private void DoShowBookDetails(object obj) => ShowBookDetails();

        //private void DoShowBookDetails(object obj) => ShowMessage?.Invoke("Button clicked!");//ShowBookDetails(obj);


        private bool CanShowBookDetails(object? arg) => SelectedInventory is not null;

        private async Task LoadStoresAsync()
        {
            Stores = new ObservableCollection<string>(
               await _db.Stores
               .Select(s => s.Name)
               .ToListAsync()

            );
            RaisePropertyChanged(nameof(Stores));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private async Task LoadInventoriesAsync()
        {
            Inventories = new ObservableCollection<Inventory>(
                 await _db.Inventories
                 .Include(i => i.Isbn13Navigation)
                 .ThenInclude(b => b.Author)
                 .Where(i => i.Store.Name == SelectedStore)
                 .ToListAsync()

            );

            RaisePropertyChanged(nameof(Inventories));
            RaisePropertyChanged(nameof(AvailableBooks));
            SaveChangesCommand.RaiseCanExecuteChanged();

        }

        private async Task LoadBooksAsync()
        {
            Books = new ObservableCollection<Book>(
                await _db.Books
                    .Include(b => b.Author)
                    .ToListAsync()
                    );

            RaisePropertyChanged(nameof(Books));
            RaisePropertyChanged(nameof(AvailableBooks));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }
    }
}
