using System.Collections.Generic;

namespace Scch.DataAccess.Hibernate
{
    public abstract class ModelContextBase : IModelContext
    {
        protected ModelContextBase(string connectionStringName, string[] entityAssemblies)
        {
            ConnectionStringName = connectionStringName;
            var list = new List<string>(entityAssemblies) { "Scch.DomainModel", "Scch.DomainModel.Hibernate" };
            EntityAssemblies = list.ToArray();
        }

        public string[] EntityAssemblies { get; private set; }

        public string ConnectionStringName { get; private set; }
    }
}
