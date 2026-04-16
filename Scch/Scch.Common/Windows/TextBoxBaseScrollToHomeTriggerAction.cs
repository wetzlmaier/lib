using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace Scch.Common.Windows
{
    public abstract class TextBoxBaseScrollToHomeTriggerAction<T> : TriggerAction<T> where T : TextBoxBase
    {
        protected override void Invoke(object parameter)
        {
            AssociatedObject.ScrollToHome();
        }
    }
}
