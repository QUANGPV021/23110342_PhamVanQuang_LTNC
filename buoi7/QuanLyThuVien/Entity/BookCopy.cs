using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuVienDAO.Entity
{
    public class BookCopy
    {
        [Key]
        [Required(ErrorMessage = "Mã bản sao sách không được để trống!")]
        [StringLength(20, ErrorMessage = "Mã bản sao sách không được vượt quá 20 ký tự!")]
        public string BookCopyId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trạng thái bản sao sách không được để trống!")]
        [StringLength(20, ErrorMessage = "Trạng thái không được vượt quá 20 ký tự!")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tình trạng bản sao sách không được để trống!")]
        [StringLength(30, ErrorMessage = "Tình trạng không được vượt quá 30 ký tự!")]
        public string Condition { get; set; } = string.Empty;

        // Khóa ngoại đến Book
        [Required(ErrorMessage = "Vui lòng chọn sách cho bản sao!")]
        [StringLength(20, ErrorMessage = "Mã sách không được vượt quá 20 ký tự!")]
        public string BookId { get; set; } = string.Empty;

        // Mỗi bản sao thuộc một sách
        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }

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
