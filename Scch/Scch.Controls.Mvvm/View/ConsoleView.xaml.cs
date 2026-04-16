using System.ComponentModel.Composition;
using System.Windows;

namespace Scch.Controls.Mvvm.View
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    [Export(typeof(IConsoleView))]
    public partial class ConsoleView : Window, IConsoleView
    {
        public ConsoleView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the <see cref="WindowState"/> to <see cref="WindowState.Maximized"/>
        /// </summary>
        public bool IsMaximized
        {
            get { return WindowState == WindowState.Maximized; }
            set
            {
                if (value)
                {
                    WindowState = WindowState.Maximized;
                }
                else if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }
    }
}
