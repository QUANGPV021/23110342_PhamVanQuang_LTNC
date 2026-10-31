using System.Drawing;
using System.Windows.Forms;

namespace QuanLyThuVienDAO.Views
{
    partial class frmQuanLyDauSach
    {
        private System.ComponentModel.IContainer? components = null;

        // Top Header & Search Bar
        private Panel pnlTopContainer = null!;
        private Panel pnlHeaderBanner = null!;
        private Label lblMainTitle = null!;
        private Label lblSubTitle = null!;
        private Panel pnlSearchBar = null!;
        private Label lblTuKhoa = null!;
        private TextBox txtTuKhoa = null!;
        private Label lblLocTheLoai = null!;
        private ComboBox cboLocTheLoai = null!;
        private Button btnTimKiem = null!;
        private Button btnLamMoi = null!;

        // Main Split Layout
        private TableLayoutPanel tlpMain = null!;

        // Left Panel: Thông tin đầu sách
        private GroupBox grpThongTinDauSach = null!;
        private Label lblMaSach = null!;
        private TextBox txtMaSach = null!;
        private Label lblTenSach = null!;
        private TextBox txtTenSach = null!;
        private Label lblTacGia = null!;
        private TextBox txtTacGia = null!;
        private Label lblNhaXuatBan = null!;
        private TextBox txtNhaXuatBan = null!;
        private Label lblTheLoai = null!;
        private ComboBox cboTheLoai = null!;
        private Label lblGiaSach = null!;
        private TextBox txtGiaSach = null!;
        private Label lblNamXuatBan = null!;
        private TextBox txtNamXuatBan = null!;
        private Label lblSoBanSao = null!;
        private TextBox txtSoBanSao = null!;

        // Left Action Buttons
        private Button btnThemDauSach = null!;
        private Button btnLuuChinhSua = null!;
        private Button btnXoaDauSach = null!;
        private Button btnLamTrong = null!;
        private Button btnNhapBoSungBanSao = null!;

        // Right Split Layout
        private TableLayoutPanel tlpRight = null!;

        // Right Top: Danh sách đầu sách
        private GroupBox grpDanhSachDauSach = null!;
        private DataGridView dgvDauSach = null!;

        // Right Bottom: Danh sách bản sao
        private GroupBox grpDanhSachBanSao = null!;
        private Panel pnlBanSaoBottom = null!;
        private DataGridView dgvBanSao = null!;
        private Label lblHuongDanBanSao = null!;
        private Button btnNhapThemBanSao = null!;
        private Button btnCapNhatBanSao = null!;
        private Button btnXoaBanSao = null!;

        // Status Strip
        private StatusStrip statusStrip = null!;
        private ToolStripStatusLabel lblStatus = null!;

        // Error Provider
        private ErrorProvider errorProvider = null!;

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
            var fontTitle = new Font("Segoe UI", 17F, FontStyle.Bold, GraphicsUnit.Point);
            var fontSubTitle = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            var fontGroup = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            var fontItalicHint = new Font("Segoe UI", 8.75F, FontStyle.Italic, GraphicsUnit.Point);

            this.errorProvider = new ErrorProvider(this.components)
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink
            };

            // =====================================================================
            // 1. TOP CONTAINER (HEADER + SEARCH BAR)
            // =====================================================================
            this.pnlTopContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 114,
                BackColor = Color.FromArgb(240, 247, 253)
            };

            this.pnlHeaderBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 66,
                BackColor = Color.FromArgb(236, 245, 252)
            };

            this.lblMainTitle = new Label
            {
                Text = "QUẢN LÝ ĐẦU SÁCH VÀ BẢN SAO",
                Font = fontTitle,
                ForeColor = Color.FromArgb(11, 106, 153),
                AutoSize = true,
                Location = new Point(16, 8)
            };

            this.lblSubTitle = new Label
            {
                Text = "Tra cứu, cập nhật thông tin đầu sách và quản lý bản sao",
                Font = fontSubTitle,
                ForeColor = Color.FromArgb(95, 112, 130),
                AutoSize = true,
                Location = new Point(18, 41)
            };

            this.pnlHeaderBanner.Controls.Add(this.lblMainTitle);
            this.pnlHeaderBanner.Controls.Add(this.lblSubTitle);

            this.pnlSearchBar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(246, 250, 254),
                Padding = new Padding(16, 8, 16, 8)
            };

            this.lblTuKhoa = new Label
            {
                Text = "Từ khóa tra cứu:",
                Font = fontBold,
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(18, 13)
            };

            this.txtTuKhoa = new TextBox
            {
                Font = fontRegular,
                Location = new Point(136, 9),
                Size = new Size(340, 27),
                PlaceholderText = "Mã sách, tên sách, tác giả..."
            };

            this.lblLocTheLoai = new Label
            {
                Text = "Thể loại:",
                Font = fontBold,
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(494, 13)
            };

            this.cboLocTheLoai = new ComboBox
            {
                Font = fontRegular,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(562, 9),
                Size = new Size(285, 27)
            };

            this.btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Font = fontBold,
                BackColor = Color.FromArgb(14, 118, 168),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(858, 8),
                Size = new Size(98, 30),
                Cursor = Cursors.Hand
            };
            this.btnTimKiem.FlatAppearance.BorderSize = 0;

            this.btnLamMoi = new Button
            {
                Text = "Làm mới",
                Font = fontRegular,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(964, 8),
                Size = new Size(92, 30),
                Cursor = Cursors.Hand
            };
            this.btnLamMoi.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);

            this.pnlSearchBar.Controls.Add(this.lblTuKhoa);
            this.pnlSearchBar.Controls.Add(this.txtTuKhoa);
            this.pnlSearchBar.Controls.Add(this.lblLocTheLoai);
            this.pnlSearchBar.Controls.Add(this.cboLocTheLoai);
            this.pnlSearchBar.Controls.Add(this.btnTimKiem);
            this.pnlSearchBar.Controls.Add(this.btnLamMoi);

            this.pnlTopContainer.Controls.Add(this.pnlSearchBar);
            this.pnlTopContainer.Controls.Add(this.pnlHeaderBanner);

            // =====================================================================
            // 2. MAIN SPLIT LAYOUT (LEFT FORM + RIGHT DATA GRIDS)
            // =====================================================================
            this.tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(12, 6, 12, 8),
                BackColor = Color.FromArgb(245, 248, 252)
            };
            this.tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
            this.tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ---------------------------------------------------------------------
            // 2.1 LEFT PANEL: THÔNG TIN ĐẦU SÁCH
            // ---------------------------------------------------------------------
            this.grpThongTinDauSach = new GroupBox
            {
                Text = "Thông tin đầu sách",
                Font = fontGroup,
                ForeColor = Color.FromArgb(15, 23, 42),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 10, 0),
                Padding = new Padding(12)
            };

            int rowY = 34;
            int rowStep = 40;
            int lblX = 14;
            int ctlX = 128;
            int ctlW = 204;

            this.lblMaSach = new Label
            {
                Text = "Mã sách (*):",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.txtMaSach = new TextBox
            {
                Font = fontRegular,
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27),
                MaxLength = 20
            };

            rowY += rowStep;
            this.lblTenSach = new Label
            {
                Text = "Tên sách (*):",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.txtTenSach = new TextBox
            {
                Font = fontRegular,
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27),
                MaxLength = 200
            };

            rowY += rowStep;
            this.lblTacGia = new Label
            {
                Text = "Tác giả (*):",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.txtTacGia = new TextBox
            {
                Font = fontRegular,
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27),
                MaxLength = 150
            };

            rowY += rowStep;
            this.lblNhaXuatBan = new Label
            {
                Text = "Nhà xuất bản:",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.txtNhaXuatBan = new TextBox
            {
                Font = fontRegular,
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27),
                MaxLength = 150
            };

            rowY += rowStep;
            this.lblTheLoai = new Label
            {
                Text = "Thể loại (*):",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.cboTheLoai = new ComboBox
            {
                Font = fontRegular,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27)
            };

            rowY += rowStep;
            this.lblGiaSach = new Label
            {
                Text = "Giá sách:",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.txtGiaSach = new TextBox
            {
                Font = fontRegular,
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27),
                TextAlign = HorizontalAlignment.Right,
                Text = "0"
            };

            rowY += rowStep;
            this.lblNamXuatBan = new Label
            {
                Text = "Năm xuất bản:",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.txtNamXuatBan = new TextBox
            {
                Font = fontRegular,
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27),
                TextAlign = HorizontalAlignment.Right,
                Text = "2026"
            };

            rowY += rowStep;
            this.lblSoBanSao = new Label
            {
                Text = "Số bản sao:",
                Font = fontRegular,
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(lblX, rowY + 4),
                AutoSize = true
            };
            this.txtSoBanSao = new TextBox
            {
                Font = fontBold,
                ForeColor = Color.FromArgb(29, 78, 216),
                BackColor = Color.FromArgb(241, 245, 249),
                Location = new Point(ctlX, rowY),
                Size = new Size(ctlW, 27),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Text = "0"
            };

            // Action Buttons (2x2 + 1 Full-Width)
            int btnTopY = rowY + 54;
            int btnW = 156;
            int btnH = 38;
            int btnGap = 8;

            this.btnThemDauSach = new Button
            {
                Text = "Thêm đầu sách",
                Font = fontBold,
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(lblX, btnTopY),
                Size = new Size(btnW, btnH),
                Cursor = Cursors.Hand
            };
            this.btnThemDauSach.FlatAppearance.BorderSize = 0;

            this.btnLuuChinhSua = new Button
            {
                Text = "Lưu chỉnh sửa",
                Font = fontBold,
                BackColor = Color.FromArgb(217, 119, 6),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(lblX + btnW + btnGap, btnTopY),
                Size = new Size(btnW, btnH),
                Cursor = Cursors.Hand
            };
            this.btnLuuChinhSua.FlatAppearance.BorderSize = 0;

            this.btnXoaDauSach = new Button
            {
                Text = "Xóa đầu sách",
                Font = fontBold,
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(lblX, btnTopY + btnH + btnGap),
                Size = new Size(btnW, btnH),
                Cursor = Cursors.Hand
            };
            this.btnXoaDauSach.FlatAppearance.BorderSize = 0;

            this.btnLamTrong = new Button
            {
                Text = "Làm trống",
                Font = fontBold,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(lblX + btnW + btnGap, btnTopY + btnH + btnGap),
                Size = new Size(btnW, btnH),
                Cursor = Cursors.Hand
            };
            this.btnLamTrong.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);

            this.btnNhapBoSungBanSao = new Button
            {
                Text = "NHẬP BỔ SUNG BẢN SAO",
                Font = fontBold,
                BackColor = Color.FromArgb(11, 114, 164),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(lblX, btnTopY + (btnH + btnGap) * 2),
                Size = new Size(btnW * 2 + btnGap, 40),
                Cursor = Cursors.Hand
            };
            this.btnNhapBoSungBanSao.FlatAppearance.BorderSize = 0;

            this.grpThongTinDauSach.Controls.AddRange(new Control[]
            {
                this.lblMaSach, this.txtMaSach,
                this.lblTenSach, this.txtTenSach,
                this.lblTacGia, this.txtTacGia,
                this.lblNhaXuatBan, this.txtNhaXuatBan,
                this.lblTheLoai, this.cboTheLoai,
                this.lblGiaSach, this.txtGiaSach,
                this.lblNamXuatBan, this.txtNamXuatBan,
                this.lblSoBanSao, this.txtSoBanSao,
                this.btnThemDauSach, this.btnLuuChinhSua,
                this.btnXoaDauSach, this.btnLamTrong,
                this.btnNhapBoSungBanSao
            });

            // ---------------------------------------------------------------------
            // 2.2 RIGHT PANEL: DANH SÁCH ĐẦU SÁCH & DANH SÁCH BẢN SAO
            // ---------------------------------------------------------------------
            this.tlpRight = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0)
            };
            this.tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
            this.tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));

            // Top Grid: Danh sách đầu sách
            this.grpDanhSachDauSach = new GroupBox
            {
                Text = "Danh sách đầu sách",
                Font = fontGroup,
                ForeColor = Color.FromArgb(15, 23, 42),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 8),
                Padding = new Padding(10, 8, 10, 10)
            };

            this.dgvDauSach = CreateStyledDataGridView(fontRegular);
            this.grpDanhSachDauSach.Controls.Add(this.dgvDauSach);

            // Bottom Grid: Danh sách bản sao
            this.grpDanhSachBanSao = new GroupBox
            {
                Text = "Danh sách bản sao",
                Font = fontGroup,
                ForeColor = Color.FromArgb(15, 23, 42),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(10, 8, 10, 8)
            };

            this.pnlBanSaoBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                BackColor = Color.White
            };

            this.lblHuongDanBanSao = new Label
            {
                Text = "Chọn một bản sao để cập nhật trạng thái, tình trạng hoặc xóa.",
                Font = fontItalicHint,
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(2, 4)
            };

            this.btnNhapThemBanSao = new Button
            {
                Text = "Nhập thêm bản sao",
                Font = fontRegular,
                BackColor = Color.FromArgb(236, 253, 245),
                ForeColor = Color.FromArgb(21, 128, 61),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(2, 27),
                Size = new Size(150, 32),
                Cursor = Cursors.Hand
            };
            this.btnNhapThemBanSao.FlatAppearance.BorderColor = Color.FromArgb(134, 239, 172);

            this.btnCapNhatBanSao = new Button
            {
                Text = "Cập nhật bản sao",
                Font = fontRegular,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(160, 27),
                Size = new Size(140, 32),
                Cursor = Cursors.Hand
            };
            this.btnCapNhatBanSao.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);

            this.btnXoaBanSao = new Button
            {
                Text = "Xóa bản sao",
                Font = fontRegular,
                BackColor = Color.FromArgb(254, 242, 242),
                ForeColor = Color.FromArgb(220, 38, 38),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(308, 27),
                Size = new Size(120, 32),
                Cursor = Cursors.Hand
            };
            this.btnXoaBanSao.FlatAppearance.BorderColor = Color.FromArgb(252, 165, 165);

            this.pnlBanSaoBottom.Controls.Add(this.lblHuongDanBanSao);
            this.pnlBanSaoBottom.Controls.Add(this.btnNhapThemBanSao);
            this.pnlBanSaoBottom.Controls.Add(this.btnCapNhatBanSao);
            this.pnlBanSaoBottom.Controls.Add(this.btnXoaBanSao);

            this.dgvBanSao = CreateStyledDataGridView(fontRegular);
            this.grpDanhSachBanSao.Controls.Add(this.dgvBanSao);
            this.grpDanhSachBanSao.Controls.Add(this.pnlBanSaoBottom);

            this.tlpRight.Controls.Add(this.grpDanhSachDauSach, 0, 0);
            this.tlpRight.Controls.Add(this.grpDanhSachBanSao, 0, 1);

            this.tlpMain.Controls.Add(this.grpThongTinDauSach, 0, 0);
            this.tlpMain.Controls.Add(this.tlpRight, 1, 0);

            // =====================================================================
            // 3. STATUS STRIP
            // =====================================================================
            this.statusStrip = new StatusStrip
            {
                BackColor = Color.FromArgb(241, 245, 249),
                SizingGrip = false
            };

            this.lblStatus = new ToolStripStatusLabel
            {
                Text = "Đang khởi tạo dữ liệu...",
                Font = fontRegular,
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            this.statusStrip.Items.Add(this.lblStatus);

            // =====================================================================
            // 4. FORM PROPERTIES
            // =====================================================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1100, 720);
            this.MinimumSize = new Size(1040, 690);
            this.BackColor = Color.FromArgb(245, 248, 252);
            this.Font = fontRegular;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý đầu sách và bản sao";

            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.pnlTopContainer);
            this.Controls.Add(this.statusStrip);
        }

        private static DataGridView CreateStyledDataGridView(Font cellFont)
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                CellBorderStyle = DataGridViewCellBorderStyle.Single,
                GridColor = Color.FromArgb(226, 232, 240),
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoGenerateColumns = false
            };

            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(217, 236, 247),
                ForeColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 4, 0),
                SelectionBackColor = Color.FromArgb(217, 236, 247),
                SelectionForeColor = Color.FromArgb(15, 23, 42)
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(15, 23, 42),
                Font = cellFont,
                SelectionBackColor = Color.FromArgb(0, 120, 215),
                SelectionForeColor = Color.White,
                Padding = new Padding(4, 0, 4, 0)
            };

            dgv.RowTemplate.Height = 29;
            return dgv;
        }
    }
}
