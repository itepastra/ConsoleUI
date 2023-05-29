using System.Drawing;

namespace ConsoleUI
{
    public class Border : IDisplayable
    {
        readonly Rect bounds;
        readonly IDisplayable subDisplay;
        public Rect Bounds { get => bounds; }

        // horizontal, vertical, left top, right top, left bottom, right bottom
        private CChar[] borders;
        public CChar[] Borders { set => borders = value; }

        public static readonly CChar[] simpleBorders = { new CChar('\u2500', Color.Purple, null), new CChar('\u2502', Color.Yellow, null), new CChar('\u250C', Color.Green, null), new CChar('\u2510', Color.AliceBlue, Color.Wheat), new CChar('\u2514', Color.Aqua, Color.Black), new CChar('\u2518', Color.Red, Color.Black) };

        public event EventHandler<ContentChangeArgs>? ContentChanged;
        public event EventHandler<SizeChangeArgs>? SizeChanged;

        public Border(Rect rect, IDisplayable sub)
        {
            bounds = rect;
            borders = simpleBorders;
            subDisplay = sub;
            subDisplay.Bounds.SetSize(BorderShrink(rect));
            subDisplay.ContentChanged += OnContentChangedEvent;

            bounds.SizeChanged += OnSizeChanged;
        }

        public static Rect BorderShrink(Rect rect)
        {
            return Rect.ShrinkCentered(rect, 1);
        }

        private void OnSizeChanged(object? sender, SizeChangeArgs e)
        {
            Rect largeRect = Rect.Union(e.oldSize, e.newSize);
            subDisplay.Bounds.SetSize(BorderShrink(e.newSize));
            SizeChanged?.Invoke(this, e);
        }

        public Border(Rect rect, IDisplayable sub, CChar[] borders)
        {
            bounds = rect;
            subDisplay = sub;
            subDisplay.Bounds.SetSize(BorderShrink(rect));
            this.borders = borders;
            subDisplay.ContentChanged += OnContentChangedEvent;
        }

        private void OnContentChangedEvent(object? sender, ContentChangeArgs e)
        {
            if (sender == null) return;
            ContentChanged?.Invoke(sender, e);
        }


        public CString? Line(int lineNum)
        {
            if (lineNum < 0 || lineNum >= bounds.H) return null;
            CString lineBuf = new(bounds.W);

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
                CString? subLine = subDisplay.Line(lineNum - 1);
                if (subLine != null) CString.Copy(subLine, 0, lineBuf, 1, subDisplay.Bounds.W);
            }

            return lineBuf;
        }
    }
}