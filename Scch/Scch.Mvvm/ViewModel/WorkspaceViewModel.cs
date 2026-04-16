using System.Drawing;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// This <see cref="ViewModelBase"/> subclass requests to be removed  from the UI when its CloseCommand executes. 
    /// </summary>
    public abstract class WorkspaceViewModel : ImageSourceViewModel, IWorkspaceViewModel
    {
        private ICommandViewModel _closeCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="displayIndex">The <see cref="ViewModelBase.DisplayIndex"/>.</param>
        protected WorkspaceViewModel(string displayName, int displayIndex)
            : base(displayName, displayIndex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="displayIndex">The <see cref="ViewModelBase.DisplayIndex"/>.</param>
        /// <param name="imageSource">The <see cref="ImageSourceViewModel.ImageSource"/>.</param>
        protected WorkspaceViewModel(string displayName, int displayIndex, ImageSource imageSource)
            : base(displayName, displayIndex, imageSource)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="displayIndex">The <see cref="ViewModelBase.DisplayIndex"/>.</param>
        /// <param name="bitmap">The <see cref="Bitmap"/> for the <see cref="ImageSourceViewModel.ImageSource"/>.</param>
        protected WorkspaceViewModel(string displayName, int displayIndex, Bitmap bitmap)
            : base(displayName, displayIndex, bitmap)
        {
        }

        /// <summary>
        /// <see cref="IWorkspaceViewModel.CloseCommand"/>
        /// </summary>
        public ICommandViewModel CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new CommandViewModel(Properties.Resources.WorkspaceViewModel_Close_DisplayName,
                    Properties.Resources.WorkspaceViewModel_Close_ToolTip, new RelayCommand(PerformClose)));
            }
        }

        /// <summary>
        /// <see cref="IWorkspaceViewModel.RefreshCommand"/>
        /// </summary>
        public ICommandViewModel RefreshCommand { get; protected set; }
    }
}
