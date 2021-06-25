using BojkoSoft.Transformations;

namespace Tests
{
    internal class TestPoint : IPoint
    {
        public double N { get; set; }
        public double E { get; set; }
        public double Z { get; set; }

        public TestPoint(double x, double y)
            : this(x, y, 0.0)
        { }

        public TestPoint(double x, double y, double z)
        {
            this.N = x;
            this.E = y;
            this.Z = z;
        }

        public IPoint Clone()
        {
            return new TestPoint(this.N, this.E, this.Z);
        }
    }
}
