using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Scch.Common;
using Scch.Common.Reflecton;
using Scch.Common.Windows;
using Scch.Mvvm.ViewModel;

namespace Scch.Controls.Mvvm.ViewModel
{
    public abstract class ConsoleViewModel : MainViewModelBase, IConsoleViewModel
    {
        protected const string FontName = "Console";
        private Task _consoleTask;
        private readonly CancellationTokenSource _cancellationTokenSource;

        protected ConsoleViewModel()
            : base(CreateTitle())
        {
            Output = new RichTextBoxViewModel();
            Output.Styles.Clear();

            var font = new Style();
            font.Setters.Add(new Setter(TextElement.FontFamilyProperty, new FontFamily("Arial")));

            var style = new Style { BasedOn = font };
            style.Setters.Add(new Setter(TextElement.FontFamilyProperty, new FontFamily("Courier New")));
            style.Setters.Add(new Setter(TextElement.FontWeightProperty, FontWeights.Normal));
            style.Setters.Add(new Setter(TextElement.FontSizeProperty, 12.0));
            var styleInfo = new StyleInfo(FontName, typeof(TextElement), style);
            Output.Styles.Add(styleInfo);

            Output.SelectedStyle = styleInfo;
            Output.Document = new FlowDocument { Background = Brushes.Black, Foreground = Brushes.LightGray };

            LoadedCommand = new RelayCommand(OnLoaded);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public virtual void OnLoaded()
        {
            _consoleTask = Task.Factory.StartNew(() =>
            {
                int exitCode = Run(_cancellationTokenSource);
                ShutdownAsync(exitCode);
            }, _cancellationTokenSource.Token);
        }

        public void ShutdownAsync(int exitCode)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    OwnerDispatcher.BeginInvoke(new Action(() => Shutdown(exitCode))).Wait(TimeSpan.FromSeconds(10));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }, CancellationToken.None);
        }

        protected virtual int Run(CancellationTokenSource cancellationTokenSource)
        {
            return ExitCodes.Ok;
        }

        protected override void OnClosed()
        {
            _cancellationTokenSource.Cancel();

            if (_consoleTask!=null)
                _consoleTask.Wait(TimeSpan.FromSeconds(10));

            base.OnClosed();
        }

        private static string CreateTitle()
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
                return string.Empty;

            return assembly.GetName().Name + " - " + AssemblyHelper.GetAssemblyCompany(assembly);
        }

        public RichTextBoxViewModel Output { get; private set; }

        public ICommand LoadedCommand { get; private set; }
    }
}
