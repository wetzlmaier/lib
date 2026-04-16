using System;

namespace Scch.DomainModel
{
    public abstract class DeletableEntity<TKey> : Entity<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private bool _isDeleted;

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                if (_isDeleted == value)
                    return;

                _isDeleted = value;
                RaisePropertyChanged(() => IsDeleted);
            }
        }
    }
}
