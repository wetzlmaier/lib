using System;
using System.Runtime.Serialization;
using System.Security;

namespace Scch.Common.Configuration
{
    [Serializable]
    public class ConfigurationValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValueException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public ConfigurationValueException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValueException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ConfigurationValueException(string message, Exception inner)
            : base(message, inner)
        {

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
    }
}
