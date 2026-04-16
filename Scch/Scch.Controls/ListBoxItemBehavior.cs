using System.Windows;
using System.Windows.Controls;

namespace Scch.Controls
{
    public static class ListBoxItemBehavior
    {
        public static bool GetBringIntoViewWhenSelected(ListBoxItem listBoxItem)
        {
            return (bool)listBoxItem.GetValue(BringIntoViewWhenSelectedProperty);
        }

        public static void SetBringIntoViewWhenSelected(ListBoxItem listBoxItem, bool value)
        {
            listBoxItem.SetValue(BringIntoViewWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty BringIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached("BringIntoViewWhenSelected", typeof(bool),
            typeof(ListBoxItemBehavior), new UIPropertyMetadata(false, OnBringIntoViewWhenSelectedChanged));

        static void OnBringIntoViewWhenSelectedChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var item = depObj as ListBoxItem;
            if (item == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                item.BringIntoView();
        }
    }
}
