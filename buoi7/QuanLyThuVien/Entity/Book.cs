using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLyThuVienDAO.Entity
{
    public class Book
    {
        [Key]
        [Required(ErrorMessage = "Mã sách không được để trống!")]
        [StringLength(20, ErrorMessage = "Mã sách không được vượt quá 20 ký tự!")]
        public string BookId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sách không được để trống!")]
        [StringLength(200, ErrorMessage = "Tên sách không được vượt quá 200 ký tự!")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nhà xuất bản không được để trống!")]
        [StringLength(150, ErrorMessage = "Nhà xuất bản không được vượt quá 150 ký tự!")]
        public string Publisher { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Giá sách không được âm!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số bản sao không được âm!")]
        public int NumberOfCopies { get; set; }

        [Required(ErrorMessage = "Tác giả không được để trống!")]
        [StringLength(150, ErrorMessage = "Tên tác giả không được vượt quá 150 ký tự!")]
        public string Author { get; set; } = string.Empty;

        [Range(1000, 9999, ErrorMessage = "Năm xuất bản phải từ 1000 đến 9999!")]
        public int PublicationYear { get; set; }

        // Khóa ngoại đến Category
        [Required(ErrorMessage = "Vui lòng chọn thể loại cho sách!")]
        [StringLength(10, ErrorMessage = "Mã thể loại không được vượt quá 10 ký tự!")]
        public string CategoryId { get; set; } = string.Empty;

        // Một sách thuộc một thể loại
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        // Một sách có nhiều bản sao
        public List<BookCopy> BookCopies { get; set; } = new();

        // Thuộc tính hỗ trợ hiển thị tên thể loại trên DataGridView (không ánh xạ vào CSDL)
        [NotMapped]
        public string CategoryName => Category?.Description ?? string.Empty;

        public List<ValidationResult>? IsInValid()
        {
            var valcontext = new ValidationContext(this);
            var results = new List<ValidationResult>();
            var kt = Validator.TryValidateObject(this, valcontext, results, validateAllProperties: true);
            if (!kt)
            {
                return results;
            }
            else
            {
                return null;
            }
        }
    }
}
