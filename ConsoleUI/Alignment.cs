using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IAlignment
    {
        CString[] AlignString(CString str, IntVec maxSize, out int usedLines) { return AlignString(str, maxSize.X, maxSize.Y, out usedLines); }
        CString[] AlignString(CString str, int lineWidth, int padToAmount, out int usedLines);
    }

    public class LeftAlignment : IAlignment
    {
        static string patternToFilter = @"\u001b\[\d+;\d+;\d+;\d+;\d+m";

        public CString[] AlignString(CString str, int lineWidth, int padToAmount, out int usedLines)
        {
            CString[] splitString = str.Split(' ');
            CString[] output = Enumerable.Repeat(new CString(lineWidth), padToAmount).ToArray();
            int currentLineLength = -1;
            int currentLine = 0;
            List<CString> lineWords = new();
            foreach (CString word in splitString)
            {
                if (word.Length > lineWidth)
                {
                    throw new NotImplementedException("words larger than the box are not yet supported");
                }
                if (currentLineLength + word.Length >= lineWidth)
                {
                    output[currentLine] = CString.Join(new(" "), lineWords.ToArray()).PadRight(lineWidth);
                    lineWords = new();
                    currentLine++;
                    if (currentLine == padToAmount)
                    {
                        usedLines = padToAmount;
                        return output; // box is full, we ignore the rest of the string
                    }
                    currentLineLength = -1;
                }
                lineWords.Add(word);
                currentLineLength += word.Length + 1;
            }
            output[currentLine] = CString.Join(new(" "), lineWords.ToArray()).PadRight(lineWidth);

            usedLines = currentLine + 1;
            return output;
        }
    }
    public class RightAlignment : IAlignment
    {
        public CString[] AlignString(CString str, int lineWidth, int padToAmount, out int usedLines)
        {
            CString[] splitString = str.Split(' ');
            CString[] output = Enumerable.Repeat(new CString(lineWidth), padToAmount).ToArray();
            int currentLineLength = -1;
            int currentLine = 0;
            List<CString> lineWords = new();
            foreach (CString word in splitString)
            {
                if (word.Length > lineWidth)
                {
                    throw new NotImplementedException("words larger than the box are not yet supported");
                }
                if (currentLineLength + word.Length >= lineWidth)
                {
                    output[currentLine] = CString.Join(new(" "), lineWords.ToArray()).PadLeft(lineWidth);
                    lineWords = new();
                    currentLine++;
                    if (currentLine == padToAmount)
                    {
                        break; // box is full, we ignore the rest of the string
                    }
                    currentLineLength = -1;
                }
                lineWords.Add(word);
                currentLineLength += word.Length + 1;
            }
            output[currentLine] = CString.Join(new(" "), lineWords.ToArray()).PadLeft(lineWidth);

            usedLines = currentLine + 1;
            return output;
        }
    }

    // private string[] MiddleLine(string displayString)
    // {
    //     usedLines = 1;

    //     throw new NotImplementedException();
    // }

    // private string[] RightLine(string displayString)
    // {
    //     string[] splitString = displayString.Split(' ');
    //     string[] output = Enumerable.Repeat("".PadLeft(Bounds.W), Bounds.W).ToArray();

    //     // string[] output = new string[bounds.h];
    //     int currentLineLength = 0;
    //     int currentLine = 0;
    //     List<string> lineWords = new();
    //     foreach (string word in splitString)
    //     {
    //         if (word.Length > Bounds.W)
    //         {
    //             throw new NotImplementedException("words larger than the box are not yet supported");
    //         }
    //         if (currentLineLength + word.Length > Bounds.W)
    //         {
    //             output[currentLine] = string.Join(" ", lineWords.ToArray()).PadLeft(Bounds.W);
    //             lineWords = new();
    //             currentLine++;
    //             if (currentLine == Bounds.H)
    //             {
    //                 throw new NotImplementedException("String must fit in the box for now");
    //             }
    //             currentLineLength = word.Length;
    //         }
    //         lineWords.Add(word);
    //     }
    //     output[currentLine] = string.Join(" ", lineWords.ToArray()).PadLeft(Bounds.W);

    //     usedLines = currentLine + 1;

    //     return output;
    // }
}
