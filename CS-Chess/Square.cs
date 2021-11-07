using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Chess
{
    struct Square
    {
        public static Square Nothing = new Square(-1, -1);
        private int x;
        private int y;

        public Square(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }


        public Square(string square) {
            if (square.Length == 2 &&
                square[0] >= 'a' &&
                square[0] <= 'h' &&
                square[1] >= '1' &&
                square[1] <= '8')
            {
                x = square[0] - 'a';
                y = square[1] - '1';
            }
            else this = Nothing;
                
        }


        public bool OnBoard => x >= 0 && x < 8 && y >= 0 && y < 8; 
        public int X { get => x; private set => x = value; }
        public int Y { get => y; private set => y = value; }
        public string Name { get => ((char)('a' + X)).ToString() + (Y + 1).ToString(); }

        public override bool Equals(object obj)
        {
            return obj is Square square &&
                   x == square.x &&
                   y == square.y;
        }

        public static bool operator ==(Square left, Square right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Square left, Square right)
        {
            return !(left == right);
        }

        internal static IEnumerable<Square> YieldSquares()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    yield return new Square(x, y);
             
        }
    }
}
