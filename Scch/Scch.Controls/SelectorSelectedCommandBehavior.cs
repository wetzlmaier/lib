using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Scch.Common.Windows.Input;

namespace Scch.Controls
{
    public class SelectorSelectedCommandBehavior : CommandBehaviorBase<Selector>
    {
        public SelectorSelectedCommandBehavior(Selector selectableObject)
            : base(selectableObject)
        {
            selectableObject.SelectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandParameter = TargetObject.SelectedItem;
            ExecuteCommand();
        }
    }
}
