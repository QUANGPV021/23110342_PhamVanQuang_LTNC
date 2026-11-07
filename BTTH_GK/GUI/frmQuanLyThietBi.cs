using System.Drawing.Drawing2D;
using System.Globalization;
using BUS;
using DAL.Entities;

namespace GUI
{
    public partial class frmQuanLyThietBi : Form
    {
        private readonly LoaiThietBiBUS _loaiThietBiBUS = new LoaiThietBiBUS();
        private readonly ThietBiBUS _thietBiBUS = new ThietBiBUS();
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN");
        private bool _dangCapNhatGiaoDien = false;

        public frmQuanLyThietBi()
        {
            InitializeComponent();
            GanBieuTuongNutBam();
        }

        private void frmQuanLyThietBi_Load(object? sender, EventArgs e)
        {
            try
            {
                _thietBiBUS.KhoiTaoCSDL();
                NapDanhSachLoaiThietBi();
                NapDanhSachThietBi();
                DatTrangThaiMacDinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể kết nối hoặc khởi tạo cơ sở dữ liệu:\n{ex.Message}",
                    "Lỗi cơ sở dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void frmQuanLyThietBi_Shown(object? sender, EventArgs e)
        {
            // Đảm bảo sau khi Form hiển thị lần đầu, không dòng nào bị tự động chọn và con trỏ nằm tại txtMaTB
            DatTrangThaiMacDinh();
        }

        private void NapDanhSachLoaiThietBi()
        {
            _dangCapNhatGiaoDien = true;
            try
            {
                List<LoaiThietBi> dsLoai = _loaiThietBiBUS.LayDanhSachLoaiThietBi();
                cboLoaiThietBi.DataSource = dsLoai;
                cboLoaiThietBi.DisplayMember = nameof(LoaiThietBi.TenLoai);
                cboLoaiThietBi.ValueMember = nameof(LoaiThietBi.MaLoai);
                cboLoaiThietBi.SelectedIndex = -1;
            }
            finally
            {
                _dangCapNhatGiaoDien = false;
            }
        }

        private void NapDanhSachThietBi()
        {
            _dangCapNhatGiaoDien = true;
            try
            {
                dgvThietBi.Rows.Clear();
                List<ThietBi> dsThietBi = _thietBiBUS.LayDanhSachThietBi();

                foreach (ThietBi tb in dsThietBi)
                {
                    int rowIndex = dgvThietBi.Rows.Add(
                        tb.MaTB,
                        tb.TenTB,
                        tb.NgayMua.ToString("dd/MM/yyyy"),
                        tb.SoLuong,
                        tb.DonGia.ToString("#,##0.##", _vnCulture),
                        tb.LoaiThietBi?.TenLoai ?? string.Empty
                    );
                    dgvThietBi.Rows[rowIndex].Tag = tb;
                }

                dgvThietBi.ClearSelection();
                dgvThietBi.CurrentCell = null;
            }
            finally
            {
                _dangCapNhatGiaoDien = false;
            }
        }

        private void DatTrangThaiMacDinh()
        {
            _dangCapNhatGiaoDien = true;
            try
            {
                cboLoaiThietBi.SelectedIndex = -1;
                txtMaTB.Clear();
                txtMaTB.ReadOnly = false;
                txtMaTB.BackColor = SystemColors.Window;

                txtTenTB.Clear();
                dtpNgayMua.Value = DateTime.Today;
                txtSoLuong.Clear();
                txtDonGia.Clear();

                dgvThietBi.ClearSelection();
                dgvThietBi.CurrentCell = null;

                btnNhap.Enabled = true;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
            finally
            {
                _dangCapNhatGiaoDien = false;
            }

            ActiveControl = txtMaTB;
            txtMaTB.Focus();
        }

        private void HienThiThongTinDongChon(DataGridViewRow? row)
        {
            if (_dangCapNhatGiaoDien || row == null || row.Index < 0 || row.Tag is not ThietBi tb)
            {
                return;
            }

            _dangCapNhatGiaoDien = true;
            try
            {
                cboLoaiThietBi.SelectedValue = tb.MaLoai;
                txtMaTB.Text = tb.MaTB;
                txtMaTB.ReadOnly = true;
                txtMaTB.BackColor = SystemColors.Window;

                txtTenTB.Text = tb.TenTB;
                dtpNgayMua.Value = tb.NgayMua;
                txtSoLuong.Text = tb.SoLuong.ToString();
                txtDonGia.Text = tb.DonGia.ToString("0.##", CultureInfo.InvariantCulture);

                btnNhap.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            finally
            {
                _dangCapNhatGiaoDien = false;
            }
        }

        private void dgvThietBi_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvThietBi.Rows.Count)
            {
                HienThiThongTinDongChon(dgvThietBi.Rows[e.RowIndex]);
            }
        }

        private void dgvThietBi_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dangCapNhatGiaoDien)
            {
                return;
            }

            if (dgvThietBi.SelectedRows.Count > 0)
            {
                HienThiThongTinDongChon(dgvThietBi.SelectedRows[0]);
            }
        }

        private bool ThuLayDuLieuTuForm(out ThietBi? thietBi)
        {
            thietBi = null;

            // Kiểm tra bổ sung tại GUI để hỗ trợ chuyển đổi kiểu dữ liệu số
            if (cboLoaiThietBi.SelectedIndex < 0 || cboLoaiThietBi.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn loại thiết bị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiThietBi.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMaTB.Text))
            {
                MessageBox.Show("Mã thiết bị không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaTB.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenTB.Text))
            {
                MessageBox.Show("Tên thiết bị không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTB.Focus();
                return false;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong))
            {
                MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                txtSoLuong.SelectAll();
                return false;
            }

            if (!ThuPhanTichDonGia(txtDonGia.Text, out decimal donGia))
            {
                MessageBox.Show("Đơn giá phải là số lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGia.Focus();
                txtDonGia.SelectAll();
                return false;
            }

            thietBi = new ThietBi
            {
                MaLoai = cboLoaiThietBi.SelectedValue.ToString() ?? string.Empty,
                MaTB = txtMaTB.Text,
                TenTB = txtTenTB.Text,
                NgayMua = dtpNgayMua.Value.Date,
                SoLuong = soLuong,
                DonGia = donGia
            };

            return true;
        }

        private static bool ThuPhanTichDonGia(string rawText, out decimal donGia)
        {
            string cleaned = (rawText ?? string.Empty).Trim().Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(cleaned))
            {
                donGia = 0m;
                return false;
            }

            // Hỗ trợ nhập số nguyên hoặc số thập phân với dấu chấm / dấu phẩy
            string normalized = cleaned.Replace(',', '.');
            return decimal.TryParse(
                normalized,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out donGia);
        }

        private void DuaConTroVeO(string? fieldName)
        {
            switch (fieldName)
            {
                case "MaLoai":
                    cboLoaiThietBi.Focus();
                    break;
                case "MaTB":
                    if (!txtMaTB.ReadOnly)
                    {
                        txtMaTB.Focus();
                        txtMaTB.SelectAll();
                    }
                    break;
                case "TenTB":
                    txtTenTB.Focus();
                    txtTenTB.SelectAll();
                    break;
                case "NgayMua":
                    dtpNgayMua.Focus();
                    break;
                case "SoLuong":
                    txtSoLuong.Focus();
                    txtSoLuong.SelectAll();
                    break;
                case "DonGia":
                    txtDonGia.Focus();
                    txtDonGia.SelectAll();
                    break;
            }
        }

        private void btnMoi_Click(object? sender, EventArgs e)
        {
            DatTrangThaiMacDinh();
        }

        private void btnNhap_Click(object? sender, EventArgs e)
        {
            if (!ThuLayDuLieuTuForm(out ThietBi? thietBi) || thietBi == null)
            {
                return;
            }

            try
            {
                OperationResult result = _thietBiBUS.ThemThietBi(thietBi);
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DuaConTroVeO(result.InvalidField);
                    return;
                }

                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapDanhSachThietBi();
                DatTrangThaiMacDinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi thêm thiết bị:\n{ex.InnerException?.Message ?? ex.Message}",
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (!ThuLayDuLieuTuForm(out ThietBi? thietBi) || thietBi == null)
            {
                return;
            }

            try
            {
                OperationResult result = _thietBiBUS.SuaThietBi(thietBi);
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DuaConTroVeO(result.InvalidField);
                    return;
                }

                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapDanhSachThietBi();
                DatTrangThaiMacDinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi cập nhật thiết bị:\n{ex.InnerException?.Message ?? ex.Message}",
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object? sender, EventArgs e)
        {
            string maTB = txtMaTB.Text.Trim();
            if (string.IsNullOrWhiteSpace(maTB))
            {
                MessageBox.Show("Vui lòng chọn thiết bị cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa thiết bị '{txtTenTB.Text.Trim()}' (Mã: {maTB}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                OperationResult result = _thietBiBUS.XoaThietBi(maTB);
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapDanhSachThietBi();
                DatTrangThaiMacDinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi xóa thiết bị:\n{ex.InnerException?.Message ?? ex.Message}",
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object? sender, EventArgs e)
        {
            // Gọi Close() để đi qua duy nhất sự kiện FormClosing, tránh hiển thị 2 lần hộp thoại xác nhận
            Close();
        }

        private void frmQuanLyThietBi_FormClosing(object? sender, FormClosingEventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void GanBieuTuongNutBam()
        {
            btnMoi.Image = VeBieuTuong("moi");
            btnNhap.Image = VeBieuTuong("nhap");
            btnSua.Image = VeBieuTuong("sua");
            btnXoa.Image = VeBieuTuong("xoa");
            btnThoat.Image = VeBieuTuong("thoat");
        }

        private static Bitmap VeBieuTuong(string loai)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            switch (loai)
            {
                case "moi":
                    using (Pen penDoc = new Pen(Color.FromArgb(30, 110, 210), 1.6f))
                    {
                        g.DrawRectangle(penDoc, 3, 2, 10, 12);
                        g.DrawLine(penDoc, 5, 6, 11, 6);
                        g.DrawLine(penDoc, 5, 9, 11, 9);
                    }
                    break;

                case "nhap":
                    using (Pen penPlus = new Pen(Color.FromArgb(40, 160, 65), 2.4f))
                    {
                        g.DrawLine(penPlus, 8, 2, 8, 14);
                        g.DrawLine(penPlus, 2, 8, 14, 8);
                    }
                    break;

                case "sua":
                    using (Pen penEdit = new Pen(Color.FromArgb(215, 140, 20), 2.2f))
                    {
                        g.DrawLine(penEdit, 3, 13, 13, 3);
                        g.DrawLine(penEdit, 2, 14, 5, 13);
                    }
                    break;

                case "xoa":
                    using (Pen penDel = new Pen(Color.FromArgb(210, 35, 42), 2.2f))
                    {
                        g.DrawLine(penDel, 3, 3, 13, 13);
                        g.DrawLine(penDel, 13, 3, 3, 13);
                    }
                    break;

                case "thoat":
                    using (Pen penExit = new Pen(Color.FromArgb(200, 30, 35), 2.0f))
                    {
                        g.DrawArc(penExit, 3, 4, 10, 10, -55, 290);
                        g.DrawLine(penExit, 8, 2, 8, 8);
                    }
                    break;
            }

            return bmp;
        }
    }
}
