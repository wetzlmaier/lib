using System;
using System.ComponentModel;

namespace Scch.Mvvm.View
{
    public interface IDialogView: IView
    {
        bool? DialogResult { get; set; }

        event CancelEventHandler Closing;

        event EventHandler Closed;

        void Show();

        void Close();
    }
}
