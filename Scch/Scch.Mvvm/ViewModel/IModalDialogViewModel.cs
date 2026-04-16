using System.Windows;
using System.Windows.Media;

namespace Scch.Mvvm.ViewModel
{
    public interface IModalDialogViewModel : IViewModel
    {
        string Title { get; set; }
        bool IsOpen { get; set; }
        ImageSource Icon { get; set; }
        ICommandViewModel OkCommand { get; set; }
        ICommandViewModel CancelCommand { get; set; }
        object Content { get; set; }
        bool IsValid { get; }
        bool? DialogResult { get; set; }
        ResizeMode DialogResizeMode { get; set; }
    }
}
