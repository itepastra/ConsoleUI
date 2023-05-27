namespace ConsoleUI
{    public class Border : IDisplayable
    {
        readonly Rect bounds;
        readonly IDisplayable subDisplay;
        public Rect Bounds { get => bounds; }

        // horizontal, vertical, left top, right top, left bottom, right bottom
        private char[] borders;
        public char[] Borders { set => borders = value; }

        public static readonly char[] simpleBorders = { '\u2500', '\u2502', '\u250C', '\u2510', '\u2514', '\u2518' };

        public event EventHandler<ContentChangeArgs>? ContentChanged;
        public event EventHandler<SizeChangeArgs>? SizeChanged;

        public Border(Rect rect, IDisplayable sub)
        {
            bounds = rect;
            borders = simpleBorders;
            subDisplay = sub;
            subDisplay.Bounds.SetSize(Rect.ShrinkCentered(rect, 1));
            subDisplay.ContentChanged += OnContentChangedEvent;

            bounds.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object? sender, SizeChangeArgs e)
        {
            Rect largeRect = Rect.Union(e.oldSize, e.newSize);
            subDisplay.Bounds.SetSize(Rect.ShrinkCentered(e.newSize, 1));
            SizeChanged?.Invoke(this, e);
        }

        public Border(Rect rect, IDisplayable sub, char[] borders)
        {
            bounds = rect;
            subDisplay = sub;
            subDisplay.Bounds.SetSize(Rect.ShrinkCentered(rect, 1));
            this.borders = borders;
            subDisplay.ContentChanged += OnContentChangedEvent;
        }

        private void OnContentChangedEvent(object? sender, ContentChangeArgs e)
        {
            if (sender == null) return;
            ContentChanged?.Invoke(sender, e);
        }


        public char[]? Line(int lineNum)
        {
            if (lineNum < 0 || lineNum >= bounds.H) return null;
            char[] lineBuf = new Char[bounds.W];

            if (lineNum == 0)
            {
                lineBuf[0] = borders[2];
                lineBuf[bounds.W - 1] = borders[3];
                for (int i = 1; i < bounds.W - 1; i++)
                {
                    lineBuf[i] = borders[0];
                }
            }
            else if (lineNum == bounds.H - 1)
            {

                lineBuf[0] = borders[4];
                lineBuf[bounds.W - 1] = borders[5];
                for (int i = 1; i < bounds.W - 1; i++)
                {
                    lineBuf[i] = borders[0];
                }
            }
            else
            {
                lineBuf[0] = borders[1];
                lineBuf[bounds.W - 1] = borders[1];
                char[]? subLine = subDisplay.Line(lineNum - 1);
                if (subLine != null) Array.Copy(subLine, 0, lineBuf, 1, subDisplay.Bounds.W);
            }

            return lineBuf;
        }
    }
}