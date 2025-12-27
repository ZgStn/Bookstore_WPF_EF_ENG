using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bookstore_WPF_EF_ENG.ViewModel
{
    internal class ViewModelBase : INotifyPropertyChanged // alla som ärver , från  Interfaced, att få det färdig implementerad. 
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
