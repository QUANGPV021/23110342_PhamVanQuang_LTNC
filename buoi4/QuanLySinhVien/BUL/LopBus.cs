using System;
using System.Collections.Generic;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.BUL
{
    /// <summary>
    /// Business Logic Layer (BUL) cho LopHoc.
    /// Cung cấp các phương thức nghiệp vụ liên quan đến danh sách lớp học.
    /// </summary>
    public class LopBus
    {
        private readonly LopHocDAL lopDal;

        public LopBus(LopHocDAL? dal = null)
        {
            lopDal = dal ?? new LopHocDAL();
        }

        public List<LopHoc> GetAllLopHoc()
        {
            return lopDal.GetAllLopHoc();
        }

        public LopHoc? GetLopHocByMa(string maLop)
        {
            return lopDal.GetLopHocByMa(maLop);
        }

        public void AddLopHoc(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));
            lopDal.AddLopHoc(lop);
        }
    }
}
