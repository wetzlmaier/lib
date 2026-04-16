using System.ComponentModel.Composition;
using Scch.Controls.Mvvm.View;
using Scch.Controls.Mvvm.ViewModel;
using Scch.Mvvm.Controller;
using Scch.Mvvm.View;

namespace Scch.Controls.Mvvm.Controller
{
    public abstract class ConsoleModuleController : IModuleController
    {
        private readonly IMainView _mainView;

        [ImportingConstructor]
        protected ConsoleModuleController(IConsoleView mainView, IConsoleViewModel mainViewModel)
        {
            _mainView = mainView;
            _mainView.DataContext = mainViewModel;
        }

        public void Initialize()
        {
        }

        public void Run()
        {
            _mainView.Show();
        }

        public void Shutdown()
        {
            _mainView.Close();
        }
    }
}
