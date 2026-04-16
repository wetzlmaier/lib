namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Viewmodel of one item of a collection.
    /// </summary>
    /// <typeparam name="T">item type</typeparam>
    public class ItemViewModel<T> : ViewModelBase, IItemViewModel<T>
    {
        private T _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemViewModel{T}"/> class.
        /// </summary>
        public ItemViewModel()
            : base(null)
        {
        }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public T Item
        {
            get { return _item; }
            set
            {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }
    }
}
