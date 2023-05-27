namespace ConsoleUI {
    public class ConsoleDisplayAdapter : IDisplayAdapter
    {
        public IntVec WindowSize => new(Console.WindowWidth, Console.WindowHeight);

        public bool MoveTo(IntVec location)
        {
            if (location.X < 0 || location.Y < 0 || location.X > WindowSize.X || location.Y > WindowSize.Y) return false;
            Console.SetCursorPosition(location.X, location.Y);
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