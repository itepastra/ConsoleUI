namespace ConsoleUI
{
    public interface IDisplayable
    {
        public Rect Bounds { get;}
        public CString? Line(int lineNum);

        public event EventHandler<ContentChangeArgs> ContentChanged;
        public event EventHandler<ContentChangeArgs> SelfChanged;

    }
}