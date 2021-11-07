using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Chess
{
    enum Color
    {
        Nothing, 
        White,
        Black
    }

    static class ColorMethods
    {
        public static Color FlipColor(Color color)
        {
            if (color == Color.White) return Color.Black;
            if (color == Color.Black) return Color.White;
            return Color.Nothing;
        }
    }
}
