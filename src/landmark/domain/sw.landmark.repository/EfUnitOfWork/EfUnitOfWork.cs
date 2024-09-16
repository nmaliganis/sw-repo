using sw.infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Transactions;
using sw.landmark.repository.DbContexts;

namespace sw.landmark.repository.EfUnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly swDbContext _context;

        public EfUnitOfWork(swDbContext context)
        {
            _context = context;
        }

        public bool TransactionHandled { get; private set; }
        public bool SessionClosed { get; private set; }

        public void Commit()
        {
            using IDbContextTransaction transaction = _context?.Database.BeginTransaction();
            try
            {
                if (_context == null) return;
                if (_context.Database.CurrentTransaction is null) return;

                //_session.Flush();
                _context.SaveChanges();
                transaction?.Commit();
                TransactionHandled = true;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                TransactionHandled = false;
                throw new TransactionException(ex.Message, ex.InnerException);
            }
        }

        public void Close()
        {
            //_context.Close();
            _context.Dispose();
            SessionClosed = true;
        }
    }
}
