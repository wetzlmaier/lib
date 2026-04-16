using System.Windows.Input;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// interface that has the position and number of items in a collection 
    /// and the commands that can be used on a collection
    /// </summary>
    public interface INavigationViewModel
    {
        ICommand AddNewCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand NextCommand { get; }
        ICommand PreviousCommand { get; }
        ICommand FirstCommand { get; }
        ICommand LastCommand { get; }
        int Position { get; set; }
        int Count { get; }
    }
}
