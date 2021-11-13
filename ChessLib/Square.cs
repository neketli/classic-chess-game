using System.Collections.Generic;

/// <summary>
/// Struct of basic type for chess - Square.
/// The Square has:
/// Fields with coordinates x, y
/// A property that returns the name of a square by coordinates as a character + digit (a1,e8...)
/// A method that checks if the square is on the board.
/// </summary>

namespace ChessLib
{
    internal struct Square
    {
        public static Square Nothing = new Square(-1, -1);
        private int x;
        private int y;
        public bool OnBoard => x >= 0 && x < 8 && y >= 0 && y < 8;
        public int X { get => x; private set => x = value; }
        public int Y { get => y; private set => y = value; }
        public string Name { get => ((char)('a' + X)).ToString() + (Y + 1).ToString(); }

        public Square(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public Square(string square)
        {
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
        public override int GetHashCode()
        {
            int hashCode = 809334081;
            hashCode = hashCode * -1521134295 + OnBoard.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
