using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Data.Entity
{
    /// <summary>
    /// Thực thể SinhVien (Student) đại diện cho thông tin một sinh viên.
    /// Quan hệ N - 1 với LopHoc (Nhiều sinh viên thuộc về một lớp).
    /// Sử dụng Data Annotations để xác thực (validate) dữ liệu đầu vào.
    /// </summary>
    public class SinhVien
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống.")]
        [RegularExpression(@"^SV\d{3,}$", ErrorMessage = "Mã sinh viên phải có định dạng bắt đầu bằng 'SV' và theo sau bởi ít nhất 3 chữ số (ví dụ: SV000123).")]
        [Display(Name = "Mã sinh viên")]
        public string MaSV { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Họ và tên phải có từ 2 đến 50 ký tự.")]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được để trống.")]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; } = new DateTime(2006, 1, 1);

        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; } = "Nam";

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng hợp lệ (ví dụ: an.nv@vju.ac.vn).")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải gồm đúng 10 chữ số và bắt đầu bằng số 0.")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = string.Empty;

        /// <summary>
        /// Alias tương thích ngược cho thuộc tính DienThoai
        /// </summary>
        public string DienThoai
        {
            get => SoDienThoai;
            set => SoDienThoai = value;
        }

        [Required(ErrorMessage = "Điểm không được để trống.")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải nằm trong khoảng từ 0.0 đến 10.0.")]
        [Display(Name = "Điểm")]
        public double Diem { get; set; } = 0.0;

        [Required(ErrorMessage = "Vui lòng chọn lớp học.")]
        [Display(Name = "Mã lớp")]
        public string MaLop { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn trạng thái.")]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Đang học";

        /// <summary>
        /// Thuộc tính điều hướng quan hệ N - 1 đến LopHoc
        /// </summary>
        public LopHoc? LopHoc { get; set; }

        /// <summary>
        /// Tên lớp học để hiển thị lên DataGridView
        /// </summary>
        public string TenLop => LopHoc != null ? LopHoc.TenLop : MaLop;

        public SinhVien() { }

        public SinhVien(string maSV, string hoTen, DateTime ngaySinh, string gioiTinh,
                        string email, string soDienThoai, double diem, string maLop, string trangThai = "Đang học")
        {
            MaSV = maSV;
            HoTen = hoTen;
            NgaySinh = ngaySinh;
            GioiTinh = gioiTinh;
            Email = email;
            SoDienThoai = soDienThoai;
            Diem = diem;
            MaLop = maLop;
            TrangThai = trangThai;
        }

        /// <summary>
        /// Phương thức kiểm tra tính không hợp lệ theo Data Annotations.
        /// Trả về danh sách ValidationResult chứa các lỗi tương ứng với từng thuộc tính.
        /// Theo đúng yêu cầu trong tài liệu Bài thực hành 3 (sv.IsInValid()).
        /// </summary>
        public List<ValidationResult> IsInValid()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, results, validateAllProperties: true);
            return results;
        }

        /// <summary>
        /// Phương thức kiểm tra xem các thuộc tính của đối tượng SinhVien có hợp lệ hay không.
        /// </summary>
        /// <param name="errors">Danh sách thông báo lỗi nếu dữ liệu không hợp lệ</param>
        /// <returns>True nếu toàn bộ thuộc tính hợp lệ, ngược lại False</returns>
        public bool IsValid(out List<string> errors)
        {
            errors = new List<string>();
            var results = IsInValid();

            if (results.Count > 0)
            {
                foreach (var validationResult in results)
                {
                    if (!string.IsNullOrWhiteSpace(validationResult.ErrorMessage))
                    {
                        errors.Add(validationResult.ErrorMessage);
                    }
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Phương thức tiện ích trả về tuple (bool IsValid, List&lt;string&gt; Errors).
        /// </summary>
        public (bool IsValid, List<string> Errors) Validate()
        {
            bool valid = IsValid(out var errors);
            return (valid, errors);
        }

        public override string ToString()
        {
            return $"[{MaSV}] {HoTen} - {GioiTinh} - {TenLop} - Điểm: {Diem:F1} - {TrangThai}";
        }
    }
}
