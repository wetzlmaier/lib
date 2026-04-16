using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Scch.Mvvm.Command
{
    public abstract class AsyncRelayCommandBase<T, TResult> : ICommand
    {
        private readonly Func<T, CancellationToken, TResult> _execute;
        private readonly Predicate<T> _canExecute;

        private Task<TResult> _task;
        private CancellationTokenSource _cancellationTokenSource;
        public event AsyncCompletedEventHandler ExecuteCompleted;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public AsyncRelayCommandBase(Func<T, CancellationToken, TResult> execute)
            : this(execute, null)
        {
        }

        public AsyncRelayCommandBase(Func<T, CancellationToken, TResult> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }
        
        protected bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return IsIdle && _canExecute((T)parameter);
            }

            return IsIdle;
        }

        private bool IsIdle
        {
            get { return _task == null; }
        }

        void NotifyCompleted()
        {
            var handler = ExecuteCompleted;
            if (handler != null)
            {
                handler(this, new AsyncCompletedEventArgs(_task.Exception, _task.IsCanceled, null));
            }

            _task = null;
            CommandManager.InvalidateRequerySuggested();
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }

        protected void Execute(object parameter)
        {
            if (!IsIdle)
                throw new InvalidOperationException("Already executing.");

            _cancellationTokenSource = new CancellationTokenSource();
            _task = Task<TResult>.Factory.StartNew(() => _execute((T)parameter, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            ((Task)_task).ContinueWith(task => NotifyCompleted(), TaskScheduler.FromCurrentSynchronizationContext());
            CommandManager.InvalidateRequerySuggested();
        }

        public void Cancel()
        {
            if (IsIdle)
                throw new InvalidOperationException("Not running.");

            _cancellationTokenSource.Cancel();
        }

        public TResult Result
        {
            get { return _task.Result; }
        }

        public bool IsCancellationRequested
        {
            get { return !IsIdle && _cancellationTokenSource.IsCancellationRequested; }
        }
    }
}
