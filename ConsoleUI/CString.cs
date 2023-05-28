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
        public Color? foreground { get; set; }
        public Color? background { get; set; }

        public CChar(char c) : this(c, null, null) { }

        public CChar(char c, Color? f, Color? b)
        {
            chr = c;
            foreground = f;
            background = b;
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

        public CString(string s)
        {
            chars = s.Select(c => new CChar(c)).ToArray();
        }

        public CString(int lineWidth)
        {
            chars = Enumerable.Repeat(new CChar(' '), lineWidth).ToArray();
        }

        public CString(CChar[] chars)
        {
            this.chars = chars;
        }


        public static explicit operator CString(string s) => new CString(s);
        public static explicit operator string(CString s) => new string(s.Chars.Select(i => i.chr).ToArray());

        internal CString[] Split(char v)
        {
            List<CString> output = new();
            int lastIndex = 0;
            for (int i = 0; i < Length; i++)
            {
                if (this[i].chr == v)
                {
                    output.Add(new(chars.Skip(lastIndex).Take(i - lastIndex).ToArray()));
                    lastIndex = i + 1;
                }
            }
            if (output.Count == 0) return new CString[] { this };
            return output.ToArray();
        }

        internal static CString Join(CString v, CString[] cStrings)
        {
            if (cStrings.Length == 0) return new CString(0);

            // CString outStr = cStrings[0];
            IEnumerable<CChar> outChars = cStrings[0].Chars;

            for (int i = 1; i < cStrings.Length; i++)
                outChars = outChars.Concat(v.Chars).Concat(cStrings[i].Chars);

            return new(outChars.ToArray());
        }

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

        internal static void Copy(CString line, int v, CString lineBuffer, int x, int w)
        {
            Array.Copy(line.Chars, v, lineBuffer.Chars, x, w);
        }

        internal string ToWriteChars(int startIdx, int width)
        {
            IEnumerable<CChar> charsToWrite = this.Chars.Skip(startIdx).Take(width);
            List<string> outStrings = new();
            List<Char> colorChars = new();

            Color fg = charsToWrite.Select(c => c.foreground).SkipWhile(c => c == null).FirstOrDefault().GetValueOrDefault(Color.White);
            Color bg = charsToWrite.Select(c => c.background).SkipWhile(c => c == null).FirstOrDefault().GetValueOrDefault(Color.Black);

            foreach (CChar c in charsToWrite)
            {
                if (c.foreground == null && c.background == null)
                {
                    colorChars.Add(c.chr);
                }
                else if (c.background != null && c.foreground != null)
                {
                    //  change both
                    string s = new string(colorChars.ToArray()).Pastel(fg).PastelBg(bg);
                    colorChars.Clear();
                    outStrings.Add(s);
                    fg = c.foreground.Value;
                    bg = c.background.Value;
                    colorChars.Add(c.chr);
                }
                else if (c.foreground != null)
                {
                    // change foreground
                    string s = new string(colorChars.ToArray()).Pastel(fg);
                    colorChars.Clear();
                    outStrings.Add(s);
                    fg = c.foreground.Value;
                    colorChars.Add(c.chr);
                }
                else if (c.background != null)
                {
                    // change background
                    string s = new string(colorChars.ToArray()).PastelBg(bg);
                    colorChars.Clear();
                    outStrings.Add(s);
                    bg = c.background.Value;
                    colorChars.Add(c.chr);
                }
            }

            outStrings.Add(new string(colorChars.ToArray()).Pastel(fg).PastelBg(bg));
            return string.Join("", outStrings.ToArray());
        }


        public IEnumerator GetEnumerator()
        {
            return Chars.GetEnumerator();
        }

    }

}
