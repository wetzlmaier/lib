using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Scch.Common.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Abstract base class for a ViewModel implementation.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public abstract class ViewModelBase : DataErrorInfo, IViewModel
    {
        private static bool? _isInDesignMode;
        private string _displayName;
        private int _displayIndex;
        private List<ViewModelBase> _childViewModels;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class. 
        /// </summary>
        /// <param name="displayName">The name to display.</param>
        protected ViewModelBase(string displayName)
            : this(displayName, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class. 
        /// </summary>
        /// <param name="displayName">The name to display.</param>
        /// <param name="displayIndex">The index for display.</param>
        protected ViewModelBase(string displayName, int displayIndex)
        {
            if (displayName == null)
                throw new ArgumentNullException("displayName");

            Initialize(displayName, displayIndex, null);
        }

        protected ViewModelBase(string displayName, int displayIndex, IMessenger messenger)
        {
            if (messenger == null)
                throw new ArgumentNullException("messenger");

            Initialize(displayName, displayIndex, messenger);
        }

        private void Initialize(string displayName, int displayIndex, IMessenger messenger)
        {
            _childViewModels = new List<ViewModelBase>();

            DisplayName = displayName;
            DisplayIndex = displayIndex;
            MessengerInstance = messenger;
            InputBindings = new InputBindingCollection();
            OwnerThread = Thread.CurrentThread;
        }

        public void PerformClose()
        {
            bool canClose = OnClosing();

            if (!canClose)
                return;

            OnClosed();
        }

        protected virtual void OnClosed()
        {
            foreach (var childViewModel in _childViewModels)
            {
                childViewModel.OnClosed();
            }
        }

        protected virtual bool OnClosing()
        {
            return _childViewModels.Aggregate(true, (current, childViewModel) => current & childViewModel.OnClosing());
        }

        /// <summary>
        /// Broadcasts a <see cref="PropertyChangedMessage{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        protected virtual void Broadcast<T>(T value)
        {
            var message = new ValueChangedMessage<T>(value);
            
            if (MessengerInstance != null)
            {
                MessengerInstance.Send(message);
            }
            else
            {
                WeakReferenceMessenger.Default.Send(message);
            }
        }

        /// <summary>
        /// Does cleanup work.
        /// </summary>
        public virtual void Dispose()
        {
            //Messenger.Default.Unregister(this);
        }

        /// <summary>
        /// Returns true, if the <see cref="IViewModel"/> is in design mode.
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

        /// <summary>
        /// Returns true, if the <see cref="IViewModel"/> is in design mode.
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
                    if (!_isInDesignMode.Value && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                    {
                        _isInDesignMode = true;
                    }
                }

                return _isInDesignMode.Value;
            }
        }

        /// <summary>
        /// <see cref="IMessenger"/>
        /// </summary>
        protected IMessenger MessengerInstance { get; private set; }

        /// <summary>
        /// <see cref="IViewModel.OwnerThread"/>
        /// </summary>
        public Thread OwnerThread { get; private set; }

        /// <summary>
        /// <see cref="IViewModel.OwnerDispatcher"/>
        /// </summary>
        public Dispatcher OwnerDispatcher
        {
            get { return Dispatcher.FromThread(OwnerThread); }
        }

        /// <summary>
        /// <see cref="IViewModel.DisplayName"/>
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName == value)
                    return;

                _displayName = value;
                RaisePropertyChanged(() => DisplayName);
            }
        }

        /// <summary>
        /// <see cref="IViewModel.DisplayIndex"/>
        /// </summary>
        public int DisplayIndex
        {
            get
            {
                return _displayIndex;
            }
            set
            {
                if (_displayIndex == value)
                    return;

                _displayIndex = value;
                RaisePropertyChanged(() => DisplayIndex);
            }
        }

        /// <summary>
        /// <see cref="IViewModel.InputBindings"/>
        /// </summary>
        public InputBindingCollection InputBindings { get; private set; }

        protected void RegisterChildViewModel(ViewModelBase viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            if (_childViewModels.Contains(viewModel))
                throw new ArgumentException();

            _childViewModels.Add(viewModel);
        }

        protected void UnregisterChildViewModel(ViewModelBase viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            if (!_childViewModels.Contains(viewModel))
                throw new ArgumentException();

            _childViewModels.Remove(viewModel);
        }

        /// <summary>
        /// Registers a collection of <see cref="InputBinding"/> from a <see cref="ICommandViewModel"/>.
        /// </summary>
        /// <param name="commandViewModels"></param>
        protected void RegisterCommands(IEnumerable<ICommandViewModel> commandViewModels)
        {
            foreach (var commandViewModel in from c in commandViewModels where c.Command != null && c.Shortcut != null select c)
                RegisterCommand(commandViewModel);
        }

        /// <summary>
        /// Registers a <see cref="InputBinding"/> from a <see cref="ICommandViewModel"/>.
        /// </summary>
        /// <param name="commandViewModel"></param>
        protected void RegisterCommand(ICommandViewModel commandViewModel)
        {
            if (commandViewModel == null)
                throw new ArgumentNullException("commandViewModel");

            RegisterCommand(commandViewModel.Command, commandViewModel.Shortcut);
        }

        /// <summary>
        /// Registers a <see cref="InputBinding"/> from a <see cref="ICommand"/> and a <see cref="KeyGesture"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="gesture"></param>
        protected void RegisterCommand(ICommand command, KeyGesture gesture)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (gesture == null)
                throw new ArgumentNullException("gesture");

            InputBindings.Add(new KeyBinding(command, gesture));
        }

        /// <summary>
        /// Clears the registerd <see cref="InputBinding"/>.
        /// </summary>
        protected void UnregisterCommands()
        {
            InputBindings.Clear();
        }
    }
}
