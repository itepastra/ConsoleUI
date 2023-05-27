namespace ConsoleUI
{
    public class IntVec
    {
        public int x { get; set; }
        public int y { get; set; }
        public IntVec(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static IntVec operator +(IntVec a, IntVec b) => new(a.x + b.x, a.y + b.y);
        public static IntVec operator -(IntVec a, IntVec b) => new(a.x - b.x, a.y - b.y);

        public static IntVec Zero = new(0, 0);
    }
}