using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.DAO;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Forms
{
    public partial class FormQuanLySinhVien : Form
    {
        private readonly LopHocDAO _lopHocDao;
        private readonly SinhVienDAO _sinhVienDao;

        private bool _isUpdatingUi = false;
        private bool _isViewingExisting = false;

        public FormQuanLySinhVien()
        {
            InitializeComponent();

            _lopHocDao = new LopHocDAO();
            _sinhVienDao = new SinhVienDAO(_lopHocDao);

            // Đăng ký các sự kiện
            this.Load += FormQuanLySinhVien_Load;
            this.Shown += FormQuanLySinhVien_Shown;

            this.txtMaSV.TextChanged += txtMaSV_TextChanged;
            this.btnThem.Click += btnThem_Click;
            this.btnSua.Click += btnSua_Click;
            this.btnXoa.Click += btnXoa_Click;
            this.btnLamMoi.Click += btnLamMoi_Click;

            this.btnTimKiem.Click += btnTimKiem_Click;
            this.btnHienThiTatCa.Click += btnHienThiTatCa_Click;

            this.dgvSinhVien.CellClick += dgvSinhVien_CellClick;
        }

        #region Form Load & Initialize

        private void FormQuanLySinhVien_Load(object? sender, EventArgs e)
        {
            SetupComboboxes();
            SetupDataGridView();
            LoadDataGrid();

            // Thiết lập giá trị enable ban đầu cho các button
            SetButtonState(canThem: true, canSua: false, canXoa: false);
        }

        private void FormQuanLySinhVien_Shown(object? sender, EventArgs e)
        {
            // Con trỏ thiết lập mặc định ở txtMaSV khi form hiển thị
            txtMaSV.Focus();
        }

        private void SetupComboboxes()
        {
            // 1. ComboBox Lớp học (Thông tin sinh viên)
            var danhSachLop = _lopHocDao.GetAll();
            cboLopHoc.DataSource = danhSachLop;
            cboLopHoc.DisplayMember = "TenLop";
            cboLopHoc.ValueMember = "MaLop";
            if (cboLopHoc.Items.Count > 0)
            {
                cboLopHoc.SelectedIndex = 0;
            }

            // 2. ComboBox Trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Đang học", "Bảo lưu", "Đã tốt nghiệp", "Nghỉ học" });
            cboTrangThai.SelectedIndex = 0;

            // 3. ComboBox Lọc theo lớp (Panel tìm kiếm)
            var locLopItems = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ALL", "Tất cả lớp")
            };
            foreach (var lop in danhSachLop)
            {
                locLopItems.Add(new KeyValuePair<string, string>(lop.MaLop, lop.TenLop));
            }
            cboLocLop.DataSource = locLopItems;
            cboLocLop.DisplayMember = "Value";
            cboLocLop.ValueMember = "Key";
            cboLocLop.SelectedIndex = 0;
        }

        private void SetupDataGridView()
        {
            dgvSinhVien.AutoGenerateColumns = false;
            dgvSinhVien.Columns.Clear();

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSV",
                HeaderText = "Mã SV",
                Width = 100,
                Name = "colMaSV"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoTen",
                HeaderText = "Họ và tên",
                Width = 160,
                Name = "colHoTen"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgaySinh",
                HeaderText = "Ngày sinh",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Name = "colNgaySinh"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GioiTinh",
                HeaderText = "Giới tính",
                Width = 90,
                Name = "colGioiTinh"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Width = 170,
                Name = "colEmail"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DienThoai",
                HeaderText = "Điện thoại",
                Width = 120,
                Name = "colDienThoai"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Diem",
                HeaderText = "Điểm",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "0.0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Name = "colDiem"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenLop",
                HeaderText = "Lớp",
                Width = 180,
                Name = "colLop"
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng thái",
                Width = 110,
                Name = "colTrangThai"
            });
        }

        private void LoadDataGrid(List<SinhVien>? danhSach = null)
        {
            var list = danhSach ?? _sinhVienDao.GetAll();
            dgvSinhVien.DataSource = null;
            dgvSinhVien.DataSource = list;

            lblTongSo.Text = $"Tổng số: {list.Count} sinh viên";
        }

        #endregion

        #region Event: Nhập mã sinh viên (txtMaSV_TextChanged)

        /// <summary>
        /// Sự kiện khi người dùng nhập mã sinh viên:
        /// - Nếu mã sinh viên tồn tại: lấy thông tin của sinh viên hiển thị tương ứng lên các điều khiển còn lại,
        ///   disable chức năng nhập (thêm), enable chức năng sửa, xóa.
        /// - Nếu chưa tồn tại: xóa giá trị các điều khiển textbox, enable chức năng nhập (thêm), disable chức năng sửa, xóa.
        /// </summary>
        private void txtMaSV_TextChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingUi) return;

            string maSV = txtMaSV.Text.Trim();
            if (string.IsNullOrWhiteSpace(maSV))
            {
                SetButtonState(canThem: true, canSua: false, canXoa: false);
                _isViewingExisting = false;
                return;
            }

            var sv = _sinhVienDao.GetById(maSV);
            if (sv != null)
            {
                // Sinh viên đã tồn tại
                _isUpdatingUi = true;
                txtHoTen.Text = sv.HoTen;
                dtpNgaySinh.Value = sv.NgaySinh;
                if (sv.GioiTinh == "Nữ")
                {
                    rdoNu.Checked = true;
                }
                else
                {
                    rdoNam.Checked = true;
                }
                txtEmail.Text = sv.Email;
                txtDienThoai.Text = sv.DienThoai;
                nudDiem.Value = (decimal)sv.Diem;
                cboLopHoc.SelectedValue = sv.MaLop;
                cboTrangThai.SelectedItem = sv.TrangThai;
                _isUpdatingUi = false;

                // Disable chức năng nhập, enable chức năng sửa, xóa
                SetButtonState(canThem: false, canSua: true, canXoa: true);
                _isViewingExisting = true;

                HighlightGridRow(sv.MaSV);
                lblStatusLeft.Text = $"Đã tải thông tin sinh viên '{sv.HoTen}' ({sv.MaSV})";
            }
            else
            {
                // Chưa tồn tại mã sinh viên này:
                // Nếu trước đó đang hiển thị thông tin 1 sinh viên tồn tại thì xóa các textbox
                if (_isViewingExisting)
                {
                    _isUpdatingUi = true;
                    txtHoTen.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtDienThoai.Text = string.Empty;
                    nudDiem.Value = 0;
                    dtpNgaySinh.Value = new DateTime(2006, 1, 1);
                    rdoNam.Checked = true;
                    if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
                    _isUpdatingUi = false;
                }

                // Enable chức năng nhập, disable chức năng sửa, xóa
                SetButtonState(canThem: true, canSua: false, canXoa: false);
                _isViewingExisting = false;
                lblStatusLeft.Text = $"Mã sinh viên '{maSV}' chưa tồn tại. Sẵn sàng thêm mới.";
            }
        }

        #endregion

        #region Event: Chức năng Thêm (btnThem_Click)

        private void btnThem_Click(object? sender, EventArgs e)
        {
            var sv = CollectFormData();

            // Kiểm tra tính hợp lệ bằng Data Annotations
            var (isValid, errors) = sv.Validate();
            if (!isValid)
            {
                string message = "Dữ liệu nhập không hợp lệ:\n\n• " + string.Join("\n• ", errors);
                MessageBox.Show(message, "Lỗi xác thực dữ liệu (Data Annotation)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra xem mã sinh viên đã tồn tại chưa
            if (_sinhVienDao.GetById(sv.MaSV) != null)
            {
                MessageBox.Show($"Mã sinh viên '{sv.MaSV}' đã tồn tại trong hệ thống!", "Cảnh báo trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSV.Focus();
                return;
            }

            bool added = _sinhVienDao.Add(sv);
            if (added)
            {
                MessageBox.Show($"Thêm mới sinh viên '{sv.HoTen}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGrid();
                HighlightGridRow(sv.MaSV);
                lblStatusLeft.Text = $"Đã thêm mới sinh viên {sv.HoTen} ({sv.MaSV})";
            }
            else
            {
                MessageBox.Show("Không thể thêm sinh viên. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event: Chức năng Sửa (btnSua_Click)

        private void btnSua_Click(object? sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            var existing = _sinhVienDao.GetById(maSV);
            if (existing == null)
            {
                MessageBox.Show("Không tìm thấy sinh viên cần sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sv = CollectFormData();

            // Kiểm tra tính hợp lệ bằng Data Annotations
            var (isValid, errors) = sv.Validate();
            if (!isValid)
            {
                string message = "Dữ liệu cập nhật không hợp lệ:\n\n• " + string.Join("\n• ", errors);
                MessageBox.Show(message, "Lỗi xác thực dữ liệu (Data Annotation)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool edited = _sinhVienDao.Edit(sv);
            if (edited)
            {
                MessageBox.Show($"Cập nhật thông tin sinh viên '{sv.HoTen}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGrid();
                HighlightGridRow(sv.MaSV);
                lblStatusLeft.Text = $"Đã cập nhật sinh viên {sv.HoTen} ({sv.MaSV})";
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi cập nhật thông tin sinh viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event: Chức năng Xóa - Xác thực hành động nguy hiểm (btnXoa_Click)

        /// <summary>
        /// Yêu cầu đề bài: "Khi người dùng thực hiện các chức năng nguy hiểm cần xác thực trước khi thực hiện."
        /// </summary>
        private void btnXoa_Click(object? sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            var sv = _sinhVienDao.GetById(maSV);
            if (sv == null)
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập mã sinh viên hợp lệ để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hộp thoại xác thực hành động nguy hiểm trước khi xóa
            var confirmResult = MessageBox.Show(
                $"⚠️ CẢNH BÁO: HÀNH ĐỘNG NGUY HIỂM!\n\n" +
                $"Bạn có thực sự chắc chắn muốn XÓA sinh viên sau khỏi hệ thống?\n\n" +
                $"• Mã sinh viên : {sv.MaSV}\n" +
                $"• Họ và tên    : {sv.HoTen}\n" +
                $"• Lớp học      : {sv.TenLop}\n" +
                $"• Điểm         : {sv.Diem:F1}\n\n" +
                $"LƯU Ý: Thao tác này sẽ xóa vĩnh viễn và KHÔNG THỂ HOÀN TÁC!",
                "Xác thực hành động nguy hiểm (Xóa sinh viên)",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                bool deleted = _sinhVienDao.Delete(sv.MaSV);
                if (deleted)
                {
                    MessageBox.Show($"Đã xóa thành công sinh viên '{sv.HoTen}' ({sv.MaSV})!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                    btnLamMoi.PerformClick();
                    lblStatusLeft.Text = $"Đã xóa sinh viên {sv.HoTen} ({sv.MaSV})";
                }
                else
                {
                    MessageBox.Show("Không thể xóa sinh viên khỏi hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Event: Button Làm mới (btnLamMoi_Click)

        /// <summary>
        /// Yêu cầu đề bài:
        /// "Khi người dùng nhấn Button Làm mới, xóa trống các thuộc tính trên form,
        /// Enable button Nhập, Diable button sửa xóa, chuyển tiêu điểm về điều khiển txtMaSV."
        /// </summary>
        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            _isUpdatingUi = true;

            // Xóa trống các thuộc tính trên form
            txtMaSV.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDienThoai.Text = string.Empty;

            dtpNgaySinh.Value = new DateTime(2006, 1, 1);
            rdoNam.Checked = true;
            nudDiem.Value = 0;

            if (cboLopHoc.Items.Count > 0) cboLopHoc.SelectedIndex = 0;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;

            // Bỏ chọn dòng trên DataGridView
            dgvSinhVien.ClearSelection();

            _isUpdatingUi = false;
            _isViewingExisting = false;

            // Enable button Nhập (Thêm), Disable button sửa xóa
            SetButtonState(canThem: true, canSua: false, canXoa: false);

            // Chuyển tiêu điểm về điều khiển txtMaSV
            txtMaSV.Focus();
            lblStatusLeft.Text = "Đã làm mới form. Con trỏ sẵn sàng tại Mã sinh viên.";
        }

        #endregion

        #region Event: Tìm kiếm & Lọc

        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            string keyword = txtTuKhoa.Text.Trim();
            string selectedMaLop = cboLocLop.SelectedValue?.ToString() ?? "ALL";
            double minDiem = (double)nudDiemTu.Value;

            var results = _sinhVienDao.Search(keyword, selectedMaLop, minDiem);
            LoadDataGrid(results);

            lblStatusLeft.Text = $"Tìm thấy {results.Count} kết quả phù hợp với điều kiện tìm kiếm.";
        }

        private void btnHienThiTatCa_Click(object? sender, EventArgs e)
        {
            txtTuKhoa.Text = string.Empty;
            cboLocLop.SelectedIndex = 0;
            nudDiemTu.Value = 0;

            LoadDataGrid();
            lblStatusLeft.Text = "Đã hiển thị toàn bộ danh sách sinh viên.";
        }

        #endregion

        #region Event: Click dòng trên DataGridView

        private void dgvSinhVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                var row = dgvSinhVien.Rows[e.RowIndex];
                if (row.DataBoundItem is SinhVien sv)
                {
                    // Gán mã sinh viên vào txtMaSV -> txtMaSV_TextChanged sẽ kích hoạt đổ dữ liệu
                    txtMaSV.Text = sv.MaSV;
                }
            }
        }

        #endregion

        #region Helper Methods

        private SinhVien CollectFormData()
        {
            return new SinhVien
            {
                MaSV = txtMaSV.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value.Date,
                GioiTinh = rdoNam.Checked ? "Nam" : "Nữ",
                Email = txtEmail.Text.Trim(),
                DienThoai = txtDienThoai.Text.Trim(),
                Diem = (double)nudDiem.Value,
                MaLop = cboLopHoc.SelectedValue?.ToString() ?? string.Empty,
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Đang học"
            };
        }

        private void SetButtonState(bool canThem, bool canSua, bool canXoa)
        {
            btnThem.Enabled = canThem;
            btnSua.Enabled = canSua;
            btnXoa.Enabled = canXoa;

            // Đổi màu để thể hiện trạng thái disable rõ nét
            btnThem.BackColor = canThem ? Color.FromArgb(22, 163, 74) : Color.FromArgb(187, 247, 208);
            btnSua.BackColor = canSua ? Color.FromArgb(37, 99, 235) : Color.FromArgb(191, 219, 254);
            btnXoa.BackColor = canXoa ? Color.FromArgb(220, 38, 38) : Color.FromArgb(254, 202, 202);
        }

        private void HighlightGridRow(string maSV)
        {
            foreach (DataGridViewRow row in dgvSinhVien.Rows)
            {
                if (row.DataBoundItem is SinhVien sv && sv.MaSV.Equals(maSV, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    dgvSinhVien.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        #endregion
    }
}
