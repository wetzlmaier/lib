using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Scch.Controls
{
    public class TreeListViewItem : TreeViewItem
    {
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(GridViewColumnCollection), typeof(TreeListViewItem));
        
        private int _level = -1;

        /// <summary>
        /// Item's hierarchy in the tree
        /// </summary>
        public int Level
        {
            get
            {
                if (_level == -1)
                {
                    var parent = ItemsControlFromItemContainer(this) as TreeListViewItem;
                    _level = (parent != null) ? parent.Level + 1 : 0;
                }
                return _level;
            }
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GridViewColumnCollection Columns
        {
            get
            {
                return (GridViewColumnCollection)GetValue(ColumnsProperty);
            }
            internal set
            {
                SetValue(ColumnsProperty, value);
            }
        }
    }
}
