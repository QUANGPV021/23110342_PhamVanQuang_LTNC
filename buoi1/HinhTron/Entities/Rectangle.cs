namespace ShapeCalculator.Entities
{
    public class Rectangle : Hinh
    {
        private double width;
        private double height;

        public string Name => "Rectangle";

        public double Width
        {
            get => width;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Chieu rong phai lon hon 0.");
                }

                width = value;
            }
        }

        public double Height
        {
            get => height;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Chieu dai phai lon hon 0.");
                }

                height = value;
            }
        }

        public Rectangle() : this(1, 1)
        {
        }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double GetDienTich()
        {
            return Width * Height;
        }

        public double GetChuVi()
        {
            return 2 * (Width + Height);
        }

        public void Nhap()
        {
            Console.Write("Nhap chieu rong: ");
            Width = ReadPositiveNumber();
            Console.Write("Nhap chieu dai: ");
            Height = ReadPositiveNumber();
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