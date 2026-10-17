using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Data Access Layer (DAL) cho thực thể SinhVien bằng ADO.Net.
    /// Thực hiện các thao tác Thêm, Sửa, Xóa, Lấy về từ cơ sở dữ liệu SQL Server.
    /// Ném ngoại lệ về tầng Business khi có lỗi kết nối hoặc truy vấn.
    /// </summary>
    public class SinhVienDAL
    {
        private readonly DataHelper dataHelper;

        public SinhVienDAL()
        {
            dataHelper = new DataHelper();
        }

        public SinhVienDAL(DataHelper helper)
        {
            dataHelper = helper ?? new DataHelper();
        }

        public SinhVienDAL(string connectionString)
        {
            dataHelper = new DataHelper(connectionString);
        }

        #region Helper Mapping

        private SinhVien MapDataRowToSinhVien(DataRow dr)
        {
            SinhVien sv = new SinhVien();
            sv.MaSV = dr["MaSV"]?.ToString() ?? string.Empty;
            sv.HoTen = dr["HoTen"]?.ToString() ?? string.Empty;
            sv.GioiTinh = dr["GioiTinh"]?.ToString() ?? "Nam";

            if (dr["NgaySinh"] != DBNull.Value && DateTime.TryParse(dr["NgaySinh"].ToString(), out DateTime ns))
            {
                sv.NgaySinh = ns;
            }

            sv.SoDienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty;
            sv.MaLop = dr["MaLop"]?.ToString() ?? string.Empty;

            if (dr["Diem"] != DBNull.Value && double.TryParse(dr["Diem"].ToString(), out double diem))
            {
                sv.Diem = diem;
            }

            sv.Email = dr["Email"]?.ToString() ?? string.Empty;

            if (dr.Table.Columns.Contains("TrangThai") && dr["TrangThai"] != DBNull.Value)
            {
                sv.TrangThai = dr["TrangThai"].ToString() ?? "Đang học";
            }

            return sv;
        }

        private SinhVien MapDataReaderToSinhVien(SqlDataReader dr)
        {
            SinhVien sv = new SinhVien();
            sv.MaSV = dr["MaSV"]?.ToString() ?? string.Empty;
            sv.HoTen = dr["HoTen"]?.ToString() ?? string.Empty;
            sv.GioiTinh = dr["GioiTinh"]?.ToString() ?? "Nam";

            if (dr["NgaySinh"] != DBNull.Value && DateTime.TryParse(dr["NgaySinh"].ToString(), out DateTime ns))
            {
                sv.NgaySinh = ns;
            }

            sv.SoDienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty;
            sv.MaLop = dr["MaLop"]?.ToString() ?? string.Empty;

            if (dr["Diem"] != DBNull.Value && double.TryParse(dr["Diem"].ToString(), out double diem))
            {
                sv.Diem = diem;
            }

            sv.Email = dr["Email"]?.ToString() ?? string.Empty;

            try
            {
                if (dr["TrangThai"] != DBNull.Value)
                {
                    sv.TrangThai = dr["TrangThai"].ToString() ?? "Đang học";
                }
            }
            catch
            {
                // Bảng chưa có cột TrangThai thì giữ giá trị mặc định
            }

            return sv;
        }

        #endregion

        #region Query Methods (ADO.Net)

        /// <summary>
        /// Lấy tất cả sinh viên bằng SqlDataReader theo code mẫu bài thực hành
        /// </summary>
        public List<SinhVien> getAllSinhVien1()
        {
            try
            {
                List<SinhVien> lv = new List<SinhVien>();
                using (SqlDataReader dr = dataHelper.ExcuteReader("SELECT * FROM SinhVien"))
                {
                    while (dr.Read())
                    {
                        lv.Add(MapDataReaderToSinhVien(dr));
                    }
                }
                dataHelper.CloseConnection();
                return lv;
            }
            catch (Exception ex)
            {
                dataHelper.CloseConnection();
                throw new Exception("Lỗi khi đọc dữ liệu sinh viên bằng SqlDataReader: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy tất cả sinh viên bằng SqlDataReader
        /// </summary>
        public List<SinhVien> GetAllSinhVien1()
        {
            return getAllSinhVien1();
        }

        /// <summary>
        /// Lấy tất cả sinh viên bằng DataTable (SqlDataAdapter)
        /// </summary>
        public List<SinhVien> GetAllSinhVien()
        {
            try
            {
                List<SinhVien> sinhViens = new List<SinhVien>();
                DataTable dt = dataHelper.GetDataTable("SELECT * FROM SinhVien");
                foreach (DataRow dr in dt.Rows)
                {
                    sinhViens.Add(MapDataRowToSinhVien(dr));
                }
                return sinhViens;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tất cả sinh viên từ CSDL: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy sinh viên theo Mã sinh viên bằng ADO.Net
        /// </summary>
        public SinhVien? GetSinhVienByMaSV(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return null;

            try
            {
                string sql = "SELECT * FROM SinhVien WHERE MaSV = @MaSV";
                SqlParameter p = new SqlParameter("@MaSV", maSV.Trim());
                DataTable dt = dataHelper.GetDataTable(sql, p);

                if (dt.Rows.Count > 0)
                {
                    return MapDataRowToSinhVien(dt.Rows[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tra cứu sinh viên mã '{maSV}' từ CSDL: {ex.Message}", ex);
            }
        }

        public SinhVien? GetSinhVienById(string maSV)
        {
            return GetSinhVienByMaSV(maSV);
        }

        /// <summary>
        /// Lấy danh sách sinh viên theo Mã lớp bằng ADO.Net
        /// </summary>
        public List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop) || maLop.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                return GetAllSinhVien();
            }

            try
            {
                List<SinhVien> lv = new List<SinhVien>();
                string sql = "SELECT * FROM SinhVien WHERE MaLop = @MaLop";
                SqlParameter p = new SqlParameter("@MaLop", maLop.Trim());
                DataTable dt = dataHelper.GetDataTable(sql, p);

                foreach (DataRow r in dt.Rows)
                {
                    lv.Add(MapDataRowToSinhVien(r));
                }
                return lv;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách sinh viên của lớp '{maLop}' từ CSDL: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sinh viên kết hợp theo từ khóa, mã lớp và điểm sàn trực tiếp từ CSDL bằng SQL
        /// </summary>
        public List<SinhVien> Search(string? keyword, string? maLop, double diemTu)
        {
            try
            {
                List<string> conditions = new List<string>();
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    conditions.Add("(MaSV LIKE @Keyword OR HoTen LIKE @Keyword OR Email LIKE @Keyword OR SoDienThoai LIKE @Keyword)");
                    parameters.Add(new SqlParameter("@Keyword", $"%{keyword.Trim()}%"));
                }

                if (!string.IsNullOrWhiteSpace(maLop) && !maLop.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                {
                    conditions.Add("MaLop = @MaLop");
                    parameters.Add(new SqlParameter("@MaLop", maLop.Trim()));
                }

                if (diemTu > 0)
                {
                    conditions.Add("Diem >= @DiemTu");
                    parameters.Add(new SqlParameter("@DiemTu", diemTu));
                }

                string sql = "SELECT * FROM SinhVien";
                if (conditions.Count > 0)
                {
                    sql += " WHERE " + string.Join(" AND ", conditions);
                }

                DataTable dt = dataHelper.GetDataTable(sql, parameters.ToArray());
                List<SinhVien> result = new List<SinhVien>();
                foreach (DataRow r in dt.Rows)
                {
                    result.Add(MapDataRowToSinhVien(r));
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sinh viên trong CSDL: " + ex.Message, ex);
            }
        }

        #endregion

        #region Mutation Methods (INSERT, UPDATE, DELETE)

        /// <summary>
        /// Thêm mới sinh viên vào CSDL bằng ADO.Net
        /// </summary>
        public void AddSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            try
            {
                string sql = @"INSERT INTO SinhVien (MaSV, MaLop, HoTen, GioiTinh, NgaySinh, Diem, SoDienThoai, Email, TrangThai)
                               VALUES (@MaSV, @MaLop, @HoTen, @GioiTinh, @NgaySinh, @Diem, @SoDienThoai, @Email, @TrangThai)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", sv.MaSV),
                    new SqlParameter("@MaLop", sv.MaLop),
                    new SqlParameter("@HoTen", sv.HoTen),
                    new SqlParameter("@GioiTinh", sv.GioiTinh ?? "Nam"),
                    new SqlParameter("@NgaySinh", sv.NgaySinh),
                    new SqlParameter("@Diem", sv.Diem),
                    new SqlParameter("@SoDienThoai", sv.SoDienThoai ?? string.Empty),
                    new SqlParameter("@Email", sv.Email ?? string.Empty),
                    new SqlParameter("@TrangThai", sv.TrangThai ?? "Đang học")
                };

                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception exx)
            {
                throw new Exception("Lỗi khi thêm sinh viên vào CSDL: " + exx.Message, exx);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sinh viên trong CSDL bằng ADO.Net
        /// </summary>
        public void UpdateSinhVien(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            try
            {
                string sql = @"UPDATE SinhVien 
                               SET HoTen = @HoTen, 
                                   MaLop = @MaLop, 
                                   GioiTinh = @GioiTinh, 
                                   NgaySinh = @NgaySinh, 
                                   Diem = @Diem, 
                                   SoDienThoai = @SoDienThoai, 
                                   Email = @Email, 
                                   TrangThai = @TrangThai
                               WHERE MaSV = @MaSV";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", sv.MaSV),
                    new SqlParameter("@MaLop", sv.MaLop),
                    new SqlParameter("@HoTen", sv.HoTen),
                    new SqlParameter("@GioiTinh", sv.GioiTinh ?? "Nam"),
                    new SqlParameter("@NgaySinh", sv.NgaySinh),
                    new SqlParameter("@Diem", sv.Diem),
                    new SqlParameter("@SoDienThoai", sv.SoDienThoai ?? string.Empty),
                    new SqlParameter("@Email", sv.Email ?? string.Empty),
                    new SqlParameter("@TrangThai", sv.TrangThai ?? "Đang học")
                };

                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception exx)
            {
                throw new Exception("Lỗi khi cập nhật sinh viên trong CSDL: " + exx.Message, exx);
            }
        }

        /// <summary>
        /// Xóa sinh viên khỏi CSDL bằng ADO.Net
        /// </summary>
        public void DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return;

            try
            {
                string sql = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
                SqlParameter p = new SqlParameter("@MaSV", maSV.Trim());
                dataHelper.ExecuteNonQuery(sql, p);
            }
            catch (Exception exx)
            {
                throw new Exception($"Lỗi khi xóa sinh viên mã '{maSV}' trong CSDL: " + exx.Message, exx);
            }
        }

        #endregion
    }

    /// <summary>
    /// Lớp bí danh SinhVienDAO để tương thích hoàn toàn với tên gọi trong đề bài
    /// </summary>
    public class SinhVienDAO : SinhVienDAL
    {
        public SinhVienDAO() : base() { }
        public SinhVienDAO(DataHelper helper) : base(helper) { }
        public SinhVienDAO(string connectionString) : base(connectionString) { }
    }
}
