using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    /// <summary>
    /// Lớp LopHoc (SchoolClass) đại diện cho một lớp học trong hệ thống.
    /// Quan hệ 1 - N với SinhVien (Một lớp học chứa nhiều sinh viên).
    /// Sử dụng Data Annotations để xác thực (validate) dữ liệu.
    /// </summary>
    public class LopHoc
    {
        [Required(ErrorMessage = "Mã lớp không được để trống.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Mã lớp phải có độ dài từ 2 đến 20 ký tự.")]
        [Display(Name = "Mã lớp")]
        public string MaLop { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên lớp không được để trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên lớp phải có độ dài từ 3 đến 100 ký tự.")]
        [Display(Name = "Tên lớp")]
        public string TenLop { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Mô tả lớp học tối đa 200 ký tự.")]
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; } = string.Empty;

        /// <summary>
        /// Danh sách sinh viên thuộc lớp học này (Quan hệ 1 - N).
        /// </summary>
        public List<SinhVien> DanhSachSinhVien { get; set; } = new List<SinhVien>();

        public LopHoc() { }

        public LopHoc(string maLop, string tenLop, string moTa = "")
        {
            MaLop = maLop;
            TenLop = tenLop;
            MoTa = moTa;
        }

        /// <summary>
        /// Phương thức kiểm tra xem các thuộc tính của đối tượng LopHoc có hợp lệ hay không
        /// dựa trên các Data Annotations đã khai báo.
        /// </summary>
        /// <param name="errors">Danh sách thông báo lỗi nếu dữ liệu không hợp lệ</param>
        /// <returns>True nếu toàn bộ thuộc tính hợp lệ, ngược lại False</returns>
        public bool IsValid(out List<string> errors)
        {
            errors = new List<string>();
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this, context, results, validateAllProperties: true);

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    if (!string.IsNullOrWhiteSpace(validationResult.ErrorMessage))
                    {
                        errors.Add(validationResult.ErrorMessage);
                    }
                }
            }

            return isValid;
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
            return $"{TenLop} ({MaLop})";
        }
    }
}
