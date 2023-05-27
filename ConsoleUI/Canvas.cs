
namespace ConsoleUI
{
    public class Canvas : IDisplayable
    {
        Rect bounds;
        List<IDisplayable> subDisplays;
        public event EventHandler<ContentChangeArgs>? ContentChanged;
        public event EventHandler<SizeChangeArgs>? SizeChanged;

        public Canvas(int width, int height)
        {
            bounds = new(new(width, height), IntVec.Zero);
            subDisplays = new();
        }

        public Canvas(IntVec size)
        {
            bounds = new(IntVec.Zero, size);
            subDisplays = new();
        }

        public Canvas(Rect bounds)
        {
            this.bounds = bounds;
            subDisplays = new();
        }

        public void AddSubDisplay(IDisplayable subDisplay)
        {
            subDisplay.ContentChanged += OnContentChangedEvent;
            subDisplays.Add(subDisplay);
            ContentChanged?.Invoke(this, new ContentChangeArgs(subDisplay.Bounds));
        }

        public void RemoveSubDisplay(IDisplayable subDisplay)
        {
            subDisplay.ContentChanged -= OnContentChangedEvent;
            subDisplays.Remove(subDisplay);
            ContentChanged?.Invoke(this, new ContentChangeArgs(subDisplay.Bounds));
        }

        private void OnContentChangedEvent(object? sender, ContentChangeArgs e)
        {
            if (sender == null) return;
            ContentChanged?.Invoke(sender, e);
        }

        public Rect Bounds { get => bounds; set => bounds = value; }

        public char[]? Line(int lineNum)
        {
            if (lineNum < 0 || lineNum >= bounds.h) return null;
            char[] lineBuf = Enumerable.Repeat(' ', bounds.w).ToArray();

            foreach (IDisplayable subDisplay in subDisplays)
            {
                int relLine = lineNum - subDisplay.Bounds.Location.y;
                char[]? subLine = subDisplay.Line(relLine);


                if (subLine != null)
                {
                    Utils.Utils.clampedCopy(subLine, lineBuf, subDisplay.Bounds.x, subDisplay.Bounds.w);
                    // Array.Copy(subLine, 0, lineBuf, startCopy, copyAmount);
                }
            }

            return lineBuf;
        }
    }
}