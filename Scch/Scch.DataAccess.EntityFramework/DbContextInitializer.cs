using System;

namespace Scch.DataAccess.EntityFramework
{
    public class DbContextInitializer
    {
        private static volatile DbContextInitializer _instance;

        private DbContextInitializer() { }

        private bool _isInitialized;

        public static DbContextInitializer Instance()
        {
            if (_instance == null) {
                lock (typeof(DbContextInitializer)) {
                    if (_instance == null) {
                        _instance = new DbContextInitializer();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// This is the method which should be given the call to intialize the DbContext; e.g.,
        /// DbContextInitializer.Instance().InitializeDbContextOnce(() => InitializeDbContext());
        /// where InitializeDbContext() is a method which calls DbContextManager.Init()
        /// </summary>
        /// <param name="initMethod"></param>
        public void InitializeDbContextOnce(Action initMethod)
        {
            lock (typeof(DbContextInitializer))
            {
                if (!_isInitialized)
                {
                    initMethod();
                    _isInitialized = true;
                }
            }
        }        
    }
}
