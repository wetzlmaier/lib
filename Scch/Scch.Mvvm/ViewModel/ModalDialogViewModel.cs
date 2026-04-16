using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Scch.Mvvm.ViewModel
{
    public class ModalDialogViewModel : ViewModelBase, IModalDialogViewModel
    {
        private string _title;
        private bool _isOpen;
        private ImageSource _icon;
        private bool? _dialogResult;
        private ICommandViewModel _okCommand;
        private ICommandViewModel _cancelCommand;
        private object _content;
        private ResizeMode _dialogResizeMode;

        public ModalDialogViewModel()
            : base("", 0)
        {
        }

        void ContentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IsValidPropertyName)
                UpdateValid();
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;

                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                /*if (value == _isOpen)
                    return;*/

                _isOpen = value;
                RaisePropertyChanged(() => IsOpen);
            }
        }

        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                if (value == _icon)
                    return;

                _icon = value;
                RaisePropertyChanged(() => Icon);
            }
        }

        public bool? DialogResult
        {
            get { return _dialogResult; }
            set
            {
                if (value == _dialogResult)
                    return;

                _dialogResult = value;
                RaisePropertyChanged(() => DialogResult);
            }
        }


        public ResizeMode DialogResizeMode
        {
            get
            {
                return _dialogResizeMode;
            }
            set
            {
                if (_dialogResizeMode == value)
                    return;

                _dialogResizeMode = value;
                RaisePropertyChanged(()=>DialogResizeMode);
            }
        }

        public ICommandViewModel OkCommand
        {
            get
            {
                return _okCommand;
            }
            set
            {
                if (_okCommand == value)
                    return;

                _okCommand = value;
                RaisePropertyChanged(() => OkCommand);
            }
        }

        public ICommandViewModel CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
            set
            {
                if (_cancelCommand == value)
                    return;

                _cancelCommand = value;
                RaisePropertyChanged(() => CancelCommand);
            }
        }

        public object Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (Content == value)
                    return;

                if (Content != null && Content is INotifyPropertyChanged)
                    ((INotifyPropertyChanged)_content).PropertyChanged -= ContentPropertyChanged;
                
                _content = value;

                if (Content != null && Content is INotifyPropertyChanged)
                    ((INotifyPropertyChanged)_content).PropertyChanged += ContentPropertyChanged;

                RaisePropertyChanged(()=>Content);
            }
        }

        public override string Error
        {
            get
            {
                if (Content != null && (Content is IDataErrorInfo))
                    return ((IDataErrorInfo)Content).Error;

                return base.Error;
            }
        }

        public override string this[string columnName]
        {
            get
            {
                if (Content != null && (Content is IDataErrorInfo))
                    return ((IDataErrorInfo)Content)[columnName];

                return base[columnName];
            }
        }
    }
}
