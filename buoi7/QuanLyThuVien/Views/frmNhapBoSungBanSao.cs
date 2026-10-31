using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVienDAO.Views
{
    /// <summary>
    /// Hộp thoại nhập bổ sung một hoặc nhiều bản sao cho đầu sách đang chọn.
    /// </summary>
    public class frmNhapBoSungBanSao : Form
    {
        private readonly Book _book;
        private readonly int _nextStartIndex;

        private NumericUpDown numSoLuong = null!;
        private ComboBox cboTrangThai = null!;
        private ComboBox cboTinhTrang = null!;
        private Label lblMaDuKienVal = null!;
        private Button btnXacNhan = null!;
        private Button btnHuy = null!;

        public int SoLuongNhapThem => (int)numSoLuong.Value;
        public string TrangThaiChon => cboTrangThai.Text.Trim();
        public string TinhTrangChon => cboTinhTrang.Text.Trim();

        public frmNhapBoSungBanSao(Book book, string nextCopyId, int defaultQuantity = 1)
        {
            _book = book ?? throw new ArgumentNullException(nameof(book));

            int parsedStart = 1;
            if (!string.IsNullOrWhiteSpace(nextCopyId))
            {
                int dotIdx = nextCopyId.LastIndexOf('.');
                if (dotIdx >= 0 && dotIdx < nextCopyId.Length - 1)
                {
                    int.TryParse(nextCopyId.Substring(dotIdx + 1), out parsedStart);
                }
            }
            _nextStartIndex = parsedStart > 0 ? parsedStart : 1;

            InitializeDialogComponents(defaultQuantity);
            UpdatePreviewLabel();
        }

        private void InitializeDialogComponents(int defaultQuantity)
        {
            this.Text = "Nhập bổ sung bản sao sách";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(460, 340);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = Color.FromArgb(238, 246, 252)
            };

            var lblHeader = new Label
            {
                Text = "NHẬP BỔ SUNG BẢN SAO",
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(11, 106, 153),
                AutoSize = true,
                Location = new Point(18, 15)
            };
            pnlHeader.Controls.Add(lblHeader);
            this.Controls.Add(pnlHeader);

            int y = 70;
            int labelX = 20;
            int inputX = 175;
            int inputW = 260;

            // Mã & tên sách
            this.Controls.Add(new Label
            {
                Text = "Đầu sách:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(labelX, y + 3),
                AutoSize = true
            });

            var txtDauSach = new TextBox
            {
                Text = $"{_book.BookId} - {_book.Title}",
                Location = new Point(inputX, y),
                Width = inputW,
                ReadOnly = true,
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(txtDauSach);

            y += 38;
            this.Controls.Add(new Label
            {
                Text = "Số bản sao hiện có:",
                Location = new Point(labelX, y + 3),
                AutoSize = true
            });

            var txtHienCo = new TextBox
            {
                Text = _book.NumberOfCopies.ToString(),
                Location = new Point(inputX, y),
                Width = 100,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(29, 78, 216),
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(txtHienCo);

            y += 38;
            this.Controls.Add(new Label
            {
                Text = "Số lượng nhập thêm (*):",
                Location = new Point(labelX, y + 3),
                AutoSize = true
            });

            numSoLuong = new NumericUpDown
            {
                Location = new Point(inputX, y),
                Width = 100,
                Minimum = 1,
                Maximum = 500,
                Value = Math.Clamp(defaultQuantity, 1, 500),
                TextAlign = HorizontalAlignment.Right
            };
            numSoLuong.ValueChanged += (s, e) => UpdatePreviewLabel();
            this.Controls.Add(numSoLuong);

            lblMaDuKienVal = new Label
            {
                Location = new Point(inputX + 110, y + 4),
                Size = new Size(155, 24),
                ForeColor = Color.FromArgb(14, 116, 144),
                Font = new Font("Segoe UI", 8.75F, FontStyle.Italic)
            };
            this.Controls.Add(lblMaDuKienVal);

            y += 38;
            this.Controls.Add(new Label
            {
                Text = "Trạng thái (*):",
                Location = new Point(labelX, y + 3),
                AutoSize = true
            });

            cboTrangThai = new ComboBox
            {
                Location = new Point(inputX, y),
                Width = inputW,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTrangThai.Items.AddRange(new object[]
            {
                "Sẵn sàng",
                "Đang mượn",
                "Đặt trước",
                "Bảo trì",
                "Thanh lý"
            });
            cboTrangThai.SelectedIndex = 0;
            this.Controls.Add(cboTrangThai);

            y += 38;
            this.Controls.Add(new Label
            {
                Text = "Tình trạng vật lý (*):",
                Location = new Point(labelX, y + 3),
                AutoSize = true
            });

            cboTinhTrang = new ComboBox
            {
                Location = new Point(inputX, y),
                Width = inputW,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTinhTrang.Items.AddRange(new object[]
            {
                "Mới",
                "Tốt",
                "Khá",
                "Cũ",
                "Hư hỏng nhẹ"
            });
            cboTinhTrang.SelectedIndex = 0;
            this.Controls.Add(cboTinhTrang);

            y += 50;
            btnXacNhan = new Button
            {
                Text = "Nhập bổ sung",
                Location = new Point(inputX, y),
                Size = new Size(140, 36),
                BackColor = Color.FromArgb(11, 114, 164),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnXacNhan);

            btnHuy = new Button
            {
                Text = "Hủy",
                Location = new Point(inputX + 150, y),
                Size = new Size(110, 36),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnHuy.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            this.Controls.Add(btnHuy);

            this.AcceptButton = btnXacNhan;
            this.CancelButton = btnHuy;
        }

        private void UpdatePreviewLabel()
        {
            int qty = (int)numSoLuong.Value;
            string firstCode = $"{_book.BookId}.{_nextStartIndex:D3}";
            if (qty == 1)
            {
                lblMaDuKienVal.Text = $"Mã: {firstCode}";
            }
            else
            {
                string lastCode = $"{_book.BookId}.{(_nextStartIndex + qty - 1):D3}";
                lblMaDuKienVal.Text = $"{firstCode} → {lastCode}";
            }
        }
    }
}
