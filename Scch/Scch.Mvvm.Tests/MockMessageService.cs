using System;
using System.Windows;
using Scch.Mvvm.Service;

namespace Scch.Mvvm.Tests
{
    public class MockMessageService : IMessageService
    {
        public Window Owner { get; set; }
        public string Message { get; set; }
        public string Caption { get; set; }
        public MessageBoxResult Result { get; set; }
        public MessageBoxButton Button { get; set; }

        public void ShowMessage(Window owner, string message, string caption)
        {
            Owner = owner;
            Message = message;
            Caption = caption;
            Result = MessageBoxResult.None;
            Button = MessageBoxButton.OK;
        }

        public void ShowMessage(string message, string caption)
        {
            ShowMessage(null, message, caption);
        }

        public void ShowWarning(string message, string caption)
        {
            ShowWarning(null, message, caption);
        }

        public void ShowWarning(string message)
        {
            ShowWarning(message, null);
        }

        public void ShowError(string message, string caption)
        {
            ShowError(null, message, caption);
        }

        public void ShowError(string message)
        {
            ShowError(message, null);
        }

        public void ShowError(AggregateException exception, string caption)
        {
            ShowError(MessageService.FlattenErrorMessages(exception), caption);
        }

        public void ShowError(AggregateException exception)
        {
            ShowError(exception, null);
        }

        public MessageBoxResult ShowYesNoCancelQuestion(string message, string caption)
        {
            return ShowYesNoCancelQuestion(null, message, caption);
        }

        public MessageBoxResult ShowYesNoQuestion(string message, string caption)
        {
            return ShowYesNoQuestion(null, message, caption);
        }

        public MessageBoxResult ShowOkCancelQuestion(Window owner, string message, string caption)
        {
            Owner = owner;
            Message = message;
            Caption = caption;
            Button = MessageBoxButton.OKCancel;
            return Result;
        }

        public MessageBoxResult ShowOkCancelQuestion(string message, string caption)
        {
            return ShowOkCancelQuestion(null, message, caption);
        }

        public MessageBoxResult ShowYesNoQuestion(Window owner, string message, string caption)
        {
            Owner = owner;
            Message = message;
            Caption = caption;
            Button = MessageBoxButton.YesNoCancel;
            return Result;
        }

        public MessageBoxResult ShowYesNoCancelQuestion(Window owner, string message, string caption)
        {
            Owner = owner;
            Message = message;
            Caption = caption;
            Button = MessageBoxButton.YesNoCancel;
            return Result;
        }

        public void ShowError(Window owner, string message, string caption)
        {
            Owner = owner;
            Message = message;
            Caption = caption;
            Result = MessageBoxResult.None;
            Button = MessageBoxButton.OK;
        }

        public void ShowWarning(Window owner, string message, string caption)
        {
            Owner = owner;
            Message = message;
            Caption = caption;
            Result = MessageBoxResult.None;
            Button = MessageBoxButton.OK;
        }
    }
}
