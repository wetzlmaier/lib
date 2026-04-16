using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Scch.Mvvm.ViewModel;

namespace Scch.Controls.Mvvm.ViewModel
{
    /// <summary>
    /// Helper class that manages the <see cref="IModalDialogViewModel"/>.
    /// </summary>
    public class ModalReferenceEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModalReferenceEditor"/> class.
        /// </summary>
        /// <param name="content"></param>
        public ModalReferenceEditor(object content)
        {
            EditViewModel = new ModalDialogViewModel
            {
                Content = content,
                OkCommand = CommonCommands.CreateOkCommand(new RelayCommand(Ok, CanOk)),
                CancelCommand = CommonCommands.CreateCancelCommand(new RelayCommand(Cancel))
            };
        }

        /// <summary>
        /// <see cref="IModalDialogViewModel"/> for the modal dialog.
        /// </summary>
        public IModalDialogViewModel EditViewModel { get; private set; }

        /// <summary>
        /// Title for the modal dialog
        /// </summary>
        public ResizeMode DialogResizeMode
        {
            get { return EditViewModel.DialogResizeMode; }
            set { EditViewModel.DialogResizeMode = value; }
        }

        /// <summary>
        /// Title for the modal dialog
        /// </summary>
        public string Title
        {
            get { return EditViewModel.Title; }
            set { EditViewModel.Title = value; }
        }

        /// <summary>
        /// Icon for the model dialog.
        /// </summary>
        public ImageSource Icon
        {
            get { return EditViewModel.Icon; }
            set { EditViewModel.Icon = value; }
        }

        /// <summary>
        /// Called, when the Cancel command is executed.
        /// </summary>
        protected virtual void Cancel()
        {
            EditViewModel.IsOpen = false;
            EditViewModel.DialogResult = false;
        }

        /// <summary>
        /// Returns true, if the Ok command can be executed.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanOk()
        {
            return EditViewModel.IsValid;
        }

        /// <summary>
        /// Called, when the Ok command is executed.
        /// </summary>
        protected virtual void Ok()
        {
            // 
            EditViewModel.IsOpen = false;
            EditViewModel.DialogResult = true;
        }
    }

}
