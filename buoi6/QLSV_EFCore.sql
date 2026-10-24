-- ============================================================================
-- SCRIPT TẠO CƠ SỞ DỮ LIỆU QLSV_EFCore THEO CHUẨN ENTITY FRAMEWORK CORE MIGRATION
-- Môn: Lập trình nâng cao (LTNC) - Buổi 6: Entity Framework Core Code First
-- Sinh viên: Phạm Văn Quang - MSSV: 23110342 - Trường Đại học Việt Nhật (VJU)
-- ============================================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'QLSV_EFCore')
BEGIN
    CREATE DATABASE QLSV_EFCore;
END
GO

USE QLSV_EFCore;
GO

-- 1. TẠO BẢNG LƯU LỊCH SỬ MIGRATION CỦA EF CORE
IF OBJECT_ID(N'dbo.__EFMigrationsHistory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.__EFMigrationsHistory (
        MigrationId NVARCHAR(150) NOT NULL PRIMARY KEY,
        ProductVersion NVARCHAR(32) NOT NULL
    );
END
GO

-- 2. TẠO BẢNG LopHoc NẾU CHƯA CÓ
IF OBJECT_ID(N'dbo.LopHoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LopHoc (
        MaLop NVARCHAR(20) NOT NULL PRIMARY KEY,
        TenLop NVARCHAR(100) NOT NULL,
        Khoa NVARCHAR(100) NULL
    );
END
GO

-- 3. TẠO BẢNG SinhVien NẾU CHƯA CÓ
IF OBJECT_ID(N'dbo.SinhVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SinhVien (
        MaSV NVARCHAR(20) NOT NULL PRIMARY KEY,
        HoTen NVARCHAR(100) NOT NULL,
        NgaySinh DATETIME NOT NULL,
        GioiTinh NVARCHAR(10) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        SoDienThoai NVARCHAR(20) NOT NULL,
        Diem FLOAT NOT NULL,
        MaLop NVARCHAR(20) NOT NULL,
        TrangThai NVARCHAR(50) NULL DEFAULT N'Đang học',
        CONSTRAINT FK_SinhVien_LopHoc FOREIGN KEY (MaLop) REFERENCES dbo.LopHoc(MaLop) ON DELETE CASCADE
    );

    CREATE INDEX IX_SinhVien_MaLop ON dbo.SinhVien (MaLop);
END
GO

-- 4. NẠP DỮ LIỆU SEED (HasData) CỦA MIGRATION InitialCreate
IF NOT EXISTS (SELECT 1 FROM dbo.LopHoc WHERE MaLop = N'KTPM01')
BEGIN
    INSERT INTO dbo.LopHoc (MaLop, TenLop, Khoa) VALUES
    (N'KTPM01', N'Kỹ thuật phần mềm 01', N'Công nghệ thông tin'),
    (N'TTNT01', N'Trí tuệ nhân tạo 01', N'Trí tuệ nhân tạo'),
    (N'KHDL01', N'Khoa học dữ liệu 01', N'Khoa học máy tính'),
    (N'HTTT01', N'Hệ thống thông tin 01', N'Hệ thống thông tin'),
    (N'CSE0001', N'Khoa học máy tính 01', N'Khoa học máy tính'),
    (N'CSE0002', N'Khoa học máy tính 02', N'Khoa học máy tính'),
    (N'CSE0003', N'Khoa học máy tính 03', N'Khoa học máy tính');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SinhVien WHERE MaSV = N'SV000123')
BEGIN
    INSERT INTO dbo.SinhVien (MaSV, MaLop, HoTen, GioiTinh, NgaySinh, Diem, SoDienThoai, Email, TrangThai) VALUES
    (N'SV000123', N'KTPM01', N'Nguyễn Văn An', N'Nam', '2006-08-15', 8.5, N'0912345678', N'an.nv@vju.ac.vn', N'Đang học'),
    (N'SV000124', N'TTNT01', N'Trần Minh Anh', N'Nữ', '2006-01-22', 9.0, N'0987654321', N'anh.tm@vju.ac.vn', N'Đang học'),
    (N'SV000125', N'KTPM01', N'Lê Hoàng Bình', N'Nam', '2006-05-09', 7.4, N'0355556677', N'binh.lh@vju.ac.vn', N'Đang học'),
    (N'SV000126', N'KHDL01', N'Đỗ Thị Hồng', N'Nữ', '2006-11-30', 8.1, N'0777888999', N'hong.dt@vju.ac.vn', N'Đang học'),
    (N'SV0001', N'CSE0001', N'Nguyễn Văn A', N'Nam', '2000-01-01', 8.0, N'0911223344', N'a.nv@vju.ac.vn', N'Đang học'),
    (N'SV0002', N'CSE0002', N'Trần Thị B', N'Nữ', '2000-02-02', 7.5, N'0922334455', N'b.tt@vju.ac.vn', N'Đang học'),
    (N'SV0003', N'CSE0003', N'Lê Văn C', N'Nam', '2000-03-03', 8.8, N'0933445566', N'c.lv@vju.ac.vn', N'Đang học');
END
GO

-- 5. GHI NHẬN MIGRATION VÀO LỊCH SỬ
IF NOT EXISTS (SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = N'20261024160000_InitialCreate')
BEGIN
    INSERT INTO dbo.__EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES (N'20261024160000_InitialCreate', N'8.0.10');
END
GO

SELECT * FROM dbo.LopHoc;
SELECT * FROM dbo.SinhVien;
SELECT * FROM dbo.__EFMigrationsHistory;
GO
