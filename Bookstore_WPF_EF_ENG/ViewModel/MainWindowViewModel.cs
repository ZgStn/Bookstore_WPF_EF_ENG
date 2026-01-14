using Bookstore.Domain;
using Bookstore.Infrastructure.Service;
using Bookstore_WPF_EF_ENG.Command;
using System.Collections.ObjectModel;

namespace Bookstore_WPF_EF_ENG.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly BookstoreService _bookstoreService = new();
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
                RemoveBookCommand.RaiseCanExecuteChanged();

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
            }
        }

        public DelegateCommand RemoveBookCommand { get; set; }
        public MainWindowViewModel()
        {
            ShowBookDetailsCommand = new DelegateCommand(ExecuteShowBookDetails, CanShowBookDetails);
            AddBookCommand = new DelegateCommand(ExecuteAddBook, CanAddBook);
            RemoveBookCommand = new DelegateCommand(ExecuteRemoveBook, CanRemoveBook);
            SaveChangesCommand = new DelegateCommand(
                async _ => await SaveChangesAsync(),
                _ => _bookstoreService.HasChanges());

            _ = InitializeAsync();
        }

        private bool CanRemoveBook(object? arg)
        {
            return SelectedStore != null
                   && SelectedInventory != null;
        }

        private async void ExecuteRemoveBook(object? obj)
        {
            await RemoveBookAsync();
        }

        private async Task RemoveBookAsync()
        {

            _bookstoreService.RemoveInventory(SelectedInventory);
            Inventories.Remove(SelectedInventory);
            RaisePropertyChanged(nameof(AvailableBooks));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private async Task SaveChangesAsync()
        {
            await _bookstoreService.SaveChangesAsync();
            RaisePropertyChanged(nameof(AvailableBooks));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private bool CanAddBook(object? arg)
        {
            return SelectedStore != null
                   && AddedBook != null;
        }

        private async void ExecuteAddBook(object? obj)
        {
            await AddBookAsync();
        }

        private async Task AddBookAsync()
        {
            // TODO: protect against SelectedStore = null (make AddButton non-clickable
            // and/or here add if (SelectedStore = null) return
            var store = await _bookstoreService.GetStoreByNameAsync(SelectedStore);

            var newInventory = new Inventory()
            {
                Isbn13 = AddedBook!.Isbn13,
                Isbn13Navigation = AddedBook,
                Store = store
                //Quantity = NewQuantity
            };

            _bookstoreService.AddInventory(newInventory);
            Inventories.Add(newInventory);
            AddedBook = null;
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
        private void ExecuteShowBookDetails(object obj) => ShowBookDetails();

        //private void DoShowBookDetails(object obj) => ShowMessage?.Invoke("Button clicked!");//ShowBookDetails(obj);


        private bool CanShowBookDetails(object? arg) => SelectedInventory is not null;

        private async Task LoadStoresAsync()
        {
            Stores = new ObservableCollection<string>(
               await _bookstoreService.GetStoresAsync()

            );
            RaisePropertyChanged(nameof(Stores));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private async Task LoadInventoriesAsync()
        {
            Inventories = new ObservableCollection<Inventory>(
                 await _bookstoreService.GetInventoriesAsync(SelectedStore)

            );

            RaisePropertyChanged(nameof(Inventories));
            RaisePropertyChanged(nameof(AvailableBooks));
            SaveChangesCommand.RaiseCanExecuteChanged();

        }

        private async Task LoadBooksAsync()
        {
            Books = new ObservableCollection<Book>(
                await _bookstoreService.GetBooksWithAuthorsAsync());

            RaisePropertyChanged(nameof(Books));
            RaisePropertyChanged(nameof(AvailableBooks));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }
    }
}
