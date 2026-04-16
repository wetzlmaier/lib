using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Viewmodel for master detail collections
    /// </summary>
    /// <typeparam name="TItem">The type of items the collection holds</typeparam>
    /// <typeparam name="TViewModel">The viewmodel of the items</typeparam>
    abstract public class CollectionViewModel<TItem, TViewModel> : NavigationViewModelBase where TViewModel : IItemViewModel<TItem>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionViewModel{TItem, TViewModel}"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        protected CollectionViewModel(IEnumerable<TItem> list)
            : this()
        {
            Data = list;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionViewModel{TItem, TViewModel}"/> class.
        /// </summary>
        protected CollectionViewModel()
        {
            ViewSource = new CollectionViewSource();
            CurrentViewModel = new TViewModel();
        }

        /// <summary>
        /// Handles the CurrentChanged event of the IView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCurrentChanged(object sender, EventArgs e)
        {
            if (View == null)
                return;

            RaisePropertyChanged(() => Position);
            CurrentViewModel.Item = CurrentItem;
            RaisePropertyChanged(() => CurrentItem);
            RaisePropertyChanged(() => CurrentViewModel);

            EventHandler<CurrentChangedArgs<TItem, TViewModel>> handler = CurrentChanged;
            if (handler != null)
                handler(null, new CurrentChangedArgs<TItem, TViewModel>(CurrentItem, CurrentViewModel));
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public IEnumerable<TItem> Data
        {
            get { return (IEnumerable<TItem>)ViewSource.Source; }
            set
            {
                ViewSource.Source = value;
                OnViewSourceChanged();
            }
        }

        abstract protected void OnViewSourceChanged();

        /// <summary>
        /// Gets the view source.
        /// </summary>
        /// <value>The view source.</value>
        public CollectionViewSource ViewSource { get; private set; }

        /// <summary>
        /// Gets the View of the ViewSource.
        /// </summary>
        protected ICollectionView View { get { return ViewSource.View; } }

        TItem _currentItem;

        /// <summary>
        /// Gets or sets the current item.
        /// </summary>
        /// <value>The current item.</value>
        public TItem CurrentItem
        {
            get
            {
                _currentItem = (View != null) ? (TItem)View.CurrentItem : default(TItem);
                return _currentItem;
            }
            set
            {
                View.MoveCurrentTo(value);
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public override int Position
        {
            get { return View.CurrentPosition + 1; }
            set
            {
                View.MoveCurrentToPosition(value - 1);
            }
        }

        /// <summary>
        /// Occurs when the current item changes.
        /// </summary>
        public event EventHandler<CurrentChangedArgs<TItem, TViewModel>> CurrentChanged;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (View != null)
                    View.CollectionChanged += value;
            }
            remove
            {
                if (View != null)
                    View.CollectionChanged -= value;
            }
        }

        /// <summary>
        /// Gets the current view model.
        /// </summary>
        /// <value>The current view model.</value>
        public TViewModel CurrentViewModel { get; protected set; }

        /// <summary>
        /// Go to the next item in the collection.
        /// </summary>
        protected override void Next()
        {
            View.MoveCurrentToNext();
        }

        /// <summary>
        /// Gets a value indicating whether this instance can go to the next item.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go to the next item; otherwise, <c>false</c>.
        /// </value>
        protected override bool CanNext
        {
            get
            {
                return View != null && View.CurrentPosition <= Count - 2;
            }
        }
        /// <summary>
        /// Go to the previous item in the collection.
        /// </summary>
        protected override void Previous()
        {
            View.MoveCurrentToPrevious();
        }

        /// <summary>
        /// Gets a value indicating whether this instance instance can go to the previous item.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go to the previous item; otherwise, <c>false</c>.
        /// </value>
        protected override bool CanPrevious
        {
            get
            {
                return View != null && View.CurrentPosition > 0;
            }
        }

        /// <summary>
        /// Go to the first item in the collection.
        /// </summary>
        protected override void First()
        {
            View.MoveCurrentToFirst();
        }
        /// <summary>
        /// Gets a value indicating whether this instance can go to the first item..
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go to the first item; otherwise, <c>false</c>.
        /// </value>
        protected override bool CanFirst
        {
            get { return View != null && View.CurrentPosition > 0; }
        }

        /// <summary>
        /// Go to the last item in the collection.
        /// </summary>
        protected override void Last()
        {
            View.MoveCurrentToLast();
        }
        /// <summary>
        /// Gets a value indicating whether this instance can go to the last item.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go to the last item; otherwise, <c>false</c>.
        /// </value>
        protected override bool CanLast
        {
            get { return View != null && View.CurrentPosition <= Count - 2; }
        }
    }
}

