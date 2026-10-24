using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.BUL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Views
{
    /// <summary>
    /// Presentation Layer (Views / GUI) quản lý sinh viên.
    /// Hoạt động với Entity Framework Core Code First thông qua tầng Business Logic Layer (BUL).
    /// Bắt và xử lý toàn diện các ngoại lệ (Exception) tránh để ứng dụng gặp sự cố.
    /// </summary>
    public partial class frmQuanLySV : Form
    {
        private SinhVienBUL svb;
        private LopBus lopBus;
        private List<SinhVien> sinhViens = new List<SinhVien>();
        private List<LopHoc> lopHocs = new List<LopHoc>();

        private bool _isInitializing = true;
        private bool _isUpdatingUi = false;

        public frmQuanLySV()
        {
            InitializeComponent();

            svb = new SinhVienBUL();
            lopBus = new LopBus();

            // Đăng ký sự kiện
            this.Load += frmQuanLySV_Load;
            this.Shown += frmQuanLySV_Shown;
            this.FormClosing += frmQuanLySV_FormClosing;

            this.txtMaSV.Enter += txtMaSV_Enter;
            this.txtMaSV.Leave += txtMaSV_Leave;
            this.txtMaSV.KeyPress += txtMaSV_KeyPress;
            this.txtMaSV.TextChanged += txtMaSV_TextChanged;

            this.cboLop.SelectedIndexChanged += cboLop_SelectedIndexChanged;

            this.butThem.Click += butThem_Click;
            this.butSua.Click += butSua_Click;
            this.butXoa.Click += butXoa_Click;
            this.butLamMoi.Click += butLamMoi_Click;

            this.butTimKiem.Click += butTimKiem_Click;
            this.butHienThiTatCa.Click += butHienThiTatCa_Click;

            this.dgvSinhVien.CellClick += dgvSinhVien_CellClick;
        }

        #region Button State Control

        private void TogAdd(bool isAdding)
        {
            butThem.Enabled = isAdding;
            butXoa.Enabled = !isAdding;
            butSua.Enabled = !isAdding;

            butThem.BackColor = isAdding ? Color.FromArgb(31, 139, 90) : Color.FromArgb(160, 174, 192);
            butSua.BackColor = !isAdding ? Color.FromArgb(42, 111, 151) : Color.FromArgb(160, 174, 192);
            butXoa.BackColor = !isAdding ? Color.FromArgb(188, 56, 56) : Color.FromArgb(160, 174, 192);
        }

        #endregion

        #region Form Load & Setup

        private void frmQuanLySV_Load(object sender, EventArgs e)
        {
            _isInitializing = true;

            try
            {
                // 1. Tải danh sách lớp qua Entity Framework Core
                lopHocs = lopBus.GetAllLopHoc();
                cboLop.DataSource = lopHocs;
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
                if (cboLop.Items.Count > 0)
                {
                    cboLop.SelectedIndex = 0;
                }

                // 2. Cấu hình ComboBox Trạng thái
                cboTrangThai.Items.Clear();
                cboTrangThai.Items.AddRange(new object[] { "Đang học", "Bảo lưu", "Đã tốt nghiệp", "Nghỉ học" });
                cboTrangThai.SelectedIndex = 0;

                // 3. Cấu hình ComboBox Lọc theo lớp
                var filterLopList = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("ALL", "Tất cả lớp")
                };
                foreach (var lop in lopHocs)
                {
                    filterLopList.Add(new KeyValuePair<string, string>(lop.MaLop, $"{lop.TenLop} ({lop.MaLop})"));
                }
                cboFilterLop.DataSource = filterLopList;
                cboFilterLop.DisplayMember = "Value";
                cboFilterLop.ValueMember = "Key";
                cboFilterLop.SelectedIndex = 0;

                // 4. Khởi tạo DataGridView
                SetupDataGridView();

                // 5. Tải dữ liệu sinh viên
                RefreshDataGridView();

                // 6. Trạng thái nút ban đầu
                TogAdd(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "LỖI KẾT NỐI ENTITY FRAMEWORK CORE:\n" + ex.Message +
                    "\n\n* Gợi ý: Hãy kiểm tra chuỗi kết nối trong QLSVDbContext.cs hoặc thực hiện lệnh 'Update-Database' trong Package Manager Console / 'dotnet ef database update'.",
                    "Lỗi Cơ sở dữ liệu EF Core",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                _isInitializing = false;
            }
        }

        private void frmQuanLySV_Shown(object sender, EventArgs e)
        {
            txtMaSV.Focus();
        }

        private void SetupDataGridView()
        {
            dgvSinhVien.AutoGenerateColumns = false;
            dgvSinhVien.Columns.Clear();

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSV",
                HeaderText = "Mã SV",
                FillWeight = 10
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoTen",
                HeaderText = "Họ và tên",
                FillWeight = 18
            });

            var colNgaySinh = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgaySinh",
                HeaderText = "Ngày sinh",
                FillWeight = 11
            };
            colNgaySinh.DefaultCellStyle.Format = "dd/MM/yyyy";
            colNgaySinh.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSinhVien.Columns.Add(colNgaySinh);

            var colGioiTinh = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GioiTinh",
                HeaderText = "Giới tính",
                FillWeight = 9
            };
            colGioiTinh.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSinhVien.Columns.Add(colGioiTinh);

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                FillWeight = 16
            });

            var colDienThoai = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoDienThoai",
                HeaderText = "Điện thoại",
                FillWeight = 12
            };
            colDienThoai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSinhVien.Columns.Add(colDienThoai);

            var colDiem = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Diem",
                HeaderText = "Điểm",
                FillWeight = 8
            };
            colDiem.DefaultCellStyle.Format = "F1";
            colDiem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSinhVien.Columns.Add(colDiem);

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenLop",
                HeaderText = "Lớp",
                FillWeight = 16
            });

            var colTrangThai = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng thái",
                FillWeight = 10
            };
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSinhVien.Columns.Add(colTrangThai);
        }

        private void BindGrid(List<SinhVien> list)
        {
            foreach (var sv in list)
            {
                if (sv.LopHoc == null && lopHocs != null)
                {
                    sv.LopHoc = lopHocs.FirstOrDefault(l => l.MaLop.Equals(sv.MaLop, StringComparison.OrdinalIgnoreCase));
                }
            }

            dgvSinhVien.DataSource = null;
            dgvSinhVien.DataSource = list;
            lblTongSo.Text = $"Tổng số: {list.Count} sinh viên";
        }

        private void RefreshDataGridView()
        {
            try
            {
                sinhViens = svb.GetAllSinhVien();
                BindGrid(sinhViens);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lại danh sách sinh viên: " + ex.Message, "Lỗi EF Core", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Focus & Keyboard Events

        private void txtMaSV_Enter(object sender, EventArgs e)
        {
            txtMaSV.SelectAll();
        }

        private void txtMaSV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                dtpNgaySinh.Focus();
            }
        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {
            errPMaSV.Clear();
        }

        private void txtMaSV_Leave(object sender, EventArgs e)
        {
            if (_isUpdatingUi) return;

            string maSV = txtMaSV.Text.Trim();
            if (string.IsNullOrWhiteSpace(maSV))
            {
                TogAdd(true);
                return;
            }

            try
            {
                var sv = svb.GetSinhVienByMaSV(maSV);
                if (sv == null)
                {
                    _isUpdatingUi = true;
                    txtHoTen.Text = "";
                    txtEmail.Text = "";
                    txtDienThoai.Text = "";
                    dtpNgaySinh.Value = new DateTime(2006, 1, 1);
                    rdoNam.Checked = true;
                    numDiem.Value = 0.0M;
                    cboTrangThai.SelectedIndex = 0;
                    _isUpdatingUi = false;

                    TogAdd(true);
                }
                else
                {
                    LoadSinhVienToForm(sv);
                    TogAdd(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tra cứu sinh viên: " + ex.Message, "Lỗi EF Core", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadSinhVienToForm(SinhVien sv)
        {
            _isUpdatingUi = true;

            txtMaSV.Text = sv.MaSV;
            txtHoTen.Text = sv.HoTen;
            txtEmail.Text = sv.Email;
            txtDienThoai.Text = sv.SoDienThoai;

            if (sv.NgaySinh >= dtpNgaySinh.MinDate && sv.NgaySinh <= dtpNgaySinh.MaxDate)
            {
                dtpNgaySinh.Value = sv.NgaySinh;
            }

            if (string.Equals(sv.GioiTinh, "Nữ", StringComparison.OrdinalIgnoreCase))
            {
                rdoNu.Checked = true;
            }
            else
            {
                rdoNam.Checked = true;
            }

            cboLop.SelectedValue = sv.MaLop;
            numDiem.Value = (decimal)Math.Clamp(sv.Diem, 0.0, 10.0);

            if (!string.IsNullOrEmpty(sv.TrangThai) && cboTrangThai.Items.Contains(sv.TrangThai))
            {
                cboTrangThai.SelectedItem = sv.TrangThai;
            }
            else
            {
                cboTrangThai.SelectedIndex = 0;
            }

            _isUpdatingUi = false;
        }

        #endregion

        #region ErrorProvider Management

        private void ClearErrorProviders()
        {
            errPMaSV.Clear();
            erpHoten.Clear();
            erpEmail.Clear();
            erpDienThoai.Clear();
            erpLopHoc.Clear();
            erpDiem.Clear();
        }

        private void DisplayValidationErrors(List<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                if (error.MemberNames != null && error.MemberNames.Any())
                {
                    string fieldName = error.MemberNames.First();
                    switch (fieldName)
                    {
                        case "MaSV":
                            errPMaSV.SetError(txtMaSV, error.ErrorMessage);
                            break;
                        case "HoTen":
                            erpHoten.SetError(txtHoTen, error.ErrorMessage);
                            txtHoTen.Focus();
                            break;
                        case "Email":
                            erpEmail.SetError(txtEmail, error.ErrorMessage);
                            txtEmail.Focus();
                            break;
                        case "SoDienThoai":
                        case "DienThoai":
                            erpDienThoai.SetError(txtDienThoai, error.ErrorMessage);
                            txtDienThoai.Focus();
                            break;
                        case "MaLop":
                            erpLopHoc.SetError(cboLop, error.ErrorMessage);
                            break;
                        case "Diem":
                            erpDiem.SetError(numDiem, error.ErrorMessage);
                            break;
                        default:
                            MessageBox.Show(error.ErrorMessage, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show(error.ErrorMessage, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region CRUD Operations

        private void butThem_Click(object sender, EventArgs e)
        {
            ClearErrorProviders();

            SinhVien sv = new SinhVien
            {
                MaSV = txtMaSV.Text.Trim(),
                HoTen = txtHoTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = rdoNam.Checked ? "Nam" : "Nữ",
                Email = txtEmail.Text.Trim(),
                SoDienThoai = txtDienThoai.Text.Trim(),
                MaLop = cboLop.SelectedValue != null ? cboLop.SelectedValue.ToString()! : "",
                Diem = (double)numDiem.Value,
                TrangThai = cboTrangThai.SelectedItem != null ? cboTrangThai.SelectedItem.ToString()! : "Đang học"
            };

            List<ValidationResult> errors = sv.IsInValid();
            if (errors.Count == 0)
            {
                try
                {
                    svb.AddSinhVien(sv);
                    MessageBox.Show($"Thêm thành công sinh viên '{sv.HoTen}' ({sv.MaSV}) bằng EF Core!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDataGridView();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LỖI KHI THÊM SINH VIÊN:\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                DisplayValidationErrors(errors);
            }
        }

        private void butSua_Click(object sender, EventArgs e)
        {
            ClearErrorProviders();

            string maSV = txtMaSV.Text.Trim();
            if (string.IsNullOrWhiteSpace(maSV))
            {
                errPMaSV.SetError(txtMaSV, "Vui lòng chọn hoặc nhập mã sinh viên cần sửa.");
                txtMaSV.Focus();
                return;
            }

            SinhVien sv = new SinhVien
            {
                MaSV = maSV,
                HoTen = txtHoTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = rdoNam.Checked ? "Nam" : "Nữ",
                Email = txtEmail.Text.Trim(),
                SoDienThoai = txtDienThoai.Text.Trim(),
                MaLop = cboLop.SelectedValue != null ? cboLop.SelectedValue.ToString()! : "",
                Diem = (double)numDiem.Value,
                TrangThai = cboTrangThai.SelectedItem != null ? cboTrangThai.SelectedItem.ToString()! : "Đang học"
            };

            List<ValidationResult> errors = sv.IsInValid();
            if (errors.Count == 0)
            {
                try
                {
                    svb.UpdateSinhVien(sv);
                    MessageBox.Show($"Cập nhật sinh viên '{sv.MaSV}' bằng EF Core thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LỖI KHI CẬP NHẬT SINH VIÊN:\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                DisplayValidationErrors(errors);
            }
        }

        private void butXoa_Click(object sender, EventArgs e)
        {
            ClearErrorProviders();

            string maSV = txtMaSV.Text.Trim();
            if (string.IsNullOrWhiteSpace(maSV))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập mã sinh viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSV.Focus();
                return;
            }

            var confirmResult = MessageBox.Show(
                $"CẢNH BÁO NGUY HIỂM:\nBạn có chắc chắn muốn xóa sinh viên '{maSV}' khỏi CSDL?\n\nHành động này sẽ thực thi qua Entity Framework Core!",
                "Xác nhận xóa dữ liệu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    svb.DeleteSinhVien(maSV);
                    MessageBox.Show($"Đã xóa sinh viên '{maSV}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDataGridView();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LỖI KHI XÓA SINH VIÊN:\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void butLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            RefreshDataGridView();
        }

        private void ResetForm()
        {
            ClearErrorProviders();

            _isUpdatingUi = true;
            txtMaSV.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDienThoai.Text = string.Empty;
            dtpNgaySinh.Value = new DateTime(2006, 1, 1);
            rdoNam.Checked = true;
            numDiem.Value = 0.0M;

            if (cboLop.Items.Count > 0)
            {
                cboLop.SelectedIndex = 0;
            }
            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }
            _isUpdatingUi = false;

            TogAdd(true);
            txtMaSV.Focus();
        }

        #endregion

        #region ComboBox & Search / Filter

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing || _isUpdatingUi) return;

            if (cboLop.SelectedValue != null)
            {
                try
                {
                    string maLop = cboLop.SelectedValue.ToString()!;
                    sinhViens = svb.GetSinhVienByMaLop(maLop);
                    BindGrid(sinhViens);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lọc sinh viên theo lớp: " + ex.Message, "Lỗi EF Core", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void butTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTuKhoa.Text.Trim();
                string selectedMaLop = cboFilterLop.SelectedValue != null ? cboFilterLop.SelectedValue.ToString()! : "ALL";
                double diemTu = (double)numDiemTu.Value;

                var ketQua = svb.Search(keyword, selectedMaLop, diemTu);
                BindGrid(ketQua);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm dữ liệu: " + ex.Message, "Lỗi EF Core", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butHienThiTatCa_Click(object sender, EventArgs e)
        {
            try
            {
                txtTuKhoa.Text = string.Empty;
                cboFilterLop.SelectedIndex = 0;
                numDiemTu.Value = 0.0M;
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lại dữ liệu: " + ex.Message, "Lỗi EF Core", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                var row = dgvSinhVien.Rows[e.RowIndex];
                string? maSV = row.Cells[0].Value?.ToString();
                if (!string.IsNullOrEmpty(maSV))
                {
                    try
                    {
                        var sv = svb.GetSinhVienByMaSV(maSV);
                        if (sv != null)
                        {
                            ClearErrorProviders();
                            LoadSinhVienToForm(sv);
                            TogAdd(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi nạp dữ liệu chi tiết sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        #endregion

        #region Form Closing

        private void frmQuanLySV_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var dialogResult = MessageBox.Show(
                    "Bạn có muốn thoát khỏi ứng dụng không?",
                    "Thông báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion
    }

    public class FormQuanLySinhVien : frmQuanLySV
    {
    }
}
