using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class SizeChangeArgs : EventArgs
    {
        public Rect oldSize;
        public Rect newSize;
        public SizeChangeArgs(Rect oldRect, Rect newRect)
        {
            oldSize = oldRect;
            newSize = newRect;
        }
    }
}
