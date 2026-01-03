using System.Collections.ObjectModel;
using Bookstore.Infrastructure.Data.Model;

namespace Bookstore_WPF_EF_ENG.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> Stores { get; private set; }

        private string _selectStore;

        public string SelectStore
        {
            get => _selectStore;

            set
            {
                _selectStore = value;
                RaisePropertyChanged();

            }
        }

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

            SelectStore = Stores.FirstOrDefault();
        }
    }
}
