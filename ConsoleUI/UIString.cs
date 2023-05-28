namespace ConsoleUI
{
    public class UIString : IDisplayable
    {

        private readonly Rect bounds;

        #region AutoHeight variables
        private readonly bool autoHeight;
        int usedLines;
        #endregion


        CString displayString;
        CString[] alignedString;
        IAlignment alignment;

        public IAlignment Alignment { get => alignment; set { alignment = value; OnContentChanged(bounds); } }
        public Rect Bounds { get => bounds; }
        public event EventHandler<SizeChangeArgs>? SizeChanged;
        public event EventHandler<ContentChangeArgs>? ContentChanged;

        public UIString(string str, Rect rect, bool autoHeight = false)
        {
            alignment = new LeftAlignment();
            this.autoHeight = autoHeight;
            bounds = rect;
            displayString = new CString(str);
            alignedString = new CString[Bounds.H];
            ChangeString(displayString);

            Bounds.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object? sender, SizeChangeArgs e)
        {
            Rect largeRect = Rect.Union(e.oldSize, e.newSize);
            int oldLines = usedLines;
            UpdateAligned();
            if (autoHeight && usedLines != oldLines) Bounds.Size -= new IntVec(0, 1);
            SizeChanged?.Invoke(this, e);
        }

        public void ChangeString(CString newString)
        {
            displayString = newString;

            UpdateAligned();
            Bounds.Size -= new IntVec(0, 1);
            OnContentChanged(Bounds);
        }

        private void UpdateAligned()
        {
            alignedString = alignment.AlignString(displayString, Bounds.W, Bounds.H, out usedLines);
        }

        private void OnContentChanged(Rect contentRect)
        {
            ContentChanged?.Invoke(this, new ContentChangeArgs(contentRect));
        }

        public CString? Line(int lineNum)
        {
            if (lineNum < 0 || lineNum >= Bounds.H) return null;
            return alignedString[lineNum];
        }


    }
}