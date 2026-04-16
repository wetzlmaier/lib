using Scch.Mvvm.ViewModel;

namespace Scch.Mvvm.Tests
{
    public abstract class ViewModelUnitTestBase<T> where T : ViewModelBase
    {
        public T ViewModel { get; set; }

        protected ViewModelUnitTestBase()
        {
            ViewModel = default(T);
        }
    }
}
