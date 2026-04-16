namespace Scch.DomainModel.EntityFramework
{
    public interface IObjectWithChangeTracker
    {
        /// <summary>
        /// Has all the change tracking information for the subgraph of a given object.
        /// </summary>
        ObjectChangeTracker ChangeTracker { get; }
    }
}
