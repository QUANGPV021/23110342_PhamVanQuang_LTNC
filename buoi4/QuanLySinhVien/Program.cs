using System;
using System.Windows.Forms;
using QuanLySinhVien.Views;

namespace QuanLySinhVien
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi nhập chính cho ứng dụng Windows Forms Quản lý sinh viên (Kiến trúc phân tầng 3-Layer: DAL - BUL - Views).
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmQuanLySV());
        }
    }
}
