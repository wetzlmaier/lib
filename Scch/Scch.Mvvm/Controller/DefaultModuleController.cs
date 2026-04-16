using System.ComponentModel;
using Scch.Mvvm.Service;
using Scch.Mvvm.View;
using Scch.Mvvm.ViewModel;

namespace Scch.Mvvm.Controller
{
    public abstract class DefaultModuleController : IModuleController
    {
        private readonly IMessageService _messageService;
        private readonly IMainView _mainView;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultModuleController"/> class.
        /// </summary>
        /// <param name="messageService">The <see cref="IMessageService"/>.</param>
        /// <param name="mainView">The <see cref="IMainView"/>.</param>
        /// <param name="mainViewModel">The <see cref="IMainViewModel"/>.</param>
        protected DefaultModuleController(IMessageService messageService, IMainView mainView, IMainViewModel mainViewModel)
        {
            mainView.DataContext = mainViewModel;
            _mainView = mainView;
            _messageService = messageService;
        }

        /// <summary>
        /// <see cref="IModuleController.Initialize"/>
        /// </summary>
        public virtual void Initialize()
        {
            
        }

        /// <summary>
        /// <see cref="IModuleController.Run"/>
        /// </summary>
        public void Run()
        {
            _mainView.Show();
        }

        /// <summary>
        /// <see cref="IModuleController.Shutdown"/>
        /// </summary>
        public virtual void Shutdown()
        {

        }

        private void ShellViewModelClosing(object sender, CancelEventArgs e)
        {

        }

        private void Close()
        {
            _mainView.Close();
        }
    }
}
