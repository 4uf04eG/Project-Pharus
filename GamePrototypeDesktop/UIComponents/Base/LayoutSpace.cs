using System;
using Microsoft.Xna.Framework;

namespace GamePrototypeDesktop.UIComponents.Base
{
    public class LayoutSpace : IComparable<LayoutSpace>
    {
        private readonly int[] _values;

        public LayoutSpace(int all)
        {
            _values = new[] {all, all, all, all};
        }

        public LayoutSpace(int left, int right, int top, int bottom)
        {
            _values = new[] {left, right, top, bottom};
        } 

        public int Left => _values[0];
        public int Right => _values[1];
        public int Top => _values[2];
        public int Bottom => _values[3];
        public int Horizontal => _values[0] + _values[1];
        public int Vertical => _values[2] + _values[3];
        
        public int CompareTo(LayoutSpace other)
        {
            if (Left == other.Left && Right == other.Right && 
                Top == other.Top && Bottom == other.Bottom)
                return 1;

            return 0;
        }
    }
}