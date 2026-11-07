using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    /// <summary>
    /// Thực thể Thiết bị phòng học (Bảng ThietBi).
    /// Mỗi thiết bị thuộc về một loại thiết bị thông qua khóa ngoại bắt buộc MaLoai.
    /// </summary>
    [Table("ThietBi")]
    public class ThietBi
    {
        /// <summary>
        /// Mã thiết bị - Khóa chính, tối đa 20 ký tự.
        /// </summary>
        [Key]
        [StringLength(20)]
        public string MaTB { get; set; } = string.Empty;

        /// <summary>
        /// Tên thiết bị - Bắt buộc nhập, tối đa 150 ký tự.
        /// </summary>
        [Required]
        [StringLength(150)]
        public string TenTB { get; set; } = string.Empty;

        /// <summary>
        /// Ngày mua thiết bị.
        /// </summary>
        [Required]
        public DateTime NgayMua { get; set; }

        /// <summary>
        /// Số lượng thiết bị (số nguyên lớn hơn 0).
        /// </summary>
        [Required]
        public int SoLuong { get; set; }

        /// <summary>
        /// Đơn giá thiết bị - kiểu decimal(18,2), lớn hơn 0.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        /// <summary>
        /// Mã loại thiết bị - Khóa ngoại bắt buộc, tối đa 10 ký tự.
        /// </summary>
        [Required]
        [StringLength(10)]
        public string MaLoai { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property: Loại thiết bị tương ứng.
        /// </summary>
        [ForeignKey(nameof(MaLoai))]
        [InverseProperty(nameof(Entities.LoaiThietBi.ThietBis))]
        public virtual LoaiThietBi? LoaiThietBi { get; set; }
    }
}
