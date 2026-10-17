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
    /// Business Logic Layer (BUL) cho thực thể SinhVien.
    /// Chịu trách nhiệm kiểm tra tính hợp lệ dữ liệu bằng Data Annotations,
    /// chuẩn hóa chuỗi nhập liệu bằng Regex, áp dụng quy tắc nghiệp vụ,
    /// và ném ngoại lệ tường minh về tầng Giao diện (Views).
    /// </summary>
    public class SinhVienBUL
    {
        private readonly SinhVienDAL svd;

        public SinhVienBUL()
        {
            svd = new SinhVienDAL();
        }

        public SinhVienBUL(SinhVienDAL? dal)
        {
            svd = dal ?? new SinhVienDAL();
        }

        public SinhVienBUL(string connectionString)
        {
            svd = new SinhVienDAL(connectionString);
        }

        public SinhVien? GetSinhVienByMaSV(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return null;

            try
            {
                return svd.GetSinhVienByMaSV(maSV.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi tìm sinh viên mã '{maSV}': {ex.Message}", ex);
            }
        }

        public List<SinhVien> GetSinhVienByMaLop(string maLop)
        {
            try
            {
                return svd.GetSinhViensByMaLop(maLop);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi lấy danh sách sinh viên lớp '{maLop}': {ex.Message}", ex);
            }
        }

        public List<SinhVien> GetAllSinhVien()
        {
            try
            {
                return svd.GetAllSinhVien();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy toàn bộ danh sách sinh viên: " + ex.Message, ex);
            }
        }

        public List<SinhVien> GetAllSinhVienUsingDataReader()
        {
            try
            {
                return svd.getAllSinhVien1();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy sinh viên bằng SqlDataReader: " + ex.Message, ex);
            }
        }

        public void AddSinhVien(SinhVien sv)
        {
            if (sv == null)
            {
                throw new ArgumentNullException(nameof(sv), "Thông tin sinh viên không được rỗng.");
            }

            // 1. Kiểm tra tính hợp lệ dữ liệu bằng Data Annotations
            List<ValidationResult> vdr = sv.IsInValid();
            if (vdr.Count > 0)
            {
                throw new Exception(string.Join("\n", vdr.Select(vr => vr.ErrorMessage)));
            }

            // 2. Chuẩn hóa dữ liệu bằng Regex: loại bỏ khoảng trắng thừa đầu/cuối và giữa các từ
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

            // 3. Gọi tầng Data Access Layer và bắt/ném ngoại lệ
            try
            {
                svd.AddSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi thêm sinh viên: " + ex.Message, ex);
            }
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            if (sv == null)
            {
                throw new ArgumentNullException(nameof(sv), "Thông tin sinh viên không được rỗng.");
            }

            // 1. Kiểm tra tính hợp lệ dữ liệu bằng Data Annotations
            var validationResults = sv.IsInValid();
            if (validationResults.Count > 0)
            {
                throw new Exception(string.Join("\n", validationResults.Select(vr => vr.ErrorMessage)));
            }

            // 2. Chuẩn hóa dữ liệu
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

            // 3. Gọi tầng DAL và ném ngoại lệ nếu có
            try
            {
                svd.UpdateSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi cập nhật sinh viên: " + ex.Message, ex);
            }
        }

        public void DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                throw new ArgumentException("Mã sinh viên cần xóa không được để trống.", nameof(maSV));
            }

            try
            {
                svd.DeleteSinhVien(maSV.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi xóa sinh viên: " + ex.Message, ex);
            }
        }

        public List<SinhVien> Search(string? keyword, string? maLop, double diemTu)
        {
            try
            {
                return svd.Search(keyword, maLop, diemTu);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi tìm kiếm sinh viên: " + ex.Message, ex);
            }
        }
    }
}
