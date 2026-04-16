using System;
using System.Threading;

namespace Scch.Mvvm.Command
{
    public class AsyncRelayCommand : AsyncRelayCommandBase<object, object>
    {
        public AsyncRelayCommand(Action<CancellationToken> execute)
            : this(execute, () => true)
        {
        }

        public AsyncRelayCommand(Action<CancellationToken> execute, Func<bool> canExecute)
            : base((p, token) =>
            {
                execute(token);
                return null;
            }, o => canExecute())
        {
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public void Execute()
        {
            Execute(null);
        }
    }

    public class AsyncRelayCommand<TResult> : AsyncRelayCommandBase<object, TResult>
    {
        public AsyncRelayCommand(Func<CancellationToken, TResult> execute)
            : this(execute, () => true)
        {
        }

        public AsyncRelayCommand(Func<CancellationToken, TResult> execute, Func<bool> canExecute)
            : base((p, token) => execute(token), o => canExecute())
        {
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public void Execute()
        {
            Execute(null);
        }
    }

    public class AsyncRelayCommand<T, TResult> : AsyncRelayCommandBase<T, TResult>
    {
        public AsyncRelayCommand(Func<T, CancellationToken, TResult> execute)
            : this(execute, o => true)
        {
        }

        public AsyncRelayCommand(Func<T, CancellationToken, TResult> execute, Predicate<T> canExecute)
            : base(execute, canExecute)
        {
        }

        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        public void Execute(T parameter)
        {
            base.Execute(parameter);
        }
    }
}
