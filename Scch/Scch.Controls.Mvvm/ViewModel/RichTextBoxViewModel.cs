using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Scch.Common.Windows;
using Scch.Mvvm.ViewModel;

namespace Scch.Controls.Mvvm.ViewModel
{
    public class RichTextBoxViewModel : ViewModelBase
    {
        public event ExecutedRoutedEventHandler InsertPicture;

        private ICollectionView _contextStyles;
        private ObservableCollection<StyleInfo> _styles;
        private StyleInfo _selectedStyle;
        private FlowDocument _document;
        private TextSelection _selection;
        private TextPointer _caretPosition;
        private bool _caretPositionChanging;
        private bool _selectedStyleChanging;
        private string _text;

        public RichTextBoxViewModel() : base("RichTextBox", 0)
        {
            _text = string.Empty;
            Styles = new ObservableCollection<StyleInfo>();

            var font = new Style();
            font.Setters.Add(new Setter(TextElement.FontFamilyProperty, new FontFamily("Tahoma")));

            var style = new Style { BasedOn = font };
            style.Setters.Add(new Setter(TextElement.FontWeightProperty, FontWeights.Normal));
            style.Setters.Add(new Setter(TextElement.FontSizeProperty, 12.0));
            _styles.Add(new StyleInfo("Normal", typeof(TextElement), style));

            SelectedStyle = _styles[0];
            ContextStyles = CollectionViewSource.GetDefaultView(_styles);

            Document = CreateEmptyDocument();

            CopyCommand = CommonCommands.CreateCopyCommand(ApplicationCommands.Copy);
            RegisterCommand(CopyCommand);
            PasteCommand = CommonCommands.CreatePasteCommand(ApplicationCommands.Paste);
            RegisterCommand(PasteCommand);
            CutCommand = CommonCommands.CreateCutCommand(ApplicationCommands.Cut);
            RegisterCommand(CutCommand);
            DeleteCommand = CommonCommands.CreateDeleteCommand(ApplicationCommands.Delete);
            RegisterCommand(DeleteCommand);

            var insertPictureCommand = new RoutedCommand("InsertPicture", typeof(BindableRichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(BindableRichTextBox), new CommandBinding(insertPictureCommand, InsertPicture, CanInsertTableOrPicture));
            InsertPictureCommand = CommonCommands.CreatePictureCommand(insertPictureCommand);

            InsertTableCommand = CommonCommands.CreateTableCommand(RichTextBoxCommands.InsertTableCommand);
            InsertLineCommand = CommonCommands.CreateLineCommand(RichTextBoxCommands.InsertLineCommand);

            UndoCommand = CommonCommands.CreateUndoCommand(ApplicationCommands.Undo);
            RegisterCommand(UndoCommand);
            RedoCommand = CommonCommands.CreateRedoCommand(ApplicationCommands.Redo);
            RegisterCommand(RedoCommand);

            NumberingCommand = CommonCommands.CreateNumberingCommand(EditingCommands.ToggleNumbering);
            RegisterCommand(NumberingCommand);
            BulletsCommand = CommonCommands.CreateBulletsCommand(EditingCommands.ToggleBullets);
            RegisterCommand(BulletsCommand);

            InsertRowBelowCommand = CommonCommands.CreateInsertRowBelowCommand(RichTextBoxCommands.InsertRowBelowCommand);
            InsertRowAboveCommand = CommonCommands.CreateInsertRowAboveCommand(RichTextBoxCommands.InsertRowAboveCommand);
            InsertColumnLeftCommand = CommonCommands.CreateInsertColumnLeftCommand(RichTextBoxCommands.InsertColumnLeftCommand);
            InsertColumnRightCommand = CommonCommands.CreateInsertColumnRightCommand(RichTextBoxCommands.InsertColumnRightCommand);
            DeleteRowCommand = CommonCommands.CreateDeleteRowCommand(RichTextBoxCommands.DeleteRowCommand);
            DeleteColumnCommand = CommonCommands.CreateDeleteColumnCommand(RichTextBoxCommands.DeleteColumnCommand);
            DeleteTableCommand = CommonCommands.CreateDeleteTableCommand(RichTextBoxCommands.DeleteTableCommand);

            PropertiesCommand = CommonCommands.CreatePropertiesCommand(RichTextBoxCommands.EditPropertiesCommand);
            RegisterCommand(PropertiesCommand);

            TextChanged = new RelayCommand(DocumentTextChanged);
        }

        FlowDocument CreateEmptyDocument()
        {
            var run = new Run(string.Empty);
            run.SetResourceReference(FrameworkContentElement.StyleProperty, "Normal");
            var para = new Paragraph(run);
            return new FlowDocument(para);
        }

        /*
        private void Save()
        {
            using (var fs = File.OpenWrite("style.xaml"))
            {
                XamlWriter.Save(new List<StyleInfo>(Styles).ToArray(), fs);
                fs.SetLength(fs.Position);
            }

            using (var fs = File.OpenWrite("doc.xaml"))
            {
                XamlWriter.Save(Document, fs);
                fs.SetLength(fs.Position);
            }
        }*/

        private void CanInsertTableOrPicture(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = RichTextBoxCommands.InsertPictureCommand.CanExecute(sender);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for copying content to the clipboard.
        /// </summary>
        public ICommandViewModel CopyCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for clipboard paste.
        /// </summary>
        public ICommandViewModel PasteCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for cutting content.
        /// </summary>
        public ICommandViewModel CutCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for deleting content.
        /// </summary>
        public ICommandViewModel DeleteCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for inserting pictures.
        /// </summary>
        public ICommandViewModel InsertPictureCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for inserting tables.
        /// </summary>
        public ICommandViewModel InsertTableCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for inserting horizontal lines.
        /// </summary>
        public ICommandViewModel InsertLineCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for undo changes.
        /// </summary>
        public ICommandViewModel UndoCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for redo changes.
        /// </summary>
        public ICommandViewModel RedoCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for numbering lists.
        /// </summary>
        public ICommandViewModel NumberingCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for bulleted lists.
        /// </summary>
        public ICommandViewModel BulletsCommand { get; private set; }

        /// <summary>
        /// The <see cref="ICommandViewModel"/> inserts a <see cref="TableRow"/> below the current row.
        /// </summary>
        public ICommandViewModel InsertRowBelowCommand { get; private set; }

        /// <summary>
        /// The <see cref="ICommandViewModel"/> inserts a <see cref="TableRow"/> above the current row.
        /// </summary>
        public ICommandViewModel InsertRowAboveCommand { get; private set; }

        /// <summary>
        /// The <see cref="ICommandViewModel"/> inserts a <see cref="TableColumn"/> left to the current column.
        /// </summary>
        public ICommandViewModel InsertColumnLeftCommand { get; private set; }

        /// <summary>
        /// The <see cref="ICommandViewModel"/> inserts a <see cref="TableColumn"/> right to the current column.
        /// </summary>
        public ICommandViewModel InsertColumnRightCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for deleting <see cref="TableRow"/>.
        /// </summary>
        public ICommandViewModel DeleteRowCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for deleting <see cref="TableColumn"/>.
        /// </summary>
        public ICommandViewModel DeleteColumnCommand { get; private set; }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for deleting <see cref="Table"/>.
        /// </summary>
        public ICommandViewModel DeleteTableCommand { get; private set; }

        /// <summary>
        /// The <see cref="ICommandViewModel"/> shows a properties dialog for <see cref="Table"/> or <see cref="System.Drawing.Image"/>.
        /// </summary>
        public ICommandViewModel PropertiesCommand { get; private set; }

        /// <summary>
        /// The <see cref="ObservableCollection{StyleInfo}"/> of available <see cref="StyleInfo"/>.
        /// </summary>
        public ObservableCollection<StyleInfo> Styles
        {
            get
            {
                return _styles;
            }
            set
            {
                if (_styles == value)
                    return;

                _styles = value;
                RaisePropertyChanged(() => Styles);
            }
        }

        /// <summary>
        /// The <see cref="ICollectionView"/> for the styles shown in the ribbon gallery.
        /// </summary>
        public ICollectionView ContextStyles
        {
            get
            {
                return _contextStyles;
            }
            private set
            {
                if (_contextStyles == value)
                    return;

                _contextStyles = value;
                RaisePropertyChanged(() => ContextStyles);
            }
        }

        /// <summary>
        /// Gets or sets the selected <see cref="StyleInfo"/> in the ribbon gallery.
        /// </summary>
        public StyleInfo SelectedStyle
        {
            get
            {
                return _selectedStyle;
            }
            set
            {
                if (_selectedStyle == value)
                    return;

                _selectedStyle = value;
                RaisePropertyChanged(() => SelectedStyle);
                OnSelectedStyleChanged();
            }
        }

        /// <summary>
        /// Gets or sets the edited <see cref="FlowDocument"/>.
        /// </summary>
        public FlowDocument Document
        {
            get
            {
                return _document;
            }
            set
            {
                if (Equals(_document, value))
                    return;

                _document = value;
                RaisePropertyChanged(() => Document);
            }
        }

        public ICommand TextChanged { get; private set; }

        private void DocumentTextChanged()
        {
            UpdateText();
        }

        private bool _updating;

        void UpdateText()
        {
            if (_updating)
                return;

            try
            {
                _updating = true;
                using (var stream = new MemoryStream())
                {
                    XamlWriter.Save(Document, stream);
                    Text = Encoding.Default.GetString(stream.ToArray());
                }
            }
            finally
            {
                _updating = false;
            }
        }

        void UpdateDocument()
        {
            if (_updating)
                return;

            try
            {
                if (string.IsNullOrEmpty(Text))
                    Document = CreateEmptyDocument();
                else
                {
                    using (var stream = new MemoryStream(Encoding.Default.GetBytes(Text)))
                    {
                        Document = (FlowDocument) XamlReader.Load(stream);
                    }
                }
            }
            finally
            {
                _updating = false;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;

                _text = value;
                RaisePropertyChanged(() => Text);
                UpdateDocument();
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="TextSelection"/>.
        /// </summary>
        public TextSelection Selection
        {
            get
            {
                return _selection;
            }
            set
            {
                if (_selection == value)
                    return;

                _selection = value;
                RaisePropertyChanged(() => Selection);
            }
        }

        /// <summary>
        /// Gets or sets the current caret position.
        /// </summary>
        public TextPointer CaretPosition
        {
            get
            {
                return _caretPosition;
            }
            set
            {
                if (_caretPosition == value)
                    return;

                _caretPosition = value;
                RaisePropertyChanged(() => CaretPosition);
                OnCarentPositionChanged();
            }
        }

        /// <summary>
        /// Called when the <see cref="CaretPosition"/> property has been changed.
        /// </summary>
        protected virtual void OnCarentPositionChanged()
        {
            if (!_selectedStyleChanging && Selection.Start.Parent is Run)
            {
                _caretPositionChanging = true;
                var run = (Run)Selection.Start.Parent;
                var value = run.GetValue(FrameworkContentElement.StyleProperty);

                SelectedStyle = (from s in Styles where s.Style == value select s).FirstOrDefault();
                _caretPositionChanging = false;
            }
        }

        /// <summary>
        /// Called when the <see cref="SelectedStyle"/> property has been changed.
        /// </summary>
        protected virtual void OnSelectedStyleChanged()
        {
            if (!_caretPositionChanging && Selection != null && Selection.Start.Parent is Run)
            {
                _selectedStyleChanging = true;

                var text = Selection.Text;
                Selection.Text = "";
                Selection.Text = text;
                Selection.ClearAllProperties();

                var run = (Run)Selection.Start.Parent;
                run.SetResourceReference(FrameworkContentElement.StyleProperty, SelectedStyle.Name);

                _selectedStyleChanging = false;

                UpdateText();
            }
        }
    }
}
