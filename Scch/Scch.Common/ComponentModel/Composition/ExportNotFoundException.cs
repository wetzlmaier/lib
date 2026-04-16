using System;

namespace Scch.Common.ComponentModel.Composition
{
    public class ExportNotFoundException : Exception
    {
        public string Export { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportNotFoundException"/> class.
        /// </summary>
        public ExportNotFoundException(string export)
            : base(string.Format("Export '{0}' not found.", export))
        {
            Initialize(export);
        }

        private void Initialize(string export)
        {
            Export = export;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportNotFoundException"/> class.
        /// </summary>
        /// <param name="export"></param>
        /// <param name="message"></param>
        public ExportNotFoundException(string export, string message)
            : base(message)
        {
            Initialize(export);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportNotFoundException"/> class.
        /// </summary>
        /// <param name="export"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ExportNotFoundException(string export, string message, Exception innerException)
            : base(message, innerException)
        {
            Initialize(export);
        }
    }
}
