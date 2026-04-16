using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Scch.Controls
{
    /// <summary>
    /// Interaction logic for CollectionEditor.xaml
    /// </summary>
    public partial class CollectionEditor : UserControl
    {
        #region DependencyProperties
        /// <summary>
        /// Identifies the <see cref="AvailableItemsSelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AvailableItemsSelectedItemProperty;
        /// <summary>
        /// Identifies the <see cref="SelectedItemsSelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsSelectedItemProperty;
        /// <summary>
        /// Identifies the <see cref="AlternationCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlternationCountProperty;
        /// <summary>
        /// Identifies the <see cref="DisplayMemberPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayMemberPathProperty;
        /// <summary>
        /// Identifies the <see cref="GroupStyleSelector"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupStyleSelectorProperty;
        /// <summary>
        /// Identifies the <see cref="ItemBindingGroup"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemBindingGroupProperty;
        /// <summary>
        /// Identifies the <see cref="ItemContainerStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleProperty;
        /// <summary>
        /// Identifies the <see cref="ItemContainerStyleSelector"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleSelectorProperty;
        /// <summary>
        /// Identifies the <see cref="ItemsPanel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsPanelProperty;
        /// <summary>
        /// Identifies the <see cref="ItemStringFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemStringFormatProperty;
        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty;
        /// <summary>
        /// Identifies the <see cref="ItemTemplateSelector"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateSelectorProperty;
        /// <summary>
        /// Identifies the <see cref="AvailableItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AvailableItemsSourceProperty;
        /// <summary>
        /// Identifies the <see cref="SelectedItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsSourceProperty;
        /// <summary>
        /// Identifies the <see cref="AvailableItemsSelectionMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AvailableItemsSelectionModeProperty;
        /// <summary>
        /// Identifies the <see cref="SelectedItemsSelectionMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsSelectionModeProperty;
        /// <summary>
        /// Identifies the <see cref="ColumnsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnsSourceProperty;
        /// <summary>
        /// Identifies the <see cref="ColumnHeaderSortingEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderSortingEnabledProperty;
        /// <summary>
        /// Identifies the <see cref="ColumnHeaderSorting"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderSortingProperty;
        /// <summary>
        /// Identifies the <see cref="ManualSortingEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ManualSortingEnabledProperty;
        /// <summary>
        /// Identifies the <see cref="AvailableItemsHeader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AvailableItemsHeaderProperty;
        /// <summary>
        /// Identifies the <see cref="SelectedItemsHeader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsHeaderProperty;

        static CollectionEditor()
        {
            AvailableItemsSelectedItemProperty = DependencyProperty.Register("AvailableItemsSelectedItem", typeof(object), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnAvailableItemsSelectedItemChanged));
            SelectedItemsSelectedItemProperty = DependencyProperty.Register("SelectedItemsSelectedItem", typeof(object), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnSelectedItemsSelectedItemChanged)); 

            AvailableItemsSourceProperty = DependencyProperty.Register("AvailableItemsSource", typeof(IEnumerable), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnAvailableItemsSourceChanged), IsValidAvailableItemsSource);
            SelectedItemsSourceProperty = DependencyProperty.Register("SelectedItemsSource", typeof(IEnumerable), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnSelectedItemsSourceChanged), IsValidSelectedItemsSource);

            DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(CollectionEditor), new FrameworkPropertyMetadata(string.Empty, OnDisplayMemberPathChanged));
            ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnItemTemplateChanged));
            ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnItemTemplateSelectorChanged));
            ItemStringFormatProperty = DependencyProperty.Register("ItemStringFormat", typeof(string), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnItemStringFormatChanged));
            ItemBindingGroupProperty = DependencyProperty.Register("ItemBindingGroup", typeof(BindingGroup), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnItemBindingGroupChanged));
            ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnItemContainerStyleChanged));
            ItemContainerStyleSelectorProperty = DependencyProperty.Register("ItemContainerStyleSelector", typeof(StyleSelector), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnItemContainerStyleSelectorChanged));
            var defaultValue = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
            defaultValue.Seal();
            ItemsPanelProperty = DependencyProperty.Register("ItemsPanel", typeof(ItemsPanelTemplate), typeof(CollectionEditor), new FrameworkPropertyMetadata(defaultValue, OnItemsPanelChanged));
            GroupStyleSelectorProperty = DependencyProperty.Register("GroupStyleSelector", typeof(GroupStyleSelector), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnGroupStyleSelectorChanged));
            AlternationCountProperty = DependencyProperty.Register("AlternationCount", typeof(int), typeof(CollectionEditor), new FrameworkPropertyMetadata(0, OnAlternationCountChanged));
            AvailableItemsSelectionModeProperty = DependencyProperty.Register("AvailableItemsSelectionMode", typeof(SelectionMode), typeof(CollectionEditor), new FrameworkPropertyMetadata(SelectionMode.Single, OnAvailableItemsSelectionModeChanged), IsValidSelectionMode);
            SelectedItemsSelectionModeProperty = DependencyProperty.Register("SelectedItemsSelectionMode", typeof(SelectionMode), typeof(CollectionEditor), new FrameworkPropertyMetadata(SelectionMode.Single, OnSelectedItemsSelectionModeChanged), IsValidSelectionMode);
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionEditor), new FrameworkPropertyMetadata(typeof(CollectionEditor)));
            ColumnsSourceProperty = DependencyProperty.Register("ColumnsSource", typeof(IEnumerable), typeof(CollectionEditor), new FrameworkPropertyMetadata(null, OnColumnsSourceChanged));

            ColumnHeaderSortingEnabledProperty = SortableListView.ColumnHeaderSortingEnabledProperty.AddOwner(typeof(CollectionEditor));
            ColumnHeaderSortingEnabledProperty.OverrideMetadata(typeof(CollectionEditor), new PropertyMetadata(false, OnColumnHeaderSortingEnabledChanged));
            ColumnHeaderSortingProperty = SortableListView.ColumnHeaderSortingProperty.AddOwner(typeof(CollectionEditor));
            ColumnHeaderSortingProperty.OverrideMetadata(typeof(CollectionEditor), new PropertyMetadata(SortableListView.UnsetOrder, OnColumnHeaderSortingChanged));

            ManualSortingEnabledProperty = DependencyProperty.Register("ManualSortingEnabled", typeof(bool), typeof(CollectionEditor), new FrameworkPropertyMetadata(false, OnManualSortingEnabledChanged));

            AvailableItemsHeaderProperty = DependencyProperty.Register("AvailableItemsHeader", typeof(string), typeof(CollectionEditor), new FrameworkPropertyMetadata(string.Empty, OnAvailableItemsHeaderChanged));
            SelectedItemsHeaderProperty = DependencyProperty.Register("SelectedItemsHeader", typeof(string), typeof(CollectionEditor), new FrameworkPropertyMetadata(string.Empty, OnSelectedItemsHeaderChanged));
        }

        private static void OnAvailableItemsSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.SelectedItem = e.NewValue;
        }

        private static void OnSelectedItemsSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvSelectedItems.SelectedItem = e.NewValue;
        }

        private static void OnAvailableItemsHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lblAvailableItemsHeader.Content = e.NewValue;
        }

        private static void OnSelectedItemsHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lblSelectedItemsHeader.Content = e.NewValue;
        }

        private static void OnColumnHeaderSortingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.UpdateSorting();
        }

        private static void OnColumnHeaderSortingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.UpdateSorting();
        }

        private void UpdateSorting()
        {
            lvAvailableItems.ColumnHeaderSortingEnabled = ColumnHeaderSortingEnabled;
            lvSelectedItems.ColumnHeaderSortingEnabled = ColumnHeaderSortingEnabled && !ManualSortingEnabled;
            lvAvailableItems.ColumnHeaderSorting = ColumnHeaderSorting;
            if (!ManualSortingEnabled)
                lvSelectedItems.ColumnHeaderSorting = ColumnHeaderSorting;
            sorter.Visibility = ManualSortingEnabled ? Visibility.Visible : Visibility.Hidden;
        }

        private static void OnManualSortingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.UpdateSorting();
        }

        #endregion DependencyProperties

        private readonly ItemsControl _availableItems;
        private readonly ItemsControl _selectedItems;
        private bool _selectedItemsInitialized;
        private bool _availableItemsInitialized;
        private IList _availableItemsInternal;
        private IList _selectedItemsInternal;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionEditor"/> class. 
        /// </summary>
        public CollectionEditor()
        {
            _availableItems = new ItemsControl();
            ((INotifyCollectionChanged)_availableItems.Items).CollectionChanged += AvailableItemsCollectionChanged;

            _selectedItems = new ItemsControl();
            ((INotifyCollectionChanged)_selectedItems.Items).CollectionChanged += SelectedItemsCollectionChanged;

            InitializeComponent();

            //Columns = new ObservableCollection<ColumnDescriptor>();

            GridViewColumns.SetDisplayMemberMember(gvAvailableItems, "DisplayMember");
            GridViewColumns.SetHeaderTextMember(gvAvailableItems, "HeaderText");

            GridViewColumns.SetDisplayMemberMember(gvSelectedItems, "DisplayMember");
            GridViewColumns.SetHeaderTextMember(gvSelectedItems, "HeaderText");

            btnAddAll.Command = new RelayCommand(AddAll, CanAddAll);
            btnAdd.Command = new RelayCommand(Add, CanAdd);
            btnRemove.Command = new RelayCommand(Remove, CanRemove);
            btnRemoveAll.Command = new RelayCommand(RemoveAll, CanRemoveAll);

            UpdateInternalCollections();
        }

        private void UpdateInternalCollections()
        {
            _availableItemsInternal = AvailableItemsSource != null ? (IList)AvailableItemsSource : AvailableItems;
            _selectedItemsInternal = SelectedItemsSource != null ? (IList)SelectedItemsSource : SelectedItems;
        }

        void AvailableItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = e.NewItems[i];
                        if (SelectedItems.Contains(item))
                            lvSelectedItems.Items.Add(item);
                        else
                            lvAvailableItems.Items.Insert(e.NewStartingIndex + i, item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object t in e.OldItems)
                    {
                        if (SelectedItems.Contains(t))
                            SelectedItems.Remove(t);
                        lvAvailableItems.Items.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    /*for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        lvAvailableItems.Items[e.NewStartingIndex + i] = e.NewItems[i];
                    }*/
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Move:
                    /*var items = e.OldItems.Cast<object>().Select((t, i) => lvAvailableItems.Items[e.OldStartingIndex + i]).ToList();
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        lvAvailableItems.Items.Insert(e.NewStartingIndex + i, items);
                    }*/
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    ResetCollection();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ResetCollection()
        {
            lvAvailableItems.Items.Clear();
            lvSelectedItems.Items.Clear();
            foreach (var item in AvailableItems)
            {
                if (SelectedItems.Contains(item))
                    lvSelectedItems.Items.Add(item);
                else
                    lvAvailableItems.Items.Add(item);
            }
        }

        private void SelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = e.NewItems[i];
                        if (!AvailableItems.Contains(item))
                            AvailableItems.Add(item);

                        lvAvailableItems.Items.Remove(item);
                        lvSelectedItems.Items.Insert(e.NewStartingIndex + i, item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object t in e.OldItems)
                    {
                        if (AvailableItems.Contains(t))
                            lvAvailableItems.Items.Add(t);
                        lvSelectedItems.Items.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        lvSelectedItems.Items[e.NewStartingIndex + i] = e.NewItems[i];
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    /*var items = e.OldItems.Cast<object>().Select((t, i) => lvSelectedItems.Items[e.OldStartingIndex + i]).ToList();
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        lvSelectedItems.Items.Insert(e.NewStartingIndex + i, items);
                    }*/
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    ResetCollection();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CanRemoveAll()
        {
            return lvSelectedItems.Items.Count > 0;
        }

        /// <summary>
        /// Removes any item from the <see cref="SelectedItems"/> collection.
        /// </summary>
        public void RemoveAll()
        {
            if (!CanRemoveAll())
                return;

            _selectedItemsInternal.Clear();
        }

        private bool CanRemove()
        {
            return lvSelectedItems.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Removes the selected items from the <see cref="SelectedItems"/> collection.
        /// </summary>
        public void Remove()
        {
            if (!CanRemove())
                return;

            foreach (var item in new ArrayList(lvSelectedItems.SelectedItems))
                _selectedItemsInternal.Remove(item);
        }

        private bool CanAdd()
        {
            return lvAvailableItems.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Adds the selected items from the <see cref="AvailableItems"/> collection to the <see cref="SelectedItems"/> collection.
        /// </summary>
        public void Add()
        {
            if (!CanAdd())
                return;

            foreach (var item in new ArrayList(lvAvailableItems.SelectedItems))
                _selectedItemsInternal.Add(item);
        }

        private bool CanAddAll()
        {
            return lvAvailableItems.Items.Count > 0;
        }

        /// <summary>
        /// Adds any item from the <see cref="AvailableItems"/> collection to the <see cref="SelectedItems"/> collection.
        /// </summary>
        public void AddAll()
        {
            if (!CanAddAll())
                return;

            foreach (var item in new ArrayList(lvAvailableItems.Items))
            {
                _selectedItemsInternal.Add(item);
            }
        }

        private static void OnColumnsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;

            GridViewColumns.SetColumnsSource(editor.gvAvailableItems, e.NewValue);
            GridViewColumns.SetColumnsSource(editor.gvSelectedItems, e.NewValue);
        }

        private static bool IsValidSelectionMode(object value)
        {
            return true;
        }

        private static void OnAvailableItemsSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.SelectionMode = (SelectionMode)e.NewValue;
        }

        private static void OnSelectedItemsSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvSelectedItems.SelectionMode = (SelectionMode)e.NewValue;
        }

        private static void OnAlternationCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.AlternationCount = (int)e.NewValue;
            editor.lvSelectedItems.AlternationCount = (int)e.NewValue;
        }

        private static void OnGroupStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.GroupStyleSelector = (GroupStyleSelector)e.NewValue;
            editor.lvSelectedItems.GroupStyleSelector = (GroupStyleSelector)e.NewValue;
        }

        private static void OnItemsPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.ItemsPanel = (ItemsPanelTemplate)e.NewValue;
            editor.lvSelectedItems.ItemsPanel = (ItemsPanelTemplate)e.NewValue;
        }

        private static void OnItemContainerStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.ItemContainerStyleSelector = (StyleSelector)e.NewValue;
            editor.lvSelectedItems.ItemContainerStyleSelector = (StyleSelector)e.NewValue;
        }

        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.ItemContainerStyle = (Style)e.NewValue;
            editor.lvSelectedItems.ItemContainerStyle = (Style)e.NewValue;
        }

        private static void OnItemBindingGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.ItemBindingGroup = (BindingGroup)e.NewValue;
            editor.lvSelectedItems.ItemBindingGroup = (BindingGroup)e.NewValue;
        }

        private static void OnItemStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.ItemStringFormat = (string)e.NewValue;
            editor.lvSelectedItems.ItemStringFormat = (string)e.NewValue;
        }

        private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.ItemTemplateSelector = (DataTemplateSelector)e.NewValue;
            editor.lvSelectedItems.ItemTemplateSelector = (DataTemplateSelector)e.NewValue;
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.ItemTemplate = (DataTemplate)e.NewValue;
            editor.lvSelectedItems.ItemTemplate = (DataTemplate)e.NewValue;
        }

        private static void OnDisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor.lvAvailableItems.DisplayMemberPath = (string)e.NewValue;
            editor.lvSelectedItems.DisplayMemberPath = (string)e.NewValue;
        }

        private static void OnSelectedItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor._selectedItems.ItemsSource = (IEnumerable)e.NewValue;

            editor.UpdateInternalCollections();
        }

        private static bool IsValidSelectedItemsSource(object value)
        {
            return value == null || value is IList;
        }

        private static void OnAvailableItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CollectionEditor)d;
            editor._availableItems.ItemsSource = (IEnumerable)e.NewValue;

            editor.UpdateInternalCollections();
        }

        private static bool IsValidAvailableItemsSource(object value)
        {
            return value == null || value is IList;
        }

        /// <summary>
        /// <see cref="FrameworkElement.BeginInit"/>
        /// </summary>
        public override void BeginInit()
        {
            base.BeginInit();

            _availableItems.BeginInit();
            _selectedItems.BeginInit();
        }

        /// <summary>
        /// <see cref="FrameworkElement.EndInit"/>
        /// </summary>
        public override void EndInit()
        {
            _availableItems.EndInit();
            _selectedItems.EndInit();

            base.EndInit();
        }

        /// <summary>
        /// Gets or sets a collection used to generate the content of the <see cref="AvailableItems"/>. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(true), Category("Content")]
        public IEnumerable AvailableItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(AvailableItemsSourceProperty);
            }
            set
            {
                SetValue(AvailableItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a collection used to generate the content of the <see cref="SelectedItems"/>. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(true), Category("Content")]
        public IEnumerable SelectedItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(SelectedItemsSourceProperty);
            }
            set
            {
                SetValue(SelectedItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets the collection of available items.
        /// </summary>
        public ItemCollection AvailableItems
        {
            get
            {
                if (IsInitialized && !_availableItemsInitialized)
                {
                    _availableItemsInitialized = true;
                    lvAvailableItems.BeginInit();
                    lvAvailableItems.EndInit();
                }

                return _availableItems.Items;
            }
        }

        /// <summary>
        /// Gets the collection of selected items.
        /// </summary>
        public ItemCollection SelectedItems
        {
            get
            {
                if (IsInitialized && !_selectedItemsInitialized)
                {
                    _selectedItemsInitialized = true;
                    lvSelectedItems.BeginInit();
                    lvSelectedItems.EndInit();
                }

                return _selectedItems.Items;
            }
        }

        /// <summary>
        /// Gets or sets a path to a value on the source object to serve as the visual representation of the object.
        /// </summary>
        [Localizability(LocalizationCategory.NeverLocalize), Bindable(true), Category("Appearance")]
        public string DisplayMemberPath
        {
            get
            {
                return (string)GetValue(DisplayMemberPathProperty);
            }
            set
            {
                SetValue(DisplayMemberPathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display each item. 
        /// </summary>
        [Bindable(true), Category("Content")]
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the custom logic for choosing a template used to display each item. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(true), Category("Content")]
        public DataTemplateSelector ItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(ItemTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a composite string that specifies how to format the items if they are displayed as strings. 
        /// </summary>
        [Bindable(true), Category("Content")]
        public string ItemStringFormat
        {
            get
            {
                return (string)GetValue(ItemStringFormatProperty);
            }
            set
            {
                SetValue(ItemStringFormatProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the BindingGroup that is copied to each item. 
        /// </summary>
        [Category("Content"), Bindable(true)]
        public BindingGroup ItemBindingGroup
        {
            get
            {
                return (BindingGroup)GetValue(ItemBindingGroupProperty);
            }
            set
            {
                SetValue(ItemBindingGroupProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Style"/> that is applied to the container element generated for each item. 
        /// </summary>
        [Bindable(true), Category("Content")]
        public Style ItemContainerStyle
        {
            get
            {
                return (Style)GetValue(ItemContainerStyleProperty);
            }
            set
            {
                SetValue(ItemContainerStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom style-selection logic for a style that can be applied to each generated container element.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced), Bindable(false), Browsable(false)]
        public StyleSelector ItemContainerStyleSelector
        {
            get
            {
                return (StyleSelector)GetValue(ItemContainerStyleSelectorProperty);
            }
            set
            {
                SetValue(ItemContainerStyleSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the template that defines the panel that controls the layout of items. 
        /// </summary>
        [Bindable(false)]
        public ItemsPanelTemplate ItemsPanel
        {
            get
            {
                return (ItemsPanelTemplate)GetValue(ItemsPanelProperty);
            }
            set
            {
                SetValue(ItemsPanelProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the control is using grouping.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
        public bool IsGrouping
        {
            get
            {
                return lvAvailableItems.IsGrouping || lvSelectedItems.IsGrouping;
            }
        }

        /// <summary>
        /// Gets or sets a method that enables you to provide custom selection logic for a <see cref="GroupStyle"/> to apply to each group in a collection. 
        /// </summary>
        [Category("Content"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(true)]
        public GroupStyleSelector GroupStyleSelector
        {
            get
            {
                return (GroupStyleSelector)GetValue(GroupStyleSelectorProperty);
            }
            set
            {
                SetValue(GroupStyleSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of alternating item containers, which enables alternating containers to have a unique appearance.
        /// </summary>
        [Category("Content"), Bindable(true)]
        public int AlternationCount
        {
            get
            {
                return (int)GetValue(AlternationCountProperty);
            }
            set
            {
                SetValue(AlternationCountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selection behavior for the <see cref="AvailableItems"/>
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public SelectionMode AvailableItemsSelectionMode
        {
            get
            {
                return (SelectionMode)GetValue(AvailableItemsSelectionModeProperty);
            }
            set
            {
                SetValue(AvailableItemsSelectionModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selection behavior for the <see cref="SelectedItems"/>
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public SelectionMode SelectedItemsSelectionMode
        {
            get
            {
                return (SelectionMode)GetValue(SelectedItemsSelectionModeProperty);
            }
            set
            {
                SetValue(SelectedItemsSelectionModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a collection used to generate the <see cref="GridView"/> columns. 
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public IEnumerable ColumnsSource
        {
            get
            {
                return (IEnumerable)GetValue(ColumnsSourceProperty);
            }
            set
            {
                SetValue(ColumnsSourceProperty, value);
            }
        }

        /// <summary>
        /// Enables or disables the sorting by clicking the column header.
        /// </summary>
        [Bindable(true), Category("Appearance")]
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
        /// Enables or disables the buttons for sorting.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public bool ManualSortingEnabled
        {
            get
            {
                return (bool)GetValue(ManualSortingEnabledProperty);
            }
            set
            {
                SetValue(ManualSortingEnabledProperty, value);
            }
        }

        /// <summary>
        /// Header for the <see cref="AvailableItems"/> <see cref="ListView"/>.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public string AvailableItemsHeader
        {
            get
            {
                return (string)GetValue(AvailableItemsHeaderProperty);
            }
            set
            {
                SetValue(AvailableItemsHeaderProperty, value);
            }
        }

        /// <summary>
        /// Header for the <see cref="SelectedItems"/> <see cref="ListView"/>.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public string SelectedItemsHeader
        {
            get
            {
                return (string)GetValue(SelectedItemsHeaderProperty);
            }
            set
            {
                SetValue(SelectedItemsHeaderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the first item in the current selection of the available items collection or returns null if the selection is empty. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), Bindable(true)]
        public object AvailableItemsSelectedItem
        {
            get
            {
                return GetValue(AvailableItemsSelectedItemProperty);
            }
            set
            {
                SetValue(AvailableItemsSelectedItemProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the first item in the current selection of the selected items collection or returns null if the selection is empty 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), Bindable(true)]
        public object SelectedItemsSelectedItem
        {
            get
            {
                return GetValue(SelectedItemsSelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemsSelectedItemProperty, value);
            }
        }

        private void AvailableItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AvailableItemsSelectedItem = lvAvailableItems.SelectedItem;
        }

        private void SelectedItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsSelectedItem = lvSelectedItems.SelectedItem;
        }

        private void AvailableItemsMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Add();
        }

        private void SelectedItemsMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Remove();
        }
    }
}
