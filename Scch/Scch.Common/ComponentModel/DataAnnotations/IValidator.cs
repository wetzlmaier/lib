using System;

namespace Scch.Common.ComponentModel.DataAnnotations
{
    public interface IValidator
    {
        ValidationSummary Validate(object entity);
        ValidationSummary ValidateDelete(object entity);

        Type ForType { get; }
    }
}
