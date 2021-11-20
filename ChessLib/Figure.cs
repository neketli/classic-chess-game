using System.Collections.Generic;

/// <summary>
/// Enum and static methods for Figures in chess (By FEN notation)
/// </summary>

namespace ChessLib
{
    enum Figure
    {
        Nothing,

        WhiteKing = 'K',
        WhiteQueen = 'Q',
        WhiteRook = 'R',
        WhiteBishop = 'B',
        WhiteKnight = 'N',
        WhitePawn = 'P',

        BlackKing = 'k',
        BlackQueen = 'q',
        BlackRook = 'r',
        BlackBishop = 'b',
        BlackKnight = 'n',
        BlackPawn = 'p'
    }

    static class FigureMethods
    {
        public static Color GetColor(this Figure figure)
        {
            if (figure == Figure.Nothing) return Color.Nothing;
            return (figure == Figure.WhiteKing ||
                    figure == Figure.WhiteQueen ||
                    figure == Figure.WhiteRook ||
                    figure == Figure.WhiteBishop ||
                    figure == Figure.WhiteKnight ||
                    figure == Figure.WhitePawn) ? Color.White : Color.Black;
        }

        public static IEnumerable<Figure> YieldPromotion(this Figure figure, Square to)
        {
            if (figure == Figure.WhitePawn && to.Y == 7)
            {
                yield return Figure.WhiteQueen;
                yield return Figure.WhiteRook;
                yield return Figure.WhiteBishop;
                yield return Figure.WhiteKnight;

            }
            else if (figure == Figure.BlackPawn && to.Y == 0)
            {
                yield return Figure.BlackQueen;
                yield return Figure.BlackRook;
                yield return Figure.BlackBishop;
                yield return Figure.BlackKnight;

            }
            else yield return Figure.Nothing;
        }
    }
}
