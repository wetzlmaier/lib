using System;
using Scch.DataAccess.EntityFramework;

namespace Scch.DataAccess.Tests
{
    public class MockRepositoryFactory<TKey> : IEntityFrameworkRepositoryFactory<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public IEntityFrameworkRepository<TKey> Create()
        {
            return new MockRepository<TKey>();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }
    }
}
