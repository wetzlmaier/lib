using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Scch.Controls
{
    /// <summary>
    /// <see cref="ListView"/> whose content an be sorted by clicking the column header.
    /// </summary>
    public class SortableListView : ListView
    {
        /// <summary>
        /// Null surrogate value.
        /// </summary>
        public static SortDescription UnsetOrder;

        /// <summary>
        /// Identifies the <see cref="ColumnHeaderSorting"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderSortingProperty;

        /// <summary>
        /// Identifies the <see cref="ColumnHeaderSortingEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderSortingEnabledProperty;

        private SortDescription _lastOrder;
        private GridView _gridView;
        private readonly DataTemplate _headerTemplateArrowUp;
        private readonly DataTemplate _headerTemplateArrowDown;

        static SortableListView()
        {
            UnsetOrder = new SortDescription(string.Empty, ListSortDirection.Ascending);
            ColumnHeaderSortingProperty = DependencyProperty.Register("ColumnHeaderSorting", typeof(SortDescription), typeof(SortableListView),
                new PropertyMetadata(UnsetOrder, OnColumnHeaderSortingPropertyChanged, CoerceColumnHeaderSorting), ValidateColumnHeaderSorting);

            ColumnHeaderSortingEnabledProperty = DependencyProperty.Register("ColumnHeaderSortingEnabled", typeof(bool), typeof(SortableListView),
                new PropertyMetadata(false, OnColumnHeaderSortingEnabledPropertyChanged));

            ViewProperty.OverrideMetadata(typeof(SortableListView), new PropertyMetadata(OnViewChanged));
        }

        private static void OnColumnHeaderSortingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = (SortableListView)d;

            if ((bool)e.NewValue == false)
                list.UpdateOrder(UnsetOrder);
        }

        private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = (SortableListView)d;

            if (list._gridView != null)
                list._gridView.Columns.CollectionChanged -= list.ColumnsChanged;

            list._gridView = e.NewValue as GridView;

            if (list._gridView != null)
                list._gridView.Columns.CollectionChanged += list.ColumnsChanged;
        }

        private void ColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateOrder(ColumnHeaderSorting);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableListView"/> class. 
        /// </summary>
        public SortableListView()
        {
            _lastOrder = UnsetOrder;
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler), false);

            var arrows = new ResourceDictionary { Source = new Uri("/Scch.Controls;component/DataTemplates.xaml", UriKind.RelativeOrAbsolute) };
            if (arrows.Contains("HeaderTemplateArrowUp"))
                _headerTemplateArrowUp = arrows["HeaderTemplateArrowUp"] as DataTemplate;
            if (arrows.Contains("HeaderTemplateArrowDown"))
                _headerTemplateArrowDown = arrows["HeaderTemplateArrowDown"] as DataTemplate;
        }

        private static bool ValidateColumnHeaderSorting(object value)
        {
            return true;
        }

        private static object CoerceColumnHeaderSorting(DependencyObject d, object basevalue)
        {
            var list = (SortableListView)d;

            return list.ColumnHeaderSortingEnabled ? basevalue : UnsetOrder;
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            UpdateOrder(ColumnHeaderSorting);
        }

        private void UpdateOrder(SortDescription newValue)
        {
            Items.SortDescriptions.Clear();

            if (newValue != UnsetOrder)
            {
                Items.SortDescriptions.Add(newValue);
            }

            GridViewColumn column = FindColumn(_lastOrder.PropertyName);
            if (column != null)
                column.HeaderTemplate = null;

            column = FindColumn(newValue.PropertyName);
            if (column != null)
                column.HeaderTemplate = newValue.Direction == ListSortDirection.Ascending ? _headerTemplateArrowUp : _headerTemplateArrowDown;

            Items.Refresh();

            _lastOrder = newValue;
        }

        private static void OnColumnHeaderSortingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = (SortableListView)d;
            list.UpdateOrder((SortDescription)e.NewValue);
        }

        private GridViewColumn FindColumn(string header)
        {
            if (_gridView == null)
                return null;

            return (from c in _gridView.Columns where header.Equals(c.Header) select c).SingleOrDefault();
        }

        /// <summary>
        /// The <see cref="SortDescription"/> generated by clicking a column header.
        /// </summary>
        public SortDescription ColumnHeaderSorting
        {
            get
            {
                return (SortDescription)GetValue(ColumnHeaderSortingProperty);
            }
            set
            {
                SetValue(ColumnHeaderSortingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the ability to sort the content by clicking a column header.
        /// </summary>
        public bool ColumnHeaderSortingEnabled
        {
            get
            {
                return (bool)GetValue(ColumnHeaderSortingEnabledProperty);
            }
            set
            {
                SetValue(ColumnHeaderSortingEnabledProperty, value);
            }
        }

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding && ColumnHeaderSortingEnabled)
            {
                ListSortDirection direction;
                if ((string)headerClicked.Column.Header != _lastOrder.PropertyName)
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    direction = _lastOrder.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }

                var header = headerClicked.Column.Header as string;
                ColumnHeaderSorting = new SortDescription(header, direction);
            }
        }
    }
}
