using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagement.Models;

namespace StudentManagement.DAO
{
    public class StudentDAO
    {
        private List<Student> students = new List<Student>();

        public StudentDAO()
        {
            students = new List<Student>
            {
                new Student("SV001", "Pham Van Quang", 8.5, 9.0),
                new Student("SV002", "Tran Thi Bich", 7.0, 8.0),
                new Student("SV003", "Le Van Cuong", 4.5, 4.5),
                new Student("SV004", "Nguyen Van An", 9.0, 9.5)
            };
        }

        // Add(Student)
        public bool Add(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Id)) return false;
            if (students.Any(s => s.Id.Equals(student.Id, StringComparison.OrdinalIgnoreCase))) return false;

            students.Add(student);
            return true;
        }

        // Edit(Student)
        public bool Edit(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Id)) return false;
            var item = students.FirstOrDefault(s => s.Id.Equals(student.Id, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Name = student.Name;
                item.Midpoint = student.Midpoint;
                item.Potalpoint = student.Potalpoint;
                return true;
            }
            return false;
        }

        // Delete(string id)
        public bool Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return students.RemoveAll(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) > 0;
        }

        // getAlls
        public List<Student> getAlls()
        {
            return new List<Student>(students);
        }

        // getById
        public Student? getById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return students.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        // getByName
        public List<Student> getByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return new List<Student>();
            return students.Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
