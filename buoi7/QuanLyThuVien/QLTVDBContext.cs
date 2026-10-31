using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVienDAO
{
    /// <summary>
    /// Lớp ngữ cảnh dữ liệu Entity Framework Core (Code First) cho hệ thống Quản lý Thư viện.
    /// Quản lý các tập thực thể Categories, Books, BookCopies, cấu hình Fluent API và nạp dữ liệu mẫu (Seed Data).
    /// </summary>
    public class QLTVDBContext : DbContext
    {
        public static string DefaultConnectionString =
            Environment.GetEnvironmentVariable("QLTV_CONNECTION_STRING")
            ?? @"Server=LAPTOP-6SN65NCS\HUECTMDB;Database=QLTV_EFCore;Integrated Security=True;TrustServerCertificate=True;";

        private readonly string? _connectionString;

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookCopy> BookCopies { get; set; } = null!;

        public QLTVDBContext()
        {
            _connectionString = DefaultConnectionString;
        }

        public QLTVDBContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public QLTVDBContext(DbContextOptions<QLTVDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connStr = string.IsNullOrWhiteSpace(_connectionString)
                    ? DefaultConnectionString
                    : _connectionString;
                optionsBuilder.UseSqlServer(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng Categories
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.CategoryId);

                entity.Property(c => c.CategoryId)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(c => c.Description)
                    .HasMaxLength(200)
                    .IsRequired();
            });

            // 2. Cấu hình bảng Books và quan hệ N - 1 với Categories
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(b => b.BookId);

                entity.Property(b => b.BookId)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(b => b.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(b => b.Publisher)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(b => b.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(b => b.NumberOfCopies)
                    .IsRequired();

                entity.Property(b => b.Author)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(b => b.PublicationYear)
                    .IsRequired();

                entity.Property(b => b.CategoryId)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Ignore(b => b.CategoryName);

                entity.HasOne(b => b.Category)
                    .WithMany(c => c.Books)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Books_Categories");
            });

            // 3. Cấu hình bảng BookCopies và quan hệ N - 1 với Books
            modelBuilder.Entity<BookCopy>(entity =>
            {
                entity.ToTable("BookCopies");
                entity.HasKey(bc => bc.BookCopyId);

                entity.Property(bc => bc.BookCopyId)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(bc => bc.Status)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(bc => bc.Condition)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(bc => bc.BookId)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasOne(bc => bc.Book)
                    .WithMany(b => b.BookCopies)
                    .HasForeignKey(bc => bc.BookId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BookCopies_Books");
            });

            // 4. Nạp dữ liệu mẫu (Seed Data)
            modelBuilder.Entity<Category>().HasData(GetSeedCategories());
            modelBuilder.Entity<Book>().HasData(GetSeedBooks());
            modelBuilder.Entity<BookCopy>().HasData(GetSeedBookCopies());
        }

        public static List<Category> GetSeedCategories()
        {
            return new List<Category>
            {
                new Category { CategoryId = "CAT01", Description = "Văn học" },
                new Category { CategoryId = "CAT02", Description = "Lịch sử" },
                new Category { CategoryId = "CAT03", Description = "Kinh tế" },
                new Category { CategoryId = "CAT04", Description = "Khoa học tự nhiên" },
                new Category { CategoryId = "CAT05", Description = "Công nghệ thông tin" },
                new Category { CategoryId = "CAT06", Description = "Kỹ năng sống" },
                new Category { CategoryId = "CAT07", Description = "Ngoại ngữ" }
            };
        }

        public static List<Book> GetSeedBooks()
        {
            return new List<Book>
            {
                new Book
                {
                    BookId = "B001",
                    Title = "Lập trình C# nâng cao với .NET",
                    CategoryId = "CAT05",
                    Author = "Nguyễn Văn An",
                    Publisher = "NXB Khoa học và Kỹ thuật",
                    Price = 155000m,
                    PublicationYear = 2024,
                    NumberOfCopies = 4
                },
                new Book
                {
                    BookId = "B002",
                    Title = "An toàn thông tin mạng",
                    CategoryId = "CAT05",
                    Author = "Đinh Văn Hùng",
                    Publisher = "NXB Thông tin và Truyền thông",
                    Price = 160000m,
                    PublicationYear = 2023,
                    NumberOfCopies = 2
                },
                new Book
                {
                    BookId = "B003",
                    Title = "Cấu trúc dữ liệu và giải thuật",
                    CategoryId = "CAT05",
                    Author = "Lê Hoàng Nam",
                    Publisher = "NXB Đại học Quốc gia",
                    Price = 135000m,
                    PublicationYear = 2021,
                    NumberOfCopies = 3
                },
                new Book
                {
                    BookId = "B004",
                    Title = "Kinh tế học vĩ mô căn bản",
                    CategoryId = "CAT03",
                    Author = "Trần Quốc Bảo",
                    Publisher = "NXB Kinh tế Quốc dân",
                    Price = 150000m,
                    PublicationYear = 2020,
                    NumberOfCopies = 2
                },
                new Book
                {
                    BookId = "B005",
                    Title = "Nguyên lý kế toán doanh nghiệp",
                    CategoryId = "CAT03",
                    Author = "Vũ Thị Mai",
                    Publisher = "NXB Tài chính",
                    Price = 125000m,
                    PublicationYear = 2021,
                    NumberOfCopies = 2
                },
                new Book
                {
                    BookId = "B006",
                    Title = "Tuyển tập truyện ngắn quê hương",
                    CategoryId = "CAT01",
                    Author = "Trần Ngọc Lan",
                    Publisher = "NXB Văn học",
                    Price = 110000m,
                    PublicationYear = 2020,
                    NumberOfCopies = 5
                },
                new Book
                {
                    BookId = "B007",
                    Title = "Tìm hiểu lịch sử Việt Nam",
                    CategoryId = "CAT02",
                    Author = "Đặng Quốc Huy",
                    Publisher = "NXB Giáo dục",
                    Price = 135000m,
                    PublicationYear = 2019,
                    NumberOfCopies = 3
                },
                new Book
                {
                    BookId = "B008",
                    Title = "Hóa học đại cương",
                    CategoryId = "CAT04",
                    Author = "Ngô Quang Vinh",
                    Publisher = "NXB Giáo dục",
                    Price = 120000m,
                    PublicationYear = 2019,
                    NumberOfCopies = 2
                },
                new Book
                {
                    BookId = "B009",
                    Title = "Lịch sử văn minh thế giới",
                    CategoryId = "CAT02",
                    Author = "Trịnh Văn Phúc",
                    Publisher = "NXB Thế giới",
                    Price = 145000m,
                    PublicationYear = 2018,
                    NumberOfCopies = 2
                },
                new Book
                {
                    BookId = "B010",
                    Title = "Ngữ pháp tiếng Anh học thuật",
                    CategoryId = "CAT07",
                    Author = "Phạm Thu Hà",
                    Publisher = "NXB Tổng hợp TP.HCM",
                    Price = 115000m,
                    PublicationYear = 2022,
                    NumberOfCopies = 2
                },
                new Book
                {
                    BookId = "B011",
                    Title = "Nhập môn Trí tuệ nhân tạo",
                    CategoryId = "CAT05",
                    Author = "Nguyễn Đức Thành",
                    Publisher = "NXB Bách Khoa",
                    Price = 180000m,
                    PublicationYear = 2023,
                    NumberOfCopies = 3
                },
                new Book
                {
                    BookId = "B012",
                    Title = "Sinh học và môi trường",
                    CategoryId = "CAT04",
                    Author = "Phan Văn Bình",
                    Publisher = "NXB Đại học Quốc gia",
                    Price = 140000m,
                    PublicationYear = 2021,
                    NumberOfCopies = 2
                },
                new Book
                {
                    BookId = "B013",
                    Title = "Kỹ năng giao tiếp và thuyết trình",
                    CategoryId = "CAT06",
                    Author = "Lê Minh Khôi",
                    Publisher = "NXB Trẻ",
                    Price = 95000m,
                    PublicationYear = 2021,
                    NumberOfCopies = 3
                },
                new Book
                {
                    BookId = "B014",
                    Title = "Quản trị doanh nghiệp hiện đại",
                    CategoryId = "CAT03",
                    Author = "Lê Thị Thanh",
                    Publisher = "NXB Tài chính",
                    Price = 175000m,
                    PublicationYear = 2023,
                    NumberOfCopies = 3
                },
                new Book
                {
                    BookId = "B015",
                    Title = "Tài chính cá nhân thực hành",
                    CategoryId = "CAT03",
                    Author = "Đỗ Minh Quân",
                    Publisher = "NXB Lao động",
                    Price = 130000m,
                    PublicationYear = 2022,
                    NumberOfCopies = 3
                },
                new Book
                {
                    BookId = "B016",
                    Title = "Phân tích dữ liệu với Python",
                    CategoryId = "CAT05",
                    Author = "Hoàng Minh Tuấn",
                    Publisher = "NXB Khoa học và Kỹ thuật",
                    Price = 165000m,
                    PublicationYear = 2024,
                    NumberOfCopies = 3
                }
            };
        }

        public static List<BookCopy> GetSeedBookCopies()
        {
            var copies = new List<BookCopy>();
            foreach (var book in GetSeedBooks())
            {
                for (int i = 1; i <= book.NumberOfCopies; i++)
                {
                    copies.Add(new BookCopy
                    {
                        BookCopyId = $"{book.BookId}.{i:D3}",
                        BookId = book.BookId,
                        Status = "Sẵn sàng",
                        Condition = "Mới"
                    });
                }
            }
            return copies;
        }
    }
}
