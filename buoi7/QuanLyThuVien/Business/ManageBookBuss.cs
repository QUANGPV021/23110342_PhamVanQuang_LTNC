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
    /// Business Logic Layer tổng hợp các nghiệp vụ cho giao diện "Quản lý đầu sách và bản sao".
    /// Xử lý tra cứu, thêm/sửa/xóa đầu sách, nhập bổ sung bản sao theo lô, cập nhật/xóa bản sao và quản lý Transaction.
    /// </summary>
    public class ManageBookBuss
    {
        private readonly QLTVDBContext _context;
        private readonly BookRepository _bookDao;
        private readonly DBReposistory _tran;
        private readonly CategoryRepository _cateDao;
        private readonly BookCopyRepository _bookCopyDao;
        private readonly BookCopyBuss _bookCopyBuss;

        public QLTVDBContext Context => _context;

        public ManageBookBuss() : this(new QLTVDBContext())
        {
        }

        public ManageBookBuss(QLTVDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bookDao = new BookRepository(_context);
            _tran = new DBReposistory(_context);
            _cateDao = new CategoryRepository(_context);
            _bookCopyDao = new BookCopyRepository(_context);
            _bookCopyBuss = new BookCopyBuss(_context);
        }

        public List<Book> getBooks()
        {
            return _bookDao.GetAll();
        }

        public List<Category> getCategories()
        {
            return _cateDao.GetAll();
        }

        public void AddBook(Book b)
        {
            Add(b);
        }

        /// <summary>
        /// Nhập lô sách không dùng Transaction tường minh (phương thức minh họa của giảng viên, đã chuẩn hóa dữ liệu).
        /// </summary>
        public void ImportBill1(List<Book> books)
        {
            if (books == null || books.Count == 0)
                return;

            foreach (var b in books)
            {
                NormalizeBook(b);
                ValidateBook(b);

                var existingBook = _bookDao.GetById(b.BookId);
                if (existingBook == null)
                {
                    _bookDao.Add(b); // Thêm mới đầu sách kèm các bản sao ban đầu
                }
                else
                {
                    int addCount = b.NumberOfCopies > 0 ? b.NumberOfCopies : 1;
                    _bookCopyBuss.AddMultipleCopies(b.BookId, addCount, "Sẵn sàng", "Mới");
                }
            }
        }

        /// <summary>
        /// Nhập lô sách/bản sao có kiểm soát Transaction toàn vẹn (All-or-Nothing).
        /// Nếu đầu sách chưa có thì thêm mới đầu sách + bản sao; nếu đã có thì bổ sung thêm bản sao và đồng bộ số lượng.
        /// </summary>
        public void ImportBill(List<Book> books)
        {
            if (books == null)
                throw new ArgumentNullException(nameof(books));

            if (books.Count == 0)
                throw new ArgumentException("Danh sách sách nhập không được để trống!");

            _tran.BeginTransaction();
            try
            {
                foreach (var b in books)
                {
                    NormalizeBook(b);
                    ValidateBook(b);

                    var existingBook = _bookDao.GetById(b.BookId);
                    if (existingBook == null)
                    {
                        _bookDao.Add(b);
                    }
                    else
                    {
                        int addCount = b.NumberOfCopies > 0 ? b.NumberOfCopies : 1;
                        _bookCopyBuss.AddMultipleCopies(b.BookId, addCount, "Sẵn sàng", "Mới");
                    }
                }
                _tran.ComitTransaction();
            }
            catch
            {
                _tran.RolbackTransaction();
                throw;
            }
        }

        public List<Book> GetAll()
        {
            return _bookDao.GetAll();
        }

        public Book? GetById(string id)
        {
            ValidateBookId(id);

            return _bookDao.GetById(id.Trim());
        }

        /// <summary>
        /// Tìm kiếm và lọc đầu sách theo từ khóa (Mã sách, Tên sách, Tác giả, NXB) và Mã thể loại.
        /// </summary>
        public List<Book> Search(string? keyword, string? categoryId)
        {
            IEnumerable<Book> books = _bookDao.GetAll();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string searchText = keyword.Trim();

                books = books.Where(b =>
                    b.BookId.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || b.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || b.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || b.Publisher.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(categoryId)
                && !string.Equals(categoryId.Trim(), "ALL", StringComparison.OrdinalIgnoreCase))
            {
                string category = categoryId.Trim();
                books = books.Where(b => b.CategoryId == category);
            }

            return books.ToList();
        }

        public void Add(Book book)
        {
            NormalizeBook(book);
            ValidateBook(book);

            var v = book.IsInValid();
            if (v == null)
            {
                _bookDao.Add(book);
            }
            else
            {
                string errors = string.Join(Environment.NewLine, v.Select(r => r.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }

        public void Update(Book book)
        {
            NormalizeBook(book);
            ValidateBook(book);

            // Đảm bảo số bản sao luôn đồng bộ chính xác với số bản sao thực tế trong CSDL
            int actualCopies = _bookCopyDao.CountByBookId(book.BookId);
            book.NumberOfCopies = actualCopies;

            if (!_bookDao.Update(book))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy đầu sách cần cập nhật!");
            }
        }

        public void Delete(string bookId)
        {
            ValidateBookId(bookId);

            if (!_bookDao.Delete(bookId.Trim()))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy đầu sách cần xóa!");
            }
        }

        public void UpdateNumCopy(string bookId)
        {
            ValidateBookId(bookId);
            string normalizedId = bookId.Trim();

            var book = _bookDao.GetById(normalizedId);
            if (book == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy sách!");
            }

            book.NumberOfCopies = book.BookCopies.Count;

            if (!_bookDao.Update(book))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy sách cần cập nhật số bản sao!");
            }
        }

        #region Nghiệp vụ quản lý Bản sao trực tiếp phục vụ Giao diện

        public List<BookCopy> GetBookCopies(string bookId)
        {
            return _bookCopyBuss.GetByBookId(bookId);
        }

        public string GenerateNextCopyId(string bookId)
        {
            return _bookCopyBuss.GenerateNextBookCopyId(bookId);
        }

        /// <summary>
        /// Phục vụ chức năng "NHẬP BỔ SUNG BẢN SAO" và "Nhập thêm bản sao" trên giao diện.
        /// </summary>
        public List<BookCopy> ImportAdditionalCopies(
            string bookId,
            int additionalCount,
            string status = "Sẵn sàng",
            string condition = "Mới")
        {
            return _bookCopyBuss.AddMultipleCopies(bookId, additionalCount, status, condition);
        }

        public void AddBookCopy(BookCopy bookCopy)
        {
            _bookCopyBuss.Add(bookCopy);
        }

        public void UpdateBookCopy(BookCopy bookCopy)
        {
            _bookCopyBuss.Update(bookCopy);
        }

        public void DeleteBookCopy(string bookCopyId)
        {
            _bookCopyBuss.Delete(bookCopyId);
        }

        #endregion

        private static void NormalizeBook(Book? book)
        {
            if (book == null)
                return;

            book.BookId = (book.BookId ?? string.Empty).Trim();
            book.Title = Regex.Replace((book.Title ?? string.Empty).Trim(), @"\s+", " ");
            book.Author = Regex.Replace((book.Author ?? string.Empty).Trim(), @"\s+", " ");
            book.Publisher = Regex.Replace((book.Publisher ?? string.Empty).Trim(), @"\s+", " ");
            book.CategoryId = (book.CategoryId ?? string.Empty).Trim();
        }

        private static void ValidateBookId(string bookId)
        {
            if (string.IsNullOrWhiteSpace(bookId))
            {
                throw new ArgumentException(
                    "Mã sách không được để trống!");
            }

            if (bookId.Trim().Length > 20)
            {
                throw new ArgumentException(
                    "Mã sách không được vượt quá 20 ký tự!");
            }
        }

        private static void ValidateBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            ValidateBookId(book.BookId);

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException(
                    "Tên sách không được để trống!");

            if (book.Title.Length > 200)
                throw new ArgumentException(
                    "Tên sách không được vượt quá 200 ký tự!");

            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException(
                    "Tác giả không được để trống!");

            if (book.Author.Length > 150)
                throw new ArgumentException(
                    "Tên tác giả không được vượt quá 150 ký tự!");

            if (string.IsNullOrWhiteSpace(book.Publisher))
                throw new ArgumentException(
                    "Nhà xuất bản không được để trống!");

            if (book.Publisher.Length > 150)
                throw new ArgumentException(
                    "Nhà xuất bản không được vượt quá 150 ký tự!");

            if (string.IsNullOrWhiteSpace(book.CategoryId))
                throw new ArgumentException(
                    "Vui lòng chọn thể loại cho sách!");

            if (book.CategoryId.Length > 10)
                throw new ArgumentException(
                    "Mã thể loại không được vượt quá 10 ký tự!");

            if (book.Price < 0)
                throw new ArgumentException(
                    "Giá sách không được âm!");

            if (book.NumberOfCopies < 0)
                throw new ArgumentException(
                    "Số bản sao không được âm!");

            if (book.PublicationYear < 1000
                || book.PublicationYear > 9999)
            {
                throw new ArgumentException(
                    "Năm xuất bản phải từ 1000 đến 9999!");
            }
        }
    }
}
