using System;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Data Access Layer (DAL) cho thực thể SinhVien.
    /// Quản lý nguồn dữ liệu trong bộ nhớ (DataList) và thực hiện các thao tác Thêm, Sửa, Xóa, Lấy về.
    /// </summary>
    public class SinhVienDAL
    {
        private readonly List<SinhVien> sinhViens = new List<SinhVien>();

        public SinhVienDAL()
        {
            // Khởi tạo danh sách sinh viên mẫu theo tài liệu và giao diện thực hành
            sinhViens.Add(new SinhVien
            {
                MaSV = "SV000123",
                HoTen = "Nguyễn Văn An",
                NgaySinh = new DateTime(2006, 8, 15),
                GioiTinh = "Nam",
                Email = "an.nv@vju.ac.vn",
                SoDienThoai = "0912345678",
                Diem = 8.5,
                MaLop = "KTPM01",
                TrangThai = "Đang học"
            });

            sinhViens.Add(new SinhVien
            {
                MaSV = "SV000124",
                HoTen = "Trần Minh Anh",
                NgaySinh = new DateTime(2006, 1, 22),
                GioiTinh = "Nữ",
                Email = "anh.tm@vju.ac.vn",
                SoDienThoai = "0987654321",
                Diem = 9.0,
                MaLop = "TTNT01",
                TrangThai = "Đang học"
            });

            sinhViens.Add(new SinhVien
            {
                MaSV = "SV000125",
                HoTen = "Lê Hoàng Bình",
                NgaySinh = new DateTime(2006, 5, 9),
                GioiTinh = "Nam",
                Email = "binh.lh@vju.ac.vn",
                SoDienThoai = "0355556677",
                Diem = 7.4,
                MaLop = "KTPM01",
                TrangThai = "Đang học"
            });

            sinhViens.Add(new SinhVien
            {
                MaSV = "SV000126",
                HoTen = "Đỗ Thị Hồng",
                NgaySinh = new DateTime(2006, 11, 30),
                GioiTinh = "Nữ",
                Email = "hong.dt@vju.ac.vn",
                SoDienThoai = "0777888999",
                Diem = 8.1,
                MaLop = "KHDL01",
                TrangThai = "Đang học"
            });

            sinhViens.Add(new SinhVien
            {
                MaSV = "SV0001",
                HoTen = "Nguyễn Văn A",
                NgaySinh = new DateTime(2000, 1, 1),
                GioiTinh = "Nam",
                Email = "a.nv@vju.ac.vn",
                SoDienThoai = "0911223344",
                Diem = 8.0,
                MaLop = "CSE0001",
                TrangThai = "Đang học"
            });

            sinhViens.Add(new SinhVien
            {
                MaSV = "SV0002",
                HoTen = "Trần Thị B",
                NgaySinh = new DateTime(2000, 2, 2),
                GioiTinh = "Nữ",
                Email = "b.tt@vju.ac.vn",
                SoDienThoai = "0922334455",
                Diem = 7.5,
                MaLop = "CSE0002",
                TrangThai = "Đang học"
            });

            sinhViens.Add(new SinhVien
            {
                MaSV = "SV0003",
                HoTen = "Lê Văn C",
                NgaySinh = new DateTime(2000, 3, 3),
                GioiTinh = "Nam",
                Email = "c.lv@vju.ac.vn",
                SoDienThoai = "0933445566",
                Diem = 8.8,
                MaLop = "CSE0003",
                TrangThai = "Đang học"
            });
        }

        public SinhVien? GetSinhVienByMaSV(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return null;
            return sinhViens.Find(s => s.MaSV.Equals(maSV.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop) || maLop.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                return GetAllSinhVien();
            }
            return sinhViens.FindAll(s => s.MaLop.Equals(maLop.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public List<SinhVien> GetAllSinhVien()
        {
            return new List<SinhVien>(sinhViens);
        }

        public void AddSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));
            if (sinhViens.Exists(s => s.MaSV.Equals(sv.MaSV, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Sinh viên có mã '{sv.MaSV}' đã tồn tại trong hệ thống.");
            }
            sinhViens.Add(sv);
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));
            SinhVien? sve = sinhViens.FirstOrDefault(s => s.MaSV.Equals(sv.MaSV, StringComparison.OrdinalIgnoreCase));
            if (sve != null)
            {
                sve.HoTen = sv.HoTen;
                sve.NgaySinh = sv.NgaySinh;
                sve.GioiTinh = sv.GioiTinh;
                sve.MaLop = sv.MaLop;
                sve.Email = sv.Email;
                sve.SoDienThoai = sv.SoDienThoai;
                sve.Diem = sv.Diem;
                sve.TrangThai = sv.TrangThai;
                sve.LopHoc = sv.LopHoc;
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy sinh viên có mã '{sv.MaSV}' để cập nhật.");
            }
        }

        public void DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return;
            SinhVien? sv = sinhViens.FirstOrDefault(s => s.MaSV.Equals(maSV.Trim(), StringComparison.OrdinalIgnoreCase));
            if (sv != null)
            {
                sinhViens.Remove(sv);
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy sinh viên có mã '{maSV}' để xóa.");
            }
        }

        public SinhVien? GetSinhVienById(string maSV)
        {
            return GetSinhVienByMaSV(maSV);
        }

        /// <summary>
        /// Tìm kiếm và lọc sinh viên theo từ khóa, mã lớp, và điểm sàn
        /// </summary>
        public List<SinhVien> Search(string? keyword, string? maLop, double diemTu)
        {
            var query = sinhViens.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(s =>
                    (s.MaSV != null && s.MaSV.ToLower().Contains(kw)) ||
                    (s.HoTen != null && s.HoTen.ToLower().Contains(kw)) ||
                    (s.Email != null && s.Email.ToLower().Contains(kw)) ||
                    (s.SoDienThoai != null && s.SoDienThoai.ToLower().Contains(kw))
                );
            }

            if (!string.IsNullOrWhiteSpace(maLop) && !maLop.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(s => s.MaLop.Equals(maLop.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            if (diemTu > 0)
            {
                query = query.Where(s => s.Diem >= diemTu);
            }

            return query.ToList();
        }
    }
}
