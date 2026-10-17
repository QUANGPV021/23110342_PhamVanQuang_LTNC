using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Data Access Layer (DAL) cho thực thể LopHoc bằng ADO.Net.
    /// Quản lý dữ liệu bảng LopHoc trong cơ sở dữ liệu SQL Server.
    /// Ném ngoại lệ về các tầng trên khi xảy ra lỗi kết nối hoặc truy vấn.
    /// </summary>
    public class LopHocDAL
    {
        private readonly DataHelper dataHelper;

        public LopHocDAL()
        {
            dataHelper = new DataHelper();
        }

        public LopHocDAL(DataHelper helper)
        {
            dataHelper = helper ?? new DataHelper();
        }

        public LopHocDAL(string connectionString)
        {
            dataHelper = new DataHelper(connectionString);
        }

        private LopHoc MapDataRowToLopHoc(DataRow dr)
        {
            LopHoc lop = new LopHoc();
            lop.MaLop = dr["MaLop"]?.ToString() ?? string.Empty;
            lop.TenLop = dr["TenLop"]?.ToString() ?? string.Empty;
            lop.Khoa = dr["Khoa"]?.ToString() ?? string.Empty;
            return lop;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách lớp học từ CSDL
        /// </summary>
        public List<LopHoc> GetAllLopHoc()
        {
            try
            {
                List<LopHoc> list = new List<LopHoc>();
                DataTable dt = dataHelper.GetDataTable("SELECT * FROM LopHoc");
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(MapDataRowToLopHoc(dr));
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách lớp học từ CSDL: " + ex.Message, ex);
            }
        }

        public List<LopHoc> GetAll() => GetAllLopHoc();

        /// <summary>
        /// Lấy thông tin lớp học theo Mã lớp
        /// </summary>
        public LopHoc? GetLopHocByMa(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return null;

            try
            {
                string sql = "SELECT * FROM LopHoc WHERE MaLop = @MaLop";
                SqlParameter p = new SqlParameter("@MaLop", maLop.Trim());
                DataTable dt = dataHelper.GetDataTable(sql, p);

                if (dt.Rows.Count > 0)
                {
                    return MapDataRowToLopHoc(dt.Rows[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tra cứu lớp học mã '{maLop}' từ CSDL: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm mới lớp học vào CSDL
        /// </summary>
        public void AddLopHoc(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));

            try
            {
                string sql = "INSERT INTO LopHoc (MaLop, TenLop, Khoa) VALUES (@MaLop, @TenLop, @Khoa)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", lop.MaLop.Trim()),
                    new SqlParameter("@TenLop", lop.TenLop.Trim()),
                    new SqlParameter("@Khoa", lop.Khoa ?? string.Empty)
                };
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm lớp học mã '{lop.MaLop}' vào CSDL: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin lớp học trong CSDL
        /// </summary>
        public void UpdateLopHoc(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));

            try
            {
                string sql = "UPDATE LopHoc SET TenLop = @TenLop, Khoa = @Khoa WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", lop.MaLop.Trim()),
                    new SqlParameter("@TenLop", lop.TenLop.Trim()),
                    new SqlParameter("@Khoa", lop.Khoa ?? string.Empty)
                };
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật lớp học mã '{lop.MaLop}' trong CSDL: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa lớp học khỏi CSDL
        /// </summary>
        public void DeleteLopHoc(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return;

            try
            {
                string sql = "DELETE FROM LopHoc WHERE MaLop = @MaLop";
                SqlParameter p = new SqlParameter("@MaLop", maLop.Trim());
                dataHelper.ExecuteNonQuery(sql, p);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa lớp học mã '{maLop}' khỏi CSDL: " + ex.Message, ex);
            }
        }
    }

    /// <summary>
    /// Lớp bí danh LopHocDAO để tương thích hoàn toàn với tên gọi trong đề bài
    /// </summary>
    public class LopHocDAO : LopHocDAL
    {
        public LopHocDAO() : base() { }
        public LopHocDAO(DataHelper helper) : base(helper) { }
        public LopHocDAO(string connectionString) : base(connectionString) { }
    }
}
