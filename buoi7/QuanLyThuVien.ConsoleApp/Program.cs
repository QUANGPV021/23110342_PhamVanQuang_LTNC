using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVienDAO;
using QuanLyThuVienDAO.Business;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVien.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("========================================================================================");
            Console.WriteLine("   KIỂM THỬ BÀI THỰC HÀNH BUỔI 7: QUẢN LÝ ĐẦU SÁCH VÀ BẢN SAO (3-LAYER + EF CORE)       ");
            Console.WriteLine("========================================================================================\n");

            // 1. Kiểm tra mô hình thực thể (Entity Layer) & Quan hệ 1 - N
            Console.WriteLine("--- [1] KHÁM PHÁ MÔ HÌNH THỰC THỂ EF CORE (CATEGORY -> BOOK -> BOOKCOPY) ---");
            using (var context = new QLTVDBContext())
            {
                foreach (var clrType in new[] { typeof(Category), typeof(Book), typeof(BookCopy) })
                {
                    var et = context.Model.FindEntityType(clrType);
                    string pk = string.Join(", ", et?.FindPrimaryKey()?.Properties.Select(p => p.Name) ?? Array.Empty<string>());
                    Console.WriteLine($" • Entity: {clrType.Name,-10} | Bảng: {et?.GetTableName(),-12} | Khóa chính: {pk}");
                    foreach (var fk in et?.GetForeignKeys() ?? Enumerable.Empty<Microsoft.EntityFrameworkCore.Metadata.IForeignKey>())
                    {
                        Console.WriteLine($"   └─ Khóa ngoại: {string.Join(", ", fk.Properties.Select(p => p.Name))} -> {fk.PrincipalEntityType.ClrType.Name} (1 - N)");
                    }
                }
            }

            Console.WriteLine($"\n • Dữ liệu mẫu khởi tạo (Seed Data): {QLTVDBContext.GetSeedCategories().Count} thể loại, " +
                              $"{QLTVDBContext.GetSeedBooks().Count} đầu sách, {QLTVDBContext.GetSeedBookCopies().Count} bản sao.");

            // 2. Kiểm thử Data Annotations & IsInValid()
            Console.WriteLine("\n--- [2] KIỂM THỬ DATA ANNOTATIONS & PHƯƠNG THỨC IsInValid() ---");
            var validBook = new Book
            {
                BookId = "B099",
                Title = "Thiết kế phần mềm hướng đối tượng",
                Author = "Phạm Văn Quang",
                Publisher = "NXB Đại học Quốc gia",
                CategoryId = "CAT05",
                Price = 150000m,
                PublicationYear = 2026,
                NumberOfCopies = 3
            };
            Console.WriteLine($" • Đầu sách hợp lệ: {(validBook.IsInValid() == null ? "PASSED (Hợp lệ)" : "FAILED")}");

            var invalidBook = new Book
            {
                BookId = "",
                Title = "",
                Author = "",
                Publisher = "",
                CategoryId = "",
                Price = -5000m,
                PublicationYear = 500,
                NumberOfCopies = -2
            };
            var validationErrors = invalidBook.IsInValid();
            Console.WriteLine($" • Đầu sách vi phạm: Đã phát hiện {validationErrors?.Count ?? 0} lỗi [PASSED]");

            // 3. Kiểm thử tầng Business Logic Layer (ManageBookBuss, BookCopyBuss, CategoryBuss)
            Console.WriteLine("\n--- [3] KIỂM THỬ TẦNG BUSINESS LOGIC LAYER ---");
            using (var context = new QLTVDBContext())
            {
                var bookBuss = new ManageBookBuss(context);
                var copyBuss = new BookCopyBuss(context);
                var cateBuss = new CategoryBuss(context);

                try
                {
                    bookBuss.Add(invalidBook);
                    Console.WriteLine(" • FAILED: Lẽ ra ManageBookBuss phải chặn sách không hợp lệ!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" • ManageBookBuss chặn sách sai quy tắc [PASSED]: {ex.Message}");
                }

                try
                {
                    copyBuss.Delete("");
                    Console.WriteLine(" • FAILED: Lẽ ra BookCopyBuss phải chặn mã bản sao trống!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" • BookCopyBuss chặn mã bản sao rỗng [PASSED]: {ex.Message}");
                }

                try
                {
                    cateBuss.Add(new Category { CategoryId = "QUA_DAI_123456", Description = "" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" • CategoryBuss chặn mã thể loại vượt quá 10 ký tự [PASSED]: {ex.Message}");
                }
            }

            // 4. Kiểm tra kết nối SQL Server thực tế (nếu máy đang bật SQL Server)
            Console.WriteLine("\n--- [4] KIỂM TRA KẾT NỐI SQL SERVER ---");
            Console.WriteLine($" • Chuỗi kết nối: {QLTVDBContext.DefaultConnectionString}");
            try
            {
                using var context = new QLTVDBContext();
                if (context.Database.CanConnect())
                {
                    var buss = new ManageBookBuss(context);
                    var books = buss.GetAll();
                    Console.WriteLine($" • KẾT NỐI THÀNH CÔNG! Đã tải {books.Count} đầu sách từ SQL Server.");
                    foreach (var b in books.Take(5))
                    {
                        Console.WriteLine($"   - [{b.BookId}] {b.Title} | {b.CategoryName} | {b.Author} | {b.Price:N0}đ ({b.NumberOfCopies} bản sao)");
                    }
                }
                else
                {
                    Console.WriteLine(" • SQL Server chưa khởi chạy hoặc CSDL chưa được tạo. Nhấn F5 trên WinForms hoặc chạy 'Update-Database' để tự động khởi tạo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • [Thông tin kết nối]: {ex.Message}");
            }

            Console.WriteLine("\n========================================================================================");
            Console.WriteLine("   HOÀN TẤT KIỂM THỬ BUỔI 7: ĐẦY ĐỦ 3 TẦNG ENTITY - REPOSISTORY - BUSINESS - VIEWS!     ");
            Console.WriteLine("========================================================================================");
        }
    }
}
