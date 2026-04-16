using System;

namespace Scch.BusinessLogic.EntityFramework
{
    /// <summary>
    /// <see cref="Exception"/> that will be thrown in context of optimistic locking.
    /// </summary>
    public class OptimisticLockingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticLockingException"/> class.
        /// </summary>
        public OptimisticLockingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticLockingException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public OptimisticLockingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticLockingException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public OptimisticLockingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticLockingException"/> class.
        /// </summary>
        /// <param name="innerException"></param>
        public OptimisticLockingException(Exception innerException)
            : base("Entities may have been modified or deleted since they were loaded.", innerException)
        {
        }
    }
}
