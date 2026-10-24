using System;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.Context
{
    /// <summary>
    /// DbContext chính của ứng dụng Entity Framework Core theo phương pháp Code First.
    /// Quản lý kết nối, ánh xạ thực thể LopHoc và SinhVien sang cơ sở dữ liệu,
    /// cấu hình quan hệ 1 - N bằng Fluent API và nạp dữ liệu mẫu (Seed Data).
    /// </summary>
    public class QLSVDbContext : DbContext
    {
        public static string DefaultConnectionString = @"Server=LAPTOP-6SN65NCS\HUECTMDB;Database=QLSV_EFCore;Integrated Security=True;TrustServerCertificate=True;";

        private readonly string? _connectionString;

        public virtual DbSet<LopHoc> LopHocs { get; set; } = null!;
        public virtual DbSet<SinhVien> SinhViens { get; set; } = null!;

        public QLSVDbContext()
        {
            _connectionString = DefaultConnectionString;
        }

        public QLSVDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public QLSVDbContext(DbContextOptions<QLSVDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connStr = string.IsNullOrWhiteSpace(_connectionString) ? DefaultConnectionString : _connectionString;
                optionsBuilder.UseSqlServer(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng LopHoc bằng Fluent API
            modelBuilder.Entity<LopHoc>(entity =>
            {
                entity.ToTable("LopHoc");
                entity.HasKey(e => e.MaLop);

                entity.Property(e => e.MaLop)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.TenLop)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Khoa)
                    .HasMaxLength(100)
                    .IsRequired(false);
            });

            // 2. Cấu hình bảng SinhVien bằng Fluent API
            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.ToTable("SinhVien");
                entity.HasKey(e => e.MaSV);

                entity.Property(e => e.MaSV)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.HoTen)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.NgaySinh)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.GioiTinh)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Diem)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.MaLop)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(50)
                    .HasDefaultValue("Đang học");

                // Cấu hình quan hệ 1 - N giữa LopHoc và SinhVien
                entity.HasOne(s => s.LopHoc)
                    .WithMany(l => l.DanhSachSinhVien)
                    .HasForeignKey(s => s.MaLop)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SinhVien_LopHoc");

                entity.Ignore(s => s.DienThoai);
                entity.Ignore(s => s.TenLop);
            });

            // 3. Khởi tạo dữ liệu mẫu (Seed Data) bằng HasData theo chuẩn EF Core Code First
            modelBuilder.Entity<LopHoc>().HasData(
                new LopHoc { MaLop = "KTPM01", TenLop = "Kỹ thuật phần mềm 01", Khoa = "Công nghệ thông tin" },
                new LopHoc { MaLop = "TTNT01", TenLop = "Trí tuệ nhân tạo 01", Khoa = "Trí tuệ nhân tạo" },
                new LopHoc { MaLop = "KHDL01", TenLop = "Khoa học dữ liệu 01", Khoa = "Khoa học máy tính" },
                new LopHoc { MaLop = "HTTT01", TenLop = "Hệ thống thông tin 01", Khoa = "Hệ thống thông tin" },
                new LopHoc { MaLop = "CSE0001", TenLop = "Khoa học máy tính 01", Khoa = "Khoa học máy tính" },
                new LopHoc { MaLop = "CSE0002", TenLop = "Khoa học máy tính 02", Khoa = "Khoa học máy tính" },
                new LopHoc { MaLop = "CSE0003", TenLop = "Khoa học máy tính 03", Khoa = "Khoa học máy tính" }
            );

            modelBuilder.Entity<SinhVien>().HasData(
                new SinhVien
                {
                    MaSV = "SV000123",
                    MaLop = "KTPM01",
                    HoTen = "Nguyễn Văn An",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2006, 8, 15),
                    Diem = 8.5,
                    SoDienThoai = "0912345678",
                    Email = "an.nv@vju.ac.vn",
                    TrangThai = "Đang học"
                },
                new SinhVien
                {
                    MaSV = "SV000124",
                    MaLop = "TTNT01",
                    HoTen = "Trần Minh Anh",
                    GioiTinh = "Nữ",
                    NgaySinh = new DateTime(2006, 1, 22),
                    Diem = 9.0,
                    SoDienThoai = "0987654321",
                    Email = "anh.tm@vju.ac.vn",
                    TrangThai = "Đang học"
                },
                new SinhVien
                {
                    MaSV = "SV000125",
                    MaLop = "KTPM01",
                    HoTen = "Lê Hoàng Bình",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2006, 5, 9),
                    Diem = 7.4,
                    SoDienThoai = "0355556677",
                    Email = "binh.lh@vju.ac.vn",
                    TrangThai = "Đang học"
                },
                new SinhVien
                {
                    MaSV = "SV000126",
                    MaLop = "KHDL01",
                    HoTen = "Đỗ Thị Hồng",
                    GioiTinh = "Nữ",
                    NgaySinh = new DateTime(2006, 11, 30),
                    Diem = 8.1,
                    SoDienThoai = "0777888999",
                    Email = "hong.dt@vju.ac.vn",
                    TrangThai = "Đang học"
                },
                new SinhVien
                {
                    MaSV = "SV0001",
                    MaLop = "CSE0001",
                    HoTen = "Nguyễn Văn A",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2000, 1, 1),
                    Diem = 8.0,
                    SoDienThoai = "0911223344",
                    Email = "a.nv@vju.ac.vn",
                    TrangThai = "Đang học"
                },
                new SinhVien
                {
                    MaSV = "SV0002",
                    MaLop = "CSE0002",
                    HoTen = "Trần Thị B",
                    GioiTinh = "Nữ",
                    NgaySinh = new DateTime(2000, 2, 2),
                    Diem = 7.5,
                    SoDienThoai = "0922334455",
                    Email = "b.tt@vju.ac.vn",
                    TrangThai = "Đang học"
                },
                new SinhVien
                {
                    MaSV = "SV0003",
                    MaLop = "CSE0003",
                    HoTen = "Lê Văn C",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2000, 3, 3),
                    Diem = 8.8,
                    SoDienThoai = "0933445566",
                    Email = "c.lv@vju.ac.vn",
                    TrangThai = "Đang học"
                }
            );
        }
    }
}
