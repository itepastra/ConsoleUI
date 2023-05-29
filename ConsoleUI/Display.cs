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
            this.content.SelfChanged += OnContentChangedEvent;
        }

        public void Refresh()
        {
            OnContentChangedEvent(content, new ContentChangeArgs(new Rect(new(0, 0), displayAdapter.WindowSize)));
        }

        private void OnContentChangedEvent(object? sender, ContentChangeArgs e)
        {
            if (sender == null) return;
            IDisplayable d = (IDisplayable)sender;
            Rect updateRect = Rect.Clamp(e.bounds, new(IntVec.Zero, displayAdapter.WindowSize));

            for (int i = 0; i < updateRect.H; i++)
            {
                int ypos = i + updateRect.Y;
                int xpos = updateRect.X;

                CString lineBuffer = new CString(displayAdapter.WindowSize.X);
                CString? line = d.Line(ypos);
                if (line != null) CString.Copy(line, 0, lineBuffer, d.Bounds.X, d.Bounds.W);
                string toWrite = lineBuffer.ToWriteChars(updateRect.X, updateRect.W);
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