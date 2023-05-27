

namespace ConsoleUI.Utils
{
    public static class Utils
    {
        public static T[] clampedCopy<T>(T[] source, T[] destination, int startIdx, int amount)
        {
            int endIdx = startIdx + amount;
            int destStart = Math.Max(startIdx, 0);
            int sourceStart = Math.Max(-startIdx, 0);
            int trueEnd = Math.Min(endIdx, destination.Length);
            int trueAmount = trueEnd - destStart;

            Array.Copy(source, sourceStart, destination, destStart, trueAmount);
            return destination;
        }


    }
}