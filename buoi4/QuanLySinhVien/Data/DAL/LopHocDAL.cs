using System;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Data Access Layer (DAL) cho thực thể LopHoc.
    /// Quản lý danh sách các lớp học trong bộ nhớ.
    /// </summary>
    public class LopHocDAL
    {
        private readonly List<LopHoc> lopHocs = new List<LopHoc>();

        public LopHocDAL()
        {
            lopHocs.Add(new LopHoc("KTPM01", "Kỹ thuật phần mềm 01", "Công nghệ thông tin"));
            lopHocs.Add(new LopHoc("TTNT01", "Trí tuệ nhân tạo 01", "Trí tuệ nhân tạo"));
            lopHocs.Add(new LopHoc("KHDL01", "Khoa học dữ liệu 01", "Khoa học máy tính"));
            lopHocs.Add(new LopHoc("HTTT01", "Hệ thống thông tin 01", "Hệ thống thông tin"));
            lopHocs.Add(new LopHoc("CSE0001", "Khoa học máy tính 01", "Khoa học máy tính"));
            lopHocs.Add(new LopHoc("CSE0002", "Khoa học máy tính 02", "Khoa học máy tính"));
            lopHocs.Add(new LopHoc("CSE0003", "Khoa học máy tính 03", "Khoa học máy tính"));
        }

        public List<LopHoc> GetAllLopHoc()
        {
            return new List<LopHoc>(lopHocs);
        }

        public List<LopHoc> GetAll() => GetAllLopHoc();

        public LopHoc? GetLopHocByMa(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return null;
            return lopHocs.Find(l => l.MaLop.Equals(maLop.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void AddLopHoc(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));
            if (lopHocs.Exists(l => l.MaLop.Equals(lop.MaLop, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Lớp học có mã '{lop.MaLop}' đã tồn tại.");
            }
            lopHocs.Add(lop);
        }
    }
}
