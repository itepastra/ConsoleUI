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
            CString[] splitString = str.Split(' ', out List<CChar> rSpaces);
            CString[] output = Enumerable.Repeat(new CString(lineWidth), padToAmount).ToArray();
            int currentLineLength = -1;
            int currentLine = 0;
            int lastLineStart = 0;
            List<CString> lineWords = new();
            for (int j= 0;j < splitString.Length;j++)
            {
                CString t = splitString[j];
                LongWord:
                if (t.Length > lineWidth)
                {
                    (CString thisLine, CString nextLine) = t.SplitAt(lineWidth - currentLineLength - 1);
                    lineWords.Add(thisLine);
                    output[currentLine] = CString.Join(rSpaces.Take(lastLineStart).ToArray(), lineWords.ToArray());
                    lineWords.Clear();
                    lastLineStart = j;
                    currentLine++;
                    if (currentLine == padToAmount)
                    {
                        usedLines = padToAmount;
                        return output; // box is full, we ignore the rest of the string
                    }
                    currentLineLength = -1;
                    t = nextLine;
                    goto LongWord;
                }
                if (currentLineLength + t.Length >= lineWidth)
                {
                    output[currentLine] = CString.Join(rSpaces.Skip(lastLineStart).ToArray(), lineWords.ToArray()).PadRight(lineWidth);
                    lineWords.Clear();
                    lastLineStart = j;
                    currentLine++;
                    if (currentLine == padToAmount)
                    {
                        usedLines = padToAmount;
                        return output; // box is full, we ignore the rest of the string
                    }
                    currentLineLength = -1;
                }
                lineWords.Add(t);
                currentLineLength += t.Length + 1;
            }
            output[currentLine] = CString.Join(rSpaces.Take(lastLineStart).ToArray(), lineWords.ToArray()).PadRight(lineWidth);

            usedLines = currentLine + 1;
            return output;
        }
    }
    public class RightAlignment : IAlignment
    {
        static string patternToFilter = @"\u001b\[\d+;\d+;\d+;\d+;\d+m";

        public CString[] AlignString(CString str, int lineWidth, int padToAmount, out int usedLines)
        {
            CString[] splitString = str.Split(' ', out List<CChar> rSpaces);
            CString[] output = Enumerable.Repeat(new CString(lineWidth), padToAmount).ToArray();
            int currentLineLength = -1;
            int currentLine = 0;
            int lastLineStart = 0;
            List<CString> lineWords = new();
            for (int j = 0; j < splitString.Length; j++)
            {
                CString t = splitString[j];
            LongWord:
                if (t.Length > lineWidth)
                {
                    (CString thisLine, CString nextLine) = t.SplitAt(lineWidth - currentLineLength - 1);
                    lineWords.Add(thisLine);
                    output[currentLine] = CString.Join(rSpaces.Take(lastLineStart).ToArray(), lineWords.ToArray());
                    lineWords.Clear();
                    lastLineStart = j;
                    currentLine++;
                    if (currentLine == padToAmount)
                    {
                        usedLines = padToAmount;
                        return output; // box is full, we ignore the rest of the string
                    }
                    currentLineLength = -1;
                    t = nextLine;
                    goto LongWord;
                }
                if (currentLineLength + t.Length >= lineWidth)
                {
                    output[currentLine] = CString.Join(rSpaces.Take(lastLineStart).ToArray(), lineWords.ToArray()).PadLeft(lineWidth);
                    lineWords.Clear();
                    currentLine++;
                    lastLineStart = j;
                    if (currentLine == padToAmount)
                    {
                        usedLines = padToAmount;
                        return output; // box is full, we ignore the rest of the string
                    }
                    currentLineLength = -1;
                }
                lineWords.Add(t);
                currentLineLength += t.Length + 1;
            }
            output[currentLine] = CString.Join(rSpaces.Take(lastLineStart).ToArray(), lineWords.ToArray()).PadLeft(lineWidth);

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
