using System;

namespace Scch.Common.ComponentModel.DataAnnotations
{
    public abstract class ValidatorBase<T> : IValidator
    {
        public ValidationSummary ValidateDelete(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return ValidateDelete((T)entity);
        }

        protected abstract ValidationSummary ValidateDelete(T entity);

        public ValidationSummary Validate(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return Validate((T)entity);
        }

        protected abstract ValidationSummary Validate(T entity);

        public Type ForType
        {
            get { return typeof(T); }
        }
    }
}
