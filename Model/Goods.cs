namespace CalculatorService.Model
{
    public class Goods
    {
        public float Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Goods(float weight, int length, int width, int height)
        {
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;
        }
    }
}
