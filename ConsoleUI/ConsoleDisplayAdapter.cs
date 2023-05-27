namespace ConsoleUI {
    public class ConsoleDisplayAdapter : IDisplayAdapter
    {
        public IntVec windowSize => new(Console.WindowWidth, Console.WindowHeight);

        public bool MoveTo(IntVec location)
        {
            if (location.x < 0 || location.y < 0 || location.x > windowSize.x || location.y > windowSize.y) return false;
            Console.SetCursorPosition(location.x, location.y);
            return true;
        }

        public bool WriteAt(IntVec location, string str)
        {
            if (!MoveTo(location)) return false;
            Console.Write(str); 
            return true;
        }
    }
}