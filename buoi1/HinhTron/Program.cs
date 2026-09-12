using ShapeCalculator.Entities;

List<Hinh> shapes = new List<Hinh>
{
    new Circle(5),
    new Rectangle(4, 6),
    new Triangle(3, 4, 5)
};

foreach (var shape in shapes)
{
    shape.HienThi();
}