using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Scch.Common.Windows;
using Scch.Mvvm.ViewModel;

namespace Scch.Mvvm
{
    public class ModalDialogManager : Control
    {
        Window _window;
        bool _internalClose;
        bool _externalClose;
        public static readonly DependencyProperty CloseCommandProperty;
        public static readonly DependencyProperty IsOpenProperty;
        public static readonly DependencyProperty DialogHeightProperty;
        public static readonly DependencyProperty DialogWidthProperty;
        public static readonly DependencyProperty DialogResizeModeProperty;
        public static readonly DependencyProperty IconProperty;
        public static readonly DependencyProperty TitleProperty;
        public static readonly DependencyProperty WindowStartupLocationProperty;
        public static readonly DependencyProperty DialogResultProperty;

        static ModalDialogManager()
        {
            CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommandViewModel), typeof(ModalDialogManager), new UIPropertyMetadata(null));
            IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ModalDialogManager), new UIPropertyMetadata(false, IsOpenChanged));
            DialogHeightProperty = DependencyProperty.Register("DialogHeight", typeof(double), typeof(ModalDialogManager));
            DialogWidthProperty = DependencyProperty.Register("DialogWidth", typeof(double), typeof(ModalDialogManager));
            DialogResizeModeProperty = DependencyProperty.Register("DialogResizeMode", typeof(ResizeMode), typeof(ModalDialogManager));
            IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ModalDialogManager), new UIPropertyMetadata(null));
            TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ModalDialogManager), new UIPropertyMetadata(null));
            WindowStartupLocationProperty = DependencyProperty.Register("WindowStartupLocation", typeof(WindowStartupLocation), typeof(ModalDialogManager), new UIPropertyMetadata(WindowStartupLocation.CenterOwner));
            DialogResultProperty = DependencyProperty.Register("DialogResult", typeof(bool?), typeof(ModalDialogManager), new UIPropertyMetadata(null));
        }

        public ModalDialogManager()
        {
            Height = 0;
            Width = 0;
        }

        /// <summary>
        /// This is invoked when the red X is clicked or a keypress closes the window - 
        /// </summary>
        public ICommandViewModel CloseCommand
        {
            get { return (ICommandViewModel)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        /// <summary>
        /// This should be bound to IsOpen (or similar) in the ViewModel associated with ModalDialogManager
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var m = (ModalDialogManager)d;
            var newVal = (bool)e.NewValue;
            if (newVal)
                m.Show();
            else
                m.Close();
        }

        void Show()
        {
            if (_window != null)
                Close();

            _window = new Window();
            _window.MaxHeight = SystemParameters.FullPrimaryScreenHeight;
            _window.MaxWidth = SystemParameters.FullPrimaryScreenWidth;
            _window.Closing += Closing;
            _window.Owner = WindowHelper.GetActiveWindow();

            _window.DataContext = DataContext;
            _window.SetBinding(ContentControl.ContentProperty, "");

            _window.Title = Title;
            _window.Icon = Icon;
            _window.Height = DialogHeight;
            _window.Width = DialogWidth;
            _window.ResizeMode = DialogResizeMode;
            _window.SizeToContent = SizeToContent.WidthAndHeight;
            _window.WindowStartupLocation = WindowStartupLocation;
            _window.ShowInTaskbar = false;
            _window.ShowDialog();
        }

        void Closing(object sender, CancelEventArgs e)
        {
            if (!_internalClose)
            {
                _externalClose = true;
                if (CloseCommand != null && CloseCommand.Command.CanExecute(null))
                    CloseCommand.Command.Execute(null);
                IsOpen = false;
                _externalClose = false;
            }
        }

        void Close()
        {
            _internalClose = true;

            if (!_externalClose)
                _window.Close();

            _window = null;
            _internalClose = false;
        }

        public double DialogHeight
        {
            get { return (double)GetValue(DialogHeightProperty); }
            set { SetValue(DialogHeightProperty, value); }
        }

        public double DialogWidth
        {
            get { return (double)GetValue(DialogWidthProperty); }
            set { SetValue(DialogWidthProperty, value); }
        }

        public ResizeMode DialogResizeMode
        {
            get { return (ResizeMode)GetValue(DialogResizeModeProperty); }
            set { SetValue(DialogResizeModeProperty, value); }
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public WindowStartupLocation WindowStartupLocation
        {
            get { return (WindowStartupLocation)GetValue(WindowStartupLocationProperty); }
            set { SetValue(WindowStartupLocationProperty, value); }
        }

        public bool? DialogResult
        {
            get { return (bool?)GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }
    }
}
