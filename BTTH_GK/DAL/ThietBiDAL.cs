using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    /// <summary>
    /// Lớp truy cập dữ liệu (DAL) cho bảng ThietBi.
    /// Đảm nhiệm truy vấn (kèm Eager Loading LoaiThietBi), thêm, sửa, xóa thiết bị qua EF Core.
    /// </summary>
    public class ThietBiDAL
    {
        /// <summary>
        /// Đảm bảo cơ sở dữ liệu đã được áp dụng Migration mới nhất khi khởi chạy.
        /// </summary>
        public void DamBaoCSDL()
        {
            using var context = new QLThietBiContext();
            context.Database.Migrate();
        }

        /// <summary>
        /// Lấy danh sách tất cả thiết bị kèm thông tin loại thiết bị (Include LoaiThietBi).
        /// </summary>
        public List<ThietBi> LayDanhSach()
        {
            using var context = new QLThietBiContext();
            return context.ThietBis
                .Include(t => t.LoaiThietBi)
                .AsNoTracking()
                .OrderBy(t => t.MaTB)
                .ToList();
        }

        /// <summary>
        /// Tìm thiết bị theo mã thiết bị.
        /// </summary>
        public ThietBi? LayTheoMa(string maTB)
        {
            using var context = new QLThietBiContext();
            return context.ThietBis
                .Include(t => t.LoaiThietBi)
                .AsNoTracking()
                .FirstOrDefault(t => t.MaTB == maTB);
        }

        /// <summary>
        /// Kiểm tra mã thiết bị đã tồn tại trong CSDL hay chưa (không phân biệt hoa thường/khoảng trắng thừa).
        /// </summary>
        public bool TonTaiMaTB(string maTB)
        {
            using var context = new QLThietBiContext();
            string normalized = maTB.Trim().ToLower();
            return context.ThietBis.Any(t => t.MaTB.Trim().ToLower() == normalized);
        }

        /// <summary>
        /// Thêm mới một thiết bị vào CSDL.
        /// </summary>
        public bool Them(ThietBi thietBi)
        {
            using var context = new QLThietBiContext();
            context.ThietBis.Add(thietBi);
            return context.SaveChanges() > 0;
        }

        /// <summary>
        /// Cập nhật thông tin thiết bị (giữ nguyên khóa chính MaTB).
        /// </summary>
        public bool Sua(ThietBi thietBi)
        {
            using var context = new QLThietBiContext();
            ThietBi? existing = context.ThietBis.FirstOrDefault(t => t.MaTB == thietBi.MaTB);
            if (existing == null)
            {
                return false;
            }

            existing.TenTB = thietBi.TenTB;
            existing.NgayMua = thietBi.NgayMua;
            existing.SoLuong = thietBi.SoLuong;
            existing.DonGia = thietBi.DonGia;
            existing.MaLoai = thietBi.MaLoai;

            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Xóa thiết bị khỏi CSDL theo mã thiết bị.
        /// </summary>
        public bool Xoa(string maTB)
        {
            using var context = new QLThietBiContext();
            ThietBi? existing = context.ThietBis.FirstOrDefault(t => t.MaTB == maTB);
            if (existing == null)
            {
                return false;
            }

            context.ThietBis.Remove(existing);
            return context.SaveChanges() > 0;
        }
    }
}
