using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVienDAO.Reposistory
{
    /// <summary>
    /// Data Access Layer (Repository) cho thực thể BookCopy (Bản sao sách).
    /// </summary>
    public class BookCopyRepository
    {
        private readonly QLTVDBContext _context;

        public BookCopyRepository() : this(new QLTVDBContext())
        {
        }

        public BookCopyRepository(QLTVDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<BookCopy> GetAll()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.BookCopies
                .Include(bc => bc.Book)
                .OrderBy(bc => bc.BookCopyId)
                .ToList();
        }

        public BookCopy? GetById(string bookCopyId)
        {
            if (string.IsNullOrWhiteSpace(bookCopyId))
                return null;

            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.BookCopies
                .Include(bc => bc.Book)
                .FirstOrDefault(bc => bc.BookCopyId == bookCopyId);
        }

        public List<BookCopy> GetByBookId(string bookId)
        {
            if (string.IsNullOrWhiteSpace(bookId))
                return new List<BookCopy>();

            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.BookCopies
                .Where(bc => bc.BookId == bookId)
                .OrderBy(bc => bc.BookCopyId)
                .ToList();
        }

        public int CountByBookId(string bookId)
        {
            if (string.IsNullOrWhiteSpace(bookId))
                return 0;

            return _context.BookCopies.Count(bc => bc.BookId == bookId);
        }

        public void Add(BookCopy bookCopy)
        {
            if (bookCopy == null)
                throw new ArgumentNullException(nameof(bookCopy));

            if (_context.BookCopies.Any(bc => bc.BookCopyId == bookCopy.BookCopyId))
            {
                throw new InvalidOperationException($"Mã bản sao '{bookCopy.BookCopyId}' đã tồn tại.");
            }

            if (!_context.Books.Any(b => b.BookId == bookCopy.BookId))
            {
                throw new InvalidOperationException("Đầu sách không tồn tại.");
            }

            try
            {
                bookCopy.Book = null;
                _context.BookCopies.Add(bookCopy);
                _context.SaveChanges();
            }
            catch
            {
                if (_context.Database.CurrentTransaction == null)
                {
                    _context.ChangeTracker.Clear();
                }
                throw;
            }
        }

        public void AddRange(IEnumerable<BookCopy> copies)
        {
            if (copies == null)
                throw new ArgumentNullException(nameof(copies));

            var copyList = copies.ToList();
            if (copyList.Count == 0)
                return;

            foreach (var copy in copyList)
            {
                if (_context.BookCopies.Any(bc => bc.BookCopyId == copy.BookCopyId))
                {
                    throw new InvalidOperationException($"Mã bản sao '{copy.BookCopyId}' đã tồn tại.");
                }
                if (!_context.Books.Any(b => b.BookId == copy.BookId))
                {
                    throw new InvalidOperationException($"Đầu sách '{copy.BookId}' không tồn tại.");
                }
                copy.Book = null;
            }

            try
            {
                _context.BookCopies.AddRange(copyList);
                _context.SaveChanges();
            }
            catch
            {
                if (_context.Database.CurrentTransaction == null)
                {
                    _context.ChangeTracker.Clear();
                }
                throw;
            }
        }

        public bool Update(BookCopy bookCopy)
        {
            if (bookCopy == null)
                throw new ArgumentNullException(nameof(bookCopy));

            if (_context.Database.CurrentTransaction == null &&
                _context.Entry(bookCopy).State == EntityState.Detached)
            {
                _context.ChangeTracker.Clear();
            }

            var existing = _context.BookCopies.Find(bookCopy.BookCopyId);
            if (existing == null)
                return false;

            if (!_context.Books.Any(b => b.BookId == bookCopy.BookId))
            {
                throw new InvalidOperationException("Đầu sách không tồn tại.");
            }

            existing.Status = bookCopy.Status;
            existing.Condition = bookCopy.Condition;
            existing.BookId = bookCopy.BookId;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(string bookCopyId)
        {
            if (string.IsNullOrWhiteSpace(bookCopyId))
                return false;

            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            var bookCopy = _context.BookCopies.Find(bookCopyId);
            if (bookCopy == null)
                return false;

            _context.BookCopies.Remove(bookCopy);
            _context.SaveChanges();
            return true;
        }
    }
}
