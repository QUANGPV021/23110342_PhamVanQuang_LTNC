using System;

namespace ManageStudent2
{
    public class Student
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double MidPoint { get; set; }
        public double FinalPoint { get; set; }
        public string Email { get; set; } = string.Empty;

        // Điểm trung bình = 50% giữa kỳ + 50% cuối kỳ
        public double AveragePoint => (MidPoint + FinalPoint) / 2.0;

        // Xếp loại học lực
        public string Grade
        {
            get
            {
                if (AveragePoint >= 8.5) return "Gioi";
                if (AveragePoint >= 7.0) return "Kha";
                if (AveragePoint >= 5.0) return "Trung Binh";
                return "Yeu";
            }
        }

        public Student() { }

        public Student(string id, string name, double midPoint, double finalPoint, string email)
        {
            Id = id;
            Name = name;
            MidPoint = midPoint;
            FinalPoint = finalPoint;
            Email = email;
        }

        public override string ToString()
        {
            return $"[ID: {Id,-6}] [Ho ten: {Name,-22}] [Diem GK: {MidPoint,4:F1}] [Diem CK: {FinalPoint,4:F1}] [DTB: {AveragePoint,4:F1}] [Xep loai: {Grade,-10}] [Email: {Email}]";
        }
    }
}
