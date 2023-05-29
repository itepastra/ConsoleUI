using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Image : IDisplayable
    {
        public Rect Bounds => throw new NotImplementedException();

        public event EventHandler<ContentChangeArgs>? ContentChanged;
        public event EventHandler<ContentChangeArgs>? SelfChanged;
        CString? IDisplayable.Line(int lineNum)
        {
            throw new NotImplementedException();
        }
    }
}
