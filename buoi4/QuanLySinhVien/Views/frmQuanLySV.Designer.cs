using System.Drawing;
using System.Windows.Forms;

namespace QuanLySinhVien.Views
{
    partial class frmQuanLySV
    {
        private System.ComponentModel.IContainer components = null;

        // Top bar & Header
        private Panel pnlTopBar;
        private Label lblLogoIcon;
        private Label lblAppTitle;
        private Panel pnlHeaderBanner;
        private Label lblMainTitle;
        private Label lblSubTitle;

        // Card Thông tin sinh viên
        private Panel pnlThongTin;
        private Label lblThongTinAccent;
        private Label lblThongTinTitle;

        // Input controls (Row 1)
        private Label lblMaSV;
        private TextBox txtMaSV;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblLopHoc;
        private ComboBox cboLop;

        // Input controls (Row 2)
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblGioiTinh;
        private Panel pnlGioiTinh;
        private RadioButton rdoNam;
        private RadioButton rdoNu;
        private Label lblDiem;
        private NumericUpDown numDiem;

        // Input controls (Row 3)
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblDienThoai;
        private TextBox txtDienThoai;
        private Label lblTrangThai;
        private ComboBox cboTrangThai;

        // Action Buttons
        private Button butThem;
        private Button butSua;
        private Button butXoa;
        private Button butLamMoi;

        // Filter / Search Panel
        private Panel pnlFilter;
        private Label lblTuKhoa;
        private TextBox txtTuKhoa;
        private Label lblLocLop;
        private ComboBox cboFilterLop;
        private Label lblDiemTu;
        private NumericUpDown numDiemTu;
        private Button butTimKiem;
        private Button butHienThiTatCa;

        // DataGridView Panel
        private Panel pnlGridContainer;
        private Label lblDanhSachTitle;
        private Label lblTongSo;
        private DataGridView dgvSinhVien;
        private Label lblHuongDan;
        private Label lblGhiChu;

        // Status strip
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatusLeft;
        private ToolStripStatusLabel lblStatusRight;

        // ErrorProviders
        private ErrorProvider errPMaSV;
        private ErrorProvider erpHoten;
        private ErrorProvider erpEmail;
        private ErrorProvider erpDienThoai;
        private ErrorProvider erpLopHoc;
        private ErrorProvider erpDiem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            var fontRegular = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            var fontBold = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            var fontHeader = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            var fontSectionTitle = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            var fontSmall = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);

            // ErrorProviders
            this.errPMaSV = new ErrorProvider(this.components);
            this.erpHoten = new ErrorProvider(this.components);
            this.erpEmail = new ErrorProvider(this.components);
            this.erpDienThoai = new ErrorProvider(this.components);
            this.erpLopHoc = new ErrorProvider(this.components);
            this.erpDiem = new ErrorProvider(this.components);

            this.errPMaSV.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.erpHoten.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.erpEmail.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.erpDienThoai.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.erpLopHoc.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.erpDiem.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // 1. Top Bar
            this.pnlTopBar = new Panel();
            this.lblLogoIcon = new Label();
            this.lblAppTitle = new Label();

            this.pnlTopBar.BackColor = Color.FromArgb(25, 57, 97);
            this.pnlTopBar.Dock = DockStyle.Top;
            this.pnlTopBar.Height = 44;
            this.pnlTopBar.Padding = new Padding(18, 0, 18, 0);

            this.lblLogoIcon.Text = "S";
            this.lblLogoIcon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblLogoIcon.ForeColor = Color.White;
            this.lblLogoIcon.BackColor = Color.FromArgb(234, 88, 12);
            this.lblLogoIcon.Size = new Size(26, 26);
            this.lblLogoIcon.Location = new Point(20, 9);
            this.lblLogoIcon.TextAlign = ContentAlignment.MiddleCenter;

            this.lblAppTitle.Text = "Ứng dụng quản lý sinh viên (Mô hình 3 tầng DAL - BUL - GUI)";
            this.lblAppTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblAppTitle.ForeColor = Color.White;
            this.lblAppTitle.Location = new Point(56, 11);
            this.lblAppTitle.AutoSize = true;

            this.pnlTopBar.Controls.Add(this.lblLogoIcon);
            this.pnlTopBar.Controls.Add(this.lblAppTitle);

            // 2. Header Banner
            this.pnlHeaderBanner = new Panel();
            this.lblMainTitle = new Label();
            this.lblSubTitle = new Label();

            this.pnlHeaderBanner.BackColor = Color.FromArgb(246, 248, 251);
            this.pnlHeaderBanner.Dock = DockStyle.Top;
            this.pnlHeaderBanner.Height = 52;
            this.pnlHeaderBanner.Padding = new Padding(24, 8, 24, 0);

            this.lblMainTitle.Text = "QUẢN LÝ SINH VIÊN";
            this.lblMainTitle.Font = fontHeader;
            this.lblMainTitle.ForeColor = Color.FromArgb(20, 52, 91);
            this.lblMainTitle.Location = new Point(20, 10);
            this.lblMainTitle.AutoSize = true;

            this.lblSubTitle.Text = "Bài tập Windows Forms • Kiến trúc phân tầng (DAL - BUL - Views) • Quan hệ LopHoc 1 — N SinhVien";
            this.lblSubTitle.Font = fontSmall;
            this.lblSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblSubTitle.Location = new Point(480, 18);
            this.lblSubTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblSubTitle.AutoSize = true;

            this.pnlHeaderBanner.Controls.Add(this.lblMainTitle);
            this.pnlHeaderBanner.Controls.Add(this.lblSubTitle);

            // 3. Card Thông tin sinh viên
            this.pnlThongTin = new Panel();
            this.pnlThongTin.BackColor = Color.White;
            this.pnlThongTin.Location = new Point(20, 105);
            this.pnlThongTin.Size = new Size(1160, 205);
            this.pnlThongTin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlThongTin.BorderStyle = BorderStyle.FixedSingle;

            this.lblThongTinAccent = new Label();
            this.lblThongTinAccent.BackColor = Color.FromArgb(234, 88, 12);
            this.lblThongTinAccent.Location = new Point(16, 14);
            this.lblThongTinAccent.Size = new Size(4, 20);

            this.lblThongTinTitle = new Label();
            this.lblThongTinTitle.Text = "Thông tin sinh viên";
            this.lblThongTinTitle.Font = fontSectionTitle;
            this.lblThongTinTitle.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblThongTinTitle.Location = new Point(26, 13);
            this.lblThongTinTitle.AutoSize = true;

            // Row 1 (Y = 46)
            this.lblMaSV = new Label();
            this.lblMaSV.Text = "Mã sinh viên *";
            this.lblMaSV.Font = fontRegular;
            this.lblMaSV.Location = new Point(16, 49);
            this.lblMaSV.Size = new Size(100, 22);

            this.txtMaSV = new TextBox();
            this.txtMaSV.Font = fontRegular;
            this.txtMaSV.Location = new Point(120, 46);
            this.txtMaSV.Size = new Size(220, 26);
            this.txtMaSV.TabIndex = 0;

            this.lblHoTen = new Label();
            this.lblHoTen.Text = "Họ và tên *";
            this.lblHoTen.Font = fontRegular;
            this.lblHoTen.Location = new Point(370, 49);
            this.lblHoTen.Size = new Size(80, 22);

            this.txtHoTen = new TextBox();
            this.txtHoTen.Font = fontRegular;
            this.txtHoTen.Location = new Point(455, 46);
            this.txtHoTen.Size = new Size(250, 26);
            this.txtHoTen.TabIndex = 1;

            this.lblLopHoc = new Label();
            this.lblLopHoc.Text = "Lớp học *";
            this.lblLopHoc.Font = fontRegular;
            this.lblLopHoc.Location = new Point(735, 49);
            this.lblLopHoc.Size = new Size(75, 22);

            this.cboLop = new ComboBox();
            this.cboLop.Font = fontRegular;
            this.cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboLop.Location = new Point(815, 46);
            this.cboLop.Size = new Size(280, 26);
            this.cboLop.TabIndex = 2;

            // Row 2 (Y = 86)
            this.lblNgaySinh = new Label();
            this.lblNgaySinh.Text = "Ngày sinh";
            this.lblNgaySinh.Font = fontRegular;
            this.lblNgaySinh.Location = new Point(16, 89);
            this.lblNgaySinh.Size = new Size(100, 22);

            this.dtpNgaySinh = new DateTimePicker();
            this.dtpNgaySinh.Font = fontRegular;
            this.dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            this.dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            this.dtpNgaySinh.Location = new Point(120, 86);
            this.dtpNgaySinh.Size = new Size(220, 26);
            this.dtpNgaySinh.TabIndex = 3;

            this.lblGioiTinh = new Label();
            this.lblGioiTinh.Text = "Giới tính";
            this.lblGioiTinh.Font = fontRegular;
            this.lblGioiTinh.Location = new Point(370, 89);
            this.lblGioiTinh.Size = new Size(80, 22);

            this.pnlGioiTinh = new Panel();
            this.pnlGioiTinh.Location = new Point(455, 84);
            this.pnlGioiTinh.Size = new Size(250, 28);

            this.rdoNam = new RadioButton();
            this.rdoNam.Text = "Nam";
            this.rdoNam.Font = fontRegular;
            this.rdoNam.Checked = true;
            this.rdoNam.Location = new Point(5, 3);
            this.rdoNam.Size = new Size(70, 24);
            this.rdoNam.TabIndex = 4;

            this.rdoNu = new RadioButton();
            this.rdoNu.Text = "Nữ";
            this.rdoNu.Font = fontRegular;
            this.rdoNu.Location = new Point(85, 3);
            this.rdoNu.Size = new Size(70, 24);
            this.rdoNu.TabIndex = 5;

            this.pnlGioiTinh.Controls.Add(this.rdoNam);
            this.pnlGioiTinh.Controls.Add(this.rdoNu);

            this.lblDiem = new Label();
            this.lblDiem.Text = "Điểm *";
            this.lblDiem.Font = fontRegular;
            this.lblDiem.Location = new Point(735, 89);
            this.lblDiem.Size = new Size(75, 22);

            this.numDiem = new NumericUpDown();
            this.numDiem.Font = fontRegular;
            this.numDiem.DecimalPlaces = 1;
            this.numDiem.Increment = 0.1M;
            this.numDiem.Minimum = 0.0M;
            this.numDiem.Maximum = 10.0M;
            this.numDiem.Value = 0.0M;
            this.numDiem.Location = new Point(815, 86);
            this.numDiem.Size = new Size(280, 26);
            this.numDiem.TabIndex = 6;

            // Row 3 (Y = 126)
            this.lblEmail = new Label();
            this.lblEmail.Text = "Email *";
            this.lblEmail.Font = fontRegular;
            this.lblEmail.Location = new Point(16, 129);
            this.lblEmail.Size = new Size(100, 22);

            this.txtEmail = new TextBox();
            this.txtEmail.Font = fontRegular;
            this.txtEmail.Location = new Point(120, 126);
            this.txtEmail.Size = new Size(220, 26);
            this.txtEmail.TabIndex = 7;

            this.lblDienThoai = new Label();
            this.lblDienThoai.Text = "Điện thoại *";
            this.lblDienThoai.Font = fontRegular;
            this.lblDienThoai.Location = new Point(370, 129);
            this.lblDienThoai.Size = new Size(80, 22);

            this.txtDienThoai = new TextBox();
            this.txtDienThoai.Font = fontRegular;
            this.txtDienThoai.Location = new Point(455, 126);
            this.txtDienThoai.Size = new Size(250, 26);
            this.txtDienThoai.TabIndex = 8;

            this.lblTrangThai = new Label();
            this.lblTrangThai.Text = "Trạng thái";
            this.lblTrangThai.Font = fontRegular;
            this.lblTrangThai.Location = new Point(735, 129);
            this.lblTrangThai.Size = new Size(75, 22);

            this.cboTrangThai = new ComboBox();
            this.cboTrangThai.Font = fontRegular;
            this.cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboTrangThai.Location = new Point(815, 126);
            this.cboTrangThai.Size = new Size(280, 26);
            this.cboTrangThai.TabIndex = 9;

            // Action Buttons (Y = 165)
            this.butThem = new Button();
            this.butThem.Text = "Thêm";
            this.butThem.Font = fontBold;
            this.butThem.ForeColor = Color.White;
            this.butThem.BackColor = Color.FromArgb(31, 139, 90);
            this.butThem.FlatStyle = FlatStyle.Flat;
            this.butThem.FlatAppearance.BorderSize = 0;
            this.butThem.Size = new Size(95, 32);
            this.butThem.Location = new Point(710, 163);
            this.butThem.Cursor = Cursors.Hand;
            this.butThem.TabIndex = 10;

            this.butSua = new Button();
            this.butSua.Text = "✎ Sửa";
            this.butSua.Font = fontBold;
            this.butSua.ForeColor = Color.White;
            this.butSua.BackColor = Color.FromArgb(42, 111, 151);
            this.butSua.FlatStyle = FlatStyle.Flat;
            this.butSua.FlatAppearance.BorderSize = 0;
            this.butSua.Size = new Size(95, 32);
            this.butSua.Location = new Point(815, 163);
            this.butSua.Cursor = Cursors.Hand;
            this.butSua.TabIndex = 11;

            this.butXoa = new Button();
            this.butXoa.Text = "Xóa";
            this.butXoa.Font = fontBold;
            this.butXoa.ForeColor = Color.White;
            this.butXoa.BackColor = Color.FromArgb(188, 56, 56);
            this.butXoa.FlatStyle = FlatStyle.Flat;
            this.butXoa.FlatAppearance.BorderSize = 0;
            this.butXoa.Size = new Size(95, 32);
            this.butXoa.Location = new Point(920, 163);
            this.butXoa.Cursor = Cursors.Hand;
            this.butXoa.TabIndex = 12;

            this.butLamMoi = new Button();
            this.butLamMoi.Text = "⟳ Làm mới";
            this.butLamMoi.Font = fontBold;
            this.butLamMoi.ForeColor = Color.White;
            this.butLamMoi.BackColor = Color.FromArgb(90, 107, 124);
            this.butLamMoi.FlatStyle = FlatStyle.Flat;
            this.butLamMoi.FlatAppearance.BorderSize = 0;
            this.butLamMoi.Size = new Size(100, 32);
            this.butLamMoi.Location = new Point(1025, 163);
            this.butLamMoi.Cursor = Cursors.Hand;
            this.butLamMoi.TabIndex = 13;

            this.pnlThongTin.Controls.Add(this.lblThongTinAccent);
            this.pnlThongTin.Controls.Add(this.lblThongTinTitle);
            this.pnlThongTin.Controls.Add(this.lblMaSV);
            this.pnlThongTin.Controls.Add(this.txtMaSV);
            this.pnlThongTin.Controls.Add(this.lblHoTen);
            this.pnlThongTin.Controls.Add(this.txtHoTen);
            this.pnlThongTin.Controls.Add(this.lblLopHoc);
            this.pnlThongTin.Controls.Add(this.cboLop);
            this.pnlThongTin.Controls.Add(this.lblNgaySinh);
            this.pnlThongTin.Controls.Add(this.dtpNgaySinh);
            this.pnlThongTin.Controls.Add(this.lblGioiTinh);
            this.pnlThongTin.Controls.Add(this.pnlGioiTinh);
            this.pnlThongTin.Controls.Add(this.lblDiem);
            this.pnlThongTin.Controls.Add(this.numDiem);
            this.pnlThongTin.Controls.Add(this.lblEmail);
            this.pnlThongTin.Controls.Add(this.txtEmail);
            this.pnlThongTin.Controls.Add(this.lblDienThoai);
            this.pnlThongTin.Controls.Add(this.txtDienThoai);
            this.pnlThongTin.Controls.Add(this.lblTrangThai);
            this.pnlThongTin.Controls.Add(this.cboTrangThai);
            this.pnlThongTin.Controls.Add(this.butThem);
            this.pnlThongTin.Controls.Add(this.butSua);
            this.pnlThongTin.Controls.Add(this.butXoa);
            this.pnlThongTin.Controls.Add(this.butLamMoi);

            // 4. Panel Filter / Search
            this.pnlFilter = new Panel();
            this.pnlFilter.BackColor = Color.White;
            this.pnlFilter.Location = new Point(20, 318);
            this.pnlFilter.Size = new Size(1160, 48);
            this.pnlFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlFilter.BorderStyle = BorderStyle.FixedSingle;

            this.lblTuKhoa = new Label();
            this.lblTuKhoa.Text = "Từ khóa";
            this.lblTuKhoa.Font = fontBold;
            this.lblTuKhoa.Location = new Point(14, 14);
            this.lblTuKhoa.Size = new Size(60, 22);

            this.txtTuKhoa = new TextBox();
            this.txtTuKhoa.Font = fontRegular;
            this.txtTuKhoa.Location = new Point(80, 11);
            this.txtTuKhoa.Size = new Size(270, 26);
            this.txtTuKhoa.PlaceholderText = "Mã, họ tên, email hoặc điện thoại";
            this.txtTuKhoa.TabIndex = 14;

            this.lblLocLop = new Label();
            this.lblLocLop.Text = "Lớp";
            this.lblLocLop.Font = fontBold;
            this.lblLocLop.Location = new Point(365, 14);
            this.lblLocLop.Size = new Size(35, 22);

            this.cboFilterLop = new ComboBox();
            this.cboFilterLop.Font = fontRegular;
            this.cboFilterLop.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboFilterLop.Location = new Point(405, 11);
            this.cboFilterLop.Size = new Size(200, 26);
            this.cboFilterLop.TabIndex = 15;

            this.lblDiemTu = new Label();
            this.lblDiemTu.Text = "Điểm từ";
            this.lblDiemTu.Font = fontBold;
            this.lblDiemTu.Location = new Point(620, 14);
            this.lblDiemTu.Size = new Size(60, 22);

            this.numDiemTu = new NumericUpDown();
            this.numDiemTu.Font = fontRegular;
            this.numDiemTu.DecimalPlaces = 1;
            this.numDiemTu.Increment = 0.5M;
            this.numDiemTu.Minimum = 0.0M;
            this.numDiemTu.Maximum = 10.0M;
            this.numDiemTu.Value = 0.0M;
            this.numDiemTu.Location = new Point(685, 11);
            this.numDiemTu.Size = new Size(80, 26);
            this.numDiemTu.TabIndex = 16;

            this.butTimKiem = new Button();
            this.butTimKiem.Text = "⚲ Tìm kiếm";
            this.butTimKiem.Font = fontBold;
            this.butTimKiem.ForeColor = Color.White;
            this.butTimKiem.BackColor = Color.FromArgb(31, 94, 140);
            this.butTimKiem.FlatStyle = FlatStyle.Flat;
            this.butTimKiem.FlatAppearance.BorderSize = 0;
            this.butTimKiem.Size = new Size(100, 28);
            this.butTimKiem.Location = new Point(780, 10);
            this.butTimKiem.Cursor = Cursors.Hand;
            this.butTimKiem.TabIndex = 17;

            this.butHienThiTatCa = new Button();
            this.butHienThiTatCa.Text = "Hiển thị tất cả";
            this.butHienThiTatCa.Font = fontRegular;
            this.butHienThiTatCa.ForeColor = Color.FromArgb(30, 41, 59);
            this.butHienThiTatCa.BackColor = Color.FromArgb(236, 239, 243);
            this.butHienThiTatCa.FlatStyle = FlatStyle.Flat;
            this.butHienThiTatCa.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            this.butHienThiTatCa.Size = new Size(110, 28);
            this.butHienThiTatCa.Location = new Point(890, 10);
            this.butHienThiTatCa.Cursor = Cursors.Hand;
            this.butHienThiTatCa.TabIndex = 18;

            this.pnlFilter.Controls.Add(this.lblTuKhoa);
            this.pnlFilter.Controls.Add(this.txtTuKhoa);
            this.pnlFilter.Controls.Add(this.lblLocLop);
            this.pnlFilter.Controls.Add(this.cboFilterLop);
            this.pnlFilter.Controls.Add(this.lblDiemTu);
            this.pnlFilter.Controls.Add(this.numDiemTu);
            this.pnlFilter.Controls.Add(this.butTimKiem);
            this.pnlFilter.Controls.Add(this.butHienThiTatCa);

            // 5. Panel DataGridView Container
            this.pnlGridContainer = new Panel();
            this.pnlGridContainer.BackColor = Color.White;
            this.pnlGridContainer.Location = new Point(20, 374);
            this.pnlGridContainer.Size = new Size(1160, 320);
            this.pnlGridContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlGridContainer.BorderStyle = BorderStyle.FixedSingle;

            this.lblDanhSachTitle = new Label();
            this.lblDanhSachTitle.Text = "Danh sách sinh viên";
            this.lblDanhSachTitle.Font = fontSectionTitle;
            this.lblDanhSachTitle.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblDanhSachTitle.Location = new Point(16, 12);
            this.lblDanhSachTitle.AutoSize = true;

            this.lblTongSo = new Label();
            this.lblTongSo.Text = "Tổng số: 0 sinh viên";
            this.lblTongSo.Font = fontBold;
            this.lblTongSo.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblTongSo.Location = new Point(1000, 14);
            this.lblTongSo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblTongSo.AutoSize = true;

            this.dgvSinhVien = new DataGridView();
            this.dgvSinhVien.Location = new Point(16, 42);
            this.dgvSinhVien.Size = new Size(1126, 240);
            this.dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvSinhVien.BackgroundColor = Color.FromArgb(248, 250, 252);
            this.dgvSinhVien.BorderStyle = BorderStyle.None;
            this.dgvSinhVien.RowHeadersVisible = false;
            this.dgvSinhVien.AllowUserToAddRows = false;
            this.dgvSinhVien.AllowUserToDeleteRows = false;
            this.dgvSinhVien.ReadOnly = true;
            this.dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSinhVien.MultiSelect = false;
            this.dgvSinhVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSinhVien.EnableHeadersVisualStyles = false;
            this.dgvSinhVien.ColumnHeadersHeight = 36;
            this.dgvSinhVien.RowTemplate.Height = 32;
            this.dgvSinhVien.Font = fontRegular;
            this.dgvSinhVien.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(236, 242, 250);
            this.dgvSinhVien.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
            this.dgvSinhVien.ColumnHeadersDefaultCellStyle.Font = fontBold;
            this.dgvSinhVien.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(243, 248, 255);
            this.dgvSinhVien.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            this.dgvSinhVien.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);

            this.lblHuongDan = new Label();
            this.lblHuongDan.Text = "Chọn một dòng để xem, sửa hoặc xóa thông tin sinh viên.";
            this.lblHuongDan.Font = fontSmall;
            this.lblHuongDan.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblHuongDan.Location = new Point(16, 292);
            this.lblHuongDan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblHuongDan.AutoSize = true;

            this.lblGhiChu = new Label();
            this.lblGhiChu.Text = "Các trường có dấu * là bắt buộc.";
            this.lblGhiChu.Font = fontSmall;
            this.lblGhiChu.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblGhiChu.Location = new Point(960, 292);
            this.lblGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.lblGhiChu.AutoSize = true;

            this.pnlGridContainer.Controls.Add(this.lblDanhSachTitle);
            this.pnlGridContainer.Controls.Add(this.lblTongSo);
            this.pnlGridContainer.Controls.Add(this.dgvSinhVien);
            this.pnlGridContainer.Controls.Add(this.lblHuongDan);
            this.pnlGridContainer.Controls.Add(this.lblGhiChu);

            // 6. Status Strip
            this.statusStrip = new StatusStrip();
            this.lblStatusLeft = new ToolStripStatusLabel();
            this.lblStatusRight = new ToolStripStatusLabel();

            this.statusStrip.BackColor = Color.FromArgb(241, 245, 249);
            this.lblStatusLeft.Text = "Bài tập: xây dựng Windows Forms quản lý sinh viên theo lớp (Mô hình 3 tầng DAL - BUL - Views)";
            this.lblStatusLeft.Spring = true;
            this.lblStatusLeft.TextAlign = ContentAlignment.MiddleLeft;

            this.lblStatusRight.Text = "Thêm • Sửa • Xóa • Tìm kiếm • Lọc theo lớp";
            this.lblStatusRight.TextAlign = ContentAlignment.MiddleRight;

            this.statusStrip.Items.AddRange(new ToolStripItem[] { this.lblStatusLeft, this.lblStatusRight });

            // Form properties
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.ClientSize = new Size(1200, 720);
            this.MinimumSize = new Size(1050, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý sinh viên - Bài thực hành 3 & 4 (Kiến trúc phân tầng 3-Layer)";

            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlGridContainer);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlThongTin);
            this.Controls.Add(this.pnlHeaderBanner);
            this.Controls.Add(this.pnlTopBar);
        }
    }
}
