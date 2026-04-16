using System;
using System.Runtime.Serialization;
using System.Security;
using Scch.Common.Properties;

namespace Scch.Common.ComponentModel.DataAnnotations
{
    /// <summary>
    /// <see cref="Exception"/> for validation failures.
    /// </summary>
    [Serializable]
    public class ValidationException : Exception
    {
        private readonly ValidationSummary _summary;

        static ValidationException()
        {
            MessageText = Resources.ValidationException_Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="summary"></param>
        public ValidationException(ValidationSummary summary)
            : this(summary, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <param name="summary"></param>
        protected ValidationException(SerializationInfo info, StreamingContext context, ValidationSummary summary)
            : base(info, context)
        {
            _summary = summary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="summary"></param>
        /// <param name="inner"></param>
        public ValidationException(ValidationSummary summary, Exception inner)
            : base(MessageText + Environment.NewLine + summary, inner)
        {
            _summary = summary;
        }

        /// <summary>
        /// <see cref="Exception.GetObjectData"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Gets the <see cref="ValidationSummary"/> that caused the <see cref="ValidationException"/>.
        /// </summary>
        public ValidationSummary ValidationSummary
        {
            get
            {
                return _summary;
            }
        }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        public static string MessageText { get; private set; }
    }
}
