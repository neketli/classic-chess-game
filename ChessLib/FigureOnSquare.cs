/// <summary>
/// Class FigureOnSquare implements connecting Figures with a Square for further placement in the Board
/// </summary>

namespace ChessLib
{
    class FigureOnSquare
    {
        private Figure figure;
        private Square square;

        public FigureOnSquare(Figure figure, Square square)
        {
            this.figure = figure;
            this.square = square;
        }

        internal Figure Figure { get => figure; private set => figure = value; }
        internal Square Square { get => square; private set => square = value; }
    }
}
