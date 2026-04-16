using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Scch.Mvvm.ViewModel;

namespace Scch.Controls
{
    public class ToolBarItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ButtonTemplate { get; set; }
        public DataTemplate SeparatorTemplate { get; set; }
        public DataTemplate ToggleButtonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var toolBarItem = (ICommandViewModel)item;
            Debug.Assert(toolBarItem != null);

            if (toolBarItem.ImageSource != null)
            {
                return toolBarItem.IsActivatable ? ToggleButtonTemplate : ButtonTemplate;
            }

            return SeparatorTemplate;
        }
    }
}
