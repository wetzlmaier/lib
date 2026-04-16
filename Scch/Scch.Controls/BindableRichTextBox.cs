using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Scch.Common.Windows;

namespace Scch.Controls
{
    /// <summary>
    /// Represents a bindable rich editing control which operates on <see cref="FlowDocument"/> objects.    
    /// </summary>
    public class BindableRichTextBox : RichTextBox
    {
        /// <summary>
        /// The identifier for the <see cref="Document"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document",
            typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(OnDocumentChanged));

        /// <summary>
        /// The identifier for the <see cref="Selection"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register("Selection",
            typeof(TextSelection), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(default(TextSelection), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,OnSelectionChanged));

        /// <summary>
        /// The identifier for the <see cref="CaretPosition"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty CaretPositionProperty = DependencyProperty.Register("CaretPosition",
            typeof(TextPointer), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(default(TextPointer), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCaretPositionChanged));

        /// <summary>
        /// The identifier for the <see cref="Styles"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty StylesProperty = DependencyProperty.Register("Styles",
            typeof(ObservableCollection<StyleInfo>), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(OnStylesChanged));

        private static void OnStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (BindableRichTextBox) d;

            if (e.OldValue != null)
            {
                ((ObservableCollection<StyleInfo>) e.OldValue).CollectionChanged -=
                    view.BindableRichTextBox_CollectionChanged;

                foreach (StyleInfo styleInfo in (ObservableCollection<StyleInfo>) e.OldValue)
                {
                    view.Resources.Remove(styleInfo.Name);
                }
            }

            if (e.NewValue != null)
            {
                foreach (StyleInfo styleInfo in (ObservableCollection<StyleInfo>) e.NewValue)
                {
                    view.Resources.Add(styleInfo.Name, styleInfo.Style);
                }

                ((ObservableCollection<StyleInfo>) e.NewValue).CollectionChanged += view.BindableRichTextBox_CollectionChanged;
            }
        }

        void BindableRichTextBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotSupportedException();
        }

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="Style"/> collection that can be applied on text portions.
        /// </summary>
        public ObservableCollection<StyleInfo> Styles
        {
            get { return (ObservableCollection<StyleInfo>)GetValue(StylesProperty); }
            set { SetValue(StylesProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableRichTextBox"/> class.
        /// </summary>
        public BindableRichTextBox()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableRichTextBox"/> class.
        /// </summary>
        /// <param name="document">A <see cref="FlowDocument"/> to be added as the initial contents of the new <see cref="BindableRichTextBox"/>.</param>
        public BindableRichTextBox(FlowDocument document): base(document)
        {
        }

        /// <summary>
        /// <see cref="FrameworkElement.OnInitialized"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized(EventArgs e)
        {
            Selection = base.Selection;

            // Hook up to get notified when DocumentProperty changes.
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(DocumentProperty, typeof(BindableRichTextBox));
            descriptor.AddValueChanged(this, delegate
            {
                // If the underlying value of the dependency property changes,
                // update the underlying document, also.
                var newValue = (FlowDocument) GetValue(DocumentProperty) ?? new FlowDocument();
                
                if (newValue.Parent!=null && newValue.Parent!=this)
                {
                    var rtb = (RichTextBox) newValue.Parent;
                    rtb.Document = new FlowDocument();
                }

                base.Document = newValue;
            });

            // By default, we support updates to the source when focus is lost (or, if the LostFocus
            // trigger is specified explicity.  We don't support the PropertyChanged trigger right now.
            LostFocus += BindableRichTextBoxLostFocus;

            base.OnInitialized(e);
        }

        void BindableRichTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            // If we have a binding that is set for LostFocus or Default (which we are specifying as default)
            // then update the source.
            Binding binding = BindingOperations.GetBinding(this, DocumentProperty);

            if (binding == null)
                return;

            if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default ||
                binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
            {
                var expression=BindingOperations.GetBindingExpression(this, DocumentProperty);
                Debug.Assert(expression != null);
                expression.UpdateSource();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="FlowDocument"/> that represents the contents of the <see cref="BindableRichTextBox"/>.
        /// </summary>
        public new FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="TextSelection"/>.
        /// </summary>
        public new TextSelection Selection
        {
            get { return (TextSelection)GetValue(SelectionProperty); }
            set { SetValue(SelectionProperty, value); }
        }

        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            Selection = base.Selection;
            base.OnSelectionChanged(e);

            CaretPosition = base.CaretPosition;
        }

        private static void OnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Gets or sets the caret position.
        /// </summary>
        public new TextPointer CaretPosition
        {
            get { return (TextPointer)GetValue(CaretPositionProperty); }
            set { SetValue(CaretPositionProperty, value); }
        }

        private static void OnCaretPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
