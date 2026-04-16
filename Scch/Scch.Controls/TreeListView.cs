using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Scch.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeListViewItem))]
    [StyleTypedProperty(Property="ColumnHeaderContainerStyle", StyleTargetType=typeof(GridViewColumnHeader))]
    public class TreeListView : TreeView
    {
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(GridViewColumnCollection), typeof(TreeListView));
        public static readonly DependencyProperty AllowsColumnReorderProperty = DependencyProperty.Register("AllowsColumnReorder", typeof(bool), typeof(TreeListView), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = DependencyProperty.Register("ColumnHeaderContainerStyle", typeof(Style), typeof(TreeListView));
        public static readonly DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(TreeListView));
        public static readonly DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(TreeListView));
        public static readonly DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register("ColumnHeaderTemplate", typeof(DataTemplate), typeof(TreeListView), new FrameworkPropertyMetadata(OnColumnHeaderTemplateChanged));

        private static void OnColumnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeListView view = (TreeListView)d;
            //Helper.CheckTemplateAndTemplateSelector("TreeListViewColumnHeader", ColumnHeaderTemplateProperty, ColumnHeaderTemplateSelectorProperty, view);
        }

        public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(TreeListView), new FrameworkPropertyMetadata(OnColumnHeaderTemplateSelectorChanged));

        private static void OnColumnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeListView view = (TreeListView)d;
            //Helper.CheckTemplateAndTemplateSelector("TreeListViewColumnHeader", ColumnHeaderTemplateProperty, ColumnHeaderTemplateSelectorProperty, view);
        }

        public static readonly DependencyProperty ColumnHeaderToolTipProperty = DependencyProperty.Register("ColumnHeaderToolTip", typeof(object), typeof(TreeListView));

        public TreeListView()
        {
            Columns = new GridViewColumnCollection();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var container = new TreeListViewItem {Columns = Columns};
            return container;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        public bool AllowsColumnReorder
        {
            get
            {
                return (bool)GetValue(AllowsColumnReorderProperty);
            }
            set
            {
                SetValue(AllowsColumnReorderProperty, value);
            }
        }

        public Style ColumnHeaderContainerStyle
        {
            get
            {
                return (Style)GetValue(ColumnHeaderContainerStyleProperty);
            }
            set
            {
                SetValue(ColumnHeaderContainerStyleProperty, value);
            }
        }

        public ContextMenu ColumnHeaderContextMenu
        {
            get
            {
                return (ContextMenu)GetValue(ColumnHeaderContextMenuProperty);
            }
            set
            {
                SetValue(ColumnHeaderContextMenuProperty, value);
            }
        }

        public string ColumnHeaderStringFormat
        {
            get
            {
                return (string)GetValue(ColumnHeaderStringFormatProperty);
            }
            set
            {
                SetValue(ColumnHeaderStringFormatProperty, value);
            }
        }

        public DataTemplate ColumnHeaderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ColumnHeaderTemplateProperty);
            }
            set
            {
                SetValue(ColumnHeaderTemplateProperty, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTemplateSelector ColumnHeaderTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ColumnHeaderTemplateSelectorProperty);
            }
            set
            {
                SetValue(ColumnHeaderTemplateSelectorProperty, value);
            }
        }

        public object ColumnHeaderToolTip
        {
            get
            {
                return GetValue(ColumnHeaderToolTipProperty);
            }
            set
            {
                SetValue(ColumnHeaderToolTipProperty, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GridViewColumnCollection Columns
        {
            get
            {
                return (GridViewColumnCollection)GetValue(ColumnsProperty);
            }
            set
            {
                SetValue(ColumnsProperty, value);
            }
        }
    }
}
