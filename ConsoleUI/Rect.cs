namespace ConsoleUI
{


    public class Rect
    {
        private IntVec location;
        private IntVec size;

        public IntVec Location { get => location; set => LocationChange(value); }
        public IntVec Size { get => size; set => SizeChange(value); }

        public int X { get => Location.X; }
        public int Y { get => Location.Y; }
        public int W { get => Size.X; }
        public int H { get => Size.Y; }

        public int Left { get => Location.X; }
        public int Top { get => Location.Y; }
        public int Right { get => Location.X + W; }
        public int Bottom { get => location.Y + H; }

        public event EventHandler<SizeChangeArgs>? SizeChanged;

        public Rect(int x, int y, int w, int h)
        {
            this.location = new(x, y);
            this.size = new(w, h);
        }

        public Rect(IntVec location, IntVec size)
        {
            this.location = location;
            this.size = size;
        }

        public void MoveRect( int dx, int dy, int dw, int dh)
        {
            IntVec oldLoc = Location.Copy();
            IntVec oldSize = Size.Copy();
            this.location = Location + new IntVec(dx, dy);
            this.size = Size + new IntVec(dw, dh);
            SizeChanged?.Invoke(this, new(new(oldLoc, oldSize), this));
        }

        public Rect()
        {
            location = IntVec.Zero;
            size = IntVec.Zero;
        }

        public override string ToString()
        {
            return $"loc: {Location}, size: {Size}";
        }

        private void LocationChange(IntVec newLoc)
        {
            SizeChanged?.Invoke(this, new(new(location, size), new(newLoc, size)));
            this.location = newLoc;
        }

        private void SizeChange(IntVec newSize)
        {
            SizeChanged?.Invoke(this, new(new(location, size), new(location, newSize)));
            this.size = newSize;
        }

        public void SetSize(int x, int y, int w, int h)
        {
            Rect old = new(location, size);
            this.location.X = x; this.location.Y = y;
            this.size.X = w; this.size.Y = h;
            SizeChanged?.Invoke(this, new(old, this));
        }
        public void SetSize(Rect r)
        {
            Rect old = new(location, size);
            this.location.X = r.X; this.location.Y = r.Y;
            this.size.X = r.W; this.size.Y = r.H;
            SizeChanged?.Invoke(this, new(new(location, size), r));
        }

        public static bool AABB(Rect rect, IntVec location)
        {
            return
                location.X < rect.Location.X + rect.W &&
                location.X > rect.Location.X &&
                location.Y < rect.Location.Y + rect.H &&
                location.Y > rect.Location.Y;
        }

        public static bool AABB(Rect rect1, Rect rect2)
        {
            return
                rect1.X < rect2.X + rect2.W &&
                rect1.X + rect1.W > rect2.X &&
                rect1.Y < rect2.Y + rect2.H &&
                rect1.H + rect1.Y > rect2.Y;
        }

        public static Rect Clamp(Rect r1, Rect r2)
        {
            int x = r1.X;
            int y = r1.Y;
            int w = r1.W;
            int h = r1.H;
            if (r1.Left < r2.Left) x = r2.Left;
            if (r1.Top < r2.Top) y = r2.Top;
            if (r1.Right > r2.Right) w = r2.Right - x;
            if (r1.Bottom > r2.Bottom) h = r2.Bottom - y;

            return new(x,y,w,h);
        }

        public static Rect? ShrinkCentered(Rect rect, int amount)
        {
            IntVec newLoc = new(rect.Location.X + amount, rect.Location.Y + amount);
            IntVec newSize = new(rect.W - 2 * amount, rect.H - 2 * amount);
            if (newSize.X < 1 || newSize.Y < 1) return null;
            return new(newLoc, newSize);
        }

        public static Rect Union(Rect r1, Rect r2)
        {
            IntVec leftTop = new(Math.Min(r1.Left, r2.Left), Math.Min(r1.Top, r2.Top));
            IntVec rightBot = new(Math.Max(r1.Right, r2.Right), Math.Max(r1.Bottom, r2.Bottom));
            IntVec newSize = rightBot - leftTop;
            return new(leftTop, newSize);
        }
    }

}