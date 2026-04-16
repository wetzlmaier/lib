using System;
using System.Collections.Generic;
using System.Linq;

namespace Scch.Common.ComponentModel.DataAnnotations
{
    public class ValidationService : IValidationService
    {
        private readonly IDictionary<Type, IValidator> _validators;

        public ValidationService(IEnumerable<IValidator> validators)
        {
            _validators = validators.ToDictionary(k => k.ForType, v => v);
        }

        public ValidationSummary Validate(object entity, bool throwOnError)
        {
            var summary = Validate(entity);
            if (!summary.IsValid && throwOnError)
                throw new ValidationException(summary);
            return summary;
        }

        public ValidationSummary Validate(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return _validators[entity.GetType()].Validate(entity);
        }

        public ValidationSummary ValidateDelete(object entity, bool throwOnError)
        {
            var summary = ValidateDelete(entity);
            if (!summary.IsValid && throwOnError)
                throw new ValidationException(summary);
            return summary;
        }

        public ValidationSummary ValidateDelete(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return _validators[entity.GetType()].ValidateDelete(entity);
        }
    }
}
