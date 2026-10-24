using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace QuanLySinhVien.Data.Entity
{
    /// <summary>
    /// Thực thể SinhVien (Student) đại diện cho thông tin một sinh viên.
    /// Sử dụng Entity Framework Core Code First ánh xạ sang bảng "SinhVien" trong CSDL.
    /// Quan hệ N - 1 với LopHoc.
    /// </summary>
    [Table("SinhVien")]
    public class SinhVien
    {
        [Key]
        [Required(ErrorMessage = "Mã sinh viên không được để trống.")]
        [StringLength(20, ErrorMessage = "Mã sinh viên tối đa 20 ký tự.")]
        [RegularExpression(@"^SV\d{3,}$", ErrorMessage = "Mã sinh viên phải có định dạng bắt đầu bằng 'SV' và theo sau bởi ít nhất 3 chữ số (ví dụ: SV000123).")]
        [Display(Name = "Mã sinh viên")]
        [Column("MaSV")]
        public string MaSV { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ và tên phải có từ 2 đến 100 ký tự.")]
        [Display(Name = "Họ và tên")]
        [Column("HoTen")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được để trống.")]
        [Display(Name = "Ngày sinh")]
        [Column("NgaySinh", TypeName = "datetime")]
        public DateTime NgaySinh { get; set; } = new DateTime(2006, 1, 1);

        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        [StringLength(10)]
        [Display(Name = "Giới tính")]
        [Column("GioiTinh")]
        public string GioiTinh { get; set; } = "Nam";

        [Required(ErrorMessage = "Email không được để trống.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng hợp lệ (ví dụ: an.nv@vju.ac.vn).")]
        [Display(Name = "Email")]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [StringLength(20)]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải gồm đúng 10 chữ số và bắt đầu bằng số 0.")]
        [Display(Name = "Số điện thoại")]
        [Column("SoDienThoai")]
        public string SoDienThoai { get; set; } = string.Empty;

        /// <summary>
        /// Alias tương thích ngược cho thuộc tính DienThoai
        /// </summary>
        [NotMapped]
        public string DienThoai
        {
            get => SoDienThoai;
            set => SoDienThoai = value;
        }

        [Required(ErrorMessage = "Điểm không được để trống.")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải nằm trong khoảng từ 0.0 đến 10.0.")]
        [Display(Name = "Điểm")]
        [Column("Diem")]
        public double Diem { get; set; } = 0.0;

        [Required(ErrorMessage = "Vui lòng chọn lớp học.")]
        [StringLength(20)]
        [Display(Name = "Mã lớp")]
        [Column("MaLop")]
        public string MaLop { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        [Column("TrangThai")]
        public string TrangThai { get; set; } = "Đang học";

        /// <summary>
        /// Navigation Property: Tham chiếu đến LopHoc trong quan hệ N - 1 (EF Core)
        /// </summary>
        [ForeignKey(nameof(MaLop))]
        public virtual LopHoc? LopHoc { get; set; }

        /// <summary>
        /// Thuộc tính tiện ích hiển thị tên lớp trên lưới
        /// </summary>
        [NotMapped]
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
            return $"[{MaSV}] {HoTen} - {GioiTinh} - {TenLop} - Điểm: {Diem:F1} - {TrangThai}";
        }
    }
}
