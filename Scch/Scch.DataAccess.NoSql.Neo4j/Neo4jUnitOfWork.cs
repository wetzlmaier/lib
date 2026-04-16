using System;
using System.Transactions;

namespace Scch.DataAccess.NoSql.Neo4j
{
    public class Neo4jUnitOfWork: NoSqlUnitOfWork, INeo4jUnitOfWork
    {
        private TransactionScope _scope;

        public Neo4jUnitOfWork()
        {
        }

        public override bool IsInTransaction
        {
            get { return _scope != null; }
        }

        public override void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            CheckIfDisposed();

            var options = new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = TransactionManager.DefaultTimeout
            };

            _scope= new TransactionScope(TransactionScopeOption.Required, options);
        }

        public override void RollBackTransaction()
        {
            Dispose(true);
        }

        public override void CommitTransaction()
        {
            _scope.Complete();
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _scope.Dispose();
                _scope = null;
            }

            base.Dispose(disposing);
        }
    }
}
