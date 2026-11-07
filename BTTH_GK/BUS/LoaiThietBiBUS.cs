using DAL;
using DAL.Entities;

namespace BUS
{
    /// <summary>
    /// Lớp xử lý nghiệp vụ (BUS/BLL) đối với Loại thiết bị.
    /// </summary>
    public class LoaiThietBiBUS
    {
        private readonly LoaiThietBiDAL _loaiThietBiDAL = new LoaiThietBiDAL();

        /// <summary>
        /// Lấy danh sách loại thiết bị phục vụ nạp vào ComboBox trên giao diện.
        /// </summary>
        public List<LoaiThietBi> LayDanhSachLoaiThietBi()
        {
            return _loaiThietBiDAL.LayDanhSach();
        }
    }
}
