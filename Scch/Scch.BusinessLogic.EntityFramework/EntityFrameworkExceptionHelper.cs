using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using Scch.Common.ComponentModel.DataAnnotations;
using Scch.Logging;
using ValidationException = Scch.Common.ComponentModel.DataAnnotations.ValidationException;

namespace Scch.BusinessLogic.EntityFramework
{
    public static class EntityFrameworkExceptionHelper
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

            switch (exception.GetType().FullName)
            {
                case "System.Data.Entity.Infrastructure.DbUpdateConcurrencyException":
                    return new OptimisticLockingException(exception);

                case "System.Data.Entity.Validation.DbEntityValidationException":

                    var summary = new ValidationSummary();
                    var ex = (DbEntityValidationException)exception;
                    foreach (var validationError in ex.EntityValidationErrors.SelectMany(validationResult => validationResult.ValidationErrors))
                    {
                        summary.Add(new ValidationResult(validationError.ErrorMessage));
                    }

                    return new ValidationException(summary, ex);

                default:
                    return exception;
            }
        }
    }
}
