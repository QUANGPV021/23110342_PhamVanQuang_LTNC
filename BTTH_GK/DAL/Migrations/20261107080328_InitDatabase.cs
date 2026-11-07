using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoaiThietBi",
                columns: table => new
                {
                    MaLoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiThietBi", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "ThietBi",
                columns: table => new
                {
                    MaTB = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenTB = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NgayMua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaLoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietBi", x => x.MaTB);
                    table.ForeignKey(
                        name: "FK_ThietBi_LoaiThietBi_MaLoai",
                        column: x => x.MaLoai,
                        principalTable: "LoaiThietBi",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LoaiThietBi",
                columns: new[] { "MaLoai", "TenLoai" },
                values: new object[,]
                {
                    { "LTB01", "Thiết bị trình chiếu" },
                    { "LTB02", "Thiết bị máy tính" },
                    { "LTB03", "Thiết bị mạng" },
                    { "LTB04", "Thiết bị âm thanh" }
                });

            migrationBuilder.InsertData(
                table: "ThietBi",
                columns: new[] { "MaTB", "DonGia", "MaLoai", "NgayMua", "SoLuong", "TenTB" },
                values: new object[,]
                {
                    { "TB001", 12500000m, "LTB01", new DateTime(2024, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Máy chiếu Epson EB-X06" },
                    { "TB002", 15000000m, "LTB02", new DateTime(2024, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, "Máy tính Dell OptiPlex" },
                    { "TB003", 1850000m, "LTB03", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "Bộ phát Wi-Fi TP-Link" },
                    { "TB004", 2400000m, "LTB04", new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Loa trợ giảng" },
                    { "TB005", 42000000m, "LTB01", new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Bảng tương tác thông minh" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThietBi_MaLoai",
                table: "ThietBi",
                column: "MaLoai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThietBi");

            migrationBuilder.DropTable(
                name: "LoaiThietBi");
        }
    }
}
