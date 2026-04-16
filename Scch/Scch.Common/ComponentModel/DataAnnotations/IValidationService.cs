namespace Scch.Common.ComponentModel.DataAnnotations
{
    public interface IValidationService
    {
        ValidationSummary Validate(object entity);
        ValidationSummary Validate(object entity, bool throwOnError);

        ValidationSummary ValidateDelete(object entity);
        ValidationSummary ValidateDelete(object entity, bool throwOnError);
    }
}
