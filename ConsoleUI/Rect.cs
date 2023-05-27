namespace ConsoleUI
{


    public class Rect
    {
        private IntVec location { get; set; }
        private IntVec size { get; set; }

        public IntVec Location { get => location; set => LocationChange(value); }
        public IntVec Size { get => size; set => SizeChange(value); }

        public int x { get => location.x; }
        public int y { get => location.y; }
        public int w { get => size.x; }
        public int h { get => size.y; }

        public int Left { get => location.x; }
        public int Top { get => location.y; }
        public int Right { get => location.x + w; }
        public int Bottom { get => location.y + h; }

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
            IntVec oldLoc = location;
            IntVec oldSize = size;
            this.location = location + new IntVec(dx, dy);
            this.size = size + new IntVec(dw, dh);
            SizeChanged?.Invoke(this, new(new(oldLoc, oldSize), new(location, size)));
        }

        public Rect()
        {
            location = IntVec.Zero;
            size = IntVec.Zero;
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
            this.location.x = x; this.location.y = y;
            this.size.x = w; this.size.y = h;
            SizeChanged?.Invoke(this, new(old, this));
        }
        public void SetSize(Rect r)
        {
            Rect old = new(location, size);
            this.location.x = r.x; this.location.y = r.y;
            this.size.x = r.w; this.size.y = r.h;
            SizeChanged?.Invoke(this, new(new(location, size), r));
        }

        public static bool AABB(Rect rect, IntVec location)
        {
            return
                location.x < rect.Location.x + rect.w &&
                location.x > rect.Location.x &&
                location.y < rect.Location.y + rect.h &&
                location.y > rect.Location.y;
        }

        public static bool AABB(Rect rect1, Rect rect2)
        {
            return
                rect1.x < rect2.x + rect2.w &&
                rect1.x + rect1.w > rect2.x &&
                rect1.y < rect2.y + rect2.h &&
                rect1.h + rect1.y > rect2.y;
        }

        public static Rect Clamp(Rect r1, Rect r2)
        {
            int x = r1.x;
            int y = r1.y;
            int w = r1.w;
            int h = r1.h;
            if (r1.Left < r2.Left) x = r2.Left;
            if (r1.Top < r2.Top) x = r2.Top;
            if (r1.Right > r2.Right) x = r2.Right;
            if (r1.Bottom > r2.Bottom) x = r2.Bottom;

            return new(x,y,w,h);
        }

        public static Rect ShrinkCentered(Rect rect, int amount)
        {
            IntVec newLoc = new(rect.Location.x + amount, rect.Location.y + amount);
            IntVec newSize = new(rect.w - 2 * amount, rect.h - 2 * amount);
            if (newSize.x < 1 || newSize.y < 1) throw new ArgumentOutOfRangeException("rect could not be shrinked by amount");
            return new(newLoc, newSize);
        }

        public static Rect Union(Rect r1, Rect r2)
        {
            IntVec leftTop = new(Math.Min(r1.Location.x, r2.Location.x), Math.Min(r1.Location.y, r2.Location.y));
            IntVec rightBot = new(Math.Max(r1.Location.x + r1.w, r2.Location.x + r2.w), Math.Max(r1.Location.y + r1.h, r2.Location.y + r2.h));
            IntVec newSize = rightBot - leftTop;
            return new(leftTop, newSize);
        }
    }

}