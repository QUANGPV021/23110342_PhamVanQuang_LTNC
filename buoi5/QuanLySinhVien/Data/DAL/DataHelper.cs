using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Lớp tiện ích kết nối và thao tác với cơ sở dữ liệu SQL Server bằng ADO.Net.
    /// Quản lý SqlConnection, SqlCommand, SqlDataAdapter, SqlDataReader.
    /// Ném ngoại lệ về các tầng trên khi xảy ra sự cố thao tác dữ liệu.
    /// </summary>
    public class DataHelper
    {
        public static string DefaultConnectionString = @"Server=LAPTOP-6SN65NCS\HUECTMDB; Database=QLSV;Integrated Security=True; TrustServerCertificate=true";

        private SqlConnection con;
        private string connectionString;

        public string ConnectionString => connectionString;

        public DataHelper() : this(DefaultConnectionString)
        {
        }

        public DataHelper(string? connString)
        {
            this.connectionString = string.IsNullOrWhiteSpace(connString) ? DefaultConnectionString : connString;
            this.con = new SqlConnection(this.connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể kết nối tới cơ sở dữ liệu: {ex.Message}", ex);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đóng kết nối: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thực thi các câu lệnh INSERT, UPDATE, DELETE bằng SQL thuần
        /// </summary>
        public void ExecuteNonQuery(string sqlExcuteQuery)
        {
            try
            {
                OpenConnection();
                using (SqlCommand comand = new SqlCommand(sqlExcuteQuery, con))
                {
                    comand.ExecuteNonQuery();
                }
            }
            catch (Exception exx)
            {
                throw new Exception($"Lỗi ExecuteNonQuery: {exx.Message}", exx);
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Thực thi các câu lệnh INSERT, UPDATE, DELETE kèm tham số Parameters an toàn
        /// </summary>
        public void ExecuteNonQuery(string sqlQuery, params SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi ExecuteNonQuery với tham số: {ex.Message}", ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT trả về SqlDataReader
        /// </summary>
        public SqlDataReader ExcuteReader(string sqlSelect)
        {
            try
            {
                OpenConnection();
                SqlCommand comand = new SqlCommand(sqlSelect, con);
                SqlDataReader red = comand.ExecuteReader(CommandBehavior.CloseConnection);
                return red;
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw new Exception($"Lỗi ExcuteReader: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT và điền vào DataTable thông qua SqlDataAdapter
        /// </summary>
        public DataTable GetDataTable(string sqlSelect)
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sqlSelect, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi GetDataTable: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT với tham số và điền vào DataTable
        /// </summary>
        public DataTable GetDataTable(string sqlSelect, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlSelect, con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi GetDataTable với tham số: {ex.Message}", ex);
            }
        }
    }
}
