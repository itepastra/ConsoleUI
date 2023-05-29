using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleUI
{
    public struct CChar
    {
        public char chr { get; }
        public Color foreground { get; set; }
        public Color background { get; set; }

        public CChar(char c) : this(c, null, null) { }

        public CChar(char c, Color? f, Color? b)
        {
            chr = c;
            foreground = f.GetValueOrDefault(Color.White);
            background = b.GetValueOrDefault(Color.Black);
        }

    }


    public class CString
    {
        private CChar[] chars;
        public CChar[] Chars { get => chars; set => chars = value; }
        public int Count => Length;
        public int Length { get => Chars.Length; }
        public bool IsSynchronized => Chars.IsSynchronized;
        public object SyncRoot => Chars.SyncRoot;
        public object Current => throw new NotImplementedException();
        public CChar this[int index] { get => Chars[index]; set => Chars[index] = value; }

        #region constructors
        public CString(string s)
        {
            chars = s.Select(c => new CChar(c)).ToArray();
        }
        public CString(string s, Color? fg, Color? bg)
        {
            chars = s.Select(c => new CChar(c, fg, bg)).ToArray();
        }

        public CString(int lineWidth)
        {
            chars = Enumerable.Repeat(new CChar(' '), lineWidth).ToArray();
        }

        public CString(CChar[] chars)
        {
            this.chars = chars;
        }
        #endregion

        #region Conversion
        public static explicit operator CString(string s) => new CString(s);
        public static explicit operator string(CString s) => new string(s.Chars.Select(s => s.chr).ToArray());

        public static explicit operator CString(CChar[] cChars) => new CString(cChars);
        #endregion


        #region Split
        internal CString[] Split(char v, out List<CChar> removedSpaces)
        {
            List<CString> output = new();
            removedSpaces = new();
            int lastIndex = 0;
            for (int i = 0; i < Length; i++)
            {
                if (this[i].chr == v)
                {
                    output.Add(new(chars.Skip(lastIndex).Take(i - lastIndex).ToArray()));
                    lastIndex = i + 1;
                    removedSpaces.Add(this[i]);
                }
            }
            if (output.Count == 0) return new CString[] { this };
            return output.ToArray();
        }

        internal (CString, CString) SplitAt(int index)
        {
            return (new(chars.Take(index).ToArray()), new (chars.Skip(index).ToArray()));
        }
        #endregion

        #region Merging

        internal static CString Join(CString v, CString[] cStrings)
        {
            if (cStrings.Length == 0) return new CString(0);

            // CString outStr = cStrings[0];
            IEnumerable<CChar> outChars = cStrings[0].Chars;

            for (int i = 1; i < cStrings.Length; i++)
                outChars = outChars.Concat(v.Chars).Concat(cStrings[i].Chars);

            return new(outChars.ToArray());
        }
        internal static CString Join(CChar[] vs, CString[] cStrings)
        {
            if (cStrings.Length == 0) return new CString(0);

            // CString outStr = cStrings[0];
            IEnumerable<CChar> outChars = cStrings[0].Chars;

            for (int i = 1; i < cStrings.Length; i++)
                outChars = outChars.Append(vs[i - 1]).Concat(cStrings[i].Chars);

            return new(outChars.ToArray());
        }


        internal static void Copy(CString line, int v, CString lineBuffer, int x, int w)
        {
            Array.Copy(line.Chars, v, lineBuffer.Chars, x, w);
        }

        #endregion

        #region Padding
        internal CString PadRight(int lineWidth)
        {
            if (this.Chars.Length >= lineWidth) return this;

            CChar[] lineBuffer = Enumerable.Repeat(new CChar(' '), lineWidth).ToArray();

            Array.Copy(this.Chars, 0, lineBuffer, 0, this.Length);
            return new(lineBuffer);
        }

        internal CString PadLeft(int lineWidth)
        {
            if (this.Chars.Length >= lineWidth) return this;

            CChar[] lineBuffer = Enumerable.Repeat(new CChar(' '), lineWidth).ToArray();

            Array.Copy(this.Chars, lineWidth - this.Length, lineBuffer, 0, this.Length);
            return new(lineBuffer);
        }
        #endregion

        #region printTools
        internal string ToWriteChars(int startIdx, int width)
        {
            IEnumerable<CChar> charsToWrite = this.Chars.Skip(startIdx).Take(width);
            List<string> outStrings = new();
            List<Char> colorChars = new();

            Color fg = charsToWrite.Select(c => c.foreground).First();
            Color bg = charsToWrite.Select(c => c.background).First();

            foreach (CChar c in charsToWrite)
            {


                if ((c.foreground != fg) || (c.background != bg))
                {
                    string thisWord = new string(colorChars.ToArray());
                    thisWord = thisWord.Pastel(fg).PastelBg(bg);
                    fg = c.foreground;
                    bg = c.background;
                    outStrings.Add(thisWord);
                    colorChars.Clear();
                }

                colorChars.Add(c.chr);
            }

            if (colorChars.Count > 0) outStrings.Add(new string(colorChars.ToArray()).Pastel(fg).PastelBg(bg));
            return string.Join("", outStrings.ToArray());
        }
        public override string ToString() => (string)this;

        #endregion

    }

}
