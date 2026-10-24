using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien.Data.Entity
{
    /// <summary>
    /// Thực thể LopHoc đại diện cho một lớp học trong hệ thống.
    /// Sử dụng Entity Framework Core Code First ánh xạ sang bảng "LopHoc" trong CSDL.
    /// Quan hệ 1 - N với SinhVien (Một lớp có nhiều sinh viên).
    /// </summary>
    [Table("LopHoc")]
    public class LopHoc
    {
        [Key]
        [Required(ErrorMessage = "Mã lớp không được để trống.")]
        [StringLength(20, ErrorMessage = "Mã lớp tối đa 20 ký tự.")]
        [RegularExpression(@"^[A-Z0-9]{4,10}$", ErrorMessage = "Mã lớp gồm 4-10 ký tự chữ in hoa và chữ số (ví dụ: KTPM01, CSE0001).")]
        [Display(Name = "Mã lớp")]
        [Column("MaLop")]
        public string MaLop { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên lớp không được để trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên lớp phải từ 3 đến 100 ký tự.")]
        [Display(Name = "Tên lớp")]
        [Column("TenLop")]
        public string TenLop { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Tên khoa tối đa 100 ký tự.")]
        [Display(Name = "Khoa / Ngành")]
        [Column("Khoa")]
        public string Khoa { get; set; } = string.Empty;

        /// <summary>
        /// Navigation Property: Danh sách sinh viên thuộc về lớp học (Quan hệ 1 - N trong EF Core)
        /// </summary>
        public virtual ICollection<SinhVien> DanhSachSinhVien { get; set; } = new List<SinhVien>();

        public LopHoc()
        {
            DanhSachSinhVien = new List<SinhVien>();
        }

        public LopHoc(string maLop, string tenLop, string khoa = "")
        {
            MaLop = maLop;
            TenLop = tenLop;
            Khoa = khoa;
            DanhSachSinhVien = new List<SinhVien>();
        }

        public void ThemSinhVien(SinhVien sv)
        {
            if (sv != null && !DanhSachSinhVien.Any(s => s.MaSV == sv.MaSV))
            {
                sv.MaLop = this.MaLop;
                sv.LopHoc = this;
                DanhSachSinhVien.Add(sv);
            }
        }

        public bool XoaSinhVien(string maSV)
        {
            var sv = DanhSachSinhVien.FirstOrDefault(s => s.MaSV == maSV);
            if (sv != null)
            {
                sv.LopHoc = null;
                return DanhSachSinhVien.Remove(sv);
            }
            return false;
        }

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
