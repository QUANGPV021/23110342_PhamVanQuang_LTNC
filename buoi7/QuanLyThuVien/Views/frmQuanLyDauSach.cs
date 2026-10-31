using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVienDAO.Business;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVienDAO.Views
{
    /// <summary>
    /// Giao diện chính "Quản lý đầu sách và bản sao" (Presentation Layer).
    /// Tương tác hoàn toàn thông qua tầng Business (ManageBookBuss, BookCopyBuss, CategoryBuss)
    /// và bọc ngoại lệ an toàn bằng try-catch.
    /// </summary>
    public partial class frmQuanLyDauSach : Form
    {
        private readonly QLTVDBContext _sharedContext;
        private readonly ManageBookBuss _bookBuss;
        private readonly BookCopyBuss _bookCopyBuss;
        private readonly CategoryBuss _categoryBuss;

        private List<Book> _currentBooks = new();
        private List<BookCopy> _currentCopies = new();
        private List<Category> _categories = new();

        private bool _isInitializing = true;
        private bool _isBindingGrid = false;

        public frmQuanLyDauSach()
        {
            InitializeComponent();

            _sharedContext = new QLTVDBContext();
            _bookBuss = new ManageBookBuss(_sharedContext);
            _bookCopyBuss = new BookCopyBuss(_sharedContext);
            _categoryBuss = new CategoryBuss(_sharedContext);

            SetupDataGridViews();
            RegisterEvents();
        }

        #region Cấu hình DataGridView & Sự kiện

        private void SetupDataGridViews()
        {
            // 1. Cột cho bảng Danh sách đầu sách
            dgvDauSach.Columns.Clear();
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colBookId",
                HeaderText = "Mã sách",
                DataPropertyName = nameof(Book.BookId),
                Width = 72
            });
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTitle",
                HeaderText = "Tên đầu sách",
                DataPropertyName = nameof(Book.Title),
                Width = 195
            });
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryName",
                HeaderText = "Thể loại",
                DataPropertyName = nameof(Book.CategoryName),
                Width = 115
            });
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAuthor",
                HeaderText = "Tác giả",
                DataPropertyName = nameof(Book.Author),
                Width = 130
            });
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPublisher",
                HeaderText = "Nhà xuất bản",
                DataPropertyName = nameof(Book.Publisher),
                Width = 130
            });
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrice",
                HeaderText = "Giá sách",
                DataPropertyName = nameof(Book.Price),
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPublicationYear",
                HeaderText = "Năm XB",
                DataPropertyName = nameof(Book.PublicationYear),
                Width = 75,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvDauSach.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNumberOfCopies",
                HeaderText = "Số bản sao",
                DataPropertyName = nameof(Book.NumberOfCopies),
                Width = 85,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // 2. Cột cho bảng Danh sách bản sao
            dgvBanSao.Columns.Clear();
            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colBookCopyId",
                HeaderText = "Mã bản sao",
                DataPropertyName = nameof(BookCopy.BookCopyId),
                Width = 190
            });
            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Trạng thái",
                DataPropertyName = nameof(BookCopy.Status),
                Width = 210
            });
            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCondition",
                HeaderText = "Tình trạng vật lý",
                DataPropertyName = nameof(BookCopy.Condition),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 160
            });
        }

        private void RegisterEvents()
        {
            this.Load += frmQuanLyDauSach_Load;
            this.FormClosed += (s, e) => _sharedContext.Dispose();

            // Tra cứu & Lọc
            this.btnTimKiem.Click += btnTimKiem_Click;
            this.btnLamMoi.Click += btnLamMoi_Click;
            this.txtTuKhoa.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    PerformSearch();
                }
            };
            this.cboLocTheLoai.SelectedIndexChanged += (s, e) =>
            {
                if (!_isInitializing)
                {
                    PerformSearch();
                }
            };

            // Chọn dòng trên lưới Đầu sách
            this.dgvDauSach.SelectionChanged += dgvDauSach_SelectionChanged;
            this.dgvDauSach.CellClick += dgvDauSach_CellClick;

            // Chọn / Double-click dòng trên lưới Bản sao
            this.dgvBanSao.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    OpenCapNhatBanSaoDialog();
                }
            };

            // Nhóm nút Đầu sách
            this.btnThemDauSach.Click += btnThemDauSach_Click;
            this.btnLuuChinhSua.Click += btnLuuChinhSua_Click;
            this.btnXoaDauSach.Click += btnXoaDauSach_Click;
            this.btnLamTrong.Click += btnLamTrong_Click;
            this.btnNhapBoSungBanSao.Click += btnNhapBoSungBanSao_Click;

            // Nhóm nút Bản sao
            this.btnNhapThemBanSao.Click += btnNhapThemBanSao_Click;
            this.btnCapNhatBanSao.Click += btnCapNhatBanSao_Click;
            this.btnXoaBanSao.Click += btnXoaBanSao_Click;
        }

        #endregion

        #region Khởi tạo & Tải dữ liệu

        private void frmQuanLyDauSach_Load(object? sender, EventArgs e)
        {
            _isInitializing = true;
            try
            {
                // Tự động áp dụng Migration / khởi tạo CSDL nếu chưa tồn tại
                try
                {
                    _sharedContext.Database.Migrate();
                }
                catch
                {
                    // Trường hợp CSDL đã được tạo sẵn bằng SQL thủ công
                }

                LoadCategories();
                LoadBooksAndSelect();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Chưa thể kết nối cơ sở dữ liệu SQL Server.";
                MessageBox.Show(
                    "Không thể kết nối hoặc tải dữ liệu từ SQL Server:\n" + ex.Message +
                    "\n\nHướng dẫn: Hãy kiểm tra chuỗi kết nối trong QLTVDBContext.cs hoặc chạy lệnh 'Update-Database' / file 'QLTV_EFCore.sql'.",
                    "Lỗi kết nối CSDL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _isInitializing = false;
            }
        }

        private void LoadCategories()
        {
            _categories = _categoryBuss.GetAll();

            // ComboBox Thể loại bên trái (Thông tin đầu sách)
            cboTheLoai.DataSource = _categories.ToList();
            cboTheLoai.DisplayMember = nameof(Category.Description);
            cboTheLoai.ValueMember = nameof(Category.CategoryId);

            // ComboBox Lọc thể loại trên thanh tìm kiếm
            var filterList = new List<Category>
            {
                new Category { CategoryId = string.Empty, Description = "-- Tất cả thể loại --" }
            };
            filterList.AddRange(_categories);

            cboLocTheLoai.DataSource = filterList;
            cboLocTheLoai.DisplayMember = nameof(Category.Description);
            cboLocTheLoai.ValueMember = nameof(Category.CategoryId);
            cboLocTheLoai.SelectedIndex = 0;
        }

        private void LoadBooksAndSelect(string? selectBookId = null, string? customStatusPrefix = null)
        {
            string keyword = txtTuKhoa.Text;
            string? selectedCatId = cboLocTheLoai.SelectedValue?.ToString();

            _currentBooks = _bookBuss.Search(keyword, selectedCatId);
            BindBooksGrid(_currentBooks, selectBookId);

            string prefix = string.IsNullOrWhiteSpace(customStatusPrefix)
                ? $"Đã tải {_currentBooks.Count} đầu sách"
                : customStatusPrefix;
            UpdateStatus($"{prefix} lúc {DateTime.Now:HH:mm:ss}");
        }

        private void BindBooksGrid(List<Book> books, string? selectBookId = null)
        {
            _isBindingGrid = true;
            try
            {
                dgvDauSach.DataSource = null;
                dgvDauSach.DataSource = books;

                if (books.Count == 0)
                {
                    ClearBookInputs(enableInitialCopies: true);
                    BindCopiesGrid(null, new List<BookCopy>());
                    return;
                }

                int targetIndex = 0;
                if (!string.IsNullOrWhiteSpace(selectBookId))
                {
                    int found = books.FindIndex(b =>
                        string.Equals(b.BookId, selectBookId, StringComparison.OrdinalIgnoreCase));
                    if (found >= 0)
                    {
                        targetIndex = found;
                    }
                }

                dgvDauSach.ClearSelection();
                dgvDauSach.Rows[targetIndex].Selected = true;
                if (dgvDauSach.Columns.Count > 0)
                {
                    dgvDauSach.CurrentCell = dgvDauSach.Rows[targetIndex].Cells[0];
                }

                DisplaySelectedBook(books[targetIndex]);
            }
            finally
            {
                _isBindingGrid = false;
            }
        }

        private void DisplaySelectedBook(Book? book)
        {
            errorProvider.Clear();

            if (book == null)
            {
                ClearBookInputs(enableInitialCopies: true);
                BindCopiesGrid(null, new List<BookCopy>());
                return;
            }

            txtMaSach.Text = book.BookId;
            txtTenSach.Text = book.Title;
            txtTacGia.Text = book.Author;
            txtNhaXuatBan.Text = book.Publisher;
            cboTheLoai.SelectedValue = book.CategoryId;
            txtGiaSach.Text = decimal.Truncate(book.Price) == book.Price
                ? ((long)book.Price).ToString(CultureInfo.InvariantCulture)
                : book.Price.ToString("0.##", CultureInfo.InvariantCulture);
            txtNamXuatBan.Text = book.PublicationYear.ToString(CultureInfo.InvariantCulture);

            // Tải danh sách bản sao của đầu sách đang chọn
            _currentCopies = _bookCopyBuss.GetByBookId(book.BookId);
            txtSoBanSao.Text = _currentCopies.Count.ToString(CultureInfo.InvariantCulture);
            txtSoBanSao.ReadOnly = true;
            txtSoBanSao.BackColor = Color.FromArgb(241, 245, 249);

            BindCopiesGrid(book.BookId, _currentCopies);
        }

        private void BindCopiesGrid(string? bookId, List<BookCopy> copies, string? selectCopyId = null)
        {
            dgvBanSao.DataSource = null;
            dgvBanSao.DataSource = copies;

            if (string.IsNullOrWhiteSpace(bookId))
            {
                grpDanhSachBanSao.Text = "Danh sách bản sao";
            }
            else
            {
                grpDanhSachBanSao.Text = $"Danh sách bản sao của sách {bookId} ({copies.Count} bản sao)";
            }

            if (copies.Count > 0)
            {
                int idx = 0;
                if (!string.IsNullOrWhiteSpace(selectCopyId))
                {
                    int found = copies.FindIndex(c =>
                        string.Equals(c.BookCopyId, selectCopyId, StringComparison.OrdinalIgnoreCase));
                    if (found >= 0) idx = found;
                }

                dgvBanSao.ClearSelection();
                dgvBanSao.Rows[idx].Selected = true;
                if (dgvBanSao.Columns.Count > 0)
                {
                    dgvBanSao.CurrentCell = dgvBanSao.Rows[idx].Cells[0];
                }
            }
        }

        private Book? GetSelectedBookFromGrid()
        {
            if (dgvDauSach.CurrentRow?.DataBoundItem is Book b)
                return b;
            return null;
        }

        private BookCopy? GetSelectedBookCopyFromGrid()
        {
            if (dgvBanSao.CurrentRow?.DataBoundItem is BookCopy bc)
                return bc;
            return null;
        }

        private void ClearBookInputs(bool enableInitialCopies)
        {
            errorProvider.Clear();
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtNhaXuatBan.Clear();
            if (cboTheLoai.Items.Count > 0)
            {
                cboTheLoai.SelectedIndex = 0;
            }
            txtGiaSach.Text = "0";
            txtNamXuatBan.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            txtSoBanSao.Text = "0";
            txtSoBanSao.ReadOnly = !enableInitialCopies;
            txtSoBanSao.BackColor = enableInitialCopies
                ? Color.White
                : Color.FromArgb(241, 245, 249);
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = message;
        }

        #endregion

        #region Xử lý sự kiện Tìm kiếm & Chọn dòng

        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            try
            {
                LoadBooksAndSelect(
                    selectBookId: null,
                    customStatusPrefix: null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm đầu sách: " + ex.Message,
                    "Lỗi tra cứu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            try
            {
                _isInitializing = true;
                txtTuKhoa.Clear();
                if (cboLocTheLoai.Items.Count > 0)
                {
                    cboLocTheLoai.SelectedIndex = 0;
                }
                _isInitializing = false;

                LoadBooksAndSelect();
            }
            catch (Exception ex)
            {
                _isInitializing = false;
                MessageBox.Show("Lỗi khi làm mới danh sách: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDauSach_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isInitializing || _isBindingGrid)
                return;

            try
            {
                var selected = GetSelectedBookFromGrid();
                if (selected != null)
                {
                    DisplaySelectedBook(selected);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Lỗi khi hiển thị chi tiết sách: " + ex.Message);
            }
        }

        private void dgvDauSach_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isBindingGrid)
                return;

            try
            {
                var selected = GetSelectedBookFromGrid();
                if (selected != null)
                {
                    DisplaySelectedBook(selected);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Lỗi khi chọn đầu sách: " + ex.Message);
            }
        }

        #endregion

        #region Xử lý sự kiện CRUD Đầu sách

        private bool TryBuildBookFromInputs(out Book book, bool isAddingNew)
        {
            errorProvider.Clear();
            book = new Book();
            bool isValid = true;

            string bookId = txtMaSach.Text.Trim();
            if (string.IsNullOrWhiteSpace(bookId))
            {
                errorProvider.SetError(txtMaSach, "Mã sách không được để trống!");
                isValid = false;
            }

            string title = txtTenSach.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                errorProvider.SetError(txtTenSach, "Tên sách không được để trống!");
                isValid = false;
            }

            string author = txtTacGia.Text.Trim();
            if (string.IsNullOrWhiteSpace(author))
            {
                errorProvider.SetError(txtTacGia, "Tác giả không được để trống!");
                isValid = false;
            }

            string publisher = txtNhaXuatBan.Text.Trim();
            if (string.IsNullOrWhiteSpace(publisher))
            {
                errorProvider.SetError(txtNhaXuatBan, "Nhà xuất bản không được để trống!");
                isValid = false;
            }

            string categoryId = cboTheLoai.SelectedValue?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                errorProvider.SetError(cboTheLoai, "Vui lòng chọn thể loại!");
                isValid = false;
            }

            string rawPrice = txtGiaSach.Text.Trim().Replace(",", "");
            if (!decimal.TryParse(rawPrice, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price) || price < 0)
            {
                errorProvider.SetError(txtGiaSach, "Giá sách phải là số không âm hợp lệ!");
                isValid = false;
            }

            if (!int.TryParse(txtNamXuatBan.Text.Trim(), out int pubYear) || pubYear < 1000 || pubYear > 9999)
            {
                errorProvider.SetError(txtNamXuatBan, "Năm xuất bản phải từ 1000 đến 9999!");
                isValid = false;
            }

            int numCopies = 0;
            if (!string.IsNullOrWhiteSpace(txtSoBanSao.Text))
            {
                if (!int.TryParse(txtSoBanSao.Text.Trim(), out numCopies) || numCopies < 0)
                {
                    errorProvider.SetError(txtSoBanSao, "Số bản sao phải là số nguyên không âm!");
                    isValid = false;
                }
            }

            if (!isValid)
            {
                MessageBox.Show(
                    "Vui lòng kiểm tra lại các thông tin đầu sách được đánh dấu cảnh báo!",
                    "Dữ liệu chưa hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            book = new Book
            {
                BookId = bookId,
                Title = title,
                Author = author,
                Publisher = publisher,
                CategoryId = categoryId,
                Price = price,
                PublicationYear = pubYear,
                NumberOfCopies = isAddingNew ? numCopies : 0
            };
            return true;
        }

        private void btnThemDauSach_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!TryBuildBookFromInputs(out Book newBook, isAddingNew: true))
                    return;

                _bookBuss.Add(newBook);

                // Reset bộ lọc nếu sách mới không nằm trong bộ lọc hiện tại
                _isInitializing = true;
                txtTuKhoa.Clear();
                cboLocTheLoai.SelectedIndex = 0;
                _isInitializing = false;

                LoadBooksAndSelect(
                    selectBookId: newBook.BookId,
                    customStatusPrefix: $"Đã thêm đầu sách '{newBook.BookId}' ({newBook.NumberOfCopies} bản sao)");

                MessageBox.Show(
                    $"Thêm đầu sách '{newBook.BookId} - {newBook.Title}' thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _isInitializing = false;
                MessageBox.Show(ex.Message, "Không thể thêm đầu sách",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLuuChinhSua_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!TryBuildBookFromInputs(out Book editedBook, isAddingNew: false))
                    return;

                _bookBuss.Update(editedBook);

                LoadBooksAndSelect(
                    selectBookId: editedBook.BookId,
                    customStatusPrefix: $"Đã lưu chỉnh sửa đầu sách '{editedBook.BookId}'");

                MessageBox.Show(
                    $"Cập nhật thông tin đầu sách '{editedBook.BookId}' thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể cập nhật đầu sách",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoaDauSach_Click(object? sender, EventArgs e)
        {
            try
            {
                string bookId = txtMaSach.Text.Trim();
                if (string.IsNullOrWhiteSpace(bookId))
                {
                    MessageBox.Show("Vui lòng chọn đầu sách cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa đầu sách '{bookId}' không?",
                    "Xác nhận xóa đầu sách",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                _bookBuss.Delete(bookId);

                LoadBooksAndSelect(
                    selectBookId: null,
                    customStatusPrefix: $"Đã xóa đầu sách '{bookId}'");

                MessageBox.Show(
                    $"Đã xóa đầu sách '{bookId}' thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa đầu sách",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLamTrong_Click(object? sender, EventArgs e)
        {
            dgvDauSach.ClearSelection();
            ClearBookInputs(enableInitialCopies: true);
            BindCopiesGrid(null, new List<BookCopy>());
            txtMaSach.Focus();
            UpdateStatus($"Đã làm trống biểu mẫu để nhập đầu sách mới lúc {DateTime.Now:HH:mm:ss}");
        }

        #endregion

        #region Xử lý sự kiện Quản lý Bản sao

        private void btnNhapBoSungBanSao_Click(object? sender, EventArgs e)
        {
            OpenNhapBoSungDialog(defaultQuantity: 2);
        }

        private void btnNhapThemBanSao_Click(object? sender, EventArgs e)
        {
            OpenNhapBoSungDialog(defaultQuantity: 1);
        }

        private void OpenNhapBoSungDialog(int defaultQuantity)
        {
            try
            {
                string bookId = txtMaSach.Text.Trim();
                if (string.IsNullOrWhiteSpace(bookId))
                {
                    MessageBox.Show("Vui lòng chọn một đầu sách trước khi nhập bổ sung bản sao!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var book = _bookBuss.GetById(bookId);
                if (book == null)
                {
                    MessageBox.Show($"Không tìm thấy đầu sách '{bookId}' trong cơ sở dữ liệu!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string nextCopyId = _bookBuss.GenerateNextCopyId(book.BookId);
                using var dlg = new frmNhapBoSungBanSao(book, nextCopyId, defaultQuantity);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var addedCopies = _bookBuss.ImportAdditionalCopies(
                        book.BookId,
                        dlg.SoLuongNhapThem,
                        dlg.TrangThaiChon,
                        dlg.TinhTrangChon);

                    string? lastAddedId = addedCopies.LastOrDefault()?.BookCopyId;
                    LoadBooksAndSelect(
                        selectBookId: book.BookId,
                        customStatusPrefix: $"Đã nhập bổ sung {addedCopies.Count} bản sao cho sách {book.BookId}");

                    if (!string.IsNullOrWhiteSpace(lastAddedId))
                    {
                        BindCopiesGrid(book.BookId, _currentCopies, lastAddedId);
                    }

                    MessageBox.Show(
                        $"Đã nhập bổ sung {addedCopies.Count} bản sao cho đầu sách '{book.BookId}' thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập bổ sung bản sao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCapNhatBanSao_Click(object? sender, EventArgs e)
        {
            OpenCapNhatBanSaoDialog();
        }

        private void OpenCapNhatBanSaoDialog()
        {
            try
            {
                var selectedCopy = GetSelectedBookCopyFromGrid();
                if (selectedCopy == null)
                {
                    MessageBox.Show("Vui lòng chọn một bản sao trong bảng để cập nhật!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string bookTitle = txtTenSach.Text.Trim();
                using var dlg = new frmCapNhatBanSao(selectedCopy, bookTitle);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var updated = new BookCopy
                    {
                        BookCopyId = selectedCopy.BookCopyId,
                        BookId = selectedCopy.BookId,
                        Status = dlg.TrangThaiMoi,
                        Condition = dlg.TinhTrangMoi
                    };

                    _bookBuss.UpdateBookCopy(updated);

                    _currentCopies = _bookCopyBuss.GetByBookId(updated.BookId);
                    BindCopiesGrid(updated.BookId, _currentCopies, updated.BookCopyId);
                    UpdateStatus($"Đã cập nhật bản sao '{updated.BookCopyId}' lúc {DateTime.Now:HH:mm:ss}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể cập nhật bản sao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoaBanSao_Click(object? sender, EventArgs e)
        {
            try
            {
                var selectedCopy = GetSelectedBookCopyFromGrid();
                if (selectedCopy == null)
                {
                    MessageBox.Show("Vui lòng chọn một bản sao trong bảng để xóa!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa bản sao '{selectedCopy.BookCopyId}' của đầu sách '{selectedCopy.BookId}' không?",
                    "Xác nhận xóa bản sao",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                string bookId = selectedCopy.BookId;
                string deletedCopyId = selectedCopy.BookCopyId;

                _bookBuss.DeleteBookCopy(deletedCopyId);

                LoadBooksAndSelect(
                    selectBookId: bookId,
                    customStatusPrefix: $"Đã xóa bản sao '{deletedCopyId}' của sách {bookId}");

                MessageBox.Show(
                    $"Đã xóa bản sao '{deletedCopyId}' thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa bản sao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
