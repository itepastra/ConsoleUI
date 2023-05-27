namespace ConsoleUI
{


    public class Display
    {

        readonly IDisplayAdapter displayAdapter;
        public event EventHandler<ContentChangeArgs>? ContentChanged;

        readonly IDisplayable content;
        public Display(IDisplayAdapter displayAdapter, IDisplayable content)
        {
            this.displayAdapter = displayAdapter;
            this.content = content;
            this.content.ContentChanged += OnContentChangedEvent;
        }

        public void Refresh()
        {
            OnContentChangedEvent(content, new ContentChangeArgs(new Rect(new(0, 0), displayAdapter.WindowSize)));
        }

        private void OnContentChangedEvent(object? sender, ContentChangeArgs e)
        {
            if (sender == null) return;
            IDisplayable d = (IDisplayable)sender;
            Rect updateRect = e.bounds;

            for (int i = 0; i < updateRect.H; i++)
            {
                int ypos = i + updateRect.Y;
                int xpos = updateRect.X;

                char[] lineBuffer = Enumerable.Repeat(' ', displayAdapter.WindowSize.X).ToArray();
                char[]? line = d.Line(ypos);
                if (line != null) Array.Copy(line, 0, lineBuffer, d.Bounds.X, d.Bounds.W);
                string toWrite = new(new ReadOnlySpan<char>(lineBuffer, updateRect.X, updateRect.W));
                displayAdapter.WriteAt(new(xpos, ypos), toWrite);
            }

            displayAdapter.MoveTo(IntVec.Zero);
        }

    }

    public interface IDisplayAdapter
    {
        IntVec WindowSize { get; }

        bool MoveTo(IntVec location);
        bool WriteAt(IntVec location, string str);
    }
}