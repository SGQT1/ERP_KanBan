using System;
using System.Threading.Tasks;
using ERP.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ERP.Data.Utilities
{
    public class UnitOfWork : IDisposable
    {
        private DbContext DbContext { get; }

        private IDbContextTransaction Transaction { get; set; }

        public UnitOfWork(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void BeginTransaction()
        {
            if (Transaction == null)
            {
                Transaction = DbContext.Database.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }
    }
}