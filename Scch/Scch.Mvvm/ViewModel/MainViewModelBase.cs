using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Scch.Common.Reflecton;
using Scch.Common.Windows.Media.Imaging;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Base class for the applications main view.
    /// </summary>
    public abstract class MainViewModelBase : ViewModelBase, IMainViewModel
    {
        protected MainViewModelBase() : this(CreateTitle())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModelBase"/> class.
        /// </summary>
        protected MainViewModelBase(string displayName) : base(displayName)
        {
            if (!IsInDesignMode)
                Application.Current.MainWindow.Closing += MainWindow_Closing;

            var icon = AssemblyHelper.LoadApplicationIcon();

            if (icon != null)
                Icon = BitmapSourceHelper.LoadBitmap(icon.ToBitmap());
        }

        public void Shutdown(int exitCode)
        {
            Application.Current.Shutdown(exitCode);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            OnClosing(e);
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            bool canClose = OnClosing();

            if (!canClose)
            {
                e.Cancel = true;
                return;
            }

            OnClosed();
        }

        private static string CreateTitle()
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
                return string.Empty;

            return AssemblyHelper.GetAssemblyProduct(assembly) + " - " + AssemblyHelper.GetAssemblyCompany(assembly);
        }

        /// <summary>
        /// The icon.
        /// </summary>
        public ImageSource Icon { get; private set; }

        public void CloseWindow()
        {
            Application.Current.MainWindow.Close();
        }
    }
}
