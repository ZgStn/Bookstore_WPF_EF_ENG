using System.Windows;
using Bookstore_WPF_EF_ENG.ViewModel;

namespace Bookstore_WPF_EF_ENG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();


        }
    }
}