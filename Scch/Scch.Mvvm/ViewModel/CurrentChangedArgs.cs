using System;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Event argument for the currentchanged eventhandler
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class CurrentChangedArgs<TItem, TViewModel> : EventArgs
    {
        //private string msg;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentChangedArgs{TItem, TViewModel}"/> class.
        /// </summary>
        /// <param name="currentItem">The current item.</param>
        /// <param name="currentViewModel">The current viewmodel.</param>
        public CurrentChangedArgs(TItem currentItem, TViewModel currentViewModel)
        {
            CurrentItem = currentItem;
            CurrentViewModel = currentViewModel;
        }

        /// <summary>
        /// Gets or sets the current item.
        /// </summary>
        /// <value>The current item.</value>
        public TItem CurrentItem { get; set; }

        /// <summary>
        /// Gets or sets the current viewmodel.
        /// </summary>
        /// <value>The current viewmodel.</value>
        public TViewModel CurrentViewModel { get; set; }
    }
}
