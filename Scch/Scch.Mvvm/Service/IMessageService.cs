using System;
using System.Windows;

namespace Scch.Mvvm.Service
{
    /// <summary>
    /// This service shows any kind of message dialog.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        void ShowMessage(Window owner, string message, string caption);

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        void ShowMessage(string message, string caption);

        /// <summary>
        /// Shows the message as warning.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        void ShowWarning(Window owner, string message, string caption);

        /// <summary>
        /// Shows the message as warning.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        void ShowWarning(string message, string caption);

        /// <summary>
        /// Shows the message as warning.
        /// </summary>
        /// <param name="message">The message.</param>
        void ShowWarning(string message);

        /// <summary>
        /// Shows the message as error.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        void ShowError(Window owner, string message, string caption);

        /// <summary>
        /// Shows the message as error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        void ShowError(string message, string caption);

        /// <summary>
        /// Shows the message as error.
        /// </summary>
        /// <param name="message">The message.</param>
        void ShowError(string message);

        /// <summary>
        /// Shows the error messages contained in the <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="exception">The <see cref="AggregateException"/></param>
        /// <param name="caption">The caption of the Message Window.</param>
        void ShowError(AggregateException exception, string caption);

        /// <summary>
        /// Shows the error messages contained in the <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="exception">The <see cref="AggregateException"/></param>
        void ShowError(AggregateException exception);

        /// <summary>
        /// Shows the specified yes/no/cancel question.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The question.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        MessageBoxResult ShowYesNoCancelQuestion(Window owner, string message, string caption);

        /// <summary>
        /// Shows the specified yes/no/cancel question.
        /// </summary>
        /// <param name="message">The question.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        MessageBoxResult ShowYesNoCancelQuestion(string message, string caption);

        /// <summary>
        /// Shows the specified yes/no question.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The question.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        MessageBoxResult ShowYesNoQuestion(Window owner, string message, string caption);

        /// <summary>
        /// Shows the specified yes/no question.
        /// </summary>
        /// <param name="message">The question.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        MessageBoxResult ShowYesNoQuestion(string message, string caption);

        /// <summary>
        /// Shows the specified ok/cancel question.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The question.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        MessageBoxResult ShowOkCancelQuestion(Window owner, string message, string caption);

        /// <summary>
        /// Shows the specified ok/cancel question.
        /// </summary>
        /// <param name="message">The question.</param>
        /// <param name="caption">The caption of the Message Window.</param>
        MessageBoxResult ShowOkCancelQuestion(string message, string caption);
    }
}
