using System;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.DAO
{
    /// <summary>
    /// Data Access Object cho lớp SinhVien.
    /// Quản lý dữ liệu sinh viên và xử lý các thao tác Thêm, Sửa, Xóa, Tìm kiếm.
    /// </summary>
    public class SinhVienDAO
    {
        private readonly List<SinhVien> _danhSachSinhVien;
        private readonly LopHocDAO _lopHocDao;

        public SinhVienDAO(LopHocDAO lopHocDao)
        {
            _lopHocDao = lopHocDao;
            _danhSachSinhVien = new List<SinhVien>();

            // Khởi tạo 4 sinh viên mẫu chính xác theo ảnh đề bài (image1.png)
            InitSampleData();
        }

        private void InitSampleData()
        {
            var sv1 = new SinhVien("SV000123", "Nguyễn Văn An", new DateTime(2006, 8, 15), "Nam", "an.nv@vju.ac.vn", "0912345678", 8.5, "KTPM01", "Đang học");
            var sv2 = new SinhVien("SV000124", "Trần Minh Anh", new DateTime(2006, 1, 22), "Nữ", "anh.tm@vju.ac.vn", "0987654321", 9.0, "TTNT01", "Đang học");
            var sv3 = new SinhVien("SV000125", "Lê Hoàng Bình", new DateTime(2006, 5, 9), "Nam", "binh.lh@vju.ac.vn", "0355556677", 7.4, "KTPM01", "Đang học");
            var sv4 = new SinhVien("SV000126", "Đỗ Thị Hồng", new DateTime(2006, 11, 30), "Nữ", "hong.dt@vju.ac.vn", "0777888999", 8.1, "KHDL01", "Đang học");

            LinkLopHoc(sv1);
            LinkLopHoc(sv2);
            LinkLopHoc(sv3);
            LinkLopHoc(sv4);

            _danhSachSinhVien.AddRange(new[] { sv1, sv2, sv3, sv4 });
        }

        private void LinkLopHoc(SinhVien sv)
        {
            var lop = _lopHocDao.GetById(sv.MaLop);
            if (lop != null)
            {
                sv.LopHoc = lop;
                if (!lop.DanhSachSinhVien.Contains(sv))
                {
                    lop.DanhSachSinhVien.Add(sv);
                }
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sinh viên.
        /// </summary>
        public List<SinhVien> GetAll()
        {
            return new List<SinhVien>(_danhSachSinhVien);
        }

        /// <summary>
        /// Tìm sinh viên theo mã sinh viên.
        /// </summary>
        public SinhVien? GetById(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return null;
            return _danhSachSinhVien.FirstOrDefault(s => s.MaSV.Equals(maSV.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Thêm một sinh viên mới.
        /// </summary>
        public bool Add(SinhVien sv)
        {
            if (sv == null || string.IsNullOrWhiteSpace(sv.MaSV)) return false;

            // Kiểm tra trùng mã sinh viên
            if (GetById(sv.MaSV) != null) return false;

            LinkLopHoc(sv);
            _danhSachSinhVien.Add(sv);
            return true;
        }

        /// <summary>
        /// Cập nhật thông tin sinh viên theo mã sinh viên.
        /// </summary>
        public bool Edit(SinhVien sv)
        {
            if (sv == null || string.IsNullOrWhiteSpace(sv.MaSV)) return false;

            var existing = GetById(sv.MaSV);
            if (existing == null) return false;

            // Nếu thay đổi lớp học, xóa khỏi danh sách sinh viên của lớp cũ
            if (existing.LopHoc != null && !existing.MaLop.Equals(sv.MaLop, StringComparison.OrdinalIgnoreCase))
            {
                existing.LopHoc.DanhSachSinhVien.Remove(existing);
            }

            existing.HoTen = sv.HoTen;
            existing.NgaySinh = sv.NgaySinh;
            existing.GioiTinh = sv.GioiTinh;
            existing.Email = sv.Email;
            existing.DienThoai = sv.DienThoai;
            existing.Diem = sv.Diem;
            existing.MaLop = sv.MaLop;
            existing.TrangThai = sv.TrangThai;

            LinkLopHoc(existing);
            return true;
        }

        /// <summary>
        /// Xóa sinh viên theo mã sinh viên.
        /// </summary>
        public bool Delete(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return false;

            var existing = GetById(maSV);
            if (existing == null) return false;

            // Xóa khỏi danh sách sinh viên của lớp
            if (existing.LopHoc != null)
            {
                existing.LopHoc.DanhSachSinhVien.Remove(existing);
            }

            return _danhSachSinhVien.Remove(existing);
        }

        /// <summary>
        /// Tìm kiếm và lọc danh sách sinh viên theo nhiều tiêu chí.
        /// </summary>
        public List<SinhVien> Search(string? keyword, string? maLop, double? minDiem)
        {
            var query = _danhSachSinhVien.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(s =>
                    s.MaSV.ToLower().Contains(kw) ||
                    s.HoTen.ToLower().Contains(kw) ||
                    s.Email.ToLower().Contains(kw) ||
                    s.DienThoai.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(maLop) && maLop != "ALL")
            {
                query = query.Where(s => s.MaLop.Equals(maLop, StringComparison.OrdinalIgnoreCase));
            }

            if (minDiem.HasValue && minDiem.Value > 0)
            {
                query = query.Where(s => s.Diem >= minDiem.Value);
            }

            return query.ToList();
        }
    }
}
