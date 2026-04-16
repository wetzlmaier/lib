using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace Scch.Common.Windows
{
    public abstract class TextBoxBaseScrollToEndTriggerAction<T> : TriggerAction<T> where T : TextBoxBase
    {
        protected override void Invoke(object parameter)
        {
            AssociatedObject.ScrollToEnd();
        }
    }
}
