using System;
using System.Windows.Forms;
using QuanLySinhVien.Views;

namespace QuanLySinhVien
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi nhập chính cho ứng dụng Quản lý sinh viên Buổi 5 (ADO.Net & Xử lý ngoại lệ 3 tầng).
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmQuanLySV());
        }
    }
}
