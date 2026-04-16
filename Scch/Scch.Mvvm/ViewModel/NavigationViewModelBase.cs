using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// ViewModel for navigating and manipulating collections
    /// </summary>
    public abstract class NavigationViewModelBase : ViewModelBase, INavigationViewModel
    {
        RelayCommand _addNewCommand;
        RelayCommand _deleteCommand;
        RelayCommand _nextCommand;
        RelayCommand _previousCommand;
        RelayCommand _firstCommand;
        RelayCommand _lastCommand;
        private bool _visibleCommandText;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewModelBase"/> class.
        /// </summary>
        protected NavigationViewModelBase()
            : base(null, 0)
        {
            VisibleCommandCommandText = false;
            NavigationViewModels = new ObservableCollection<ViewModelBase>();
        }

        /// <summary>
        /// Gets the viewmodels of the <see cref="NavigationViewModelBase"/>.
        /// </summary>
        /// <value>The <see cref="NavigationViewModelBase"/> viewmodels.</value>
        public ObservableCollection<ViewModelBase> NavigationViewModels { get; private set; }

        /// <summary>
        /// Gets the <see cref="ICommand"/> for <see cref="Add"/>.
        /// </summary>
        /// <value>The add command.</value>
        public ICommand AddNewCommand
        {
            get { return _addNewCommand ?? (_addNewCommand = new RelayCommand(Add, () => CanAdd)); }
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> for <see cref="Delete"/>.
        /// </summary>
        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(Delete, () => CanDelete)); }
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> for <see cref="Next"/>.
        /// </summary>
        public ICommand NextCommand
        {
            get { return _nextCommand ?? (_nextCommand = new RelayCommand(Next, () => CanNext)); }
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> for <see cref="Previous"/>.
        /// </summary>
        public ICommand PreviousCommand
        {
            get { return _previousCommand ?? (_previousCommand = new RelayCommand(Previous, () => CanPrevious)); }
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> for <see cref="First"/>.
        /// </summary>
        public ICommand FirstCommand
        {
            get { return _firstCommand ?? (_firstCommand = new RelayCommand(First, () => CanFirst)); }
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> for <see cref="Last"/>.
        /// </summary>
        public ICommand LastCommand
        {
            get { return _lastCommand ?? (_lastCommand = new RelayCommand(Last, () => CanLast)); }
        }

        /// <summary>
        /// Gets or sets the position within the navigation model.
        /// </summary>
        public abstract int Position { get; set; }

        /// <summary>
        /// Gets the count of the navigation model.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Gets or sets the <see cref="ICommandViewModel.TextVisibility"/> of any item in the <see cref="NavigationViewModels"/>.
        /// </summary>
        public bool VisibleCommandCommandText
        {
            get { return _visibleCommandText; }
            set
            {
                if (_visibleCommandText == value)
                    return;

                _visibleCommandText = value;
                Visibility vis = (_visibleCommandText) ? Visibility.Visible : Visibility.Collapsed;
                foreach (ICommandViewModel cmd in NavigationViewModels.OfType<ICommandViewModel>())
                    cmd.TextVisibility = vis;

                RaisePropertyChanged(() => VisibleCommandCommandText);
            }
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        protected abstract void Add();

        /// <summary>
        /// Gets a value indicating whether this instance can add an item.
        /// </summary>
        /// <value><c>true</c> if this instance can add an item; otherwise, <c>false</c>.</value>
        protected abstract bool CanAdd { get; }

        /// <summary>
        /// Performs the Delete operation.
        /// </summary>
        protected abstract void Delete();

        /// <summary>
        /// Returns true, if the <see cref="Delete"/> operation is allowed.
        /// </summary>
        protected abstract bool CanDelete { get; }

        /// <summary>
        /// Moves the <see cref="Position"/> to the next item.
        /// </summary>
        protected abstract void Next();

        /// <summary>
        /// Returns true, if <see cref="Next"/> is allowed.
        /// </summary>
        protected abstract bool CanNext { get; }

        /// <summary>
        /// Moves the <see cref="Position"/> to the previous item.
        /// </summary>
        protected abstract void Previous();

        /// <summary>
        /// Returns true, if <see cref="Previous"/> is allowed.
        /// </summary>
        protected abstract bool CanPrevious { get; }

        /// <summary>
        /// Moves the <see cref="Position"/> to the first item.
        /// </summary>
        protected abstract void First();

        /// <summary>
        /// Returns true, if <see cref="First"/> is allowed.
        /// </summary>
        protected abstract bool CanFirst { get; }

        /// <summary>
        /// Moves the <see cref="Position"/> to the last item.
        /// </summary>
        protected abstract void Last();

        /// <summary>
        /// Returns true, if <see cref="Last"/> is allowed.
        /// </summary>
        protected abstract bool CanLast { get; }
    }
}
