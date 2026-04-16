namespace Scch.DataAccess.Hibernate
{
    public interface IModelContext
    {
        string[] EntityAssemblies { get; }

        string ConnectionStringName { get; }
    }
}
