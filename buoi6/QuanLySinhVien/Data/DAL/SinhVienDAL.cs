using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data.Context;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Data Access Layer (DAL) cho thực thể SinhVien sử dụng Entity Framework Core.
    /// Tương tác với cơ sở dữ liệu SQL Server thông qua QLSVDbContext.
    /// Ném ngoại lệ về các tầng trên khi xảy ra lỗi.
    /// </summary>
    public class SinhVienDAL
    {
        private readonly string? _connectionString;

        public SinhVienDAL()
        {
        }

        public SinhVienDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private QLSVDbContext CreateContext()
        {
            return string.IsNullOrWhiteSpace(_connectionString)
                ? new QLSVDbContext()
                : new QLSVDbContext(_connectionString);
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sinh viên kèm thông tin lớp học (Eager Loading với Include)
        /// </summary>
        public List<SinhVien> GetAllSinhVien()
        {
            try
            {
                using var context = CreateContext();
                return context.SinhViens
                    .Include(s => s.LopHoc)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sinh viên bằng EF Core: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Tìm sinh viên theo Mã sinh viên
        /// </summary>
        public SinhVien? GetSinhVienByMaSV(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return null;

            try
            {
                using var context = CreateContext();
                return context.SinhViens
                    .Include(s => s.LopHoc)
                    .AsNoTracking()
                    .FirstOrDefault(s => s.MaSV.Equals(maSV.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tra cứu sinh viên mã '{maSV}' bằng EF Core: {ex.Message}", ex);
            }
        }

        public SinhVien? GetSinhVienById(string maSV) => GetSinhVienByMaSV(maSV);

        /// <summary>
        /// Lấy danh sách sinh viên theo Mã lớp
        /// </summary>
        public List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop) || maLop.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                return GetAllSinhVien();
            }

            try
            {
                using var context = CreateContext();
                return context.SinhViens
                    .Include(s => s.LopHoc)
                    .AsNoTracking()
                    .Where(s => s.MaLop.Equals(maLop.Trim(), StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lọc sinh viên theo lớp '{maLop}' bằng EF Core: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sinh viên kết hợp LINQ to Entities
        /// </summary>
        public List<SinhVien> Search(string? keyword, string? maLop, double diemTu)
        {
            try
            {
                using var context = CreateContext();
                var query = context.SinhViens
                    .Include(s => s.LopHoc)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string kw = keyword.Trim().ToLower();
                    query = query.Where(s =>
                        s.MaSV.ToLower().Contains(kw) ||
                        s.HoTen.ToLower().Contains(kw) ||
                        s.Email.ToLower().Contains(kw) ||
                        s.SoDienThoai.ToLower().Contains(kw));
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
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sinh viên bằng EF Core: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Thêm mới sinh viên vào CSDL qua DbContext
        /// </summary>
        public void AddSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            try
            {
                using var context = CreateContext();
                bool exists = context.SinhViens.Any(s => s.MaSV.Equals(sv.MaSV.Trim(), StringComparison.OrdinalIgnoreCase));
                if (exists)
                {
                    throw new InvalidOperationException($"Sinh viên có mã '{sv.MaSV}' đã tồn tại trong CSDL.");
                }

                // Không gắn kèm đối tượng navigation để tránh EF Core cố thêm lại LopHoc đã có
                var newSv = new SinhVien
                {
                    MaSV = sv.MaSV.Trim(),
                    HoTen = sv.HoTen.Trim(),
                    NgaySinh = sv.NgaySinh,
                    GioiTinh = sv.GioiTinh,
                    Email = sv.Email.Trim(),
                    SoDienThoai = sv.SoDienThoai.Trim(),
                    Diem = sv.Diem,
                    MaLop = sv.MaLop.Trim(),
                    TrangThai = sv.TrangThai
                };

                context.SinhViens.Add(newSv);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sinh viên vào CSDL bằng EF Core: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sinh viên qua DbContext
        /// </summary>
        public void UpdateSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            try
            {
                using var context = CreateContext();
                var existing = context.SinhViens.FirstOrDefault(s => s.MaSV.Equals(sv.MaSV.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy sinh viên có mã '{sv.MaSV}' để cập nhật.");
                }

                existing.HoTen = sv.HoTen.Trim();
                existing.NgaySinh = sv.NgaySinh;
                existing.GioiTinh = sv.GioiTinh;
                existing.Email = sv.Email.Trim();
                existing.SoDienThoai = sv.SoDienThoai.Trim();
                existing.Diem = sv.Diem;
                existing.MaLop = sv.MaLop.Trim();
                existing.TrangThai = sv.TrangThai;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sinh viên bằng EF Core: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa sinh viên khỏi CSDL qua DbContext
        /// </summary>
        public void DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return;

            try
            {
                using var context = CreateContext();
                var existing = context.SinhViens.FirstOrDefault(s => s.MaSV.Equals(maSV.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy sinh viên có mã '{maSV}' để xóa.");
                }

                context.SinhViens.Remove(existing);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa sinh viên mã '{maSV}' bằng EF Core: " + ex.Message, ex);
            }
        }
    }

    /// <summary>
    /// Lớp bí danh SinhVienDAO để tương thích hoàn toàn với các tên gọi trong môn học
    /// </summary>
    public class SinhVienDAO : SinhVienDAL
    {
        public SinhVienDAO() : base() { }
        public SinhVienDAO(string connectionString) : base(connectionString) { }
    }
}
