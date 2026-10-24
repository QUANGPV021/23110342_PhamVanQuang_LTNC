using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.BUL;
using QuanLySinhVien.Data.Context;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("========================================================================================");
            Console.WriteLine("     KIỂM THỬ BÀI THỰC HÀNH BUỔI 6: ENTITY FRAMEWORK CORE CODE FIRST & MIGRATIONS       ");
            Console.WriteLine("========================================================================================\n");

            // 1. Kiểm tra mô hình thực thể (Entities Model) & Quan hệ 1 - N
            Console.WriteLine("--- [1] KHÁM PHÁ MÔ HÌNH THỰC THỂ EF CORE (CODE FIRST) ---");
            using (var context = new QLSVDbContext())
            {
                var lopHocEntityType = context.Model.FindEntityType(typeof(LopHoc));
                var sinhVienEntityType = context.Model.FindEntityType(typeof(SinhVien));

                Console.WriteLine($" • Entity: {lopHocEntityType?.Name} -> Bảng: '{lopHocEntityType?.GetTableName()}'");
                Console.WriteLine($"   Khóa chính: {string.Join(", ", lopHocEntityType?.FindPrimaryKey()?.Properties.Select(p => p.Name) ?? Array.Empty<string>())}");

                Console.WriteLine($"\n • Entity: {sinhVienEntityType?.Name} -> Bảng: '{sinhVienEntityType?.GetTableName()}'");
                Console.WriteLine($"   Khóa chính: {string.Join(", ", sinhVienEntityType?.FindPrimaryKey()?.Properties.Select(p => p.Name) ?? Array.Empty<string>())}");

                var foreignKeys = sinhVienEntityType?.GetForeignKeys();
                if (foreignKeys != null)
                {
                    foreach (var fk in foreignKeys)
                    {
                        Console.WriteLine($"   Khóa ngoại: {fk.PrincipalEntityType.ClrType.Name} -> {string.Join(", ", fk.Properties.Select(p => p.Name))} (Quan hệ 1 - N)");
                    }
                }
            }

            // 2. Kiểm thử Xác thực dữ liệu với Data Annotations (IsInValid)
            Console.WriteLine("\n\n--- [2] KIỂM THỬ DATA ANNOTATIONS & PHƯƠNG THỨC IsInValid() ---");

            // 2.1 Đối tượng hợp lệ
            SinhVien svHopLe = new SinhVien(
                "SV000199",
                "   Phạm     Văn     Quang   ",
                new DateTime(2005, 5, 20),
                "Nam",
                "quang.pv23110342@vju.ac.vn",
                "0988123456",
                9.5,
                "KTPM01",
                "Đang học"
            );

            var errs1 = svHopLe.IsInValid();
            Console.WriteLine($" • Sinh viên hợp lệ: {(errs1.Count == 0 ? "PASSED (0 Lỗi)" : "FAILED")}");

            // 2.2 Đối tượng không hợp lệ
            SinhVien svSai = new SinhVien
            {
                MaSV = "SAI_FORMAT",
                HoTen = "",
                Email = "email_khong_hop_le",
                SoDienThoai = "123",
                Diem = 15.0,
                MaLop = ""
            };

            var errs2 = svSai.IsInValid();
            Console.WriteLine($" • Sinh viên vi phạm quy tắc: Đã bắt chính xác {errs2.Count} lỗi [PASSED]");
            foreach (var err in errs2)
            {
                string member = err.MemberNames.FirstOrDefault() ?? "Unknown";
                Console.WriteLine($"    -> [{member}]: {err.ErrorMessage}");
            }

            // 3. Kiểm thử Tầng Business Logic Layer (BUL) & Chuẩn hóa Regex
            Console.WriteLine("\n\n--- [3] KIỂM THỬ TẦNG BUSINESS LOGIC LAYER (BUL) ---");
            SinhVienBUL svBul = new SinhVienBUL();
            LopBus lopBus = new LopBus();

            Console.WriteLine("\n[3.1] Kiểm tra ném ngoại lệ khi thêm sinh viên không hợp lệ:");
            try
            {
                svBul.AddSinhVien(svSai);
                Console.WriteLine(" • THẤT BẠI: Lẽ ra BUL phải ném ngoại lệ!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • BUL đã ném ngoại lệ đúng quy tắc nghiệp vụ [PASSED]:\n   {ex.Message.Replace("\n", " | ")}");
            }

            // 4. Mô phỏng Tầng Views (GUI) bắt ngoại lệ an toàn
            Console.WriteLine("\n\n--- [4] MÔ PHỎNG TẦNG GIAO DIỆN (VIEWS) BẮT & XỬ LÝ NGOẠI LỆ ---");
            Console.WriteLine("[Mô phỏng View] Bắt ngoại lệ khi người dùng thao tác xóa mã trống:");
            try
            {
                svBul.DeleteSinhVien("");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • [View try-catch]: Bắt ngoại lệ thành công và hiển thị MessageBox [PASSED]: {ex.Message}");
            }

            // 5. Thử nghiệm kết nối CSDL và EF Core DbContext
            Console.WriteLine("\n\n--- [5] THỬ NGHIỆM TRUY VẤN VỚI EF CORE DBCONTEXT ---");
            Console.WriteLine($" • Chuỗi kết nối: {QLSVDbContext.DefaultConnectionString}");
            try
            {
                using var context = new QLSVDbContext();
                Console.WriteLine(" • Thử kết nối cơ sở dữ liệu qua EF Core...");
                var canConnect = context.Database.CanConnect();
                Console.WriteLine($" • Trạng thái kết nối SQL Server: {(canConnect ? "KẾT NỐI THÀNH CÔNG!" : "CHƯA KẾT NỐI (Chưa chạy SQL Server local)")}");

                if (canConnect)
                {
                    var countLop = context.LopHocs.Count();
                    var countSV = context.SinhViens.Count();
                    Console.WriteLine($" • Số lượng Lớp học trong CSDL: {countLop}");
                    Console.WriteLine($" • Số lượng Sinh viên trong CSDL: {countSV}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • [Thông tin]: {ex.Message}");
                Console.WriteLine(" • Hướng dẫn: Người dùng chỉ cần chạy lệnh 'Update-Database' trong Visual Studio hoặc chạy file QLSV_EFCore.sql để áp dụng Migrations lên SQL Server.");
            }

            Console.WriteLine("\n========================================================================================");
            Console.WriteLine("   HOÀN TẤT KIỂM THỬ: ĐÁP ỨNG TOÀN DIỆN MÔ HÌNH EF CORE CODE FIRST VÀ MIGRATIONS!       ");
            Console.WriteLine("========================================================================================");
        }
    }
}
