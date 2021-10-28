/// <summary>
/// Copied from https://github.com/marchello2000/kdbush
/// </summary>
namespace KDBush
{
    internal class Point<T>
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public T UserData { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(double x, double y, T userData)
        {
            X = x;
            Y = y;
            UserData = userData;
        }
    }
}