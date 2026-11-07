using System.Configuration;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    /// <summary>
    /// Ngữ cảnh cơ sở dữ liệu Entity Framework Core (Code First) cho ứng dụng Quản lý Thiết bị.
    /// Đọc chuỗi kết nối 'QLThietBiConn' từ tệp App.config và khởi tạo dữ liệu mẫu thông qua Migration.
    /// </summary>
    public class QLThietBiContext : DbContext
    {
        public QLThietBiContext()
        {
        }

        public QLThietBiContext(DbContextOptions<QLThietBiContext> options)
            : base(options)
        {
        }

        public DbSet<LoaiThietBi> LoaiThietBis { get; set; } = null!;
        public DbSet<ThietBi> ThietBis { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connStr = ConfigurationManager.ConnectionStrings["QLThietBiConn"]?.ConnectionString;

                // Hỗ trợ đọc tệp App.config của GUI khi chạy lệnh Add-Migration / Update-Database (Design-time)
                if (string.IsNullOrWhiteSpace(connStr))
                {
                    string[] candidatePaths =
                    {
                        Path.Combine(AppContext.BaseDirectory, "App.config"),
                        Path.Combine(Directory.GetCurrentDirectory(), "App.config"),
                        Path.Combine(Directory.GetCurrentDirectory(), "..", "GUI", "App.config"),
                        Path.Combine(Directory.GetCurrentDirectory(), "GUI", "App.config")
                    };

                    foreach (string path in candidatePaths)
                    {
                        if (File.Exists(path))
                        {
                            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = path };
                            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                            connStr = config.ConnectionStrings.ConnectionStrings["QLThietBiConn"]?.ConnectionString;
                            if (!string.IsNullOrWhiteSpace(connStr))
                            {
                                break;
                            }
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(connStr))
                {
                    throw new InvalidOperationException("Không tìm thấy chuỗi kết nối 'QLThietBiConn' trong tệp App.config.");
                }

                optionsBuilder.UseSqlServer(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nạp dữ liệu mẫu ban đầu thông qua Migration (cấu hình bảng/khóa đã khai báo bằng Data Annotations ở Entity)
            modelBuilder.Entity<LoaiThietBi>().HasData(
                new LoaiThietBi { MaLoai = "LTB01", TenLoai = "Thiết bị trình chiếu" },
                new LoaiThietBi { MaLoai = "LTB02", TenLoai = "Thiết bị máy tính" },
                new LoaiThietBi { MaLoai = "LTB03", TenLoai = "Thiết bị mạng" },
                new LoaiThietBi { MaLoai = "LTB04", TenLoai = "Thiết bị âm thanh" }
            );

            modelBuilder.Entity<ThietBi>().HasData(
                new ThietBi
                {
                    MaTB = "TB001",
                    TenTB = "Máy chiếu Epson EB-X06",
                    NgayMua = new DateTime(2024, 8, 15),
                    SoLuong = 10,
                    DonGia = 12500000m,
                    MaLoai = "LTB01"
                },
                new ThietBi
                {
                    MaTB = "TB002",
                    TenTB = "Máy tính Dell OptiPlex",
                    NgayMua = new DateTime(2024, 8, 20),
                    SoLuong = 25,
                    DonGia = 15000000m,
                    MaLoai = "LTB02"
                },
                new ThietBi
                {
                    MaTB = "TB003",
                    TenTB = "Bộ phát Wi-Fi TP-Link",
                    NgayMua = new DateTime(2024, 9, 1),
                    SoLuong = 12,
                    DonGia = 1850000m,
                    MaLoai = "LTB03"
                },
                new ThietBi
                {
                    MaTB = "TB004",
                    TenTB = "Loa trợ giảng",
                    NgayMua = new DateTime(2024, 9, 5),
                    SoLuong = 8,
                    DonGia = 2400000m,
                    MaLoai = "LTB04"
                },
                new ThietBi
                {
                    MaTB = "TB005",
                    TenTB = "Bảng tương tác thông minh",
                    NgayMua = new DateTime(2024, 9, 10),
                    SoLuong = 2,
                    DonGia = 42000000m,
                    MaLoai = "LTB01"
                }
            );
        }
    }
}
