# BÀI THỰC HÀNH BUỔI 5: THAO TÁC CƠ SỞ DỮ LIỆU BẰNG ADO.NET & XỬ LÝ NGOẠI LỆ 3 TẦNG

**Môn học:** Lập trình nâng cao (LTNC)  
**Sinh viên thực hiện:** Phạm Văn Quang  
**Mã số sinh viên (MSSV):** 23110342  
**Đơn vị:** Đại học Quốc gia Hà Nội - Trường Đại học Việt Nhật (VJU)  
**Kho lưu trữ (GitHub):** [https://github.com/QUANGPV021/23110342_PhamVanQuang_LTNC/tree/main/buoi5](https://github.com/QUANGPV021/23110342_PhamVanQuang_LTNC/tree/main/buoi5)

---

## 1. Yêu cầu đề bài

1. **Thay đổi hai lớp `LopHocDAO` và `SinhVienDAO` (hoặc `LopHocDAL` và `SinhVienDAL`):**
   - Chuyển đổi toàn bộ thao tác từ danh sách đối tượng trong bộ nhớ sang thao tác với cơ sở dữ liệu quan hệ **SQL Server** thông qua thư viện **ADO.Net** (`Microsoft.Data.SqlClient`).
   - Xây dựng lớp dùng chung `DataHelper` để quản lý chuỗi kết nối (`ConnectionString`), đối tượng kết nối `SqlConnection`, thực thi câu lệnh SQL qua `SqlCommand`, `SqlDataAdapter`, `SqlDataReader`, `ExecuteNonQuery`, `ExcuteReader`, và `GetDataTable`.
   - Cung cấp đầy đủ các thao tác CRUD (Thêm, Sửa, Xóa, Lấy danh sách, Lấy theo ID, Lọc theo lớp, Tìm kiếm).
2. **Cơ chế ném và xử lý ngoại lệ theo mô hình 3 tầng (3-Layer Exception Handling):**
   - **Tầng DataAccess Layer (DAL):** Khi thực thi câu truy vấn hoặc kết nối CSDL xảy ra sự cố, phải bắt lỗi và ném tiếp về tầng trên: `throw new Exception("Lỗi DataAccess: " + ex.Message, ex);`.
   - **Tầng Business Layer (BUL):** Kiểm tra tính hợp lệ dữ liệu bằng Data Annotations (`sv.IsInValid()`). Nếu có lỗi dữ liệu hoặc khi gọi tầng DAL bị lỗi, phải ném ngoại lệ tường minh: `throw new Exception(errorMessage);`.
   - **Tầng View (GUI - `frmQuanLySV`):** Tuyệt đối không để ứng dụng bị crash. Mọi sự kiện của người dùng (`Form_Load`, `txtMaSV_Leave`, `butThem_Click`, `butSua_Click`, `butXoa_Click`, `cboLop_SelectedIndexChanged`, `butTimKiem_Click`) đều được bao bọc trong khối `try ... catch (Exception ex)` để xử lý ngoại lệ và hiển thị thông báo lỗi thân thiện qua `MessageBox` và `ErrorProvider`.
3. **Commit lên Git với comment:**
   - `Bài tập về ADO.Net ngày 17/10/2026`

---

## 2. Cấu trúc thư mục `buoi5`

```text
buoi5/
├── .gitignore                               # Bỏ qua bin/, obj/, .vs/
├── code.slnx                                # File Solution XML thế hệ mới
├── QuanLySinhVien.sln                       # Solution tiêu chuẩn Visual Studio 2022
├── QLSV.sql                                 # Script tạo Database QLSV, bảng LopHoc, SinhVien và dữ liệu mẫu
├── README.md                                # Báo cáo chi tiết và tài liệu hướng dẫn
│
├── QuanLySinhVien/                          # Dự án Windows Forms chính (ADO.Net & 3-Layer)
│   ├── QuanLySinhVien.csproj                # Đã thêm PackageReference Microsoft.Data.SqlClient
│   ├── Program.cs                           # Entry point của ứng dụng WinForms
│   ├── Data/
│   │   ├── Entity/
│   │   │   ├── SinhVien.cs                  # Thực thể SinhVien (Data Annotations, IsInValid())
│   │   │   └── LopHoc.cs                    # Thực thể LopHoc (Quan hệ 1 - N)
│   │   └── DAL/
│   │       ├── DataHelper.cs                # Lớp kết nối ADO.Net (SqlConnection, SqlCommand, Adapter)
│   │       ├── SinhVienDAL.cs               # Data Access: Thao tác bảng SinhVien bằng ADO.Net (alias SinhVienDAO)
│   │       └── LopHocDAL.cs                 # Data Access: Thao tác bảng LopHoc bằng ADO.Net (alias LopHocDAO)
│   ├── BUL/
│   │   ├── SinhVienBUL.cs                   # Business: Kiểm tra hợp lệ, chuẩn hóa Regex, ném ngoại lệ
│   │   └── LopBus.cs                        # Business: Quản lý nghiệp vụ lớp học, ném ngoại lệ
│   └── Views/
│       ├── frmQuanLySV.cs                   # Tầng View: Xử lý ngoại lệ try-catch toàn diện, ErrorProvider
│       └── frmQuanLySV.Designer.cs          # Thiết kế giao diện WinForms hiện đại, chuẩn ảnh mẫu
│
└── QuanLySinhVien.ConsoleApp/               # Dự án Console kiểm thử tự động
    ├── QuanLySinhVien.ConsoleApp.csproj     # Đã thêm PackageReference Microsoft.Data.SqlClient
    └── Program.cs                           # Kịch bản kiểm thử tự động DAL, BUL, ném & bắt ngoại lệ
```

---

## 3. Chi tiết triển khai ADO.Net & Xử lý ngoại lệ

```mermaid
graph TD
    subgraph "Presentation Layer (QuanLySinhVien.Views - frmQuanLySV)"
        UI[Sự kiện giao diện: butThem, butSua, butXoa, txtMaSV_Leave...]
        Catch[try ... catch (Exception ex) -> Hiển thị MessageBox / ErrorProvider]
        UI --> Catch
    end

    subgraph "Business Layer (QuanLySinhVien.BUL - SinhVienBUL, LopBus)"
        Validate[Kiểm tra sv.IsInValid -> throw new Exception]
        RegexNorm[Chuẩn hóa Regex khoảng trắng thừa]
        RethrowBUL[catch (Exception ex) -> throw new Exception('Lỗi nghiệp vụ...')]
        Catch -->|Gọi nghiệp vụ| Validate
        Validate --> RegexNorm
        RegexNorm --> RethrowBUL
    end

    subgraph "Data Access Layer (QuanLySinhVien.Data.DAL - SinhVienDAL, LopHocDAL)"
        Helper[DataHelper: OpenConnection, ExecuteNonQuery, ExcuteReader, GetDataTable]
        RethrowDAL[catch (Exception ex) -> throw new Exception('Lỗi DataAccess...')]
        RethrowBUL -->|Gọi DAL| Helper
        Helper --> RethrowDAL
    end

    subgraph "Database (SQL Server)"
        DB[(QLSV: LopHoc, SinhVien)]
        Helper <-->|ADO.Net: SqlConnection, SqlCommand| DB
    end
```

### 3.1. Lớp tiện ích `DataHelper`
Quản lý chuỗi kết nối và các phương thức thực thi ADO.Net:
- `OpenConnection()`: Mở kết nối nếu đang đóng.
- `CloseConnection()`: Đóng kết nối an toàn.
- `ExecuteNonQuery(sql)`: Thực thi câu lệnh `INSERT`, `UPDATE`, `DELETE`.
- `ExcuteReader(sql)`: Thực thi câu lệnh `SELECT` và trả về `SqlDataReader`.
- `GetDataTable(sql)`: Điền dữ liệu `SELECT` vào `DataTable` thông qua `SqlDataAdapter`.

Chuỗi kết nối mặc định:
```csharp
Server=LAPTOP-6SN65NCS\HUECTMDB; Database=QLSV; Integrated Security=True; TrustServerCertificate=true
```

### 3.2. Tầng DataAccess: `SinhVienDAL` và `LopHocDAL`
- `SinhVienDAL` thực hiện các truy vấn bằng ADO.Net:
  - `getAllSinhVien1()` / `GetAllSinhVien1()`: Sử dụng `SqlDataReader` đọc từng dòng dữ liệu và nạp vào danh sách.
  - `GetAllSinhVien()`: Sử dụng `SqlDataAdapter` lấy `DataTable` và ánh xạ thành danh sách `SinhVien`.
  - `GetSinhVienByMaSV(maSV)`: Truy vấn có tham số `SELECT * FROM SinhVien WHERE MaSV = @MaSV`.
  - `GetSinhViensByMaLop(maLop)`: Lọc theo mã lớp `SELECT * FROM SinhVien WHERE MaLop = @MaLop`.
  - `AddSinhVien(sv)`: Thực thi câu lệnh `INSERT INTO SinhVien (...) VALUES (...)`.
  - `UpdateSinhVien(sv)`: Thực thi câu lệnh `UPDATE SinhVien SET ... WHERE MaSV = @MaSV`.
  - `DeleteSinhVien(maSV)`: Thực thi câu lệnh `DELETE FROM SinhVien WHERE MaSV = @MaSV`.
  - `Search(keyword, maLop, diemTu)`: Truy vấn động kết hợp mệnh đề `WHERE` và tham số `SqlParameter`.
- **Ném ngoại lệ:** Khi xảy ra `SqlException` hoặc lỗi kết nối, tất cả các phương thức đều ném tiếp về tầng trên:
  ```csharp
  catch (Exception ex)
  {
      throw new Exception("Lỗi khi thêm sinh viên vào CSDL: " + ex.Message, ex);
  }
  ```

### 3.3. Tầng Business: `SinhVienBUL` và `LopBus`
- **Xác thực dữ liệu:** Gọi `sv.IsInValid()`. Nếu danh sách lỗi > 0, ném ngay ngoại lệ với toàn bộ thông báo lỗi:
  ```csharp
  List<ValidationResult> vdr = sv.IsInValid();
  if (vdr.Count > 0)
  {
      throw new Exception(string.Join("\n", vdr.Select(vr => vr.ErrorMessage)));
  }
  ```
- **Chuẩn hóa dữ liệu:**
  ```csharp
  sv.HoTen = Regex.Replace(sv.HoTen.Trim(), @"\s+", " ");
  ```
- **Ném ngoại lệ:** Bắt lỗi từ tầng DAL và ném ngoại lệ nghiệp vụ lên tầng View:
  ```csharp
  catch (Exception ex)
  {
      throw new Exception("Lỗi nghiệp vụ khi thêm sinh viên: " + ex.Message, ex);
  }
  ```

### 3.4. Tầng View: Xử lý ngoại lệ tại `frmQuanLySV`
Tất cả các hàm xử lý sự kiện giao diện đều sử dụng cấu trúc `try - catch`:
```csharp
private void butThem_Click(object sender, EventArgs e)
{
    ClearErrorProviders();
    SinhVien sv = new SinhVien { ... };

    List<ValidationResult> errors = sv.IsInValid();
    if (errors.Count == 0)
    {
        try
        {
            svb.AddSinhVien(sv);
            MessageBox.Show("Thêm thành công sinh viên vào CSDL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshDataGridView();
            ResetForm();
        }
        catch (Exception ex)
        {
            // Tầng View bắt và xử lý ngoại lệ nghiệp vụ/CSDL ném từ tầng BUL/DAL
            MessageBox.Show("LỖI KHI THÊM SINH VIÊN:\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    else
    {
        DisplayValidationErrors(errors);
    }
}
```

---

## 4. Hướng dẫn khởi tạo Cơ sở dữ liệu và Chạy ứng dụng

### Bước 1: Khởi tạo CSDL SQL Server
1. Mở **SQL Server Management Studio (SSMS)** hoặc **Azure Data Studio**.
2. Mở file [QLSV.sql](file:///Users/phamquang/Projects/ltnc/buoi5/QLSV.sql) và nhấn **Execute (F5)** để tạo CSDL `QLSV` cùng các bảng `LopHoc`, `SinhVien` và nạp sẵn dữ liệu mẫu.
3. Nếu tên máy chủ SQL Server của bạn khác với `LAPTOP-6SN65NCS\HUECTMDB`, chỉ cần đổi chuỗi kết nối trong [DataHelper.cs](file:///Users/phamquang/Projects/ltnc/buoi5/QuanLySinhVien/Data/DAL/DataHelper.cs) (ví dụ: `Server=localhost; Database=QLSV; Integrated Security=True; TrustServerCertificate=true`).

### Bước 2: Chạy ứng dụng
- **Visual Studio 2022:** Mở file `QuanLySinhVien.sln` hoặc `code.slnx`, đặt `QuanLySinhVien` làm Startup Project và nhấn **F5**.
- **Kiểm thử tự động ConsoleApp:** Đặt `QuanLySinhVien.ConsoleApp` làm Startup Project để kiểm tra chuỗi kịch bản ném và bắt ngoại lệ 3 tầng.
