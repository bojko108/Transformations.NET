using BojkoSoft.Transformations;

namespace Tests
{
    internal class TestExtent : IExtent
    {
        public double MinN { get; set; }
        public double MinE { get; set; }
        public double MaxN { get; set; }
        public double MaxE { get; set; }
        public double Width => this.MaxE - this.MinE;
        public double Height => this.MaxN - this.MinN;
        public bool IsEmpty => this.Width <= 0 && this.Height <= 0;

        public TestExtent(double northingMax, double northingMin, double eastingMax, double eastingMin)
        {
            this.MaxN = northingMax;
            this.MinN = northingMin;
            this.MaxE = eastingMax;
            this.MinE = eastingMin;
        }

        public void Expand(double meters)
        {
            this.MaxN += meters;
            this.MaxE += meters;
            this.MinN -= meters;
            this.MinE -= meters;
        }

        public IExtent Clone()
            => new TestExtent(this.MaxN, this.MinN, this.MaxE, this.MinE);
    }
}
