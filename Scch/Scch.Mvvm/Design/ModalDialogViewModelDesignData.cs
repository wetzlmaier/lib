using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Scch.Common.Windows.Media.Imaging;
using Scch.Mvvm.ViewModel;
using Brushes = System.Windows.Media.Brushes;
using CommunityToolkit.Mvvm.Input;

namespace Scch.Mvvm.Design
{
    public class ModalDialogViewModelDesignData : ViewModelBase, IModalDialogViewModel
    {
        public ModalDialogViewModelDesignData() : base(null, 0)
        {
            Title = "ModalDialogViewModelDesignData";
            IsOpen = false;
            Icon = BitmapSourceHelper.LoadBitmap(new Bitmap(16, 16));
            OkCommand = new CommandViewModel("Ok", new RelayCommand(Dummy));
            CancelCommand = new CommandViewModel("Abbrechen", new RelayCommand(Dummy));
            Content = new TextBlock
            {
                Text = "Content",
                Height = 150,
                Width = 200,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = Brushes.Red
            };
            DialogResizeMode=ResizeMode.NoResize;
        }

        public string Title { get; set; }

        public bool IsOpen { get; set; }

        public ImageSource Icon { get; set; }

        public ICommandViewModel OkCommand { get; set; }

        public ICommandViewModel CancelCommand { get; set; }

        public object Content { get; set; }

        public bool? DialogResult { get; set; }

        public ResizeMode DialogResizeMode { get; set; }

        private void Dummy()
        {
        }
    }
}
