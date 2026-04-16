namespace Scch.DomainModel
{
    public interface IDescribable
    {
        /// <summary>
        /// Override this method to provide a description of the entity for audit purposes.
        /// </summary>
        /// <returns>The description of the entity.</returns>
        string Describe();
    }
}
