using System;
using System.Runtime.Serialization;
using System.Security;

namespace Scch.DomainModel.NoSql
{
    [Serializable]
    public class NoSqlException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSqlException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public NoSqlException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSqlException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NoSqlException(string message, Exception inner)
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
