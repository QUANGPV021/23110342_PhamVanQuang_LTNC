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
    /// Sử dụng mô hình 3 tầng (3-Layer Architecture): View giao tiếp qua Business Layer (BUL).
    /// </summary>
    public partial class frmQuanLySV : Form
    {
        private SinhVienBUL svb;
        private LopBus LopBus;
        private List<SinhVien> sinhViens;
        private List<LopHoc> lopHocs;

        private bool _isInitializing = true;
        private bool _isUpdatingUi = false;

        public frmQuanLySV()
        {
            svb = new SinhVienBUL();
            LopBus = new LopBus();
            lopHocs = LopBus.GetAllLopHoc();
            sinhViens = svb.GetAllSinhVien();

            InitializeComponent();

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

        #region Toggle Button State

        /// <summary>
        /// Điều chỉnh trạng thái Enable của các button Thêm, Sửa, Xóa theo trạng thái thao tác.
        /// </summary>
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

        #region Form Load & Initialize

        private void frmQuanLySV_Load(object sender, EventArgs e)
        {
            _isInitializing = true;

            // 1. Cấu hình ComboBox Lớp học
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
            RefreshDataGridView();

            // 5. Thiết lập trạng thái ban đầu: Enable Thêm, Disable Sửa & Xóa
            TogAdd(true);

            _isInitializing = false;
        }

        private void frmQuanLySV_Shown(object sender, EventArgs e)
        {
            // Con trỏ thiết lập mặc định ở txtMaSV khi Form hiển thị
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
            // Đồng bộ tên lớp hiển thị cho sinh viên
            foreach (var sv in list)
            {
                if (sv.LopHoc == null)
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
            sinhViens = svb.GetAllSinhVien();
            BindGrid(sinhViens);
        }

        #endregion

        #region Focus & Keyboard Events

        private void txtMaSV_Enter(object sender, EventArgs e)
        {
            txtMaSV.SelectAll();
        }

        private void txtMaSV_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Khi nhấn Enter ở txtMaSV -> chuyển tiêu điểm đến dtpNgaySinh
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

            // Tìm sinh viên từ tầng Business
            var sv = svb.GetSinhVienByMaSV(maSV);
            if (sv == null)
            {
                // Chưa tồn tại -> xóa trắng các TextBox, enable chức năng Thêm (Nhập), disable Sửa, Xóa
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
                // Tồn tại -> hiển thị dữ liệu tương ứng, disable Thêm (Nhập), enable Sửa, Xóa
                LoadSinhVienToForm(sv);
                TogAdd(false);
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
                    MessageBox.Show($"Thêm thành công sinh viên '{sv.HoTen}' ({sv.MaSV})!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDataGridView();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"Cập nhật thông tin sinh viên '{sv.MaSV}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sinh viên: " + ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Xác thực trước khi thực hiện chức năng nguy hiểm
            var result = MessageBox.Show(
                $"CẢNH BÁO NGUY HIỂM:\nBạn có chắc chắn muốn xóa sinh viên '{maSV}' ra khỏi hệ thống?\n\nHành động này không thể hoàn tác!",
                "Xác nhận xóa dữ liệu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (result == DialogResult.Yes)
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
                    MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void butLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
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

            // Reset button states: Enable Thêm, Disable Sửa & Xóa
            TogAdd(true);

            // Chuyển tiêu điểm về txtMaSV
            txtMaSV.Focus();
        }

        #endregion

        #region ComboBox & Search / Filter

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing || _isUpdatingUi) return;

            if (cboLop.SelectedValue != null)
            {
                string maLop = cboLop.SelectedValue.ToString()!;
                sinhViens = svb.GetSinhVienByMaLop(maLop);
                BindGrid(sinhViens);
            }
        }

        private void butTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTuKhoa.Text.Trim();
            string selectedMaLop = cboFilterLop.SelectedValue != null ? cboFilterLop.SelectedValue.ToString()! : "ALL";
            double diemTu = (double)numDiemTu.Value;

            var ketQua = svb.Search(keyword, selectedMaLop, diemTu);
            BindGrid(ketQua);
        }

        private void butHienThiTatCa_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Text = string.Empty;
            cboFilterLop.SelectedIndex = 0;
            numDiemTu.Value = 0.0M;
            RefreshDataGridView();
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                var row = dgvSinhVien.Rows[e.RowIndex];
                string? maSV = row.Cells[0].Value?.ToString();
                if (!string.IsNullOrEmpty(maSV))
                {
                    var sv = svb.GetSinhVienByMaSV(maSV);
                    if (sv != null)
                    {
                        ClearErrorProviders();
                        LoadSinhVienToForm(sv);
                        TogAdd(false);
                    }
                }
            }
        }

        #endregion

        #region Form Closing Confirmation

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

    /// <summary>
    /// Lớp bí danh FormQuanLySinhVien kế thừa frmQuanLySV nhằm đảm bảo tính tương thích
    /// </summary>
    public class FormQuanLySinhVien : frmQuanLySV
    {
    }
}
