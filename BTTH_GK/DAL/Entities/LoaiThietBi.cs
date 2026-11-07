using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    /// <summary>
    /// Thực thể Loại thiết bị (Bảng LoaiThietBi).
    /// Quan hệ 1 - N với ThietBi: một loại thiết bị gồm nhiều thiết bị.
    /// </summary>
    [Table("LoaiThietBi")]
    public class LoaiThietBi
    {
        /// <summary>
        /// Mã loại thiết bị - Khóa chính, tối đa 10 ký tự.
        /// </summary>
        [Key]
        [StringLength(10)]
        public string MaLoai { get; set; } = string.Empty;

        /// <summary>
        /// Tên loại thiết bị - Bắt buộc nhập, tối đa 100 ký tự.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string TenLoai { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property: Danh sách các thiết bị thuộc loại này.
        /// </summary>
        [InverseProperty(nameof(ThietBi.LoaiThietBi))]
        public virtual ICollection<ThietBi> ThietBis { get; set; } = new List<ThietBi>();
    }
}
