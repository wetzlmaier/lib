using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Scch.Mvvm.ViewModel;

namespace Scch.Controls
{
    public class ToolBarWorkspaceTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ButtonTemplate { get; set; }
        public DataTemplate SeparatorTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var toolBarItem = (IWorkspaceViewModel)item;
            Debug.Assert(toolBarItem != null);

            if (toolBarItem.ImageSource != null)
            {
                return ButtonTemplate;
            }

            return SeparatorTemplate;
        }
    }
}
