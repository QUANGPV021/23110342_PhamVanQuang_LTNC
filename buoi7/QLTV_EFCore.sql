-- =================================================================================
-- SCRIPT KHỞI TẠO CSDL QUẢN LÝ THƯ VIỆN (EF CORE CODE FIRST - BUỔI 7)
-- Sinh viên: Phạm Văn Quang - MSSV: 23110342
-- Ngày thực hiện: 31/10/2026
-- =================================================================================

IF DB_ID(N'QLTV_EFCore') IS NULL
BEGIN
    CREATE DATABASE [QLTV_EFCore];
END
GO

USE [QLTV_EFCore];
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261031160000_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [CategoryId] nvarchar(10) NOT NULL,
        [Description] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
    );

    CREATE TABLE [Books] (
        [BookId] nvarchar(20) NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Publisher] nvarchar(150) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [NumberOfCopies] int NOT NULL,
        [Author] nvarchar(150) NOT NULL,
        [PublicationYear] int NOT NULL,
        [CategoryId] nvarchar(10) NOT NULL,
        CONSTRAINT [PK_Books] PRIMARY KEY ([BookId]),
        CONSTRAINT [FK_Books_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION
    );

    CREATE TABLE [BookCopies] (
        [BookCopyId] nvarchar(20) NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [Condition] nvarchar(30) NOT NULL,
        [BookId] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_BookCopies] PRIMARY KEY ([BookCopyId]),
        CONSTRAINT [FK_BookCopies_Books] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE NO ACTION
    );

    CREATE INDEX [IX_Books_CategoryId] ON [Books] ([CategoryId]);
    CREATE INDEX [IX_BookCopies_BookId] ON [BookCopies] ([BookId]);

    -- Nạp 7 thể loại sách
    INSERT INTO [Categories] ([CategoryId], [Description])
    VALUES
        (N'CAT01', N'Văn học'),
        (N'CAT02', N'Lịch sử'),
        (N'CAT03', N'Kinh tế'),
        (N'CAT04', N'Khoa học tự nhiên'),
        (N'CAT05', N'Công nghệ thông tin'),
        (N'CAT06', N'Kỹ năng sống'),
        (N'CAT07', N'Ngoại ngữ');

    -- Nạp 16 đầu sách
    INSERT INTO [Books] ([BookId], [Author], [CategoryId], [NumberOfCopies], [Price], [PublicationYear], [Publisher], [Title])
    VALUES
        (N'B001', N'Nguyễn Văn An', N'CAT05', 4, 155000.00, 2024, N'NXB Khoa học và Kỹ thuật', N'Lập trình C# nâng cao với .NET'),
        (N'B002', N'Đinh Văn Hùng', N'CAT05', 2, 160000.00, 2023, N'NXB Thông tin và Truyền thông', N'An toàn thông tin mạng'),
        (N'B003', N'Lê Hoàng Nam', N'CAT05', 3, 135000.00, 2021, N'NXB Đại học Quốc gia', N'Cấu trúc dữ liệu và giải thuật'),
        (N'B004', N'Trần Quốc Bảo', N'CAT03', 2, 150000.00, 2020, N'NXB Kinh tế Quốc dân', N'Kinh tế học vĩ mô căn bản'),
        (N'B005', N'Vũ Thị Mai', N'CAT03', 2, 125000.00, 2021, N'NXB Tài chính', N'Nguyên lý kế toán doanh nghiệp'),
        (N'B006', N'Trần Ngọc Lan', N'CAT01', 5, 110000.00, 2020, N'NXB Văn học', N'Tuyển tập truyện ngắn quê hương'),
        (N'B007', N'Đặng Quốc Huy', N'CAT02', 3, 135000.00, 2019, N'NXB Giáo dục', N'Tìm hiểu lịch sử Việt Nam'),
        (N'B008', N'Ngô Quang Vinh', N'CAT04', 2, 120000.00, 2019, N'NXB Giáo dục', N'Hóa học đại cương'),
        (N'B009', N'Trịnh Văn Phúc', N'CAT02', 2, 145000.00, 2018, N'NXB Thế giới', N'Lịch sử văn minh thế giới'),
        (N'B010', N'Phạm Thu Hà', N'CAT07', 2, 115000.00, 2022, N'NXB Tổng hợp TP.HCM', N'Ngữ pháp tiếng Anh học thuật'),
        (N'B011', N'Nguyễn Đức Thành', N'CAT05', 3, 180000.00, 2023, N'NXB Bách Khoa', N'Nhập môn Trí tuệ nhân tạo'),
        (N'B012', N'Phan Văn Bình', N'CAT04', 2, 140000.00, 2021, N'NXB Đại học Quốc gia', N'Sinh học và môi trường'),
        (N'B013', N'Lê Minh Khôi', N'CAT06', 3, 95000.00, 2021, N'NXB Trẻ', N'Kỹ năng giao tiếp và thuyết trình'),
        (N'B014', N'Lê Thị Thanh', N'CAT03', 3, 175000.00, 2023, N'NXB Tài chính', N'Quản trị doanh nghiệp hiện đại'),
        (N'B015', N'Đỗ Minh Quân', N'CAT03', 3, 130000.00, 2022, N'NXB Lao động', N'Tài chính cá nhân thực hành'),
        (N'B016', N'Hoàng Minh Tuấn', N'CAT05', 3, 165000.00, 2024, N'NXB Khoa học và Kỹ thuật', N'Phân tích dữ liệu với Python');

    -- Nạp 44 bản sao tương ứng
    INSERT INTO [BookCopies] ([BookCopyId], [BookId], [Condition], [Status])
    VALUES
        (N'B001.001', N'B001', N'Mới', N'Sẵn sàng'),
        (N'B001.002', N'B001', N'Mới', N'Sẵn sàng'),
        (N'B001.003', N'B001', N'Mới', N'Sẵn sàng'),
        (N'B001.004', N'B001', N'Mới', N'Sẵn sàng'),
        (N'B002.001', N'B002', N'Mới', N'Sẵn sàng'),
        (N'B002.002', N'B002', N'Mới', N'Sẵn sàng'),
        (N'B003.001', N'B003', N'Mới', N'Sẵn sàng'),
        (N'B003.002', N'B003', N'Mới', N'Sẵn sàng'),
        (N'B003.003', N'B003', N'Mới', N'Sẵn sàng'),
        (N'B004.001', N'B004', N'Mới', N'Sẵn sàng'),
        (N'B004.002', N'B004', N'Mới', N'Sẵn sàng'),
        (N'B005.001', N'B005', N'Mới', N'Sẵn sàng'),
        (N'B005.002', N'B005', N'Mới', N'Sẵn sàng'),
        (N'B006.001', N'B006', N'Mới', N'Sẵn sàng'),
        (N'B006.002', N'B006', N'Mới', N'Sẵn sàng'),
        (N'B006.003', N'B006', N'Mới', N'Sẵn sàng'),
        (N'B006.004', N'B006', N'Mới', N'Sẵn sàng'),
        (N'B006.005', N'B006', N'Mới', N'Sẵn sàng'),
        (N'B007.001', N'B007', N'Mới', N'Sẵn sàng'),
        (N'B007.002', N'B007', N'Mới', N'Sẵn sàng'),
        (N'B007.003', N'B007', N'Mới', N'Sẵn sàng'),
        (N'B008.001', N'B008', N'Mới', N'Sẵn sàng'),
        (N'B008.002', N'B008', N'Mới', N'Sẵn sàng'),
        (N'B009.001', N'B009', N'Mới', N'Sẵn sàng'),
        (N'B009.002', N'B009', N'Mới', N'Sẵn sàng'),
        (N'B010.001', N'B010', N'Mới', N'Sẵn sàng'),
        (N'B010.002', N'B010', N'Mới', N'Sẵn sàng'),
        (N'B011.001', N'B011', N'Mới', N'Sẵn sàng'),
        (N'B011.002', N'B011', N'Mới', N'Sẵn sàng'),
        (N'B011.003', N'B011', N'Mới', N'Sẵn sàng'),
        (N'B012.001', N'B012', N'Mới', N'Sẵn sàng'),
        (N'B012.002', N'B012', N'Mới', N'Sẵn sàng'),
        (N'B013.001', N'B013', N'Mới', N'Sẵn sàng'),
        (N'B013.002', N'B013', N'Mới', N'Sẵn sàng'),
        (N'B013.003', N'B013', N'Mới', N'Sẵn sàng'),
        (N'B014.001', N'B014', N'Mới', N'Sẵn sàng'),
        (N'B014.002', N'B014', N'Mới', N'Sẵn sàng'),
        (N'B014.003', N'B014', N'Mới', N'Sẵn sàng'),
        (N'B015.001', N'B015', N'Mới', N'Sẵn sàng'),
        (N'B015.002', N'B015', N'Mới', N'Sẵn sàng'),
        (N'B015.003', N'B015', N'Mới', N'Sẵn sàng'),
        (N'B016.001', N'B016', N'Mới', N'Sẵn sàng'),
        (N'B016.002', N'B016', N'Mới', N'Sẵn sàng'),
        (N'B016.003', N'B016', N'Mới', N'Sẵn sàng');

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261031160000_InitialCreate', N'8.0.10');
END;
GO

COMMIT;
GO
