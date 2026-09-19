using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageStudent2
{
    public class StudentDAO
    {
        private List<Student> students;

        public StudentDAO()
        {
            // Khởi tạo danh sách sinh viên mẫu ban đầu
            students = new List<Student>
            {
                new Student("SV001", "Pham Van Quang", 8.5, 9.0, "quang.pham@example.com"),
                new Student("SV002", "Tran Thi Bich", 7.0, 8.0, "bich.tran@example.com"),
                new Student("SV003", "Le Van Cuong", 4.5, 4.5, "cuong.le@example.com"),
                new Student("SV004", "Nguyen Van An", 9.0, 9.5, "an.nguyen@example.com"),
                new Student("SV005", "Hoang Thi Mai", 5.0, 4.0, "mai.hoang@example.com")
            };
        }

        // 1. Thêm sinh viên mới (Add)
        public bool Add(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Id))
            {
                return false;
            }

            // Kiểm tra trùng Id
            if (students.Any(s => s.Id.Equals(student.Id, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            students.Add(student);
            return true;
        }

        // 2. Sửa thông tin sinh viên theo Id của đối tượng truyền vào (Edit)
        public bool Edit(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Id))
            {
                return false;
            }

            var existing = students.FirstOrDefault(s => s.Id.Equals(student.Id, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.Name = student.Name;
                existing.MidPoint = student.MidPoint;
                existing.FinalPoint = student.FinalPoint;
                existing.Email = student.Email;
                return true;
            }

            return false;
        }

        // 3. Xóa sinh viên theo mã Id (Delete)
        public bool Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            int removedCount = students.RemoveAll(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            return removedCount > 0;
        }

        // 4. Lấy danh sách tất cả sinh viên (getAlls)
        public List<Student> getAlls()
        {
            return new List<Student>(students);
        }

        // 5. Lấy thông tin sinh viên theo Id (getById)
        public Student? getById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            return students.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        // 6. Tìm kiếm sinh viên theo tên (getByName - tìm kiếm gần đúng, không phân biệt hoa thường)
        public List<Student> getByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<Student>();
            }

            return students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 7. Lấy danh sách sinh viên đạt (qua môn: DTB >= passCondition)
        public List<Student> getStudentsPassed(double passCondition = 5.0)
        {
            return students.Where(s => s.AveragePoint >= passCondition).ToList();
        }

        // 8. Lấy danh sách sinh viên không đạt (rớt môn: DTB < passCondition)
        public List<Student> getStudentsFailed(double passCondition = 5.0)
        {
            return students.Where(s => s.AveragePoint < passCondition).ToList();
        }

        // 9. Lấy về danh sách sinh viên mà mỗi sinh viên có điểm giữa kỳ + 1 (tối đa 10)
        public List<Student> getStudentsMidPointPlus1()
        {
            return students.Select(s => new Student
            {
                Id = s.Id,
                Name = s.Name,
                MidPoint = Math.Min(10.0, s.MidPoint + 1.0),
                FinalPoint = s.FinalPoint,
                Email = s.Email
            }).ToList();
        }
    }
}
