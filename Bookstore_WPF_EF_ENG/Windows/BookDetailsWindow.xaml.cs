using System.Windows;
using Bookstore.Domain;
using Bookstore.Presentation.ViewModel;

namespace Bookstore.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for BookDetailsWindow.xaml
    /// </summary>
    public partial class BookDetailsWindow : Window
    {
        public BookDetailsWindow(Inventory inventory)
        {
            InitializeComponent();
            DataContext = new BookDetailsViewModel(inventory);
        }
    }
}
