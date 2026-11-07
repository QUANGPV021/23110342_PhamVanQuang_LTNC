using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    /// <summary>
    /// Lớp truy cập dữ liệu (DAL) cho bảng LoaiThietBi.
    /// Chỉ thực hiện truy vấn CSDL qua EF Core, không tham chiếu giao diện WinForms.
    /// </summary>
    public class LoaiThietBiDAL
    {
        /// <summary>
        /// Lấy toàn bộ danh sách loại thiết bị sắp xếp theo mã loại.
        /// </summary>
        public List<LoaiThietBi> LayDanhSach()
        {
            using var context = new QLThietBiContext();
            return context.LoaiThietBis
                .AsNoTracking()
                .OrderBy(l => l.MaLoai)
                .ToList();
        }

        /// <summary>
        /// Kiểm tra mã loại thiết bị có tồn tại trong cơ sở dữ liệu hay không.
        /// </summary>
        public bool TonTaiMaLoai(string maLoai)
        {
            using var context = new QLThietBiContext();
            return context.LoaiThietBis.Any(l => l.MaLoai == maLoai);
        }
    }
}
