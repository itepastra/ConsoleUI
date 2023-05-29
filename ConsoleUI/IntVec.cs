namespace ConsoleUI
{
    public class IntVec
    {
        private int x;
        private int y;
        public IntVec(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public static IntVec operator +(IntVec a, IntVec b) => new(a.X + b.X, a.Y + b.Y);
        public static IntVec operator -(IntVec a, IntVec b) => new(a.X - b.X, a.Y - b.Y);

        public static readonly IntVec Zero = new(0,0);

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        internal IntVec Copy()
        {
            return new IntVec(X, Y);
        }
    }
}