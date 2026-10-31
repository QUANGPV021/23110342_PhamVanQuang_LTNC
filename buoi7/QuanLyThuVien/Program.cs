using System;
using System.Windows.Forms;
using QuanLyThuVienDAO.Views;

namespace QuanLyThuVienDAO
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi nhập chính cho ứng dụng Quản lý đầu sách và bản sao (Buổi 7).
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmQuanLyDauSach());
        }
    }
}
