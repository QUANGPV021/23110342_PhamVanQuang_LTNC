using System;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.DAO
{
    /// <summary>
    /// Data Access Object cho lớp LopHoc.
    /// Quản lý dữ liệu danh sách lớp học trong ứng dụng.
    /// </summary>
    public class LopHocDAO
    {
        private readonly List<LopHoc> _danhSachLop;

        public LopHocDAO()
        {
            // Khởi tạo danh sách các lớp học ban đầu phù hợp với đề bài và ảnh mẫu
            _danhSachLop = new List<LopHoc>
            {
                new LopHoc("KTPM01", "Kỹ thuật phần mềm 01", "Chương trình đào tạo Kỹ thuật Phần mềm"),
                new LopHoc("TTNT01", "Trí tuệ nhân tạo 01", "Chương trình đào tạo Trí tuệ Nhân tạo"),
                new LopHoc("KHDL01", "Khoa học dữ liệu 01", "Chương trình đào tạo Khoa học Dữ liệu"),
                new LopHoc("HTTT01", "Hệ thống thông tin 01", "Chương trình đào tạo Hệ thống Thông tin")
            };
        }

        /// <summary>
        /// Lấy tất cả danh sách các lớp học.
        /// </summary>
        public List<LopHoc> GetAll()
        {
            return new List<LopHoc>(_danhSachLop);
        }

        /// <summary>
        /// Tìm lớp học theo mã lớp.
        /// </summary>
        public LopHoc? GetById(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return null;
            return _danhSachLop.FirstOrDefault(l => l.MaLop.Equals(maLop.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Tìm lớp học theo tên lớp.
        /// </summary>
        public LopHoc? GetByName(string tenLop)
        {
            if (string.IsNullOrWhiteSpace(tenLop)) return null;
            return _danhSachLop.FirstOrDefault(l => l.TenLop.Equals(tenLop.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Thêm lớp học mới.
        /// </summary>
        public bool Add(LopHoc lop)
        {
            if (lop == null || string.IsNullOrWhiteSpace(lop.MaLop)) return false;
            if (_danhSachLop.Any(l => l.MaLop.Equals(lop.MaLop.Trim(), StringComparison.OrdinalIgnoreCase))) return false;

            _danhSachLop.Add(lop);
            return true;
        }
    }
}
