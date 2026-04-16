using System;
#if (DEBUG != true)
using System.Diagnostics;
using System.Globalization;
using System.Windows.Threading;
using Scch.Common.Reflecton;
#endif

using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Scch.Common;
using Scch.Common.ComponentModel.Composition;
using Scch.Common.Diagnostics;
using Scch.Common.Windows.Input;
using Scch.Logging;
using Scch.Mvvm.Controller;

namespace Scch.Mvvm
{
    public class MvvmApplication : Application
    {
        protected CompositionContainer Container { get; private set; }
        protected IModuleController[] _moduleControllers;
        private readonly bool _requestSingleInstance;
        private readonly SearchOption _searchOption;

        protected string[] ModuleAssemblies { get; private set; }
        protected string[] IgnoreAssemblies { get; private set; }

        public MvvmApplication(SearchOption searchOption = SearchOption.TopDirectoryOnly)
            : this(true, new string[0], searchOption)
        {

        }

        public MvvmApplication(bool requestSingleInstance, string[] ignoreAssemblies, SearchOption searchOption = SearchOption.TopDirectoryOnly)
            : this(requestSingleInstance, new string[0], ignoreAssemblies, searchOption)
        {

        }

        public MvvmApplication(bool requestSingleInstance, string[] moduleAssemblies, string[] ignoreAssemblies, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            _requestSingleInstance = requestSingleInstance;
            ModuleAssemblies = moduleAssemblies;
            IgnoreAssemblies = ignoreAssemblies;
            _searchOption = searchOption;

            KeyboardHooks = new KeyboardHook();
        }

        public static KeyboardHook KeyboardHooks { get; private set; }

        /// <summary>
        /// <see cref="Application.OnStartup"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!ProcessHelper.IsSingleInstance() && _requestSingleInstance)
            {
                Current.Shutdown(ExitCodes.Error);
                return;
            }

#if (DEBUG != true)
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif
            Container = CompositionHelper.CreateCompositionContainer(ModuleAssemblies.ToArray(), IgnoreAssemblies.ToArray(), _searchOption);

            try
            {
                _moduleControllers = Container.GetExportedValues<IModuleController>().ToArray();
            }
            catch (ReflectionTypeLoadException refEx)
            {
                foreach (var ex in refEx.LoaderExceptions)
                    Logger.Write(new ExceptionLogEntry(ex));

                Current.Shutdown(ExitCodes.Error);
                return;
            }

            foreach (IModuleController moduleController in _moduleControllers)
            {
                moduleController.Initialize();
            }

            foreach (IModuleController moduleController in _moduleControllers)
            {
                moduleController.Run();
            }
        }

        /// <summary>
        /// <see cref="Application.OnExit"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            if (ProcessHelper.IsSingleInstance() || !_requestSingleInstance)
            {
                foreach (IModuleController moduleController in _moduleControllers.Reverse())
                {
                    moduleController.Shutdown();
                }
            }

            if (Container != null)
                Container.Dispose();

            KeyboardHooks.Dispose();
            base.OnExit(e);
        }

#if (DEBUG != true)
        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
            e.Handled = true;
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private static void HandleException(Exception e, bool isTerminating)
        {
            if (e == null) { return; }

            Trace.TraceError(e.ToString());

            if (!isTerminating)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Mvvm.Properties.Resources.UnhandledException_UnknownError, e)
                    , AssemblyHelper.GetAssemblyProduct(Assembly.GetEntryAssembly()), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
#endif
    }
}
