using System;

namespace Scch.DomainModel.Hibernate
{
    public abstract class HibernateEntity<TKey> : Entity<TKey>, IHibernateEntity<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public override bool IsTransient
        {
            get { return Equals(Id, default(TKey)); }
        }

        public virtual byte[] RowVersion { get; }

        public virtual void IncrementVersion()
        {
            throw new NotImplementedException();
        }
    }
}
