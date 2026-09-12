namespace ShapeCalculator.Entities
{
    public class Triangle : Hinh
    {
        private double sideA;
        private double sideB;
        private double sideC;

        public string Name => "Triangle";

        public double SideA
        {
            get => sideA;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Canh tam giac phai lon hon 0.");
                }

                sideA = value;
            }
        }

        public double SideB
        {
            get => sideB;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Canh tam giac phai lon hon 0.");
                }

                sideB = value;
            }
        }

        public double SideC
        {
            get => sideC;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Canh tam giac phai lon hon 0.");
                }

                sideC = value;
            }
        }

        public Triangle() : this(1, 1, 1)
        {
        }

        public Triangle(double sideA, double sideB, double sideC)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;

            if (!IsTamGiac())
            {
                throw new ArgumentException("Ba canh khong tao thanh tam giac.");
            }
        }

        public bool IsTamGiac()
        {
            return SideA + SideB > SideC
                && SideA + SideC > SideB
                && SideB + SideC > SideA;
        }

        public double GetChuVi()
        {
            return SideA + SideB + SideC;
        }

        public double GetDienTich()
        {
            double halfPerimeter = GetChuVi() / 2;
            return Math.Sqrt(halfPerimeter * (halfPerimeter - SideA)
                * (halfPerimeter - SideB) * (halfPerimeter - SideC));
        }

        public void Nhap()
        {
            do
            {
                Console.Write("Nhap canh A: ");
                SideA = ReadPositiveNumber();
                Console.Write("Nhap canh B: ");
                SideB = ReadPositiveNumber();
                Console.Write("Nhap canh C: ");
                SideC = ReadPositiveNumber();

                if (!IsTamGiac())
                {
                    Console.WriteLine("Ba canh khong tao thanh tam giac, vui long nhap lai.");
                }
            }
            while (!IsTamGiac());
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