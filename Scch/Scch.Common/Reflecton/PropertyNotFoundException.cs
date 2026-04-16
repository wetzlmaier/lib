using System;
using System.Runtime.Serialization;

namespace Scch.Common.Reflecton
{
    /// <summary>
    /// Thrown, if a property has not been found.
    /// </summary>
    [Serializable]
    public class PropertyNotFoundException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="PropertyNotFoundException"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public PropertyNotFoundException(string propertyName)
            : base(string.Format("Property '{0}' not found.", propertyName))
        {
        }

        /// <summary>
        /// <see cref="Exception(SerializationInfo, StreamingContext)"/>
        /// </summary>
        /// <param name="si"></param>
        /// <param name="sc"></param>
        protected PropertyNotFoundException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }
        #endregion Constructors
    }
}
