using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scch.Common.Windows.Input
{
    public class CommandBehaviorBase<T> where T : Control
    {
        // Fields
        private ICommand _command;
        private readonly EventHandler _commandCanExecuteChangedHandler;
        private object _commandParameter;
        private readonly WeakReference _targetObject;

        // Methods
        public CommandBehaviorBase(T targetObject)
        {
            _targetObject = new WeakReference(targetObject);
            _commandCanExecuteChangedHandler = CommandCanExecuteChanged;
        }

        private void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateEnabledState();
        }

        protected virtual void ExecuteCommand()
        {
            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }

        protected virtual void UpdateEnabledState()
        {
            if (TargetObject == null)
            {
                Command = null;
                CommandParameter = null;
            }
            else if (Command != null)
            {
                TargetObject.IsEnabled = Command.CanExecute(CommandParameter);
            }
        }

        // Properties
        public ICommand Command
        {
            get
            {
                return _command;
            }
            set
            {
                if (_command != null)
                {
                    _command.CanExecuteChanged -= _commandCanExecuteChangedHandler;
                }
                _command = value;
                if (_command != null)
                {
                    _command.CanExecuteChanged += _commandCanExecuteChangedHandler;
                    UpdateEnabledState();
                }
            }
        }

        public object CommandParameter
        {
            get
            {
                return _commandParameter;
            }
            set
            {
                if (_commandParameter != value)
                {
                    _commandParameter = value;
                    UpdateEnabledState();
                }
            }
        }

        protected T TargetObject
        {
            get
            {
                return (_targetObject.Target as T);
            }
        }
    }
}
