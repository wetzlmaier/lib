using System.Windows;

namespace Scch.Mvvm.View
{
    public interface IModalDialogView : IDialogView
    {
        Window Owner { get; }
    }
}
