using System;
using System.Data.Entity.ModelConfiguration;

namespace Scch.DomainModel.EntityFramework.Mapping
{
    public abstract class EntityFrameworkEntityMappingBase<TKey, TEntity> : EntityTypeConfiguration<TEntity>
        where TEntity : class, IEntityFrameworkEntity<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected EntityFrameworkEntityMappingBase()
        {
            Ignore(x => x.Error);
            Ignore(x => x.IsValid);
        }
    }
}
