using DAL;
using DAL.Entities;

namespace BUS
{
    /// <summary>
    /// Lớp xử lý quy tắc nghiệp vụ (BUS/BLL) đối với Thiết bị phòng học.
    /// Kiểm tra tính hợp lệ dữ liệu và kiểm tra trùng mã trước khi yêu cầu tầng DAL lưu vào CSDL.
    /// </summary>
    public class ThietBiBUS
    {
        private readonly ThietBiDAL _thietBiDAL = new ThietBiDAL();
        private readonly LoaiThietBiDAL _loaiThietBiDAL = new LoaiThietBiDAL();

        /// <summary>
        /// Đảm bảo CSDL đã áp dụng Migration trước khi giao diện tải dữ liệu.
        /// </summary>
        public void KhoiTaoCSDL()
        {
            _thietBiDAL.DamBaoCSDL();
        }

        /// <summary>
        /// Lấy danh sách thiết bị kèm tên loại thiết bị để hiển thị lên DataGridView.
        /// </summary>
        public List<ThietBi> LayDanhSachThietBi()
        {
            return _thietBiDAL.LayDanhSach();
        }

        /// <summary>
        /// Kiểm tra các quy tắc nghiệp vụ chung:
        /// - Chọn loại thiết bị hợp lệ
        /// - Mã và tên thiết bị không trống (sau khi Trim), đúng độ dài tối đa
        /// - Ngày mua không lớn hơn ngày hiện tại
        /// - Số lượng nguyên lớn hơn 0, đơn giá lớn hơn 0
        /// </summary>
        private OperationResult? KiemTraNghiepVuChung(ThietBi? thietBi)
        {
            if (thietBi == null)
            {
                return OperationResult.Failure("Dữ liệu thiết bị không hợp lệ!");
            }

            // Chuẩn hóa loại bỏ khoảng trắng đầu/cuối
            thietBi.MaTB = (thietBi.MaTB ?? string.Empty).Trim();
            thietBi.TenTB = (thietBi.TenTB ?? string.Empty).Trim();
            thietBi.MaLoai = (thietBi.MaLoai ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(thietBi.MaLoai))
            {
                return OperationResult.Failure("Vui lòng chọn loại thiết bị!", "MaLoai");
            }

            if (string.IsNullOrWhiteSpace(thietBi.MaTB))
            {
                return OperationResult.Failure("Mã thiết bị không được để trống!", "MaTB");
            }

            if (thietBi.MaTB.Length > 20)
            {
                return OperationResult.Failure("Mã thiết bị không được vượt quá 20 ký tự!", "MaTB");
            }

            if (string.IsNullOrWhiteSpace(thietBi.TenTB))
            {
                return OperationResult.Failure("Tên thiết bị không được để trống!", "TenTB");
            }

            if (thietBi.TenTB.Length > 150)
            {
                return OperationResult.Failure("Tên thiết bị không được vượt quá 150 ký tự!", "TenTB");
            }

            if (thietBi.NgayMua.Date > DateTime.Today)
            {
                return OperationResult.Failure("Ngày mua không được lớn hơn ngày hiện tại!", "NgayMua");
            }

            if (thietBi.SoLuong <= 0)
            {
                return OperationResult.Failure("Số lượng phải là số nguyên lớn hơn 0!", "SoLuong");
            }

            if (thietBi.DonGia <= 0)
            {
                return OperationResult.Failure("Đơn giá phải là số lớn hơn 0!", "DonGia");
            }

            if (!_loaiThietBiDAL.TonTaiMaLoai(thietBi.MaLoai))
            {
                return OperationResult.Failure("Loại thiết bị đã chọn không tồn tại trong hệ thống!", "MaLoai");
            }

            return null;
        }

        /// <summary>
        /// Nghiệp vụ thêm mới thiết bị: kiểm tra hợp lệ và kiểm tra mã không trùng sau khi Trim().
        /// </summary>
        public OperationResult ThemThietBi(ThietBi thietBi)
        {
            OperationResult? validationError = KiemTraNghiepVuChung(thietBi);
            if (validationError != null)
            {
                return validationError;
            }

            if (_thietBiDAL.TonTaiMaTB(thietBi.MaTB))
            {
                return OperationResult.Failure($"Mã thiết bị '{thietBi.MaTB}' đã tồn tại! Vui lòng nhập mã khác.", "MaTB");
            }

            thietBi.NgayMua = thietBi.NgayMua.Date;
            bool saved = _thietBiDAL.Them(thietBi);
            return saved
                ? OperationResult.Success("Thêm thiết bị mới thành công!")
                : OperationResult.Failure("Lưu thiết bị mới thất bại!");
        }

        /// <summary>
        /// Nghiệp vụ cập nhật thiết bị: giữ nguyên khóa chính MaTB và kiểm tra hợp lệ dữ liệu.
        /// </summary>
        public OperationResult SuaThietBi(ThietBi thietBi)
        {
            OperationResult? validationError = KiemTraNghiepVuChung(thietBi);
            if (validationError != null)
            {
                return validationError;
            }

            if (!_thietBiDAL.TonTaiMaTB(thietBi.MaTB))
            {
                return OperationResult.Failure($"Không tìm thấy thiết bị có mã '{thietBi.MaTB}' để cập nhật!", "MaTB");
            }

            thietBi.NgayMua = thietBi.NgayMua.Date;
            bool updated = _thietBiDAL.Sua(thietBi);
            return updated
                ? OperationResult.Success("Cập nhật thông tin thiết bị thành công!")
                : OperationResult.Failure("Cập nhật thông tin thiết bị thất bại!");
        }

        /// <summary>
        /// Nghiệp vụ xóa thiết bị theo mã thiết bị.
        /// </summary>
        public OperationResult XoaThietBi(string maTB)
        {
            string normalizedMaTB = (maTB ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedMaTB))
            {
                return OperationResult.Failure("Vui lòng chọn thiết bị cần xóa!", "MaTB");
            }

            if (!_thietBiDAL.TonTaiMaTB(normalizedMaTB))
            {
                return OperationResult.Failure($"Không tìm thấy thiết bị có mã '{normalizedMaTB}' để xóa!", "MaTB");
            }

            bool deleted = _thietBiDAL.Xoa(normalizedMaTB);
            return deleted
                ? OperationResult.Success("Xóa thiết bị thành công!")
                : OperationResult.Failure("Xóa thiết bị thất bại!");
        }
    }
}
