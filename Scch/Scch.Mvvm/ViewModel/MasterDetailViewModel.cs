using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace EplusE.Inspire.Mvvm.ViewModel
{
    /// <summary>
    /// Viewmodel for master detail collections
    /// </summary>
    /// <typeparam name="TItem">The type of items the collection holds</typeparam>
    /// <typeparam name="TView">The viewmodel of the items</typeparam>
    public class MasterDetailViewModel<TItem, TView> : CollectionViewModel<TItem, TView>, IMasterDetailViewModel where TView : IItemViewModel<TItem>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDetailViewModel{TItem, TView}"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public MasterDetailViewModel(IEnumerable<TItem> list)
            : this()
        {
            Data = list;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDetailViewModel{TItem, TView}"/> class.
        /// </summary>
        private MasterDetailViewModel()
        {
            Details = new List<IMasterDetailViewModel>();
            OnMasterChanged = MasterChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDetailViewModel{TItem, TView}"/> class.
        /// </summary>
        /// <param name="master">The master collection</param>
        /// <param name="path">The path.</param>
        public MasterDetailViewModel(IMasterDetailViewModel master, string path)
            : this()
        {
            var b = new Binding { Source = master.ViewSource, Path = new PropertyPath(path) };
            BindingOperations.SetBinding(ViewSource, CollectionViewSource.SourceProperty, b);
            master.Details.Add(this);
            OnViewSourceChanged();
        }

        /// <summary>
        /// Adds new handlers to the new View's CurrentChanged event and calls View_CurrentChanged.
        /// </summary>
        protected override void OnViewSourceChanged()
        {
            if (View == null) return;
            View.CurrentChanged -= OnCurrentChanged;
            View.CurrentChanged += OnCurrentChanged;
            OnCurrentChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the CurrentChanged event of the View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnCurrentChanged(object sender, EventArgs e)
        {
            base.OnCurrentChanged(sender, e);
            foreach (IMasterDetailViewModel detail in Details)
                _currentDispatcher.BeginInvoke(detail.OnMasterChanged, DispatcherPriority.Render);
        }

        /// <summary>
        /// Called by the master viewmodel. Adds new handlers to the View's CurrentChanged event and
        /// Notifies the Count and View properties have changed.
        /// </summary>
        void MasterChanged()
        {
            OnViewSourceChanged();
            RaisePropertyChanged(() => Count);
            RaisePropertyChanged(() => View);
        }

        #region IMasterDetailViewModel Members

        /// <summary>
        /// Gets or sets the details viewmodels.
        /// </summary>
        /// <value>The detail viewmodels.</value>
        public List<IMasterDetailViewModel> Details { get; private set; }

        /// <summary>
        /// Gets or sets the on MasterChanged action.
        /// </summary>
        /// <value>The OnMasterChanged.</value>
        public Action OnMasterChanged { get; private set; }

        #endregion

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public BindingListCollectionView CollectionView { get { return (BindingListCollectionView)View; } }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        protected override void Add()
        {
            CollectionView.AddNew();
            CollectionView.CommitNew();
            RaisePropertyChanged(() => Count);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can add an item.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can add an item; otherwise, <c>false</c>.
        /// </value>
        protected override bool CanAdd
        {
            get
            {
                return View != null && CollectionView.CanAddNew;
            }
        }

        /// <summary>
        /// Deletes an item from the collection.
        /// </summary>
        protected override void Delete()
        {
            CollectionView.Remove(CurrentItem);
            RaisePropertyChanged(() => Count);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can delete an item.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can delete an item; otherwise, <c>false</c>.
        /// </value>
        protected override bool CanDelete
        {
            get
            {
                return View != null && CollectionView.CanRemove;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public override int Count
        {
            get { return CollectionView.Count; }
        }
    }
}

