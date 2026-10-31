using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVienDAO.Views
{
    /// <summary>
    /// Hộp thoại cập nhật trạng thái và tình trạng vật lý của một bản sao sách.
    /// </summary>
    public class frmCapNhatBanSao : Form
    {
        private readonly BookCopy _bookCopy;
        private readonly string _bookTitle;

        private ComboBox cboTrangThai = null!;
        private ComboBox cboTinhTrang = null!;
        private Button btnLuu = null!;
        private Button btnHuy = null!;

        public string TrangThaiMoi => cboTrangThai.Text.Trim();
        public string TinhTrangMoi => cboTinhTrang.Text.Trim();

        public frmCapNhatBanSao(BookCopy bookCopy, string bookTitle)
        {
            _bookCopy = bookCopy ?? throw new ArgumentNullException(nameof(bookCopy));
            _bookTitle = bookTitle ?? string.Empty;

            InitializeDialogComponents();
        }

        private void InitializeDialogComponents()
        {
            this.Text = "Cập nhật bản sao sách";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(440, 290);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(238, 246, 252)
            };

            var lblHeader = new Label
            {
                Text = "CẬP NHẬT THÔNG TIN BẢN SAO",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(11, 106, 153),
                AutoSize = true,
                Location = new Point(18, 14)
            };
            pnlHeader.Controls.Add(lblHeader);
            this.Controls.Add(pnlHeader);

            int y = 68;
            int labelX = 20;
            int inputX = 165;
            int inputW = 250;

            this.Controls.Add(new Label
            {
                Text = "Mã bản sao:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(labelX, y + 3),
                AutoSize = true
            });

            var txtMaBanSao = new TextBox
            {
                Text = _bookCopy.BookCopyId,
                Location = new Point(inputX, y),
                Width = inputW,
                ReadOnly = true,
                BackColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            this.Controls.Add(txtMaBanSao);

            y += 38;
            this.Controls.Add(new Label
            {
                Text = "Đầu sách:",
                Location = new Point(labelX, y + 3),
                AutoSize = true
            });

            var txtDauSach = new TextBox
            {
                Text = string.IsNullOrWhiteSpace(_bookTitle)
                    ? _bookCopy.BookId
                    : $"{_bookCopy.BookId} - {_bookTitle}",
                Location = new Point(inputX, y),
                Width = inputW,
                ReadOnly = true,
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(txtDauSach);

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
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cboTrangThai.Items.AddRange(new object[]
            {
                "Sẵn sàng",
                "Đang mượn",
                "Đặt trước",
                "Bảo trì",
                "Đã mất",
                "Thanh lý"
            });
            cboTrangThai.Text = _bookCopy.Status;
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
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cboTinhTrang.Items.AddRange(new object[]
            {
                "Mới",
                "Tốt",
                "Khá",
                "Cũ",
                "Hư hỏng nhẹ",
                "Hư hỏng nặng"
            });
            cboTinhTrang.Text = _bookCopy.Condition;
            this.Controls.Add(cboTinhTrang);

            y += 48;
            btnLuu = new Button
            {
                Text = "Lưu cập nhật",
                Location = new Point(inputX, y),
                Size = new Size(135, 36),
                BackColor = Color.FromArgb(217, 119, 6),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cboTrangThai.Text))
                {
                    MessageBox.Show("Trạng thái bản sao không được để trống!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboTrangThai.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(cboTinhTrang.Text))
                {
                    MessageBox.Show("Tình trạng vật lý không được để trống!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboTinhTrang.Focus();
                    return;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnLuu);

            btnHuy = new Button
            {
                Text = "Hủy",
                Location = new Point(inputX + 145, y),
                Size = new Size(105, 36),
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

            this.AcceptButton = btnLuu;
            this.CancelButton = btnHuy;
        }
    }
}
