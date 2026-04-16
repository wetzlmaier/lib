namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Interface of an item viewmodel. An item is one item of a collection.
    /// </summary>
    /// <typeparam name="T">item type</typeparam>
    public interface IItemViewModel<T>
    {
        T Item { get; set; }
    }
}
