using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Data.Entity
{
    /// <summary>
    /// Thực thể LopHoc đại diện cho một lớp học trong hệ thống.
    /// Quan hệ 1 - N với SinhVien (Một lớp có nhiều sinh viên).
    /// </summary>
    public class LopHoc
    {
        [Required(ErrorMessage = "Mã lớp không được để trống.")]
        [RegularExpression(@"^[A-Z0-9]{4,10}$", ErrorMessage = "Mã lớp gồm 4-10 ký tự chữ in hoa và chữ số (ví dụ: KTPM01, CSE0001).")]
        [Display(Name = "Mã lớp")]
        public string MaLop { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên lớp không được để trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên lớp phải từ 3 đến 100 ký tự.")]
        [Display(Name = "Tên lớp")]
        public string TenLop { get; set; } = string.Empty;

        [Display(Name = "Khoa / Ngành")]
        public string Khoa { get; set; } = string.Empty;

        /// <summary>
        /// Danh sách sinh viên thuộc về lớp học (Quan hệ 1 - N)
        /// </summary>
        public List<SinhVien> DanhSachSinhVien { get; set; } = new List<SinhVien>();

        public LopHoc() { }

        public LopHoc(string maLop, string tenLop, string khoa = "")
        {
            MaLop = maLop;
            TenLop = tenLop;
            Khoa = khoa;
            DanhSachSinhVien = new List<SinhVien>();
        }

        /// <summary>
        /// Thêm một sinh viên vào danh sách của lớp và gán tham chiếu ngược
        /// </summary>
        public void ThemSinhVien(SinhVien sv)
        {
            if (sv != null && !DanhSachSinhVien.Exists(s => s.MaSV == sv.MaSV))
            {
                sv.MaLop = this.MaLop;
                sv.LopHoc = this;
                DanhSachSinhVien.Add(sv);
            }
        }

        /// <summary>
        /// Xóa sinh viên khỏi lớp theo mã sinh viên
        /// </summary>
        public bool XoaSinhVien(string maSV)
        {
            var sv = DanhSachSinhVien.Find(s => s.MaSV == maSV);
            if (sv != null)
            {
                sv.LopHoc = null;
                return DanhSachSinhVien.Remove(sv);
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ bằng Data Annotations
        /// </summary>
        public List<ValidationResult> IsInValid()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, results, validateAllProperties: true);
            return results;
        }

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

        public (bool IsValid, List<string> Errors) Validate()
        {
            bool valid = IsValid(out var errors);
            return (valid, errors);
        }

        public override string ToString()
        {
            return $"{TenLop} ({MaLop}) - Sĩ số: {DanhSachSinhVien.Count}";
        }
    }
}
