using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Scch.Common.Windows.Input
{
    public static class InputBindings
    {
        public static readonly DependencyProperty BindingsSourceProperty = DependencyProperty.RegisterAttached("BindingsSource", typeof(InputBindingCollection),
                                                typeof(InputBindings), new FrameworkPropertyMetadata(new InputBindingCollection(), OnBindingsSourceChanged));

        private static void OnBindingsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as UIElement;

            if (element == null)
                throw new ArgumentException();

            if (!(e.NewValue is IEnumerable<InputBinding>) && !(e.NewValue is InputBindingCollection))
                throw new ArgumentException();

            element.InputBindings.Clear();
            foreach (InputBinding inputBinding in (IEnumerable)e.NewValue)
                element.InputBindings.Add(inputBinding);
        }

        public static InputBindingCollection GetBindingsSource(UIElement element)
        {
            return (InputBindingCollection)element.GetValue(BindingsSourceProperty);
        }

        public static void SetBindingsSource(UIElement element, InputBindingCollection inputBindings)
        {
            element.SetValue(BindingsSourceProperty, inputBindings);
        }
    }
}
