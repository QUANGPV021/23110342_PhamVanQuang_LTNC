using System;
using System.Collections.Generic;
using QuanLySinhVien.DAO;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("========================================================================================");
            Console.WriteLine("                KIỂM THỬ BÀI THỰC HÀNH 3: LẬP TRÌNH SỰ KIỆN & VALIDATION                ");
            Console.WriteLine("========================================================================================\n");

            // 1. Khởi tạo DAO & kiểm tra quan hệ 1 - N
            LopHocDAO lopHocDao = new LopHocDAO();
            SinhVienDAO sinhVienDao = new SinhVienDAO(lopHocDao);

            Console.WriteLine("--- [1] DANH SÁCH LỚP HỌC (LopHoc) & SINH VIÊN (Quan hệ 1 - N) ---");
            foreach (var lop in lopHocDao.GetAll())
            {
                Console.WriteLine($"\n[LỚP]: {lop.TenLop} (Mã: {lop.MaLop}) - Số SV: {lop.DanhSachSinhVien.Count}");
                foreach (var sv in lop.DanhSachSinhVien)
                {
                    Console.WriteLine($"   -> {sv}");
                }
            }

            // 2. Kiểm thử phương thức kiểm tra hợp lệ bằng Data Annotations
            Console.WriteLine("\n\n--- [2] KIỂM THỬ XÁC THỰC DATA ANNOTATIONS TRÊN MODEL ---");
            
            // Trường hợp 2.1: Đối tượng hợp lệ
            Console.WriteLine("\n[2.1] Kiểm tra đối tượng SinhVien HỢP LỆ:");
            SinhVien svHopLe = new SinhVien(
                "SV000199",
                "Phạm Văn Quang",
                new DateTime(2005, 5, 20),
                "Nam",
                "phamvanquang@vju.ac.vn",
                "0988123456",
                9.2,
                "KTPM01",
                "Đang học"
            );
            var (valid1, errors1) = svHopLe.Validate();
            Console.WriteLine($"Kết quả kiểm tra: {(valid1 ? "HỢP LỆ (PASSED)" : "KHÔNG HỢP LỆ (FAILED)")}");
            if (!valid1)
            {
                errors1.ForEach(e => Console.WriteLine($" - Lỗi: {e}"));
            }

            // Trường hợp 2.2: Đối tượng không hợp lệ (sai định dạng MaSV, Email, Phone, Điểm quá 10)
            Console.WriteLine("\n[2.2] Kiểm tra đối tượng SinhVien KHÔNG HỢP LỆ (cố tình sai nhiều trường):");
            SinhVien svSai = new SinhVien(
                "ABC12",               // Sai format SVxxxxxx
                "",                    // Trống họ tên
                DateTime.Now,
                "Khác",
                "email-khong-dung",    // Sai email
                "12345",               // Sai sdt (không đủ 10 số, không bắt đầu bằng 0)
                15.5,                  // Điểm vượt quá 10
                "",                    // Chưa chọn lớp
                ""
            );
            var (valid2, errors2) = svSai.Validate();
            Console.WriteLine($"Kết quả kiểm tra: {(valid2 ? "HỢP LỆ" : "ĐÃ PHÁT HIỆN LỖI CHÍNH XÁC (PASSED)")}");
            Console.WriteLine($"Số lỗi phát hiện: {errors2.Count}");
            foreach (var err in errors2)
            {
                Console.WriteLine($" • {err}");
            }

            // 3. Kiểm thử thêm mới sinh viên (Add)
            Console.WriteLine("\n\n--- [3] KIỂM THỬ THÊM MỚI SINH VIÊN (Add) ---");
            bool themThanhCong = sinhVienDao.Add(svHopLe);
            Console.WriteLine($"Thêm sinh viên '{svHopLe.HoTen}': {(themThanhCong ? "THÀNH CÔNG" : "THẤT BẠI")}");
            Console.WriteLine($"Tổng số sinh viên sau khi thêm: {sinhVienDao.GetAll().Count}");

            // 4. Kiểm thử cập nhật sinh viên (Edit)
            Console.WriteLine("\n--- [4] KIỂM THỬ CẬP NHẬT THÔNG TIN (Edit) ---");
            svHopLe.HoTen = "Phạm Văn Quang (Đã cập nhật)";
            svHopLe.Diem = 9.8;
            bool suaThanhCong = sinhVienDao.Edit(svHopLe);
            Console.WriteLine($"Sửa sinh viên '{svHopLe.MaSV}': {(suaThanhCong ? "THÀNH CÔNG" : "THẤT BẠI")}");
            var svSauKhiSua = sinhVienDao.GetById("SV000199");
            Console.WriteLine($"Thông tin mới: {svSauKhiSua}");

            // 5. Kiểm thử tìm kiếm và lọc
            Console.WriteLine("\n--- [5] KIỂM THỬ TÌM KIẾM & LỌC DỮ LIỆU ---");
            Console.WriteLine("Tìm sinh viên có từ khóa 'Anh':");
            var timKiemAnh = sinhVienDao.Search("Anh", "ALL", 0);
            timKiemAnh.ForEach(s => Console.WriteLine($" -> {s}"));

            Console.WriteLine("\nTìm sinh viên thuộc lớp 'KTPM01' và điểm >= 8.0:");
            var timTheoLop = sinhVienDao.Search(null, "KTPM01", 8.0);
            timTheoLop.ForEach(s => Console.WriteLine($" -> {s}"));

            // 6. Mô phỏng xử lý sự kiện giao diện (Event Simulation)
            Console.WriteLine("\n--- [6] MÔ PHỎNG XỬ LÝ SỰ KIỆN GIAO DIỆN (UI EVENT LOGIC) ---");
            
            // 6.1 Sự kiện nhập mã sinh viên đã tồn tại
            string nhapMa1 = "SV000123";
            Console.WriteLine($"\n[Sự kiện txtMaSV_TextChanged] Người dùng nhập: '{nhapMa1}'");
            var tim1 = sinhVienDao.GetById(nhapMa1);
            if (tim1 != null)
            {
                Console.WriteLine($" -> Mã TỒN TẠI: Hiển thị Họ tên = '{tim1.HoTen}', Lớp = '{tim1.TenLop}'");
                Console.WriteLine($" -> Trạng thái Button: Enable [Nhập]: FALSE | Enable [Sửa]: TRUE | Enable [Xóa]: TRUE");
            }

            // 6.2 Sự kiện nhập mã sinh viên chưa tồn tại
            string nhapMa2 = "SV000999";
            Console.WriteLine($"\n[Sự kiện txtMaSV_TextChanged] Người dùng nhập: '{nhapMa2}'");
            var tim2 = sinhVienDao.GetById(nhapMa2);
            if (tim2 == null)
            {
                Console.WriteLine($" -> Mã CHƯA TỒN TẠI: Xóa trắng các TextBox khác");
                Console.WriteLine($" -> Trạng thái Button: Enable [Nhập]: TRUE | Enable [Sửa]: FALSE | Enable [Xóa]: FALSE");
            }

            // 6.3 Sự kiện Button Làm mới
            Console.WriteLine("\n[Sự kiện btnLamMoi_Click] Người dùng nhấn Làm mới:");
            Console.WriteLine(" -> Xóa trống toàn bộ form, đưa con trỏ về txtMaSV");
            Console.WriteLine(" -> Trạng thái Button: Enable [Nhập]: TRUE | Enable [Sửa]: FALSE | Enable [Xóa]: FALSE");

            // 6.4 Chức năng nguy hiểm: Xóa sinh viên
            Console.WriteLine($"\n[Sự kiện btnXoa_Click] Thực hiện chức năng nguy hiểm (Xóa SV000199):");
            Console.WriteLine(" -> Hiển thị hộp thoại xác thực: 'Bạn có chắc chắn muốn xóa không?'");
            Console.WriteLine(" -> Người dùng chọn YES: Thực hiện xóa");
            bool xoaThanhCong = sinhVienDao.Delete("SV000199");
            Console.WriteLine($" -> Kết quả xóa: {(xoaThanhCong ? "ĐÃ XÓA THÀNH CÔNG" : "THẤT BẠI")}");
            Console.WriteLine($" -> Tổng số sinh viên hiện tại: {sinhVienDao.GetAll().Count}");

            Console.WriteLine("\n========================================================================================");
            Console.WriteLine("                            HOÀN TẤT TẤT CẢ CÁC KIỂM THỬ!                               ");
            Console.WriteLine("========================================================================================");
        }
    }
}
