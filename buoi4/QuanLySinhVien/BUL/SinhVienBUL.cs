using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.BUL
{
    /// <summary>
    /// Business Logic Layer (BUL) cho SinhVien.
    /// Đảm nhiệm xử lý nghiệp vụ: kiểm tra tính hợp lệ qua Data Annotations,
    /// chuẩn hóa dữ liệu đầu vào (loại bỏ khoảng trắng thừa), và chuyển tiếp tới tầng DAL.
    /// </summary>
    public class SinhVienBUL
    {
        private readonly SinhVienDAL svd;

        public SinhVienBUL(SinhVienDAL? dal = null)
        {
            svd = dal ?? new SinhVienDAL();
        }

        public SinhVien? GetSinhVienByMaSV(string maSV)
        {
            return svd.GetSinhVienByMaSV(maSV);
        }

        public List<SinhVien> GetSinhVienByMaLop(string maLop)
        {
            return svd.GetSinhViensByMaLop(maLop);
        }

        public List<SinhVien> GetAllSinhVien()
        {
            return svd.GetAllSinhVien();
        }

        public void DeleteSinhVien(string maSV)
        {
            try
            {
                svd.DeleteSinhVien(maSV);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sinh viên: " + ex.Message, ex);
            }
        }

        public void AddSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            // Kiểm tra tính hợp lệ của sinh viên
            List<ValidationResult> vdr = sv.IsInValid();
            if (vdr.Count > 0)
            {
                throw new Exception(string.Join("\n", vdr.Select(vr => vr.ErrorMessage)));
            }

            // Chuẩn hóa dữ liệu: cắt khoảng trắng đầu/cuối và rút gọn khoảng trắng liên tiếp
            if (!string.IsNullOrEmpty(sv.HoTen))
            {
                sv.HoTen = Regex.Replace(sv.HoTen.Trim(), @"\s+", " ");
            }
            if (!string.IsNullOrEmpty(sv.MaSV))
            {
                sv.MaSV = sv.MaSV.Trim();
            }
            if (!string.IsNullOrEmpty(sv.Email))
            {
                sv.Email = sv.Email.Trim();
            }
            if (!string.IsNullOrEmpty(sv.SoDienThoai))
            {
                sv.SoDienThoai = sv.SoDienThoai.Trim();
            }

            try
            {
                svd.AddSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sinh viên: " + ex.Message, ex);
            }
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            // Nhận thông tin sinh viên từ giao diện và kiểm tra tính hợp lệ
            var validationResults = sv.IsInValid();
            if (validationResults.Count > 0)
            {
                throw new Exception(string.Join("\n", validationResults.Select(vr => vr.ErrorMessage)));
            }

            // Chuẩn hóa dữ liệu
            if (!string.IsNullOrEmpty(sv.HoTen))
            {
                sv.HoTen = Regex.Replace(sv.HoTen.Trim(), @"\s+", " ");
            }
            if (!string.IsNullOrEmpty(sv.MaSV))
            {
                sv.MaSV = sv.MaSV.Trim();
            }
            if (!string.IsNullOrEmpty(sv.Email))
            {
                sv.Email = sv.Email.Trim();
            }
            if (!string.IsNullOrEmpty(sv.SoDienThoai))
            {
                sv.SoDienThoai = sv.SoDienThoai.Trim();
            }

            try
            {
                svd.UpdateSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sinh viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Tìm kiếm nâng cao kết hợp từ khóa, lớp học và ngưỡng điểm sàn
        /// </summary>
        public List<SinhVien> Search(string? keyword, string? maLop, double diemTu)
        {
            return svd.Search(keyword, maLop, diemTu);
        }
    }
}
