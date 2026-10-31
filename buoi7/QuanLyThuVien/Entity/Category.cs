using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLyThuVienDAO.Entity
{
    public class Category
    {
        [Key]
        [Required(ErrorMessage = "Mã thể loại không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã thể loại không được vượt quá 10 ký tự!")]
        public string CategoryId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên hoặc mô tả thể loại không được để trống!")]
        [StringLength(200, ErrorMessage = "Mô tả thể loại không được vượt quá 200 ký tự!")]
        public string Description { get; set; } = string.Empty;

        // Một thể loại có nhiều sách
        public List<Book> Books { get; set; } = new();

        public List<ValidationResult>? IsInValid()
        {
            var valcontext = new ValidationContext(this);
            var results = new List<ValidationResult>();
            var kt = Validator.TryValidateObject(this, valcontext, results, validateAllProperties: true);
            if (!kt)
            {
                return results;
            }
            return null;
        }
    }
}
