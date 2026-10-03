# BÀI THỰC HÀNH 3: LẬP TRÌNH SỰ KIỆN & DATA ANNOTATIONS

**Môn học:** Lập trình nâng cao (LTNC)  
**Sinh viên thực hiện:** Phạm Văn Quang  
**Mã số sinh viên (MSSV):** 23110342  
**Đơn vị:** Đại học Quốc gia Hà Nội - Trường Đại học Việt Nhật (VJU)

---

## 1. Yêu cầu đề bài

1. **Thiết kế giao diện quản lý sinh viên** chuẩn theo mẫu thiết kế (`image1.png` trong tài liệu `Bài thực hành 3 Lập trình sự kiện.docx`):
   - Tiêu đề ứng dụng: *Ứng dụng quản lý sinh viên - QUẢN LÝ SINH VIÊN*
   - Card nhập liệu *Thông tin sinh viên*: Mã sinh viên, Họ và tên, Lớp học, Ngày sinh, Giới tính, Điểm, Email, Điện thoại, Trạng thái.
   - 4 nút chức năng: **Thêm (Nhập)**, **Sửa**, **Xóa**, **Làm mới**.
   - Thanh tìm kiếm và bộ lọc: Tìm kiếm theo từ khóa (Mã SV, họ tên, email, SĐT), lọc theo lớp học, lọc theo điểm sàn.
   - Bảng hiển thị danh sách sinh viên (*DataGridView*) hiển thị đầy đủ các cột thông tin, số lượng sinh viên hiện có.
2. **Xây dựng lớp `LopHoc`, `SinhVien` có quan hệ 1 - N**:
   - Một lớp học (`LopHoc`) quản lý một danh sách nhiều sinh viên (`List<SinhVien>`).
   - Mỗi sinh viên (`SinhVien`) có tham chiếu đến lớp học thuộc về (`LopHoc?`).
3. **Data Annotations & Phương thức kiểm tra hợp lệ**:
   - Sử dụng các Data Annotations: `[Required]`, `[StringLength]`, `[RegularExpression]`, `[EmailAddress]`, `[Range]`,...
   - Xây dựng phương thức `Validate()` / `IsValid(out List<string> errors)` kiểm tra xem các thuộc tính của đối tượng có hợp lệ hay không.
4. **Xử lý sự kiện khi FormLoad**:
   - Thứ tự chuyển tiêu điểm (Tab index) từ trên xuống dưới, từ trái qua phải.
   - Con trỏ thiết lập mặc định ở `txtMaSV`.
   - Thiết lập giá trị Enable phù hợp cho các nút: **Thêm: Enable**, **Sửa: Disable**, **Xóa: Disable**, **Làm mới: Enable**.
   - Lấy về danh sách lớp học hiển thị lên ComboBox.
   - Lấy về danh sách sinh viên ban đầu hiển thị lên DataGridView.
5. **Xử lý sự kiện khi người dùng nhập mã sinh viên (`txtMaSV_TextChanged`)**:
   - Nếu mã sinh viên tồn tại: lấy thông tin của sinh viên hiển thị tương ứng lên các điều khiển còn lại; disable chức năng Nhập (Thêm); enable chức năng Sửa, Xóa.
   - Nếu chưa tồn tại: xóa giá trị các điều khiển textbox; enable chức năng Nhập (Thêm); disable chức năng Sửa, Xóa.
6. **Xử lý sự kiện khi nhấn nút Làm mới (`btnLamMoi_Click`)**:
   - Xóa trống các thuộc tính trên form.
   - Enable button Nhập (Thêm), Disable button Sửa, Xóa.
   - Chuyển tiêu điểm về điều khiển `txtMaSV`.
7. **Xác thực trước khi thực hiện chức năng nguy hiểm**:
   - Khi xóa sinh viên: hiển thị hộp thoại cảnh báo nguy hiểm (`MessageBox`) yêu cầu người dùng xác nhận (`Yes/No`) trước khi xóa vĩnh viễn dữ liệu.

---

## 2. Cấu trúc thư mục `buoi3`

```text
buoi3/
├── Bài thực hành 3 Lập trình sự kiện.docx   # File đề bài gốc
├── .gitignore                               # File loại trừ bin/, obj/, .vs/
├── code.slnx                                # File Solution định dạng mới (Visual Studio 2022 v17.10+)
├── QuanLySinhVien.sln                       # File Solution chuẩn của Visual Studio
├── README.md                                # Tài liệu hướng dẫn và báo cáo bài tập
│
├── QuanLySinhVien/                          # Dự án Windows Forms chính
│   ├── QuanLySinhVien.csproj
│   ├── Program.cs                           # Entry point của ứng dụng WinForms
│   ├── Models/
│   │   ├── LopHoc.cs                        # Lớp LopHoc (Quan hệ 1 - N, Data Annotations, Validate())
│   │   └── SinhVien.cs                      # Lớp SinhVien (Quan hệ N - 1, Data Annotations, Validate())
│   ├── DAO/
│   │   ├── LopHocDAO.cs                     # Xử lý dữ liệu danh sách Lớp học
│   │   └── SinhVienDAO.cs                   # Xử lý CRUD & Tìm kiếm Sinh viên
│   └── Forms/
│       ├── FormQuanLySinhVien.cs            # Code xử lý logic và toàn bộ sự kiện của Form
│       └── FormQuanLySinhVien.Designer.cs   # Thiết kế giao diện WinForms chuẩn theo ảnh mẫu
│
└── QuanLySinhVien.ConsoleApp/               # Dự án Console kiểm thử logic & validation
    ├── QuanLySinhVien.ConsoleApp.csproj
    └── Program.cs                           # Thực hiện kiểm thử tự động Data Annotations & Sự kiện
```

---

## 3. Chi tiết kỹ thuật

### 3.1. Các luật kiểm tra tính hợp lệ (Data Annotations)
- **Mã sinh viên (`MaSV`)**:
  - `[Required(ErrorMessage = "Mã sinh viên không được để trống.")]`
  - `[RegularExpression(@"^SV\d{3,}$", ErrorMessage = "Mã sinh viên phải có định dạng bắt đầu bằng 'SV' và theo sau bởi ít nhất 3 chữ số (ví dụ: SV000123).")]`
- **Họ và tên (`HoTen`)**:
  - `[Required(ErrorMessage = "Họ và tên không được để trống.")]`
  - `[StringLength(50, MinimumLength = 2, ErrorMessage = "Họ và tên phải có từ 2 đến 50 ký tự.")]`
- **Email (`Email`)**:
  - `[Required(ErrorMessage = "Email không được để trống.")]`
  - `[EmailAddress(ErrorMessage = "Email không đúng định dạng hợp lệ (ví dụ: an.nv@vju.ac.vn).")]`
- **Số điện thoại (`DienThoai`)**:
  - `[Required(ErrorMessage = "Số điện thoại không được để trống.")]`
  - `[RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải gồm đúng 10 chữ số và bắt đầu bằng số 0.")]`
- **Điểm (`Diem`)**:
  - `[Range(0.0, 10.0, ErrorMessage = "Điểm phải nằm trong khoảng từ 0.0 đến 10.0.")]`
- **Lớp học (`MaLop`)**:
  - `[Required(ErrorMessage = "Vui lòng chọn lớp học.")]`
- **Trạng thái (`TrangThai`)**:
  - `[Required(ErrorMessage = "Vui lòng chọn trạng thái.")]`

### 3.2. Phương thức kiểm tra hợp lệ
```csharp
public bool IsValid(out List<string> errors)
{
    errors = new List<string>();
    var context = new ValidationContext(this, null, null);
    var results = new List<ValidationResult>();

    bool isValid = Validator.TryValidateObject(this, context, results, validateAllProperties: true);

    if (!isValid)
    {
        foreach (var r in results)
        {
            if (!string.IsNullOrWhiteSpace(r.ErrorMessage))
                errors.Add(r.ErrorMessage);
        }
    }

    return isValid;
}
```

### 3.3. Dữ liệu mẫu (Khởi tạo sẵn như trong ảnh đề bài)
1. **SV000123** | Nguyễn Văn An | 15/08/2006 | Nam | an.nv@vju.ac.vn | 0912345678 | 8.5 | Kỹ thuật phần mềm 01 | Đang học
2. **SV000124** | Trần Minh Anh | 22/01/2006 | Nữ | anh.tm@vju.ac.vn | 0987654321 | 9.0 | Trí tuệ nhân tạo 01 | Đang học
3. **SV000125** | Lê Hoàng Bình | 09/05/2006 | Nam | binh.lh@vju.ac.vn | 0355556677 | 7.4 | Kỹ thuật phần mềm 01 | Đang học
4. **SV000126** | Đỗ Thị Hồng | 30/11/2006 | Nữ | hong.dt@vju.ac.vn | 0777888999 | 8.1 | Khoa học dữ liệu 01 | Đang học

---

## 4. Hướng dẫn chạy chương trình

### Cách 1: Chạy bằng Visual Studio trên Windows (Khuyên dùng)
1. Mở file `QuanLySinhVien.sln` hoặc `code.slnx` bằng **Visual Studio 2022** (hoặc mới hơn).
2. Đặt `QuanLySinhVien` làm **Startup Project**.
3. Nhấn **F5** hoặc nút **Start** để biên dịch và trải nghiệm giao diện người dùng Windows Forms.

### Cách 2: Chạy kiểm thử Console (Hoạt động trên mọi hệ điều hành: Windows / macOS / Linux)
```bash
cd buoi3/QuanLySinhVien.ConsoleApp
dotnet run
```
Chương trình sẽ tự động thực hiện các ca kiểm thử:
- Khởi tạo danh sách lớp và sinh viên theo quan hệ 1-N.
- Xác thực dữ liệu hợp lệ và không hợp lệ qua Data Annotations.
- Kiểm thử các thao tác CRUD (Thêm, Sửa, Xóa).
- Kiểm thử mô phỏng toàn bộ logic sự kiện trên giao diện người dùng.
