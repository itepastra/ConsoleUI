namespace ConsoleUI
{
    public interface IDisplayable
    {
        public Rect Bounds { get;}
        public char[]? Line(int lineNum);

        public event EventHandler<ContentChangeArgs> ContentChanged;
        public event EventHandler<SizeChangeArgs> SizeChanged;

    }
}