-- ============================================================================
-- SCRIPT TẠO CƠ SỞ DỮ LIỆU QLSV CHO BÀI TẬP BUỔI 5: THAO TÁC CSDL BẰNG ADO.NET
-- Sinh viên: Phạm Văn Quang - MSSV: 23110342 - Trường Đại học Việt Nhật (VJU)
-- ============================================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'QLSV')
BEGIN
    CREATE DATABASE QLSV;
END
GO

USE QLSV;
GO

-- 1. XÓA BẢNG NẾU ĐÃ TỒN TẠI ĐỂ TẠO MỚI SẠCH SẼ
IF OBJECT_ID(N'dbo.SinhVien', N'U') IS NOT NULL
    DROP TABLE dbo.SinhVien;
GO

IF OBJECT_ID(N'dbo.LopHoc', N'U') IS NOT NULL
    DROP TABLE dbo.LopHoc;
GO

-- 2. TẠO BẢNG LopHoc
CREATE TABLE dbo.LopHoc (
    MaLop NVARCHAR(20) NOT NULL PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    Khoa NVARCHAR(100) NULL
);
GO

-- 3. TẠO BẢNG SinhVien
CREATE TABLE dbo.SinhVien (
    MaSV NVARCHAR(20) NOT NULL PRIMARY KEY,
    MaLop NVARCHAR(20) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10) NULL,
    NgaySinh DATETIME NULL,
    Diem FLOAT NULL DEFAULT 0.0,
    SoDienThoai NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    TrangThai NVARCHAR(50) NULL DEFAULT N'Đang học',
    CONSTRAINT FK_SinhVien_LopHoc FOREIGN KEY (MaLop) REFERENCES dbo.LopHoc(MaLop) ON DELETE CASCADE ON UPDATE CASCADE
);
GO

-- 4. THÊM DỮ LIỆU MẪU CHO BẢNG LopHoc
INSERT INTO dbo.LopHoc (MaLop, TenLop, Khoa) VALUES
(N'KTPM01', N'Kỹ thuật phần mềm 01', N'Công nghệ thông tin'),
(N'TTNT01', N'Trí tuệ nhân tạo 01', N'Trí tuệ nhân tạo'),
(N'KHDL01', N'Khoa học dữ liệu 01', N'Khoa học máy tính'),
(N'HTTT01', N'Hệ thống thông tin 01', N'Hệ thống thông tin'),
(N'CSE0001', N'Khoa học máy tính 01', N'Khoa học máy tính'),
(N'CSE0002', N'Khoa học máy tính 02', N'Khoa học máy tính'),
(N'CSE0003', N'Khoa học máy tính 03', N'Khoa học máy tính');
GO

-- 5. THÊM DỮ LIỆU MẪU CHO BẢNG SinhVien
INSERT INTO dbo.SinhVien (MaSV, MaLop, HoTen, GioiTinh, NgaySinh, Diem, SoDienThoai, Email, TrangThai) VALUES
(N'SV000123', N'KTPM01', N'Nguyễn Văn An', N'Nam', '2006-08-15', 8.5, N'0912345678', N'an.nv@vju.ac.vn', N'Đang học'),
(N'SV000124', N'TTNT01', N'Trần Minh Anh', N'Nữ', '2006-01-22', 9.0, N'0987654321', N'anh.tm@vju.ac.vn', N'Đang học'),
(N'SV000125', N'KTPM01', N'Lê Hoàng Bình', N'Nam', '2006-05-09', 7.4, N'0355556677', N'binh.lh@vju.ac.vn', N'Đang học'),
(N'SV000126', N'KHDL01', N'Đỗ Thị Hồng', N'Nữ', '2006-11-30', 8.1, N'0777888999', N'hong.dt@vju.ac.vn', N'Đang học'),
(N'SV0001', N'CSE0001', N'Nguyễn Văn A', N'Nam', '2000-01-01', 8.0, N'0911223344', N'a.nv@vju.ac.vn', N'Đang học'),
(N'SV0002', N'CSE0002', N'Trần Thị B', N'Nữ', '2000-02-02', 7.5, N'0922334455', N'b.tt@vju.ac.vn', N'Đang học'),
(N'SV0003', N'CSE0003', N'Lê Văn C', N'Nam', '2000-03-03', 8.8, N'0933445566', N'c.lv@vju.ac.vn', N'Đang học');
GO

-- 6. KIỂM TRA DỮ LIỆU VỪA TẠO
SELECT * FROM dbo.LopHoc;
SELECT * FROM dbo.SinhVien;
GO
