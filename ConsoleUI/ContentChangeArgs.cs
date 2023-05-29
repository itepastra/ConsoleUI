namespace ConsoleUI
{
    public class ContentChangeArgs
    {
        public Rect bounds { get; }

        /// <summary>
        /// Event for a change in an IDisplayable
        /// </summary>
        /// <param name="rect">The bounding box of the change. can be the union of the old rect and new rect if moved</param>
        public ContentChangeArgs(Rect rect)
        {
            this.bounds = rect;
        }
    }
}