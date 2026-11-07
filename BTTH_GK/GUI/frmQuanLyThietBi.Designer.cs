namespace GUI
{
    partial class frmQuanLyThietBi
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle rightAlignStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle centerAlignStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.lblLoaiThietBi = new System.Windows.Forms.Label();
            this.cboLoaiThietBi = new System.Windows.Forms.ComboBox();
            this.lblMaTB = new System.Windows.Forms.Label();
            this.txtMaTB = new System.Windows.Forms.TextBox();
            this.lblTenTB = new System.Windows.Forms.Label();
            this.txtTenTB = new System.Windows.Forms.TextBox();
            this.lblNgayMua = new System.Windows.Forms.Label();
            this.dtpNgayMua = new System.Windows.Forms.DateTimePicker();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.btnMoi = new System.Windows.Forms.Button();
            this.btnNhap = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.grpDanhSach = new System.Windows.Forms.GroupBox();
            this.dgvThietBi = new System.Windows.Forms.DataGridView();
            this.colMaTB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenTB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayMua = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoaiThietBi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpThongTin.SuspendLayout();
            this.grpDanhSach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(24)))), ((int)(((byte)(168)))));
            this.lblTitle.Location = new System.Drawing.Point(14, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(832, 34);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ THIẾT BỊ PHÒNG HỌC";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpThongTin
            // 
            this.grpThongTin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpThongTin.Controls.Add(this.lblLoaiThietBi);
            this.grpThongTin.Controls.Add(this.cboLoaiThietBi);
            this.grpThongTin.Controls.Add(this.lblMaTB);
            this.grpThongTin.Controls.Add(this.txtMaTB);
            this.grpThongTin.Controls.Add(this.lblTenTB);
            this.grpThongTin.Controls.Add(this.txtTenTB);
            this.grpThongTin.Controls.Add(this.lblNgayMua);
            this.grpThongTin.Controls.Add(this.dtpNgayMua);
            this.grpThongTin.Controls.Add(this.lblSoLuong);
            this.grpThongTin.Controls.Add(this.txtSoLuong);
            this.grpThongTin.Controls.Add(this.lblDonGia);
            this.grpThongTin.Controls.Add(this.txtDonGia);
            this.grpThongTin.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpThongTin.Location = new System.Drawing.Point(14, 54);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(684, 162);
            this.grpThongTin.TabIndex = 0;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin thiết bị";
            // 
            // lblLoaiThietBi
            // 
            this.lblLoaiThietBi.AutoSize = true;
            this.lblLoaiThietBi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLoaiThietBi.Location = new System.Drawing.Point(18, 34);
            this.lblLoaiThietBi.Name = "lblLoaiThietBi";
            this.lblLoaiThietBi.Size = new System.Drawing.Size(82, 17);
            this.lblLoaiThietBi.TabIndex = 10;
            this.lblLoaiThietBi.Text = "Loại thiết bị:";
            // 
            // cboLoaiThietBi
            // 
            this.cboLoaiThietBi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiThietBi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLoaiThietBi.FormattingEnabled = true;
            this.cboLoaiThietBi.Location = new System.Drawing.Point(110, 30);
            this.cboLoaiThietBi.Name = "cboLoaiThietBi";
            this.cboLoaiThietBi.Size = new System.Drawing.Size(235, 25);
            this.cboLoaiThietBi.TabIndex = 0;
            // 
            // lblMaTB
            // 
            this.lblMaTB.AutoSize = true;
            this.lblMaTB.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaTB.Location = new System.Drawing.Point(18, 76);
            this.lblMaTB.Name = "lblMaTB";
            this.lblMaTB.Size = new System.Drawing.Size(76, 17);
            this.lblMaTB.TabIndex = 11;
            this.lblMaTB.Text = "Mã thiết bị:";
            // 
            // txtMaTB
            // 
            this.txtMaTB.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaTB.Location = new System.Drawing.Point(110, 72);
            this.txtMaTB.MaxLength = 20;
            this.txtMaTB.Name = "txtMaTB";
            this.txtMaTB.Size = new System.Drawing.Size(235, 24);
            this.txtMaTB.TabIndex = 1;
            // 
            // lblTenTB
            // 
            this.lblTenTB.AutoSize = true;
            this.lblTenTB.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTenTB.Location = new System.Drawing.Point(18, 118);
            this.lblTenTB.Name = "lblTenTB";
            this.lblTenTB.Size = new System.Drawing.Size(78, 17);
            this.lblTenTB.TabIndex = 12;
            this.lblTenTB.Text = "Tên thiết bị:";
            // 
            // txtTenTB
            // 
            this.txtTenTB.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTenTB.Location = new System.Drawing.Point(110, 114);
            this.txtTenTB.MaxLength = 150;
            this.txtTenTB.Name = "txtTenTB";
            this.txtTenTB.Size = new System.Drawing.Size(260, 24);
            this.txtTenTB.TabIndex = 2;
            // 
            // lblNgayMua
            // 
            this.lblNgayMua.AutoSize = true;
            this.lblNgayMua.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNgayMua.Location = new System.Drawing.Point(392, 34);
            this.lblNgayMua.Name = "lblNgayMua";
            this.lblNgayMua.Size = new System.Drawing.Size(69, 17);
            this.lblNgayMua.TabIndex = 13;
            this.lblNgayMua.Text = "Ngày mua:";
            // 
            // dtpNgayMua
            // 
            this.dtpNgayMua.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayMua.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpNgayMua.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayMua.Location = new System.Drawing.Point(476, 30);
            this.dtpNgayMua.Name = "dtpNgayMua";
            this.dtpNgayMua.Size = new System.Drawing.Size(188, 24);
            this.dtpNgayMua.TabIndex = 3;
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSoLuong.Location = new System.Drawing.Point(392, 76);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(62, 17);
            this.lblSoLuong.TabIndex = 14;
            this.lblSoLuong.Text = "Số lượng:";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSoLuong.Location = new System.Drawing.Point(476, 72);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(188, 24);
            this.txtSoLuong.TabIndex = 4;
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDonGia.Location = new System.Drawing.Point(392, 118);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(56, 17);
            this.lblDonGia.TabIndex = 15;
            this.lblDonGia.Text = "Đơn giá:";
            // 
            // txtDonGia
            // 
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDonGia.Location = new System.Drawing.Point(476, 114);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(188, 24);
            this.txtDonGia.TabIndex = 5;
            // 
            // btnMoi
            // 
            this.btnMoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMoi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoi.Location = new System.Drawing.Point(712, 50);
            this.btnMoi.Name = "btnMoi";
            this.btnMoi.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnMoi.Size = new System.Drawing.Size(134, 30);
            this.btnMoi.TabIndex = 1;
            this.btnMoi.Text = "Mới";
            this.btnMoi.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMoi.UseVisualStyleBackColor = true;
            this.btnMoi.Click += new System.EventHandler(this.btnMoi_Click);
            // 
            // btnNhap
            // 
            this.btnNhap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNhap.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNhap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNhap.Location = new System.Drawing.Point(712, 84);
            this.btnNhap.Name = "btnNhap";
            this.btnNhap.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnNhap.Size = new System.Drawing.Size(134, 30);
            this.btnNhap.TabIndex = 2;
            this.btnNhap.Text = "Nhập";
            this.btnNhap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNhap.UseVisualStyleBackColor = true;
            this.btnNhap.Click += new System.EventHandler(this.btnNhap_Click);
            // 
            // btnSua
            // 
            this.btnSua.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSua.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSua.Location = new System.Drawing.Point(712, 118);
            this.btnSua.Name = "btnSua";
            this.btnSua.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnSua.Size = new System.Drawing.Size(134, 30);
            this.btnSua.TabIndex = 3;
            this.btnSua.Text = "Sửa";
            this.btnSua.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnXoa.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXoa.Location = new System.Drawing.Point(712, 152);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnXoa.Size = new System.Drawing.Size(134, 30);
            this.btnXoa.TabIndex = 4;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThoat.CausesValidation = false;
            this.btnThoat.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnThoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThoat.Location = new System.Drawing.Point(712, 186);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnThoat.Size = new System.Drawing.Size(134, 30);
            this.btnThoat.TabIndex = 5;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // grpDanhSach
            // 
            this.grpDanhSach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDanhSach.Controls.Add(this.dgvThietBi);
            this.grpDanhSach.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpDanhSach.Location = new System.Drawing.Point(14, 226);
            this.grpDanhSach.Name = "grpDanhSach";
            this.grpDanhSach.Size = new System.Drawing.Size(832, 278);
            this.grpDanhSach.TabIndex = 6;
            this.grpDanhSach.TabStop = false;
            this.grpDanhSach.Text = "Danh sách thiết bị";
            // 
            // dgvThietBi
            // 
            this.dgvThietBi.AllowUserToAddRows = false;
            this.dgvThietBi.AllowUserToDeleteRows = false;
            this.dgvThietBi.AllowUserToResizeRows = false;
            this.dgvThietBi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvThietBi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThietBi.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = System.Drawing.SystemColors.Control;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            headerStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            headerStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            headerStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            headerStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvThietBi.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvThietBi.ColumnHeadersHeight = 30;
            this.dgvThietBi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvThietBi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaTB,
            this.colTenTB,
            this.colNgayMua,
            this.colSoLuong,
            this.colDonGia,
            this.colLoaiThietBi});
            this.dgvThietBi.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dgvThietBi.Location = new System.Drawing.Point(10, 26);
            this.dgvThietBi.MultiSelect = false;
            this.dgvThietBi.Name = "dgvThietBi";
            this.dgvThietBi.ReadOnly = true;
            this.dgvThietBi.RowHeadersWidth = 28;
            this.dgvThietBi.RowTemplate.Height = 26;
            this.dgvThietBi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThietBi.Size = new System.Drawing.Size(812, 240);
            this.dgvThietBi.TabIndex = 0;
            this.dgvThietBi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThietBi_CellClick);
            this.dgvThietBi.SelectionChanged += new System.EventHandler(this.dgvThietBi_SelectionChanged);
            // 
            // colMaTB
            // 
            this.colMaTB.FillWeight = 12F;
            this.colMaTB.HeaderText = "Mã TB";
            this.colMaTB.Name = "colMaTB";
            this.colMaTB.ReadOnly = true;
            // 
            // colTenTB
            // 
            this.colTenTB.FillWeight = 28F;
            this.colTenTB.HeaderText = "Tên thiết bị";
            this.colTenTB.Name = "colTenTB";
            this.colTenTB.ReadOnly = true;
            // 
            // colNgayMua
            // 
            this.colNgayMua.FillWeight = 14F;
            this.colNgayMua.HeaderText = "Ngày mua";
            this.colNgayMua.Name = "colNgayMua";
            this.colNgayMua.ReadOnly = true;
            // 
            // colSoLuong
            // 
            centerAlignStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSoLuong.DefaultCellStyle = centerAlignStyle;
            this.colSoLuong.FillWeight = 11F;
            this.colSoLuong.HeaderText = "Số lượng";
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            // 
            // colDonGia
            // 
            rightAlignStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colDonGia.DefaultCellStyle = rightAlignStyle;
            this.colDonGia.FillWeight = 15F;
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            // 
            // colLoaiThietBi
            // 
            this.colLoaiThietBi.FillWeight = 20F;
            this.colLoaiThietBi.HeaderText = "Loại thiết bị";
            this.colLoaiThietBi.Name = "colLoaiThietBi";
            this.colLoaiThietBi.ReadOnly = true;
            // 
            // frmQuanLyThietBi
            // 
            this.AcceptButton = this.btnNhap;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.CancelButton = this.btnThoat;
            this.ClientSize = new System.Drawing.Size(860, 518);
            this.Controls.Add(this.grpDanhSach);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnNhap);
            this.Controls.Add(this.btnMoi);
            this.Controls.Add(this.grpThongTin);
            this.Controls.Add(this.lblTitle);
            this.MinimumSize = new System.Drawing.Size(820, 500);
            this.Name = "frmQuanLyThietBi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý thiết bị – 23110342 – Phạm Văn Quang";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmQuanLyThietBi_FormClosing);
            this.Load += new System.EventHandler(this.frmQuanLyThietBi_Load);
            this.Shown += new System.EventHandler(this.frmQuanLyThietBi_Shown);
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            this.grpDanhSach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Label lblTitle = null!;
        private GroupBox grpThongTin = null!;
        private Label lblLoaiThietBi = null!;
        private ComboBox cboLoaiThietBi = null!;
        private Label lblMaTB = null!;
        private TextBox txtMaTB = null!;
        private Label lblTenTB = null!;
        private TextBox txtTenTB = null!;
        private Label lblNgayMua = null!;
        private DateTimePicker dtpNgayMua = null!;
        private Label lblSoLuong = null!;
        private TextBox txtSoLuong = null!;
        private Label lblDonGia = null!;
        private TextBox txtDonGia = null!;
        private Button btnMoi = null!;
        private Button btnNhap = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnThoat = null!;
        private GroupBox grpDanhSach = null!;
        private DataGridView dgvThietBi = null!;
        private DataGridViewTextBoxColumn colMaTB = null!;
        private DataGridViewTextBoxColumn colTenTB = null!;
        private DataGridViewTextBoxColumn colNgayMua = null!;
        private DataGridViewTextBoxColumn colSoLuong = null!;
        private DataGridViewTextBoxColumn colDonGia = null!;
        private DataGridViewTextBoxColumn colLoaiThietBi = null!;
    }
}
