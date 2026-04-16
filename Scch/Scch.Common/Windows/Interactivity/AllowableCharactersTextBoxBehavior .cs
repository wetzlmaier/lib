using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Scch.Common.Windows.Interactivity
{
    public class AllowableCharactersTextBoxBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty RegularExpressionProperty = DependencyProperty.Register("RegularExpression", typeof(string),
            typeof(AllowableCharactersTextBoxBehavior), new FrameworkPropertyMetadata("*"));

        public string RegularExpression
        {
            get
            {
                return (string)GetValue(RegularExpressionProperty);
            }
            set
            {
                SetValue(RegularExpressionProperty, value);
            }
        }

        public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register("MaxLength", typeof(int), typeof(AllowableCharactersTextBoxBehavior),
        new FrameworkPropertyMetadata(int.MinValue));

        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }
            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));
                bool exceedsMaxLength = false;
                if (MaxLength > 0)
                {
                    exceedsMaxLength = text.Length > MaxLength;
                }

                if (!IsMatch(text, RegularExpression) || exceedsMaxLength)
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            bool exceedsMaxLength = false;
            string text = AssociatedObject.Text.Insert(AssociatedObject.CaretIndex, e.Text);
            if (MaxLength > 0)
            {
                exceedsMaxLength = text.Length > MaxLength;
            }

            e.Handled = IsMatch(text, RegularExpression) || exceedsMaxLength;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        private bool IsMatch(string text, string regex)
        {
            var emailregex = new Regex(regex);
            Match m = emailregex.Match(text);
            return m.Success;
        }
    }
}
