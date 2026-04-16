using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using Scch.Common.Windows;
using Scch.Mvvm.Properties;

namespace Scch.Mvvm.Service
{
    /// <summary>
    /// This is the default implementation of the <see cref="IMessageService"/> interface. It shows messages via the MessageBox 
    /// to the user.
    /// </summary>
    /// <remarks>
    /// If the default implementation of this service doesn't serve your need then you can provide your own implementation.
    /// </remarks>
    [Export(typeof(IMessageService))]
    public class MessageService : IMessageService
    {
        /// <summary>
        /// <see cref="IMessageService.ShowMessage(Window,string,string)"/>
        /// </summary>
        /// <param name="owner"> </param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public void ShowMessage(Window owner, string message, string caption)
        {
            ShowCore(owner, message, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowMessage(string,string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public void ShowMessage(string message, string caption)
        {
            ShowMessage(WindowHelper.GetActiveWindow(), message, caption);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowWarning(Window,string,string)"/>
        /// </summary>
        /// <param name="owner"> </param>
        /// <param name="message"></param>
        /// <param name="caption"> </param>
        public void ShowWarning(Window owner, string message, string caption)
        {
            ShowCore(owner, message, caption, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowWarning(string,string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"> </param>
        public void ShowWarning(string message, string caption)
        {
            ShowWarning(WindowHelper.GetActiveWindow(), message, caption);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowWarning(Window,string,string)"/>
        /// </summary>
        /// <param name="message"></param>
        public void ShowWarning(string message)
        {
            ShowWarning(message, Resources.MessageService_Warning);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowError(Window,string,string)"/>
        /// </summary>
        /// <param name="owner"> </param>
        /// <param name="message"></param>
        /// <param name="caption"> </param>
        public void ShowError(Window owner, string message, string caption)
        {
            ShowCore(owner, message, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowError(string,string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"> </param>
        public void ShowError(string message, string caption)
        {
           ShowError(WindowHelper.GetActiveWindow(), message, caption);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowError(string)"/>
        /// </summary>
        /// <param name="message"></param>
        public void ShowError(string message)
        {
            ShowError(message, Resources.MessageService_Error);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowError(AggregateException)"/>
        /// </summary>
        /// <param name="exception"></param>
        public void ShowError(AggregateException exception)
        {
            ShowError(exception, Resources.MessageService_Error);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowError(AggregateException, string)"/>
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="caption"></param>
        public void ShowError(AggregateException exception, string caption)
        {
            ShowError(FlattenErrorMessages(exception), caption);
        }

        internal static string FlattenErrorMessages(AggregateException exception)
        {
            var uniqueMessages = new List<string>();
            foreach (var ex in exception.Flatten().InnerExceptions)
            {
                if (uniqueMessages.Contains(ex.Message))
                    continue;

                uniqueMessages.Add(ex.Message);
            }

            if (uniqueMessages.Count > 1)
            {
                var sb = new StringBuilder();
                sb.AppendLine(Resources.MessageService_AggregatedErrors);
                sb.AppendLine();

                foreach (var message in uniqueMessages)
                {
                    sb.AppendLine(message);
                }

                return sb.ToString();
            }

            return uniqueMessages[0];
        }

        /// <summary>
        /// <see cref="IMessageService.ShowYesNoCancelQuestion(Window,string,string)"/>
        /// </summary>
        /// <param name="owner"> </param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public MessageBoxResult ShowYesNoCancelQuestion(Window owner, string message, string caption)
        {
            return ShowCore(owner, message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowYesNoCancelQuestion(Window,string,string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public MessageBoxResult ShowYesNoCancelQuestion(string message, string caption)
        {
            return ShowYesNoCancelQuestion(WindowHelper.GetActiveWindow(), message, caption);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowYesNoQuestion(Window,string,string)"/>
        /// </summary>
        /// <param name="owner"> </param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public MessageBoxResult ShowYesNoQuestion(Window owner, string message, string caption)
        {
            return ShowCore(owner, message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowYesNoQuestion(string,string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public MessageBoxResult ShowYesNoQuestion(string message, string caption)
        {
            return ShowYesNoQuestion(WindowHelper.GetActiveWindow(), message, caption);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowOkCancelQuestion(Window,string,string)"/>
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public MessageBoxResult ShowOkCancelQuestion(Window owner, string message, string caption)
        {
            return ShowCore(owner, message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
        }

        /// <summary>
        /// <see cref="IMessageService.ShowOkCancelQuestion(string,string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public MessageBoxResult ShowOkCancelQuestion(string message, string caption)
        {
            return ShowOkCancelQuestion(WindowHelper.GetActiveWindow(), message, caption);
        }

        private MessageBoxResult ShowCore(Window owner, string message, string caption, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult)
        {
            if (owner == null)
                return MessageBox.Show(message, caption, button, image, defaultResult);

            return MessageBox.Show(owner, message, caption, button, image, defaultResult);
        }
    }
}
