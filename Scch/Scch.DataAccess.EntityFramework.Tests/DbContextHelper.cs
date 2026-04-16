namespace Scch.DataAccess.EntityFramework.Tests
{
    public static class DbContextHelper
    {
        private static bool _initialized;

        public static void Init()
        {
            if (_initialized)
                return;

            lock (typeof(DbContextHelper))
            {
                if (_initialized)
                    return;

                DbContextManager<long>.Init("Database", new[] { "Scch.DomainModel", "Scch.DomainModel.EntityFramework", "Scch.DataAccess.EntityFramework.Tests", "Scch.DomainModel.EntityFramework.Tests" }, true);
                _initialized = true;
            }
        }
    }
}
