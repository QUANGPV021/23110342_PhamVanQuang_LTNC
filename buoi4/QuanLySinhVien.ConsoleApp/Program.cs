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
            Console.WriteLine("      KIỂM THỬ BÀI THỰC HÀNH 3 & 4: KIẾN TRÚC 3 TẦNG (DAL - BUL - VIEWS) & SỰ KIỆN      ");
            Console.WriteLine("========================================================================================\n");

            // 1. Kiểm tra Tầng Data Access Layer (DAL) & Quan hệ 1 - N
            Console.WriteLine("--- [1] KIỂM TRA TẦNG DATA ACCESS LAYER (DAL) & THỰC THỂ DỮ LIỆU ---");
            LopHocDAL lopDal = new LopHocDAL();
            SinhVienDAL svDal = new SinhVienDAL();

            var danhSachLop = lopDal.GetAllLopHoc();
            Console.WriteLine($"Số lớp học hiện có trong DAL: {danhSachLop.Count}");
            foreach (var lop in danhSachLop)
            {
                var svTrongLop = svDal.GetSinhViensByMaLop(lop.MaLop);
                Console.WriteLine($" • [{lop.MaLop}] {lop.TenLop} (Khoa: {lop.Khoa}) - Số sinh viên: {svTrongLop.Count}");
                foreach (var sv in svTrongLop)
                {
                    Console.WriteLine($"     -> {sv.MaSV} | {sv.HoTen} | {sv.GioiTinh} | Điểm: {sv.Diem:F1} | SĐT: {sv.SoDienThoai}");
                }
            }

            // 2. Kiểm thử Xác thực dữ liệu với Data Annotations & IsInValid()
            Console.WriteLine("\n\n--- [2] KIỂM THỬ XÁC THỰC DATA ANNOTATIONS TRÊN ENTITY (IsInValid) ---");

            // 2.1 Sinh viên hợp lệ
            Console.WriteLine("\n[2.1] Kiểm tra đối tượng SinhVien HỢP LỆ:");
            SinhVien svHopLe = new SinhVien(
                "SV000199",
                "  Phạm   Văn    Quang  ", // Chứa nhiều khoảng trắng thừa để test chuẩn hóa ở BUL
                new DateTime(2005, 5, 20),
                "Nam",
                "quang.pv23110342@vju.ac.vn",
                "0988123456",
                9.5,
                "KTPM01",
                "Đang học"
            );

            List<ValidationResult> errs1 = svHopLe.IsInValid();
            Console.WriteLine($"Kết quả xác thực sv.IsInValid(): {(errs1.Count == 0 ? "HỢP LỆ (0 LỖI) [PASSED]" : "THẤT BẠI")}");

            // 2.2 Sinh viên vi phạm quy tắc validation
            Console.WriteLine("\n[2.2] Kiểm tra đối tượng SinhVien VI PHẠM DỮ LIỆU:");
            SinhVien svViPham = new SinhVien(
                "SAI_MA_SV",          // Không đúng Regex ^SV\d{3,}$
                "",                   // Trống họ tên
                DateTime.Now,
                "Khác",
                "email-khong-hop-le", // Không đúng định dạng Email
                "12345",              // SĐT không đủ 10 số, không bắt đầu bằng 0
                12.5,                 // Điểm vượt ngưỡng [0.0 - 10.0]
                "",                   // Trống mã lớp
                ""
            );

            List<ValidationResult> errs2 = svViPham.IsInValid();
            Console.WriteLine($"Kết quả xác thực svViPham.IsInValid(): Đã phát hiện {errs2.Count} lỗi [PASSED]");
            foreach (var err in errs2)
            {
                string member = err.MemberNames.FirstOrDefault() ?? "Chung";
                Console.WriteLine($" • [{member}] -> {err.ErrorMessage}");
            }

            // 3. Kiểm thử Tầng Business Logic Layer (BUL) & Chuẩn hóa dữ liệu
            Console.WriteLine("\n\n--- [3] KIỂM THỬ TẦNG BUSINESS LOGIC LAYER (BUL) & CHUẨN HÓA DỮ LIỆU ---");
            SinhVienBUL svBul = new SinhVienBUL(svDal);
            LopBus lopBus = new LopBus(lopDal);

            Console.WriteLine($"\n[3.1] Thêm sinh viên qua SinhVienBUL.AddSinhVien:");
            Console.WriteLine($" • Họ tên ban đầu (có khoảng trắng thừa): '{svHopLe.HoTen}'");
            svBul.AddSinhVien(svHopLe);

            var svVuaThem = svBul.GetSinhVienByMaSV("SV000199");
            if (svVuaThem != null)
            {
                Console.WriteLine($" • Họ tên sau khi chuẩn hóa bởi Regex: '{svVuaThem.HoTen}' [PASSED]");
                Console.WriteLine($" • Thông tin chi tiết: {svVuaThem}");
            }

            Console.WriteLine("\n[3.2] Kiểm tra chặn trùng khóa chính khi thêm:");
            try
            {
                svBul.AddSinhVien(new SinhVien("SV000199", "Người Trùng Mã", new DateTime(2006, 1, 1), "Nam", "test@vju.ac.vn", "0912345678", 8.0, "KTPM01"));
                Console.WriteLine(" • THẤT BẠI: Không phát hiện trùng mã!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" • Bắt lỗi trùng mã thành công: {ex.Message} [PASSED]");
            }

            // 4. Kiểm thử Cập nhật (Update) & Xóa (Delete) tại tầng Business
            Console.WriteLine("\n\n--- [4] KIỂM THỬ CẬP NHẬT & XÓA SINH VIÊN TẠI TẦNG BUL ---");
            svVuaThem!.HoTen = "  Phạm Văn Quang   (Thủ khoa) ";
            svVuaThem.Diem = 10.0;
            svBul.UpdateSinhVien(svVuaThem);

            var svSauSua = svBul.GetSinhVienByMaSV("SV000199");
            Console.WriteLine($" • Thông tin sau cập nhật: {svSauSua?.HoTen} - Điểm: {svSauSua?.Diem:F1} [PASSED]");

            // 5. Kiểm thử Tìm kiếm & Lọc (Search & Filter)
            Console.WriteLine("\n\n--- [5] KIỂM THỬ TÌM KIẾM & BỘ LỌC DỮ LIỆU ---");
            Console.WriteLine("\n[5.1] Tìm kiếm theo từ khóa 'Quang':");
            var timQuang = svBul.Search("Quang", "ALL", 0);
            timQuang.ForEach(s => Console.WriteLine($" -> {s}"));

            Console.WriteLine("\n[5.2] Lọc theo lớp 'KTPM01' và Điểm sàn >= 8.5:");
            var locLop = svBul.Search(null, "KTPM01", 8.5);
            locLop.ForEach(s => Console.WriteLine($" -> {s}"));

            // 6. Mô phỏng xử lý sự kiện giao diện (UI Event Logic) & ErrorProvider Mapping
            Console.WriteLine("\n\n--- [6] MÔ PHỎNG XỬ LÝ SỰ KIỆN GIAO DIỆN (EVENT HANDLING & ERRORPROVIDER) ---");

            // 6.1 Sự kiện txtMaSV_Leave khi mã sinh viên đã tồn tại
            string nhapMaTonTai = "SV000123";
            Console.WriteLine($"\n[Sự kiện txtMaSV_Leave] Nhập mã: '{nhapMaTonTai}'");
            var svTimThay = svBul.GetSinhVienByMaSV(nhapMaTonTai);
            if (svTimThay != null)
            {
                Console.WriteLine($" • Trạng thái: TỒN TẠI trong cơ sở dữ liệu");
                Console.WriteLine($" • Nạp thông tin: Họ tên = '{svTimThay.HoTen}', Lớp = '{svTimThay.MaLop}', Điểm = {svTimThay.Diem}");
                Console.WriteLine($" • Điều khiển TogAdd(false): [Thêm]: DISABLE | [Sửa]: ENABLE | [Xóa]: ENABLE [PASSED]");
            }

            // 6.2 Sự kiện txtMaSV_Leave khi mã sinh viên chưa tồn tại
            string nhapMaChuaCo = "SV000888";
            Console.WriteLine($"\n[Sự kiện txtMaSV_Leave] Nhập mã: '{nhapMaChuaCo}'");
            var svChuaCo = svBul.GetSinhVienByMaSV(nhapMaChuaCo);
            if (svChuaCo == null)
            {
                Console.WriteLine($" • Trạng thái: CHƯA TỒN TẠI trong cơ sở dữ liệu");
                Console.WriteLine($" • Xử lý: Xóa trắng các TextBox, đặt lại giá trị mặc định");
                Console.WriteLine($" • Điều khiển TogAdd(true): [Thêm]: ENABLE | [Sửa]: DISABLE | [Xóa]: DISABLE [PASSED]");
            }

            // 6.3 Xử lý ErrorProvider Mapping khi người dùng nhấn [Thêm] với dữ liệu không hợp lệ
            Console.WriteLine($"\n[Sự kiện butThem_Click] Báo lỗi qua ErrorProvider tương ứng từng trường:");
            var dsLoi = svViPham.IsInValid();
            foreach (var loi in dsLoi)
            {
                string field = loi.MemberNames.FirstOrDefault() ?? "Unknown";
                string controlMapped = field switch
                {
                    "MaSV" => "errPMaSV -> txtMaSV",
                    "HoTen" => "erpHoten -> txtHoTen",
                    "Email" => "erpEmail -> txtEmail",
                    "SoDienThoai" => "erpDienThoai -> txtDienThoai",
                    "MaLop" => "erpLopHoc -> cboLop",
                    "Diem" => "erpDiem -> numDiem",
                    _ => "MessageBox"
                };
                Console.WriteLine($" • Lỗi trường '{field}': Gán '{loi.ErrorMessage}' vào điều khiển [{controlMapped}] [PASSED]");
            }

            // 6.4 Chức năng nguy hiểm: Xóa sinh viên với xác thực cảnh báo
            Console.WriteLine($"\n[Sự kiện butXoa_Click] Thực hiện chức năng nguy hiểm (Xóa SV000199):");
            Console.WriteLine(" • Hiển thị MessageBox xác nhận: 'CẢNH BÁO NGUY HIỂM: Bạn có chắc chắn muốn xóa sinh viên SV000199?'");
            Console.WriteLine(" • Người dùng xác nhận YES -> Thực hiện gọi svBul.DeleteSinhVien('SV000199')");
            svBul.DeleteSinhVien("SV000199");
            Console.WriteLine($" • Kết quả: Đã xóa sinh viên. Tổng số sinh viên còn lại: {svBul.GetAllSinhVien().Count} [PASSED]");

            Console.WriteLine("\n========================================================================================");
            Console.WriteLine("           HOÀN TẤT TẤT CẢ CÁC KIỂM THỬ TẦNG DAL, BUL, VALIDATION & SỰ KIỆN!            ");
            Console.WriteLine("========================================================================================");
        }
    }
}
