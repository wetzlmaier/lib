using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Interface for viewmodels.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {

        /// <summary>
        /// The name to display.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The index for display.
        /// </summary>
        int DisplayIndex { get; }

        /// <summary>
        /// The <see cref="InputBinding"/> for <see cref="UIElement.InputBindings"/>.
        /// </summary>
        InputBindingCollection InputBindings { get; }

        /// <summary>
        /// <see cref="Thread"/> of the owner.
        /// </summary>
        Thread OwnerThread { get; }

        /// <summary>
        /// <see cref="Dispatcher"/> of the owner.
        /// </summary>
        Dispatcher OwnerDispatcher { get; }
    }
}
