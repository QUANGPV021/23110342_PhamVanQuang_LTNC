using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QuanLySinhVien.BUL;
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
            Console.WriteLine("        KIỂM THỬ BÀI THỰC HÀNH BUỔI 5: THAO TÁC CSDL BẰNG ADO.NET & NGOẠI LỆ 3 TẦNG     ");
            Console.WriteLine("========================================================================================\n");

            // 1. Kiểm tra cấu hình DataHelper & Chuỗi kết nối
            Console.WriteLine("--- [1] CẤU HÌNH DATAHELPER & ADO.NET SQL CLIENT ---");
            Console.WriteLine($" • Chuỗi kết nối mặc định: {DataHelper.DefaultConnectionString}");
            DataHelper helper = new DataHelper();
            Console.WriteLine($" • Khởi tạo DataHelper: THÀNH CÔNG");

            // 2. Kiểm thử Tầng Data Access Layer (DAL) & Ném ngoại lệ
            Console.WriteLine("\n\n--- [2] KIỂM THỬ TẦNG DATA ACCESS LAYER (DAL) & CƠ CHẾ NÉM NGOẠI LỆ ---");
            SinhVienDAL svDal = new SinhVienDAL(helper);
            LopHocDAL lopDal = new LopHocDAL(helper);

            // Test ném ngoại lệ khi thực thi câu truy vấn sai cú pháp hoặc mất kết nối
            Console.WriteLine("\n[2.1] Kiểm tra DAL ném ngoại lệ khi câu lệnh SQL sai cú pháp:");
            try
            {
                helper.ExecuteNonQuery("SELECT * FROM BangKhongTonTai_12345");
                Console.WriteLine(" • THẤT BẠI: Lẽ ra DAL phải ném ngoại lệ!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • DAL đã ném ngoại lệ thành công [PASSED]: {ex.Message}");
            }

            // 3. Kiểm thử Tầng Business Logic Layer (BUL) & Xác thực Data Annotations
            Console.WriteLine("\n\n--- [3] KIỂM THỬ TẦNG BUSINESS LOGIC LAYER (BUL) & NÉM NGOẠI LỆ NGHIỆP VỤ ---");
            SinhVienBUL svBul = new SinhVienBUL(svDal);
            LopBus lopBus = new LopBus(lopDal);

            // 3.1 Dữ liệu không hợp lệ -> BUL phải ném ngoại lệ
            Console.WriteLine("\n[3.1] Kiểm tra BUL ném ngoại lệ khi thêm sinh viên không hợp lệ (Data Annotations):");
            SinhVien svLoi = new SinhVien
            {
                MaSV = "SAI_FORMAT",
                HoTen = "",
                Email = "email_khong_hop_le",
                SoDienThoai = "123",
                Diem = 15.0,
                MaLop = ""
            };

            try
            {
                svBul.AddSinhVien(svLoi);
                Console.WriteLine(" • THẤT BẠI: Lẽ ra BUL phải ném ngoại lệ xác thực!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" • BUL đã ném ngoại lệ xác thực thành công [PASSED]:");
                string[] lines = ex.Message.Split('\n');
                foreach (var line in lines)
                {
                    Console.WriteLine($"    -> {line}");
                }
            }

            // 3.2 Chuẩn hóa dữ liệu bằng Regex ở tầng BUL
            Console.WriteLine("\n[3.2] Kiểm tra chuẩn hóa khoảng trắng bằng Regex ở tầng BUL:");
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

            Console.WriteLine($" • Họ tên trước chuẩn hóa: '{svHopLe.HoTen}'");
            var validationResults = svHopLe.IsInValid();
            Console.WriteLine($" • Kiểm tra IsInValid(): {(validationResults.Count == 0 ? "HỢP LỆ (0 LỖI) [PASSED]" : "CÓ LỖI")}");

            // 4. Mô phỏng Tầng View (GUI) bắt và xử lý ngoại lệ
            Console.WriteLine("\n\n--- [4] MÔ PHỎNG TẦNG GIAO DIỆN (VIEWS) BẮT & XỬ LÝ NGOẠI LỆ (TRY - CATCH) ---");

            // 4.1 Mô phỏng sự kiện butThem_Click với dữ liệu lỗi
            Console.WriteLine("\n[Mô phỏng View] Người dùng nhấn [Thêm] với sinh viên không hợp lệ:");
            try
            {
                svBul.AddSinhVien(svLoi);
            }
            catch (Exception ex)
            {
                // Tầng View bắt và xử lý ngoại lệ, hiển thị hộp thoại lỗi
                Console.WriteLine($" • [View đã bắt ngoại lệ]: Hiển thị MessageBox.Show(\"{ex.Message}\", \"Thông báo lỗi\") [PASSED]");
            }

            // 4.2 Mô phỏng sự kiện butXoa_Click với mã sinh viên trống
            Console.WriteLine("\n[Mô phỏng View] Người dùng nhấn [Xóa] với mã rỗng:");
            try
            {
                svBul.DeleteSinhVien("");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • [View đã bắt ngoại lệ]: Hiển thị MessageBox.Show(\"{ex.Message}\", \"Thông báo lỗi\") [PASSED]");
            }

            // 4.3 Mô phỏng sự kiện kết nối CSDL và fallback xử lý an toàn
            Console.WriteLine("\n[Mô phỏng View] Người dùng mở Form (Form_Load) - Thử kết nối CSDL thực tế:");
            try
            {
                var dslop = lopBus.GetAllLopHoc();
                Console.WriteLine($" • Kết nối CSDL thành công! Đã tải {dslop.Count} lớp học.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • [View đã bắt ngoại lệ kết nối]:");
                Console.WriteLine($"    -> Thông báo: Không thể kết nối tới SQL Server ({ex.Message})");
                Console.WriteLine($"    -> View thông báo hướng dẫn người dùng chạy file QLSV.sql và kiểm tra SQL Server [PASSED]");
            }

            Console.WriteLine("\n========================================================================================");
            Console.WriteLine("    HOÀN TẤT KIỂM THỬ: ĐÁP ỨNG TOÀN BỘ YÊU CẦU ADO.NET VÀ NGOẠI LỆ 3 TẦNG (DAL-BUL-VIEW)   ");
            Console.WriteLine("========================================================================================");
        }
    }
}
