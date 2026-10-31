using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using QuanLyThuVienDAO.Entity;
using QuanLyThuVienDAO.Reposistory;
using QuanLyThuVienService.Reposistory;

namespace QuanLyThuVienDAO.Business
{
    /// <summary>
    /// Business Logic Layer cho thực thể BookCopy (Bản sao sách).
    /// Kiểm tra hợp lệ, tự động sinh mã bản sao kế tiếp và đồng bộ tổng số bản sao của đầu sách.
    /// </summary>
    public class BookCopyBuss
    {
        private readonly QLTVDBContext _context;
        private readonly BookCopyRepository _bookCopyDao;
        private readonly BookRepository _bookDao;
        private readonly DBReposistory _tran;

        public BookCopyBuss() : this(new QLTVDBContext())
        {
        }

        public BookCopyBuss(QLTVDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bookCopyDao = new BookCopyRepository(_context);
            _bookDao = new BookRepository(_context);
            _tran = new DBReposistory(_context);
        }

        public List<BookCopy> GetAll()
        {
            return _bookCopyDao.GetAll();
        }

        public BookCopy? GetById(string bookCopyId)
        {
            ValidateId(bookCopyId);

            return _bookCopyDao.GetById(bookCopyId.Trim());
        }

        public List<BookCopy> GetByBookId(string bookId)
        {
            ValidateBookId(bookId);

            return _bookCopyDao.GetByBookId(bookId.Trim());
        }

        /// <summary>
        /// Tự động sinh mã bản sao kế tiếp theo định dạng {BookId}.{STT:D3} (ví dụ: B006.006).
        /// </summary>
        public string GenerateNextBookCopyId(string bookId)
        {
            ValidateBookId(bookId);
            string normalizedBookId = bookId.Trim();

            var existingCopies = _bookCopyDao.GetByBookId(normalizedBookId);
            int maxIndex = 0;

            foreach (var bc in existingCopies)
            {
                int dotIdx = bc.BookCopyId.LastIndexOf('.');
                if (dotIdx >= 0 && dotIdx < bc.BookCopyId.Length - 1)
                {
                    string suffix = bc.BookCopyId.Substring(dotIdx + 1);
                    if (int.TryParse(suffix, out int num) && num > maxIndex)
                    {
                        maxIndex = num;
                    }
                }
            }

            return $"{normalizedBookId}.{(maxIndex + 1):D3}";
        }

        public void Add(BookCopy bookCopy)
        {
            NormalizeBookCopy(bookCopy);
            ValidateBookCopy(bookCopy);

            bool ownTran = _context.Database.CurrentTransaction == null;
            if (ownTran) _tran.BeginTransaction();

            try
            {
                _bookCopyDao.Add(bookCopy);
                SyncBookCopyCount(bookCopy.BookId);

                if (ownTran) _tran.ComitTransaction();
            }
            catch
            {
                if (ownTran) _tran.RolbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// Nhập bổ sung nhiều bản sao cùng lúc cho một đầu sách trong một giao dịch (Transaction).
        /// </summary>
        public List<BookCopy> AddMultipleCopies(
            string bookId,
            int quantity,
            string status = "Sẵn sàng",
            string condition = "Mới")
        {
            ValidateBookId(bookId);

            if (quantity <= 0)
            {
                throw new ArgumentException("Số lượng bản sao nhập thêm phải lớn hơn 0!");
            }

            if (quantity > 500)
            {
                throw new ArgumentException("Số lượng bản sao nhập thêm mỗi lần không vượt quá 500!");
            }

            string normalizedBookId = bookId.Trim();
            string normalizedStatus = Regex.Replace((status ?? string.Empty).Trim(), @"\s+", " ");
            string normalizedCondition = Regex.Replace((condition ?? string.Empty).Trim(), @"\s+", " ");

            if (string.IsNullOrWhiteSpace(normalizedStatus))
                normalizedStatus = "Sẵn sàng";
            if (string.IsNullOrWhiteSpace(normalizedCondition))
                normalizedCondition = "Mới";

            bool ownTran = _context.Database.CurrentTransaction == null;
            if (ownTran) _tran.BeginTransaction();

            try
            {
                var book = _bookDao.GetById(normalizedBookId);
                if (book == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy đầu sách '{normalizedBookId}'!");
                }

                var existingCopies = _bookCopyDao.GetByBookId(normalizedBookId);
                int maxIndex = 0;
                foreach (var bc in existingCopies)
                {
                    int dotIdx = bc.BookCopyId.LastIndexOf('.');
                    if (dotIdx >= 0 && dotIdx < bc.BookCopyId.Length - 1)
                    {
                        if (int.TryParse(bc.BookCopyId.Substring(dotIdx + 1), out int num) && num > maxIndex)
                        {
                            maxIndex = num;
                        }
                    }
                }

                var createdCopies = new List<BookCopy>();
                for (int i = 1; i <= quantity; i++)
                {
                    var newCopy = new BookCopy
                    {
                        BookCopyId = $"{normalizedBookId}.{(maxIndex + i):D3}",
                        BookId = normalizedBookId,
                        Status = normalizedStatus,
                        Condition = normalizedCondition
                    };
                    ValidateBookCopy(newCopy);
                    createdCopies.Add(newCopy);
                }

                _bookCopyDao.AddRange(createdCopies);
                SyncBookCopyCount(normalizedBookId);

                if (ownTran) _tran.ComitTransaction();
                return createdCopies;
            }
            catch
            {
                if (ownTran) _tran.RolbackTransaction();
                throw;
            }
        }

        public void Update(BookCopy bookCopy)
        {
            NormalizeBookCopy(bookCopy);
            ValidateBookCopy(bookCopy);

            if (!_bookCopyDao.Update(bookCopy))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy bản sao sách cần cập nhật!");
            }
        }

        public void Delete(string bookCopyId)
        {
            ValidateId(bookCopyId);
            string normalizedCopyId = bookCopyId.Trim();

            var existing = _bookCopyDao.GetById(normalizedCopyId);
            if (existing == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy bản sao sách cần xóa!");
            }

            if (string.Equals(existing.Status, "Đang mượn", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Bản sao '{normalizedCopyId}' đang ở trạng thái 'Đang mượn' nên không thể xóa!");
            }

            string parentBookId = existing.BookId;

            bool ownTran = _context.Database.CurrentTransaction == null;
            if (ownTran) _tran.BeginTransaction();

            try
            {
                if (!_bookCopyDao.Delete(normalizedCopyId))
                {
                    throw new InvalidOperationException(
                        "Không tìm thấy bản sao sách cần xóa!");
                }

                SyncBookCopyCount(parentBookId);

                if (ownTran) _tran.ComitTransaction();
            }
            catch
            {
                if (ownTran) _tran.RolbackTransaction();
                throw;
            }
        }

        private void SyncBookCopyCount(string bookId)
        {
            var book = _bookDao.GetById(bookId);
            if (book != null)
            {
                book.NumberOfCopies = _bookCopyDao.CountByBookId(bookId);
                _bookDao.Update(book);
            }
        }

        private static void NormalizeBookCopy(BookCopy? bookCopy)
        {
            if (bookCopy == null)
                return;

            bookCopy.BookCopyId = (bookCopy.BookCopyId ?? string.Empty).Trim();
            bookCopy.BookId = (bookCopy.BookId ?? string.Empty).Trim();
            bookCopy.Status = Regex.Replace((bookCopy.Status ?? string.Empty).Trim(), @"\s+", " ");
            bookCopy.Condition = Regex.Replace((bookCopy.Condition ?? string.Empty).Trim(), @"\s+", " ");
        }

        private static void ValidateBookId(string bookId)
        {
            if (string.IsNullOrWhiteSpace(bookId))
            {
                throw new ArgumentException("Mã sách không được để trống!");
            }

            if (bookId.Trim().Length > 20)
            {
                throw new ArgumentException("Mã sách không được vượt quá 20 ký tự!");
            }
        }

        private static void ValidateId(string bookCopyId)
        {
            if (string.IsNullOrWhiteSpace(bookCopyId))
            {
                throw new ArgumentException(
                    "Mã bản sao sách không được để trống!");
            }

            if (bookCopyId.Trim().Length > 20)
            {
                throw new ArgumentException(
                    "Mã bản sao sách không được vượt quá 20 ký tự!");
            }
        }

        private static void ValidateBookCopy(BookCopy bookCopy)
        {
            if (bookCopy == null)
                throw new ArgumentNullException(nameof(bookCopy));

            ValidateId(bookCopy.BookCopyId);

            if (string.IsNullOrWhiteSpace(bookCopy.BookId))
            {
                throw new ArgumentException(
                    "Vui lòng chọn sách cho bản sao!");
            }

            if (bookCopy.BookId.Length > 20)
            {
                throw new ArgumentException(
                    "Mã sách không được vượt quá 20 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(bookCopy.Status))
            {
                throw new ArgumentException(
                    "Trạng thái bản sao sách không được để trống!");
            }

            if (bookCopy.Status.Length > 20)
            {
                throw new ArgumentException(
                    "Trạng thái không được vượt quá 20 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(bookCopy.Condition))
            {
                throw new ArgumentException(
                    "Tình trạng bản sao sách không được để trống!");
            }

            if (bookCopy.Condition.Length > 30)
            {
                throw new ArgumentException(
                    "Tình trạng không được vượt quá 30 ký tự!");
            }
        }
    }
}
