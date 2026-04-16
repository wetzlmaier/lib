using Scch.DomainModel.EntityFramework.Mapping;

namespace Scch.DomainModel.EntityFramework.Tests.Mapping
{
    public abstract class EntityWithBezeichnungMapping<T> : EntityFrameworkEntityMappingBase<long, T> where T : EntityWithBezeichnung
    {
        protected EntityWithBezeichnungMapping()
        {
            Property(x => x.Bezeichnung);
        }
    }
}
