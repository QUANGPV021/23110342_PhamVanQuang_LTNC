using System;
using System.Windows.Forms;
using QuanLySinhVien.Forms;

namespace QuanLySinhVien
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi chạy chính (Main entry point) của ứng dụng Windows Forms.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormQuanLySinhVien());
        }
    }
}
