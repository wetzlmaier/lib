using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Scch.Common.Windows.Input;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// <see cref="IViewModel"/> that provides additional information for an <see cref="ICommand"/>.
    /// </summary>
    public class CommandViewModel : ImageSourceViewModel, ICommandViewModel
    {
        private ICommand _command;
        private KeyGesture _shortcut;
        private bool _isActive;
        private string _toolTip;
        private static readonly KeyGestureToStringConverter Converter;

        static CommandViewModel()
        {
            Converter = new KeyGestureToStringConverter();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        public CommandViewModel(string displayName, ICommand command)
            : base(displayName)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            Initialize(string.Empty, command, null, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command)
            : base(displayName)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            Initialize(toolTip, command, null, false);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="bitmap">The <see cref="Bitmap"/> for the <see cref="ImageSource"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, Bitmap bitmap) :
            base(displayName, bitmap)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            Initialize(toolTip, command, null, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="isActivatable"><see cref="IsActivatable"/>.</param>
        /// <param name="bitmap">The <see cref="Bitmap"/> for the <see cref="ImageSource"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, bool isActivatable, Bitmap bitmap) :
            base(displayName, bitmap)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            Initialize(toolTip, command, null, isActivatable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="isActivatable"><see cref="IsActivatable"/>.</param>
        /// <param name="imageSource">The <see cref="ImageSource"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, bool isActivatable, ImageSource imageSource)
            : base(displayName, imageSource)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            Initialize(toolTip, command, null, isActivatable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="imageSource">The <see cref="ImageSource"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, ImageSource imageSource)
            : this(displayName, toolTip, command, null, false, imageSource)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            Initialize(toolTip, command, null, false);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="shortcut">The <see cref="Shortcut"/>.</param>
        public CommandViewModel(string displayName, ICommand command, KeyGesture shortcut)
            : base(displayName)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (shortcut == null)
                throw new ArgumentNullException("shortcut");

            Initialize(string.Empty, command, shortcut, false);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="shortcut">The <see cref="Shortcut"/>.</param>
        /// <param name="isActivatable"><see cref="IsActivatable"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, KeyGesture shortcut, bool isActivatable)
            : base(displayName)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            if (shortcut == null)
                throw new ArgumentNullException("shortcut");

            Initialize(toolTip, command, shortcut, isActivatable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="shortcut">The <see cref="Shortcut"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, KeyGesture shortcut)
            : base(displayName)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            if (shortcut == null)
                throw new ArgumentNullException("shortcut");

            Initialize(toolTip, command, shortcut, false);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="shortcut">The <see cref="Shortcut"/>.</param>
        /// <param name="bitmap">The <see cref="Bitmap"/> for the <see cref="ImageSource"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, KeyGesture shortcut, Bitmap bitmap) :
            base(displayName, bitmap)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            if (shortcut == null)
                throw new ArgumentNullException("shortcut");

            Initialize(toolTip, command, shortcut, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="shortcut">The <see cref="Shortcut"/>.</param>
        /// <param name="isActivatable"><see cref="IsActivatable"/>.</param>
        /// <param name="bitmap">The <see cref="Bitmap"/> for the <see cref="ImageSource"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, KeyGesture shortcut, bool isActivatable, Bitmap bitmap) :
            base(displayName, bitmap)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            if (shortcut == null)
                throw new ArgumentNullException("shortcut");

            Initialize(toolTip, command, shortcut, isActivatable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="displayName">The <see cref="ViewModelBase.DisplayName"/>.</param>
        /// <param name="toolTip">The <see cref="ToolTip"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        /// <param name="shortcut">The <see cref="Shortcut"/>.</param>
        /// <param name="isActivatable"><see cref="IsActivatable"/>.</param>
        /// <param name="imageSource">The <see cref="ImageSource"/>.</param>
        public CommandViewModel(string displayName, string toolTip, ICommand command, KeyGesture shortcut, bool isActivatable, ImageSource imageSource)
            : base(displayName, imageSource)
        {
            if (toolTip == null)
                throw new ArgumentNullException("toolTip");

            if (command == null)
                throw new ArgumentNullException("command");

            if (shortcut == null)
                throw new ArgumentNullException("shortcut");

            Initialize(toolTip, command, shortcut, isActivatable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class. 
        /// </summary>
        /// <param name="toolTip"></param>
        /// <param name="command"></param>
        /// <param name="shortcut"></param>
        /// <param name="isActivatable"></param>
        private void Initialize(string toolTip, ICommand command, KeyGesture shortcut, bool isActivatable)
        {
            ToolTip = toolTip;
            Command = command;
            IsActivatable = isActivatable;
            Shortcut = shortcut;
            TextVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Tooltip
        /// </summary>
        public string ToolTip
        {
            get
            {
                return Shortcut != null ? string.Format("{0} ({1})", _toolTip, Converter.Convert(Shortcut, typeof(string), null, CultureInfo.CurrentUICulture)) : _toolTip;
            }
            set
            {
                if (_toolTip == value)
                    return;

                _toolTip = value;
                RaisePropertyChanged(() => ToolTip);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> of the <see cref="ViewModelBase.DisplayName"/>.
        /// </summary>
        public Visibility TextVisibility { get; set; }

        /// <summary>
        /// The <see cref="ICommand"/> to perform the operation.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return _command;
            }
            set
            {
                if (_command == value)
                    return;

                _command = value;
                RaisePropertyChanged(() => Command);
            }
        }

        /// <summary>
        /// Returns true, if the operation is activatable. 
        /// </summary>
        public bool IsActivatable { get; private set; }

        /// <summary>
        /// Gets or sets the active flag of the operation
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive == value)
                    return;

                _isActive = value;
                RaisePropertyChanged(() => IsActive);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="KeyGesture"/> to perform the operation.
        /// </summary>
        public KeyGesture Shortcut
        {
            get
            {
                return _shortcut;
            }
            set
            {
                if (_shortcut == value)
                    return;

                _shortcut = value;
                RaisePropertyChanged(() => Shortcut);
                RaisePropertyChanged(() => ToolTip);
            }
        }
    }
}
