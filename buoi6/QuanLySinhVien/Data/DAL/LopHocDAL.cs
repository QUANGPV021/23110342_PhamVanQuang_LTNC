using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data.Context;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Data Access Layer (DAL) cho thực thể LopHoc sử dụng Entity Framework Core.
    /// Tương tác với cơ sở dữ liệu qua QLSVDbContext.
    /// </summary>
    public class LopHocDAL
    {
        private readonly string? _connectionString;

        public LopHocDAL()
        {
        }

        public LopHocDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private QLSVDbContext CreateContext()
        {
            return string.IsNullOrWhiteSpace(_connectionString)
                ? new QLSVDbContext()
                : new QLSVDbContext(_connectionString);
        }

        public List<LopHoc> GetAllLopHoc()
        {
            try
            {
                using var context = CreateContext();
                return context.LopHocs
                    .Include(l => l.DanhSachSinhVien)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách lớp học bằng EF Core: " + ex.Message, ex);
            }
        }

        public List<LopHoc> GetAll() => GetAllLopHoc();

        public LopHoc? GetLopHocByMa(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return null;

            try
            {
                using var context = CreateContext();
                return context.LopHocs
                    .Include(l => l.DanhSachSinhVien)
                    .AsNoTracking()
                    .FirstOrDefault(l => l.MaLop.Equals(maLop.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tra cứu lớp học mã '{maLop}' bằng EF Core: {ex.Message}", ex);
            }
        }

        public void AddLopHoc(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));

            try
            {
                using var context = CreateContext();
                bool exists = context.LopHocs.Any(l => l.MaLop.Equals(lop.MaLop.Trim(), StringComparison.OrdinalIgnoreCase));
                if (exists)
                {
                    throw new InvalidOperationException($"Lớp học có mã '{lop.MaLop}' đã tồn tại trong CSDL.");
                }

                var newLop = new LopHoc
                {
                    MaLop = lop.MaLop.Trim(),
                    TenLop = lop.TenLop.Trim(),
                    Khoa = lop.Khoa ?? string.Empty
                };

                context.LopHocs.Add(newLop);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm lớp học mã '{lop.MaLop}' bằng EF Core: {ex.Message}", ex);
            }
        }

        public void UpdateLopHoc(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));

            try
            {
                using var context = CreateContext();
                var existing = context.LopHocs.FirstOrDefault(l => l.MaLop.Equals(lop.MaLop.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy lớp học có mã '{lop.MaLop}' để cập nhật.");
                }

                existing.TenLop = lop.TenLop.Trim();
                existing.Khoa = lop.Khoa ?? string.Empty;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật lớp học mã '{lop.MaLop}' bằng EF Core: {ex.Message}", ex);
            }
        }

        public void DeleteLopHoc(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return;

            try
            {
                using var context = CreateContext();
                var existing = context.LopHocs.FirstOrDefault(l => l.MaLop.Equals(maLop.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy lớp học có mã '{maLop}' để xóa.");
                }

                context.LopHocs.Remove(existing);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa lớp học mã '{maLop}' bằng EF Core: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// Lớp bí danh LopHocDAO để tương thích hoàn toàn
    /// </summary>
    public class LopHocDAO : LopHocDAL
    {
        public LopHocDAO() : base() { }
        public LopHocDAO(string connectionString) : base(connectionString) { }
    }
}
