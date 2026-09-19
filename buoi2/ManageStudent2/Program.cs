using System;
using System.Collections.Generic;

namespace ManageStudent2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("========================================================================================================");
            Console.WriteLine("                  CHUONG TRINH QUAN LY SINH VIEN (LOP STUDENT & LOP STUDENTDAO)                        ");
            Console.WriteLine("========================================================================================================\n");

            StudentDAO dao = new StudentDAO();

            // 1. In toàn bộ danh sách ban đầu (getAlls)
            Console.WriteLine("--- [1] DANH SACH SINH VIEN BAN DAU (getAlls) ---");
            PrintList(dao.getAlls());

            // 2. Thêm mới sinh viên (Add)
            Console.WriteLine("\n--- [2] THEM MOI SINH VIEN (Add) ---");
            Student newStudent = new Student("SV006", "Dang Van Lam", 8.0, 8.5, "lam.dang@example.com");
            bool added = dao.Add(newStudent);
            Console.WriteLine(added ? $"-> Da them thanh cong: {newStudent.Name} ({newStudent.Id})" : "-> Them that bai!");
            PrintList(dao.getAlls());

            // 3. Sửa thông tin sinh viên (Edit)
            Console.WriteLine("\n--- [3] SUA THONG TIN SINH VIEN (Edit: cap nhat diem va email cho SV003) ---");
            Student updatedStudent = new Student("SV003", "Le Van Cuong (Cap nhat)", 7.0, 7.5, "cuong.updated@example.com");
            bool edited = dao.Edit(updatedStudent);
            Console.WriteLine(edited ? "-> Da cap nhat thong tin SV003 thanh cong!" : "-> Cap nhat that bai!");
            Console.WriteLine($"   {dao.getById("SV003")}");

            // 4. Tìm kiếm sinh viên theo ID (getById)
            Console.WriteLine("\n--- [4] TIM KIEM THEO ID (getById: 'SV001') ---");
            Student? foundById = dao.getById("SV001");
            if (foundById != null)
            {
                Console.WriteLine($"-> Tim thay: {foundById}");
            }
            else
            {
                Console.WriteLine("-> Khong tim thay sinh vien voi ID da cho!");
            }

            // 5. Tìm kiếm sinh viên theo Tên (getByName)
            Console.WriteLine("\n--- [5] TIM KIEM THEO TEN (getByName: 'Van') ---");
            List<Student> foundByName = dao.getByName("Van");
            PrintList(foundByName);

            // 6. Xóa sinh viên theo ID (Delete)
            Console.WriteLine("\n--- [6] XOA SINH VIEN (Delete: 'SV005') ---");
            bool deleted = dao.Delete("SV005");
            Console.WriteLine(deleted ? "-> Da xoa thanh cong SV005!" : "-> Xoa that bai!");
            PrintList(dao.getAlls());

            // 7. Lọc danh sách sinh viên ĐẠT và RỚT
            Console.WriteLine("\n--- [7] DANH SACH SINH VIEN QUA MON (DTB >= 5.0) ---");
            PrintList(dao.getStudentsPassed(5.0));

            Console.WriteLine("\n--- [8] DANH SACH SINH VIEN ROT MON (DTB < 5.0) ---");
            var failedList = dao.getStudentsFailed(5.0);
            if (failedList.Count == 0)
            {
                Console.WriteLine("(Khong co sinh vien nao bi rot mon)");
            }
            else
            {
                PrintList(failedList);
            }

            // 9. Danh sách sinh viên được cộng 1 điểm giữa kỳ
            Console.WriteLine("\n--- [9] DANH SACH SINH VIEN SAU KHI CONG 1 DIEM GIUA KY (MidPoint + 1) ---");
            PrintList(dao.getStudentsMidPointPlus1());

            Console.WriteLine("\n========================================================================================================");
            Console.WriteLine("                                       HOAN THANH KIEM THU                                              ");
            Console.WriteLine("========================================================================================================");
        }

        static void PrintList(List<Student> list)
        {
            if (list == null || list.Count == 0)
            {
                Console.WriteLine("(Danh sach trong)");
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1,2}. {list[i]}");
            }
        }
    }
}