using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyThuVienDAO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfCopies = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PublicationYear = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Categories",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookCopies",
                columns: table => new
                {
                    BookCopyId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BookId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCopies", x => x.BookCopyId);
                    table.ForeignKey(
                        name: "FK_BookCopies_Books",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description" },
                values: new object[,]
                {
                    { "CAT01", "Văn học" },
                    { "CAT02", "Lịch sử" },
                    { "CAT03", "Kinh tế" },
                    { "CAT04", "Khoa học tự nhiên" },
                    { "CAT05", "Công nghệ thông tin" },
                    { "CAT06", "Kỹ năng sống" },
                    { "CAT07", "Ngoại ngữ" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "CategoryId", "NumberOfCopies", "Price", "PublicationYear", "Publisher", "Title" },
                values: new object[,]
                {
                    { "B001", "Nguyễn Văn An", "CAT05", 4, 155000m, 2024, "NXB Khoa học và Kỹ thuật", "Lập trình C# nâng cao với .NET" },
                    { "B002", "Đinh Văn Hùng", "CAT05", 2, 160000m, 2023, "NXB Thông tin và Truyền thông", "An toàn thông tin mạng" },
                    { "B003", "Lê Hoàng Nam", "CAT05", 3, 135000m, 2021, "NXB Đại học Quốc gia", "Cấu trúc dữ liệu và giải thuật" },
                    { "B004", "Trần Quốc Bảo", "CAT03", 2, 150000m, 2020, "NXB Kinh tế Quốc dân", "Kinh tế học vĩ mô căn bản" },
                    { "B005", "Vũ Thị Mai", "CAT03", 2, 125000m, 2021, "NXB Tài chính", "Nguyên lý kế toán doanh nghiệp" },
                    { "B006", "Trần Ngọc Lan", "CAT01", 5, 110000m, 2020, "NXB Văn học", "Tuyển tập truyện ngắn quê hương" },
                    { "B007", "Đặng Quốc Huy", "CAT02", 3, 135000m, 2019, "NXB Giáo dục", "Tìm hiểu lịch sử Việt Nam" },
                    { "B008", "Ngô Quang Vinh", "CAT04", 2, 120000m, 2019, "NXB Giáo dục", "Hóa học đại cương" },
                    { "B009", "Trịnh Văn Phúc", "CAT02", 2, 145000m, 2018, "NXB Thế giới", "Lịch sử văn minh thế giới" },
                    { "B010", "Phạm Thu Hà", "CAT07", 2, 115000m, 2022, "NXB Tổng hợp TP.HCM", "Ngữ pháp tiếng Anh học thuật" },
                    { "B011", "Nguyễn Đức Thành", "CAT05", 3, 180000m, 2023, "NXB Bách Khoa", "Nhập môn Trí tuệ nhân tạo" },
                    { "B012", "Phan Văn Bình", "CAT04", 2, 140000m, 2021, "NXB Đại học Quốc gia", "Sinh học và môi trường" },
                    { "B013", "Lê Minh Khôi", "CAT06", 3, 95000m, 2021, "NXB Trẻ", "Kỹ năng giao tiếp và thuyết trình" },
                    { "B014", "Lê Thị Thanh", "CAT03", 3, 175000m, 2023, "NXB Tài chính", "Quản trị doanh nghiệp hiện đại" },
                    { "B015", "Đỗ Minh Quân", "CAT03", 3, 130000m, 2022, "NXB Lao động", "Tài chính cá nhân thực hành" },
                    { "B016", "Hoàng Minh Tuấn", "CAT05", 3, 165000m, 2024, "NXB Khoa học và Kỹ thuật", "Phân tích dữ liệu với Python" }
                });

            migrationBuilder.InsertData(
                table: "BookCopies",
                columns: new[] { "BookCopyId", "BookId", "Condition", "Status" },
                values: new object[,]
                {
                    { "B001.001", "B001", "Mới", "Sẵn sàng" },
                    { "B001.002", "B001", "Mới", "Sẵn sàng" },
                    { "B001.003", "B001", "Mới", "Sẵn sàng" },
                    { "B001.004", "B001", "Mới", "Sẵn sàng" },
                    { "B002.001", "B002", "Mới", "Sẵn sàng" },
                    { "B002.002", "B002", "Mới", "Sẵn sàng" },
                    { "B003.001", "B003", "Mới", "Sẵn sàng" },
                    { "B003.002", "B003", "Mới", "Sẵn sàng" },
                    { "B003.003", "B003", "Mới", "Sẵn sàng" },
                    { "B004.001", "B004", "Mới", "Sẵn sàng" },
                    { "B004.002", "B004", "Mới", "Sẵn sàng" },
                    { "B005.001", "B005", "Mới", "Sẵn sàng" },
                    { "B005.002", "B005", "Mới", "Sẵn sàng" },
                    { "B006.001", "B006", "Mới", "Sẵn sàng" },
                    { "B006.002", "B006", "Mới", "Sẵn sàng" },
                    { "B006.003", "B006", "Mới", "Sẵn sàng" },
                    { "B006.004", "B006", "Mới", "Sẵn sàng" },
                    { "B006.005", "B006", "Mới", "Sẵn sàng" },
                    { "B007.001", "B007", "Mới", "Sẵn sàng" },
                    { "B007.002", "B007", "Mới", "Sẵn sàng" },
                    { "B007.003", "B007", "Mới", "Sẵn sàng" },
                    { "B008.001", "B008", "Mới", "Sẵn sàng" },
                    { "B008.002", "B008", "Mới", "Sẵn sàng" },
                    { "B009.001", "B009", "Mới", "Sẵn sàng" },
                    { "B009.002", "B009", "Mới", "Sẵn sàng" },
                    { "B010.001", "B010", "Mới", "Sẵn sàng" },
                    { "B010.002", "B010", "Mới", "Sẵn sàng" },
                    { "B011.001", "B011", "Mới", "Sẵn sàng" },
                    { "B011.002", "B011", "Mới", "Sẵn sàng" },
                    { "B011.003", "B011", "Mới", "Sẵn sàng" },
                    { "B012.001", "B012", "Mới", "Sẵn sàng" },
                    { "B012.002", "B012", "Mới", "Sẵn sàng" },
                    { "B013.001", "B013", "Mới", "Sẵn sàng" },
                    { "B013.002", "B013", "Mới", "Sẵn sàng" },
                    { "B013.003", "B013", "Mới", "Sẵn sàng" },
                    { "B014.001", "B014", "Mới", "Sẵn sàng" },
                    { "B014.002", "B014", "Mới", "Sẵn sàng" },
                    { "B014.003", "B014", "Mới", "Sẵn sàng" },
                    { "B015.001", "B015", "Mới", "Sẵn sàng" },
                    { "B015.002", "B015", "Mới", "Sẵn sàng" },
                    { "B015.003", "B015", "Mới", "Sẵn sàng" },
                    { "B016.001", "B016", "Mới", "Sẵn sàng" },
                    { "B016.002", "B016", "Mới", "Sẵn sàng" },
                    { "B016.003", "B016", "Mới", "Sẵn sàng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_BookId",
                table: "BookCopies",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCopies");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
