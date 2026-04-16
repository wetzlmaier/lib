using System.Collections;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Scch.Common.Collections;

namespace Scch.Controls
{
    /// <summary>
    /// Interaction logic for ListBoxSorter.xaml
    /// </summary>
    public partial class ListBoxSorter : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="Target"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetProperty;

        static ListBoxSorter()
        {
            TargetProperty = DependencyProperty.Register("Target", typeof(ListBox), typeof(ListBoxSorter), new FrameworkPropertyMetadata(null, OnTargetChanged));
        }

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorter = (ListBoxSorter)d;
            sorter.Target = (ListBox)e.NewValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxSorter"/> class. 
        /// </summary>
        public ListBoxSorter()
        {
            InitializeComponent();

            btnMoveUp.Command = new RelayCommand(MoveUp, CanMoveUp);
            btnMoveDown.Command = new RelayCommand(MoveDown, CanMoveDown);
        }

        private bool CanMoveDown()
        {
            return Target != null && Target.SelectedIndex >= 0 && Target.SelectedIndex < Target.Items.Count - 1;
        }

        /// <summary>
        /// Increases the index of the selected item by 1.
        /// </summary>
        public void MoveDown()
        {
            if (!CanMoveDown())
                return;

            var newIndex = Target.SelectedIndex + 1;
            var items = Target.ItemsSource != null ? (IList)Target.ItemsSource : Target.Items;
            items.Move(Target.SelectedIndex, newIndex);
            Target.SelectedIndex = newIndex;
        }

        private bool CanMoveUp()
        {
            return Target != null && Target.SelectedIndex > 0;
        }

        /// <summary>
        /// Decreases the index of the selected item by 1.
        /// </summary>
        public void MoveUp()
        {
            if (!CanMoveUp())
                return;

            var newIndex = Target.SelectedIndex - 1;
            var items = Target.ItemsSource != null ? (IList)Target.ItemsSource : Target.Items;
            items.Move(Target.SelectedIndex, newIndex);
            Target.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Gets or sets the target <see cref="ListBox"/>.
        /// </summary>
        public ListBox Target
        {
            get { return (ListBox)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
    }
}
