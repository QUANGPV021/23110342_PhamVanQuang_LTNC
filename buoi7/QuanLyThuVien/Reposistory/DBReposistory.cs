using System;
using Microsoft.EntityFrameworkCore.Storage;
using QuanLyThuVienDAO;

namespace QuanLyThuVienService.Reposistory
{
    /// <summary>
    /// Lớp quản lý giao dịch (Transaction) trên cùng ngữ cảnh QLTVDBContext.
    /// </summary>
    public class DBReposistory
    {
        private readonly QLTVDBContext _context;
        private IDbContextTransaction? transaction;

        public QLTVDBContext Context => _context;

        public DBReposistory() : this(new QLTVDBContext())
        {
        }

        public DBReposistory(QLTVDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void BeginTransaction()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                transaction = _context.Database.BeginTransaction();
            }
        }

        public void ComitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
        }

        public void CommitTransaction() => ComitTransaction();

        public void RolbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
            _context.ChangeTracker.Clear();
        }

        public void RollbackTransaction() => RolbackTransaction();
    }
}
