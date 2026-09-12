namespace ShapeCalculator.Entities
{
    public class Circle : Hinh
    {
        private double radius;

        public string Name => "Circle";

        public double Radius
        {
            get => radius;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Ban kinh phai lon hon 0.");
                }

                radius = value;
            }
        }

        public Circle() : this(1)
        {
        }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public double GetDienTich()
        {
            return Math.PI * Radius * Radius;
        }

        public double GetChuVi()
        {
            return 2 * Math.PI * Radius;
        }

        public void Nhap()
        {
            Console.Write("Nhap ban kinh: ");
            Radius = ReadPositiveNumber();
        }

        public void HienThi()
        {
            Console.WriteLine($"{Name}: Dien tich = {GetDienTich():F2}, Chu vi = {GetChuVi():F2}");
        }

        private static double ReadPositiveNumber()
        {
            while (!double.TryParse(Console.ReadLine(), out double value) || value <= 0)
            {
                Console.Write("Gia tri phai lon hon 0, nhap lai: ");
            }

            return value;
        }
    }
}