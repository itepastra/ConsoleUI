namespace ConsoleUI
{
    public class UIString : IDisplayable
    {
        public enum AlignmentEnum
        {
            Left, Right, Middle
        }

        Rect bounds;

        bool autoHeight;
        int usedLines;


        string displayString;
        string[] alignedString;
        AlignmentEnum alignment;

        public AlignmentEnum Alignment { get => alignment; set { alignment = value; OnContentChanged(bounds); } }
        Rect IDisplayable.Bounds { get => bounds; }
        public event EventHandler<SizeChangeArgs>? SizeChanged;
        public event EventHandler<ContentChangeArgs>? ContentChanged;

        public UIString(string str, Rect rect, bool autoHeight = false)
        {
            this.autoHeight = autoHeight;
            bounds = rect;
            displayString = str;
            alignedString = new string[bounds.h];
            ChangeString(str);

            bounds.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object? sender, SizeChangeArgs e)
        {
            Rect largeRect = Rect.Union(e.oldSize, e.newSize);
            UpdateAligned();
            SizeChanged?.Invoke(this, e);
        }

        public void ChangeString(string newString)
        {
            displayString = newString;
            
            UpdateAligned();
            bounds.Size -= new IntVec(0, 1);
            OnContentChanged(bounds);
        }

        private void UpdateAligned()
        {
            alignedString = alignment switch
            {
                AlignmentEnum.Left => LeftLine(displayString),
                AlignmentEnum.Middle => MiddleLine(displayString),
                AlignmentEnum.Right => RightLine(displayString),
                _ => RightLine(displayString)
            };
        }

        private void OnContentChanged(Rect contentRect)
        {
            ContentChanged?.Invoke(this, new ContentChangeArgs(contentRect));
        }

        public char[]? Line(int lineNum)
        {
            if (lineNum < 0 || lineNum >= bounds.h) return null;
            return alignedString[lineNum].ToCharArray();
        }

        private string[] LeftLine(string displayString)
        {
            string[] splitString = displayString.Split(' ');
            string[] output = Enumerable.Repeat("".PadRight(bounds.w), bounds.h).ToArray();
            int currentLineLength = -1;
            int currentLine = 0;
            List<string> lineWords = new();
            foreach (string word in splitString)
            {
                if (word.Length > bounds.w)
                {
                    throw new NotImplementedException("words larger than the box are not yet supported");
                }
                if (currentLineLength + word.Length >= bounds.w)
                {
                    output[currentLine] = string.Join(" ", lineWords.ToArray()).PadRight(bounds.w);
                    lineWords = new();
                    currentLine++;
                    if (currentLine == bounds.h)
                    {
                        throw new NotImplementedException("String must fit in the box for now");
                    }
                    currentLineLength = -1;
                }
                lineWords.Add(word);
                currentLineLength += word.Length + 1;
            }
            output[currentLine] = string.Join(" ", lineWords.ToArray()).PadRight(bounds.w);

            usedLines = currentLine + 1;
            return output;
        }

        private string[] MiddleLine(string displayString)
        {
            usedLines = 1;

            throw new NotImplementedException();
        }

        private string[] RightLine(string displayString)
        {
            string[] splitString = displayString.Split(' ');
            string[] output = Enumerable.Repeat("".PadLeft(bounds.w), bounds.w).ToArray();

            // string[] output = new string[bounds.h];
            int currentLineLength = 0;
            int currentLine = 0;
            List<string> lineWords = new();
            foreach (string word in splitString)
            {
                if (word.Length > bounds.w)
                {
                    throw new NotImplementedException("words larger than the box are not yet supported");
                }
                if (currentLineLength + word.Length > bounds.w)
                {
                    output[currentLine] = string.Join(" ", lineWords.ToArray()).PadLeft(bounds.w);
                    lineWords = new();
                    currentLine++;
                    if (currentLine == bounds.h)
                    {
                        throw new NotImplementedException("String must fit in the box for now");
                    }
                    currentLineLength = word.Length;
                }
                lineWords.Add(word);
            }
            output[currentLine] = string.Join(" ", lineWords.ToArray()).PadLeft(bounds.w);

            usedLines = currentLine + 1;

            return output;
        }

    }
}