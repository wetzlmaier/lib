using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Scch.Controls
{
    public class ConsoleToFlowDocumentWriter : TextWriter
    {
        private readonly Paragraph _paragraph;
        private readonly StringBuilder _buffer;
        private Color _foregroundColor;
        private Color _backgroundColor;
        private readonly string _styleName;

        public ConsoleToFlowDocumentWriter(FlowDocument document)
        {
            if (document==null)
                throw new ArgumentNullException("document");

            _buffer = new StringBuilder();

            BackgroundColor = Brushes.Gray.Color;
            ForegroundColor = Brushes.Black.Color;

            _backgroundColor = BackgroundColor;
            _foregroundColor = ForegroundColor;

            _paragraph = new Paragraph();
            document.Blocks.Add(_paragraph);

            Console.SetOut(this);            
        }

        public ConsoleToFlowDocumentWriter(FlowDocument document, string styleName):this(document)
        {
            if (styleName == null)
                throw new ArgumentNullException("styleName");

            _styleName = styleName;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        public override void Write(char value)
        {
            base.Write(value);

            if (_foregroundColor != ForegroundColor || _backgroundColor != BackgroundColor)
            {
                FlushInternal();

                _backgroundColor = BackgroundColor;
                _foregroundColor = ForegroundColor;
            }

            _buffer.Append(value);
        }

        public Color ForegroundColor { get; set; }

        public Color BackgroundColor { get; set; }

        private void FlushInternal()
        {
            if (!Flushable)
                return;

            var run = new Run(_buffer.ToString())
            {
                Background = new SolidColorBrush(BackgroundColor),
                Foreground = new SolidColorBrush(ForegroundColor)
            };

            if (_styleName!=null)
                run.SetResourceReference(FrameworkContentElement.StyleProperty, _styleName);

            _paragraph.Inlines.Add(run);
            _buffer.Clear();
        }

        public bool Flushable
        {
            get { return (_buffer.Length > 0); }
        }

        public override void Flush()
        {
            base.Flush();

            FlushInternal();
        }
    }
}
