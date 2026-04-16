using System.Collections.ObjectModel;
using System.Linq;

namespace Scch.Mvvm.ViewModel
{
    public abstract class WorkspaceMainViewModel : MainViewModelBase, IMainViewModel
    {
        private ObservableCollection<IWorkspaceViewModel> _workspaces;
        private IWorkspaceViewModel _currentWorkspace;

        /// <summary>
        /// Gets or sets an array of <see cref="IWorkspaceViewModel"/>.
        /// </summary>
        public abstract IWorkspaceViewModel[] ViewModels { get; set; }

        public ObservableCollection<IWorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<IWorkspaceViewModel>();

                    foreach (var viewModel in from vm in ViewModels orderby vm.DisplayIndex select vm)
                    {
                        _workspaces.Add(viewModel);
                        RegisterChildViewModel((ViewModelBase)viewModel);
                    }

                    if (_workspaces.Count > 0)
                        CurrentWorkspace = _workspaces[0];
                }

                return _workspaces;
            }
        }

        public IWorkspaceViewModel CurrentWorkspace
        {
            get { return _currentWorkspace; }
            set
            {
                if (_currentWorkspace == value)
                    return;

                _currentWorkspace = value;
                RaisePropertyChanged(() => CurrentWorkspace);
            }
        }
    }
}
