using System.Drawing;
using System.Windows.Forms;

namespace QuanLySinhVien.Forms
{
    partial class FormQuanLySinhVien
    {
        private System.ComponentModel.IContainer components = null;

        // Header controls
        private Panel pnlTopBar;
        private Label lblLogoIcon;
        private Label lblAppTitle;
        private Panel pnlHeaderBanner;
        private Label lblMainTitle;
        private Label lblSubTitle;

        // Thông tin sinh viên card
        private Panel pnlThongTin;
        private Label lblThongTinAccent;
        private Label lblThongTinTitle;

        // Row 1
        private Label lblMaSV;
        private TextBox txtMaSV;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblLopHoc;
        private ComboBox cboLopHoc;

        // Row 2
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblGioiTinh;
        private Panel pnlGioiTinh;
        private RadioButton rdoNam;
        private RadioButton rdoNu;
        private Label lblDiem;
        private NumericUpDown nudDiem;

        // Row 3
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblDienThoai;
        private TextBox txtDienThoai;
        private Label lblTrangThai;
        private ComboBox cboTrangThai;

        // Buttons
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        // Filter panel
        private Panel pnlFilter;
        private Label lblTuKhoa;
        private TextBox txtTuKhoa;
        private Label lblLocLop;
        private ComboBox cboLocLop;
        private Label lblDiemTu;
        private NumericUpDown nudDiemTu;
        private Button btnTimKiem;
        private Button btnHienThiTatCa;

        // Grid panel
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
            var fontRegular = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            var fontBold = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            var fontHeader = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            var fontSectionTitle = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            var fontSmall = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);

            this.components = new System.ComponentModel.Container();

            // 1. Top Bar
            this.pnlTopBar = new Panel();
            this.lblLogoIcon = new Label();
            this.lblAppTitle = new Label();

            this.pnlTopBar.BackColor = Color.FromArgb(15, 42, 74);
            this.pnlTopBar.Dock = DockStyle.Top;
            this.pnlTopBar.Height = 40;
            this.pnlTopBar.Padding = new Padding(16, 6, 16, 6);

            this.lblLogoIcon.BackColor = Color.FromArgb(234, 88, 12);
            this.lblLogoIcon.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblLogoIcon.ForeColor = Color.White;
            this.lblLogoIcon.Location = new Point(16, 7);
            this.lblLogoIcon.Size = new Size(26, 26);
            this.lblLogoIcon.Text = "S";
            this.lblLogoIcon.TextAlign = ContentAlignment.MiddleCenter;

            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblAppTitle.ForeColor = Color.White;
            this.lblAppTitle.Location = new Point(48, 10);
            this.lblAppTitle.Text = "Ứng dụng quản lý sinh viên";

            this.pnlTopBar.Controls.Add(this.lblLogoIcon);
            this.pnlTopBar.Controls.Add(this.lblAppTitle);

            // 2. Header Banner
            this.pnlHeaderBanner = new Panel();
            this.lblMainTitle = new Label();
            this.lblSubTitle = new Label();

            this.pnlHeaderBanner.BackColor = Color.FromArgb(248, 250, 252);
            this.pnlHeaderBanner.Dock = DockStyle.Top;
            this.pnlHeaderBanner.Height = 55;
            this.pnlHeaderBanner.Padding = new Padding(20, 10, 20, 0);

            this.lblMainTitle.AutoSize = true;
            this.lblMainTitle.Font = fontHeader;
            this.lblMainTitle.ForeColor = Color.FromArgb(15, 42, 74);
            this.lblMainTitle.Location = new Point(20, 10);
            this.lblMainTitle.Text = "QUẢN LÝ SINH VIÊN";

            this.lblSubTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = fontRegular;
            this.lblSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblSubTitle.Location = new Point(720, 18);
            this.lblSubTitle.Text = "Bài tập Windows Forms • Quan hệ SchoolClass 1 — n Student";

            this.pnlHeaderBanner.Controls.Add(this.lblMainTitle);
            this.pnlHeaderBanner.Controls.Add(this.lblSubTitle);

            // 3. Thông tin sinh viên card
            this.pnlThongTin = new Panel();
            this.lblThongTinAccent = new Label();
            this.lblThongTinTitle = new Label();

            this.pnlThongTin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlThongTin.BackColor = Color.White;
            this.pnlThongTin.BorderStyle = BorderStyle.FixedSingle;
            this.pnlThongTin.Location = new Point(20, 100);
            this.pnlThongTin.Size = new Size(1124, 200);

            // Accent & Title
            this.lblThongTinAccent.BackColor = Color.FromArgb(234, 88, 12);
            this.lblThongTinAccent.Location = new Point(16, 14);
            this.lblThongTinAccent.Size = new Size(4, 20);

            this.lblThongTinTitle.AutoSize = true;
            this.lblThongTinTitle.Font = fontSectionTitle;
            this.lblThongTinTitle.ForeColor = Color.FromArgb(15, 42, 74);
            this.lblThongTinTitle.Location = new Point(26, 13);
            this.lblThongTinTitle.Text = "Thông tin sinh viên";

            // Row 1: MaSV (X:20), HoTen (X:390), LopHoc (X:760)
            this.lblMaSV = new Label();
            this.lblMaSV.AutoSize = true;
            this.lblMaSV.Font = fontRegular;
            this.lblMaSV.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblMaSV.Location = new Point(20, 52);
            this.lblMaSV.Text = "Mã sinh viên *";

            this.txtMaSV = new TextBox();
            this.txtMaSV.Font = fontRegular;
            this.txtMaSV.Location = new Point(130, 48);
            this.txtMaSV.Size = new Size(230, 29);
            this.txtMaSV.TabIndex = 0;

            this.lblHoTen = new Label();
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = fontRegular;
            this.lblHoTen.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblHoTen.Location = new Point(390, 52);
            this.lblHoTen.Text = "Họ và tên *";

            this.txtHoTen = new TextBox();
            this.txtHoTen.Font = fontRegular;
            this.txtHoTen.Location = new Point(480, 48);
            this.txtHoTen.Size = new Size(250, 29);
            this.txtHoTen.TabIndex = 1;

            this.lblLopHoc = new Label();
            this.lblLopHoc.AutoSize = true;
            this.lblLopHoc.Font = fontRegular;
            this.lblLopHoc.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblLopHoc.Location = new Point(760, 52);
            this.lblLopHoc.Text = "Lớp học *";

            this.cboLopHoc = new ComboBox();
            this.cboLopHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboLopHoc.Font = fontRegular;
            this.cboLopHoc.Location = new Point(840, 48);
            this.cboLopHoc.Size = new Size(260, 29);
            this.cboLopHoc.TabIndex = 2;

            // Row 2: NgaySinh (X:20), GioiTinh (X:390), Diem (X:760)
            this.lblNgaySinh = new Label();
            this.lblNgaySinh.AutoSize = true;
            this.lblNgaySinh.Font = fontRegular;
            this.lblNgaySinh.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblNgaySinh.Location = new Point(20, 92);
            this.lblNgaySinh.Text = "Ngày sinh";

            this.dtpNgaySinh = new DateTimePicker();
            this.dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            this.dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            this.dtpNgaySinh.Font = fontRegular;
            this.dtpNgaySinh.Location = new Point(130, 88);
            this.dtpNgaySinh.Size = new Size(230, 29);
            this.dtpNgaySinh.TabIndex = 3;

            this.lblGioiTinh = new Label();
            this.lblGioiTinh.AutoSize = true;
            this.lblGioiTinh.Font = fontRegular;
            this.lblGioiTinh.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblGioiTinh.Location = new Point(390, 92);
            this.lblGioiTinh.Text = "Giới tính";

            this.pnlGioiTinh = new Panel();
            this.pnlGioiTinh.Location = new Point(480, 88);
            this.pnlGioiTinh.Size = new Size(250, 28);

            this.rdoNam = new RadioButton();
            this.rdoNam.AutoSize = true;
            this.rdoNam.Checked = true;
            this.rdoNam.Font = fontRegular;
            this.rdoNam.Location = new Point(0, 3);
            this.rdoNam.Size = new Size(62, 23);
            this.rdoNam.TabIndex = 4;
            this.rdoNam.Text = "Nam";

            this.rdoNu = new RadioButton();
            this.rdoNu.AutoSize = true;
            this.rdoNu.Font = fontRegular;
            this.rdoNu.Location = new Point(80, 3);
            this.rdoNu.Size = new Size(49, 23);
            this.rdoNu.TabIndex = 5;
            this.rdoNu.Text = "Nữ";

            this.pnlGioiTinh.Controls.Add(this.rdoNam);
            this.pnlGioiTinh.Controls.Add(this.rdoNu);

            this.lblDiem = new Label();
            this.lblDiem.AutoSize = true;
            this.lblDiem.Font = fontRegular;
            this.lblDiem.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblDiem.Location = new Point(760, 92);
            this.lblDiem.Text = "Điểm *";

            this.nudDiem = new NumericUpDown();
            this.nudDiem.DecimalPlaces = 1;
            this.nudDiem.Font = fontRegular;
            this.nudDiem.Increment = new decimal(new int[] { 5, 0, 0, 65536 }); // 0.5
            this.nudDiem.Location = new Point(840, 88);
            this.nudDiem.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudDiem.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.nudDiem.Size = new Size(260, 29);
            this.nudDiem.TabIndex = 6;
            this.nudDiem.Value = new decimal(new int[] { 85, 0, 0, 65536 }); // 8.5

            // Row 3: Email (X:20), DienThoai (X:390), TrangThai (X:760)
            this.lblEmail = new Label();
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = fontRegular;
            this.lblEmail.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblEmail.Location = new Point(20, 130);
            this.lblEmail.Text = "Email *";

            this.txtEmail = new TextBox();
            this.txtEmail.Font = fontRegular;
            this.txtEmail.Location = new Point(130, 126);
            this.txtEmail.Size = new Size(230, 29);
            this.txtEmail.TabIndex = 7;

            this.lblDienThoai = new Label();
            this.lblDienThoai.AutoSize = true;
            this.lblDienThoai.Font = fontRegular;
            this.lblDienThoai.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblDienThoai.Location = new Point(390, 130);
            this.lblDienThoai.Text = "Điện thoại *";

            this.txtDienThoai = new TextBox();
            this.txtDienThoai.Font = fontRegular;
            this.txtDienThoai.Location = new Point(480, 126);
            this.txtDienThoai.Size = new Size(250, 29);
            this.txtDienThoai.TabIndex = 8;

            this.lblTrangThai = new Label();
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = fontRegular;
            this.lblTrangThai.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblTrangThai.Location = new Point(760, 130);
            this.lblTrangThai.Text = "Trạng thái";

            this.cboTrangThai = new ComboBox();
            this.cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = fontRegular;
            this.cboTrangThai.Location = new Point(840, 126);
            this.cboTrangThai.Size = new Size(260, 29);
            this.cboTrangThai.TabIndex = 9;

            // Row 4: Buttons (Right aligned at bottom of pnlThongTin)
            this.btnThem = new Button();
            this.btnThem.BackColor = Color.FromArgb(22, 163, 74); // Green
            this.btnThem.Cursor = Cursors.Hand;
            this.btnThem.FlatStyle = FlatStyle.Flat;
            this.btnThem.Font = fontBold;
            this.btnThem.ForeColor = Color.White;
            this.btnThem.Location = new Point(720, 162);
            this.btnThem.Size = new Size(88, 30);
            this.btnThem.TabIndex = 10;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;

            this.btnSua = new Button();
            this.btnSua.BackColor = Color.FromArgb(37, 99, 235); // Blue
            this.btnSua.Cursor = Cursors.Hand;
            this.btnSua.FlatStyle = FlatStyle.Flat;
            this.btnSua.Font = fontBold;
            this.btnSua.ForeColor = Color.White;
            this.btnSua.Location = new Point(818, 162);
            this.btnSua.Size = new Size(88, 30);
            this.btnSua.TabIndex = 11;
            this.btnSua.Text = "✎ Sửa";
            this.btnSua.UseVisualStyleBackColor = false;

            this.btnXoa = new Button();
            this.btnXoa.BackColor = Color.FromArgb(220, 38, 38); // Red
            this.btnXoa.Cursor = Cursors.Hand;
            this.btnXoa.FlatStyle = FlatStyle.Flat;
            this.btnXoa.Font = fontBold;
            this.btnXoa.ForeColor = Color.White;
            this.btnXoa.Location = new Point(916, 162);
            this.btnXoa.Size = new Size(88, 30);
            this.btnXoa.TabIndex = 12;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;

            this.btnLamMoi = new Button();
            this.btnLamMoi.BackColor = Color.FromArgb(75, 85, 99); // Dark slate
            this.btnLamMoi.Cursor = Cursors.Hand;
            this.btnLamMoi.FlatStyle = FlatStyle.Flat;
            this.btnLamMoi.Font = fontBold;
            this.btnLamMoi.ForeColor = Color.White;
            this.btnLamMoi.Location = new Point(1014, 162);
            this.btnLamMoi.Size = new Size(88, 30);
            this.btnLamMoi.TabIndex = 13;
            this.btnLamMoi.Text = "↻ Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;

            this.pnlThongTin.Controls.Add(this.lblThongTinAccent);
            this.pnlThongTin.Controls.Add(this.lblThongTinTitle);
            this.pnlThongTin.Controls.Add(this.lblMaSV);
            this.pnlThongTin.Controls.Add(this.txtMaSV);
            this.pnlThongTin.Controls.Add(this.lblHoTen);
            this.pnlThongTin.Controls.Add(this.txtHoTen);
            this.pnlThongTin.Controls.Add(this.lblLopHoc);
            this.pnlThongTin.Controls.Add(this.cboLopHoc);
            this.pnlThongTin.Controls.Add(this.lblNgaySinh);
            this.pnlThongTin.Controls.Add(this.dtpNgaySinh);
            this.pnlThongTin.Controls.Add(this.lblGioiTinh);
            this.pnlThongTin.Controls.Add(this.pnlGioiTinh);
            this.pnlThongTin.Controls.Add(this.lblDiem);
            this.pnlThongTin.Controls.Add(this.nudDiem);
            this.pnlThongTin.Controls.Add(this.lblEmail);
            this.pnlThongTin.Controls.Add(this.txtEmail);
            this.pnlThongTin.Controls.Add(this.lblDienThoai);
            this.pnlThongTin.Controls.Add(this.txtDienThoai);
            this.pnlThongTin.Controls.Add(this.lblTrangThai);
            this.pnlThongTin.Controls.Add(this.cboTrangThai);
            this.pnlThongTin.Controls.Add(this.btnThem);
            this.pnlThongTin.Controls.Add(this.btnSua);
            this.pnlThongTin.Controls.Add(this.btnXoa);
            this.pnlThongTin.Controls.Add(this.btnLamMoi);

            // 4. Filter Panel
            this.pnlFilter = new Panel();
            this.lblTuKhoa = new Label();
            this.txtTuKhoa = new TextBox();
            this.lblLocLop = new Label();
            this.cboLocLop = new ComboBox();
            this.lblDiemTu = new Label();
            this.nudDiemTu = new NumericUpDown();
            this.btnTimKiem = new Button();
            this.btnHienThiTatCa = new Button();

            this.pnlFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlFilter.BackColor = Color.White;
            this.pnlFilter.BorderStyle = BorderStyle.FixedSingle;
            this.pnlFilter.Location = new Point(20, 310);
            this.pnlFilter.Size = new Size(1124, 52);

            this.lblTuKhoa.AutoSize = true;
            this.lblTuKhoa.Font = fontRegular;
            this.lblTuKhoa.Location = new Point(14, 15);
            this.lblTuKhoa.Text = "Từ khóa";

            this.txtTuKhoa.Font = fontRegular;
            this.txtTuKhoa.Location = new Point(78, 12);
            this.txtTuKhoa.Size = new Size(270, 29);
            this.txtTuKhoa.TabIndex = 14;

            this.lblLocLop.AutoSize = true;
            this.lblLocLop.Font = fontRegular;
            this.lblLocLop.Location = new Point(368, 15);
            this.lblLocLop.Text = "Lớp";

            this.cboLocLop.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboLocLop.Font = fontRegular;
            this.cboLocLop.Location = new Point(404, 12);
            this.cboLocLop.Size = new Size(190, 29);
            this.cboLocLop.TabIndex = 15;

            this.lblDiemTu.AutoSize = true;
            this.lblDiemTu.Font = fontRegular;
            this.lblDiemTu.Location = new Point(610, 15);
            this.lblDiemTu.Text = "Điểm từ";

            this.nudDiemTu.DecimalPlaces = 1;
            this.nudDiemTu.Font = fontRegular;
            this.nudDiemTu.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            this.nudDiemTu.Location = new Point(676, 12);
            this.nudDiemTu.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudDiemTu.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.nudDiemTu.Size = new Size(80, 29);
            this.nudDiemTu.TabIndex = 16;
            this.nudDiemTu.Value = new decimal(new int[] { 0, 0, 0, 0 });

            this.btnTimKiem.BackColor = Color.FromArgb(37, 99, 235);
            this.btnTimKiem.Cursor = Cursors.Hand;
            this.btnTimKiem.FlatStyle = FlatStyle.Flat;
            this.btnTimKiem.Font = fontBold;
            this.btnTimKiem.ForeColor = Color.White;
            this.btnTimKiem.Location = new Point(774, 11);
            this.btnTimKiem.Size = new Size(100, 30);
            this.btnTimKiem.TabIndex = 17;
            this.btnTimKiem.Text = "🔍 Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;

            this.btnHienThiTatCa.BackColor = Color.White;
            this.btnHienThiTatCa.Cursor = Cursors.Hand;
            this.btnHienThiTatCa.FlatStyle = FlatStyle.Flat;
            this.btnHienThiTatCa.Font = fontRegular;
            this.btnHienThiTatCa.ForeColor = Color.FromArgb(51, 65, 85);
            this.btnHienThiTatCa.Location = new Point(884, 11);
            this.btnHienThiTatCa.Size = new Size(120, 30);
            this.btnHienThiTatCa.TabIndex = 18;
            this.btnHienThiTatCa.Text = "Hiển thị tất cả";
            this.btnHienThiTatCa.UseVisualStyleBackColor = false;

            this.pnlFilter.Controls.Add(this.lblTuKhoa);
            this.pnlFilter.Controls.Add(this.txtTuKhoa);
            this.pnlFilter.Controls.Add(this.lblLocLop);
            this.pnlFilter.Controls.Add(this.cboLocLop);
            this.pnlFilter.Controls.Add(this.lblDiemTu);
            this.pnlFilter.Controls.Add(this.nudDiemTu);
            this.pnlFilter.Controls.Add(this.btnTimKiem);
            this.pnlFilter.Controls.Add(this.btnHienThiTatCa);

            // 5. DataGridView Container
            this.pnlGridContainer = new Panel();
            this.lblDanhSachTitle = new Label();
            this.lblTongSo = new Label();
            this.dgvSinhVien = new DataGridView();
            this.lblHuongDan = new Label();
            this.lblGhiChu = new Label();

            this.pnlGridContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlGridContainer.BackColor = Color.White;
            this.pnlGridContainer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlGridContainer.Location = new Point(20, 372);
            this.pnlGridContainer.Size = new Size(1124, 380);

            this.lblDanhSachTitle.AutoSize = true;
            this.lblDanhSachTitle.Font = fontSectionTitle;
            this.lblDanhSachTitle.ForeColor = Color.FromArgb(15, 42, 74);
            this.lblDanhSachTitle.Location = new Point(16, 12);
            this.lblDanhSachTitle.Text = "Danh sách sinh viên";

            this.lblTongSo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblTongSo.AutoSize = true;
            this.lblTongSo.Font = fontBold;
            this.lblTongSo.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblTongSo.Location = new Point(960, 12);
            this.lblTongSo.Text = "Tổng số: 4 sinh viên";

            // Grid styling
            this.dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvSinhVien.AllowUserToAddRows = false;
            this.dgvSinhVien.AllowUserToDeleteRows = false;
            this.dgvSinhVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSinhVien.BackgroundColor = Color.White;
            this.dgvSinhVien.BorderStyle = BorderStyle.None;
            this.dgvSinhVien.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSinhVien.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvSinhVien.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            this.dgvSinhVien.ColumnHeadersDefaultCellStyle.Font = fontBold;
            this.dgvSinhVien.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            this.dgvSinhVien.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 10, 6, 10);
            this.dgvSinhVien.ColumnHeadersHeight = 40;
            this.dgvSinhVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSinhVien.DefaultCellStyle.Font = fontRegular;
            this.dgvSinhVien.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            this.dgvSinhVien.DefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            this.dgvSinhVien.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            this.dgvSinhVien.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            this.dgvSinhVien.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            this.dgvSinhVien.EnableHeadersVisualStyles = false;
            this.dgvSinhVien.GridColor = Color.FromArgb(226, 232, 240);
            this.dgvSinhVien.Location = new Point(16, 42);
            this.dgvSinhVien.MultiSelect = false;
            this.dgvSinhVien.ReadOnly = true;
            this.dgvSinhVien.RowHeadersVisible = false;
            this.dgvSinhVien.RowTemplate.Height = 36;
            this.dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSinhVien.Size = new Size(1090, 298);
            this.dgvSinhVien.TabIndex = 19;

            this.lblHuongDan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblHuongDan.AutoSize = true;
            this.lblHuongDan.Font = fontSmall;
            this.lblHuongDan.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblHuongDan.Location = new Point(16, 350);
            this.lblHuongDan.Text = "Chọn một dòng để xem, sửa hoặc xóa thông tin sinh viên.";

            this.lblGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = fontSmall;
            this.lblGhiChu.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblGhiChu.Location = new Point(940, 350);
            this.lblGhiChu.Text = "Các trường có dấu * là bắt buộc.";

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
            this.statusStrip.Font = fontSmall;
            this.statusStrip.ForeColor = Color.FromArgb(71, 85, 105);

            this.lblStatusLeft.Name = "lblStatusLeft";
            this.lblStatusLeft.Size = new Size(330, 20);
            this.lblStatusLeft.Text = "Bài tập: xây dựng Windows Forms quản lý sinh viên theo lớp";

            this.lblStatusRight.Name = "lblStatusRight";
            this.lblStatusRight.Spring = true;
            this.lblStatusRight.TextAlign = ContentAlignment.MiddleRight;
            this.lblStatusRight.Text = "Thêm • Sửa • Xóa • Tìm kiếm • Lọc theo lớp";

            this.statusStrip.Items.AddRange(new ToolStripItem[] {
                this.lblStatusLeft,
                this.lblStatusRight
            });

            // Form QuanLySinhVien settings
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.ClientSize = new Size(1164, 785);
            this.MinimumSize = new Size(1000, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ứng dụng quản lý sinh viên";

            this.Controls.Add(this.pnlGridContainer);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlThongTin);
            this.Controls.Add(this.pnlHeaderBanner);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.statusStrip);
        }
    }
}
