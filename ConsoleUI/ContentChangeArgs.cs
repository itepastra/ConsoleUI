namespace ConsoleUI
{
    public class ContentChangeArgs
    {
        public Rect bounds { get; }

        public ContentChangeArgs(Rect rect)
        {
            this.bounds = rect;
        }
    }
}