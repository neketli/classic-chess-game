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
    }
}
