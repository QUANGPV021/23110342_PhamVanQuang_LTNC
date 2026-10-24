using System;
using System.Collections.Generic;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.BUL
{
    /// <summary>
    /// Business Logic Layer (BUL) cho thực thể LopHoc.
    /// Điều phối nghiệp vụ danh sách lớp học và ném ngoại lệ khi có lỗi phát sinh từ EF Core.
    /// </summary>
    public class LopBus
    {
        private readonly LopHocDAL lopDal;

        public LopBus()
        {
            lopDal = new LopHocDAL();
        }

        public LopBus(LopHocDAL? dal)
        {
            lopDal = dal ?? new LopHocDAL();
        }

        public LopBus(string connectionString)
        {
            lopDal = new LopHocDAL(connectionString);
        }

        public List<LopHoc> GetAllLopHoc()
        {
            try
            {
                return lopDal.GetAllLopHoc();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy danh sách lớp học: " + ex.Message, ex);
            }
        }

        public LopHoc? GetLopHocByMa(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) return null;

            try
            {
                return lopDal.GetLopHocByMa(maLop.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi tìm lớp học mã '{maLop}': {ex.Message}", ex);
            }
        }

        public void AddLopHoc(LopHoc lop)
        {
            if (lop == null)
            {
                throw new ArgumentNullException(nameof(lop), "Thông tin lớp học không được rỗng.");
            }

            var errors = lop.IsInValid();
            if (errors.Count > 0)
            {
                throw new Exception(string.Join("\n", errors));
            }

            try
            {
                lopDal.AddLopHoc(lop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi thêm lớp học: " + ex.Message, ex);
            }
        }

        public void UpdateLopHoc(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));

            var errors = lop.IsInValid();
            if (errors.Count > 0)
            {
                throw new Exception(string.Join("\n", errors));
            }

            try
            {
                lopDal.UpdateLopHoc(lop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi cập nhật lớp học: " + ex.Message, ex);
            }
        }

        public void DeleteLopHoc(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                throw new ArgumentException("Mã lớp cần xóa không được để trống.", nameof(maLop));
            }

            try
            {
                lopDal.DeleteLopHoc(maLop.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi xóa lớp học: " + ex.Message, ex);
            }
        }
    }
}
