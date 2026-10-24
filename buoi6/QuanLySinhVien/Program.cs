using System;
using System.Windows.Forms;
using QuanLySinhVien.Views;

namespace QuanLySinhVien
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi nhập chính cho ứng dụng Quản lý sinh viên Buổi 6 (Entity Framework Core Code First).
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmQuanLySV());
        }
    }
}
