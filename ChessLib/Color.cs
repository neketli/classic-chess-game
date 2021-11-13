/// <summary>
/// Enum and static methods for colors in chess.
/// </summary>

namespace ChessLib
{
    enum Color
    {
        Nothing,
        White,
        Black
    }

    static class ColorMethods
    {
        public static Color FlipColor(this Color color)
        {
            if (color == Color.White) return Color.Black;
            if (color == Color.Black) return Color.White;
            return Color.Nothing;
        }
    }
}
