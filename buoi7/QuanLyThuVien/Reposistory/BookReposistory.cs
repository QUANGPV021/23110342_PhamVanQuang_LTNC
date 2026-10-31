using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVienDAO.Reposistory
{
    /// <summary>
    /// Data Access Layer (Repository) cho thực thể Book (Đầu sách).
    /// Tích hợp Eager Loading (.Include) cho Category và BookCopies, đồng thời hỗ trợ Transaction.
    /// </summary>
    public class BookRepository
    {
        private readonly QLTVDBContext _context;

        public BookRepository() : this(new QLTVDBContext())
        {
        }

        public BookRepository(QLTVDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Book> getBooks()
        {
            return GetAll();
        }

        public List<Book> getBookbyCategory(string cid)
        {
            return GetByCategoryId(cid);
        }

        public void AddBoo(Book b)
        {
            Add(b);
        }

        public bool updateBook(Book b)
        {
            return Update(b);
        }

        public bool deleteBook(string bid)
        {
            return Delete(bid);
        }

        public List<Book> GetAll()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookCopies)
                .OrderByDescending(b => b.Title)
                .ToList();
        }

        public Book? GetById(string bookId)
        {
            if (string.IsNullOrWhiteSpace(bookId))
                return null;

            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookCopies)
                .FirstOrDefault(b => b.BookId == bookId);
        }

        public List<Book> GetByCategoryId(string categoryId)
        {
            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookCopies)
                .Where(b => b.CategoryId == categoryId)
                .OrderByDescending(b => b.Title)
                .ToList();
        }

        private string genBookCopyId(string bookId, int stt)
        {
            var v = stt.ToString();
            while (v.Length < 3)
                v = "0" + v;
            return bookId + "." + v;
        }

        public void Add(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            var ownTransaction = _context.Database.CurrentTransaction == null
                ? _context.Database.BeginTransaction()
                : null;

            try
            {
                // Kiểm tra trùng mã sách
                var existing = _context.Books.Find(book.BookId);
                if (existing != null)
                {
                    throw new InvalidOperationException($"Mã đầu sách '{book.BookId}' đã tồn tại!");
                }

                // Kiểm tra thể loại tồn tại
                if (!_context.Categories.Any(c => c.CategoryId == book.CategoryId))
                {
                    throw new InvalidOperationException("Thể loại sách không tồn tại!");
                }

                // Tự động khởi tạo danh sách bản sao từ 001..N nếu NumberOfCopies > 0
                if (book.NumberOfCopies > 0 && book.BookCopies.Count == 0)
                {
                    for (int i = 1; i <= book.NumberOfCopies; i++)
                    {
                        var bc = new BookCopy
                        {
                            BookCopyId = genBookCopyId(book.BookId, i),
                            BookId = book.BookId,
                            Status = "Sẵn sàng",
                            Condition = "Mới"
                        };
                        book.BookCopies.Add(bc);
                    }
                }
                else if (book.BookCopies.Count > 0)
                {
                    book.NumberOfCopies = book.BookCopies.Count;
                }

                book.Category = null;
                _context.Books.Add(book);
                _context.SaveChanges();
                ownTransaction?.Commit();
            }
            catch
            {
                ownTransaction?.Rollback();
                if (ownTransaction != null)
                {
                    _context.ChangeTracker.Clear();
                }
                throw;
            }
            finally
            {
                ownTransaction?.Dispose();
            }
        }

        public bool Update(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (_context.Database.CurrentTransaction == null &&
                _context.Entry(book).State == EntityState.Detached)
            {
                _context.ChangeTracker.Clear();
            }

            var existing = _context.Books.Find(book.BookId);
            if (existing == null)
                return false;

            if (!_context.Categories.Any(c => c.CategoryId == book.CategoryId))
            {
                throw new InvalidOperationException("Thể loại không tồn tại.");
            }

            existing.Title = book.Title;
            existing.Publisher = book.Publisher;
            existing.Price = book.Price;
            existing.NumberOfCopies = book.NumberOfCopies;
            existing.Author = book.Author;
            existing.PublicationYear = book.PublicationYear;
            existing.CategoryId = book.CategoryId;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(string bookId)
        {
            if (string.IsNullOrWhiteSpace(bookId))
                return false;

            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            var book = _context.Books.Find(bookId);
            if (book == null)
                return false;

            if (_context.BookCopies.Any(bc => bc.BookId == bookId))
            {
                throw new InvalidOperationException("Không thể xóa đầu sách đang có bản sao. Vui lòng xóa hết các bản sao trước!");
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }
    }
}
