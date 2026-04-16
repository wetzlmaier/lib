using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Scch.DomainModel.Hibernate.Mapping
{
    public abstract class HibernateEntityMappingBase<TKey, TEntity> : ClassMapping<TEntity>
        where TEntity : class, IHibernateEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected HibernateEntityMappingBase()
        {
            //Ignore(x => x.Error);
            //Ignore(x => x.IsValid);

            Version(x => x.RowVersion, m =>
            {
                m.Column("RowVersion");
                m.Type(NHibernateUtil.BinaryBlob);
                m.Generated(VersionGeneration.Always);
                m.UnsavedValue(null);
            });
        }
    }
}
