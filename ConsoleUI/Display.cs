namespace ConsoleUI
{


    public class Display
    {

        IDisplayAdapter displayAdapter;
        public event EventHandler<ContentChangeArgs>? ContentChanged;

        IDisplayable content;
        public Display(IDisplayAdapter displayAdapter, IDisplayable content)
        {
            this.displayAdapter = displayAdapter;
            this.content = content;
            this.content.ContentChanged += OnContentChangedEvent;
        }

        public void Refresh()
        {
            OnContentChangedEvent(content, new ContentChangeArgs(new Rect(new(0, 0), displayAdapter.windowSize)));
        }

        private void OnContentChangedEvent(object? sender, ContentChangeArgs e)
        {
            if (sender == null) return;
            IDisplayable d = (IDisplayable)sender;
            Rect updateRect = e.bounds;

            for (int i = 0; i < updateRect.h; i++)
            {
                int ypos = i + updateRect.y;
                int xpos = updateRect.x;

                char[] lineBuffer = Enumerable.Repeat(' ', displayAdapter.windowSize.x).ToArray();
                char[]? line = d.Line(ypos);
                if (line != null) Array.Copy(line, 0, lineBuffer, d.Bounds.x, d.Bounds.w);
                string toWrite = new(new ReadOnlySpan<char>(lineBuffer, updateRect.x, updateRect.w));
                displayAdapter.WriteAt(new(xpos, ypos), toWrite);
            }

            displayAdapter.MoveTo(IntVec.Zero);
        }

    }

    public interface IDisplayAdapter
    {
        IntVec windowSize { get; }

        bool MoveTo(IntVec zero);
        bool WriteAt(IntVec location, string str);
    }
}