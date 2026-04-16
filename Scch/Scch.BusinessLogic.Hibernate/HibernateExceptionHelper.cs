using System;
using System.Linq;
using Scch.Common.ComponentModel.DataAnnotations;
using Scch.Logging;
using ValidationException = Scch.Common.ComponentModel.DataAnnotations.ValidationException;

namespace Scch.BusinessLogic.Hibernate
{
    public static class HibernateExceptionHelper
    {
        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception"></param>
        public static Exception HandleException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            Logger.Write(new ExceptionLogEntry(exception));

            return exception;
        }
    }
}
