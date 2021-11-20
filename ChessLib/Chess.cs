using System.Collections.Generic;


/// <summary>
/// The main public chess class needed to provide the basic logic of the game and the bundles of all components.
/// Implements a public interaction interface.
///
/// Public methods:
/// Chess - Creates a board according to the given FEN
/// Move - On a given move generates a new board with the completed move.
/// GetFigure - Returns a figure standing on this square by a given move or coordinates
/// IsCheck - Check for Check
/// 
/// </summary>

namespace ChessLib
{
    public class Chess
    {
        public string Fen { get; private set; }

        private Board board;
        private Moves moves;
        List<FigureMoving> allMoves;

        /// <summary>
        /// Constructor generating the initial board
        /// </summary>
        /// <param name="fen">String in international format FEN</param>
        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            Fen = fen;
            board = new Board(fen);
            moves = new Moves(board);
        }

        private Chess(Board board)
        {
            this.board = board;
            Fen = board.Fen;
            moves = new Moves(board);
        }

        /// <summary>
        /// Method for moving a figure
        /// </summary>
        /// <param name="move">String motion specification in (*figure**SquareFrom**SquareTo*) format.
        /// For example: Pe2e4 - WhitePawn from e2 to e4
        /// </param>
        /// <returns>An instance of the Chess class with the move made.</returns>
        public Chess Move(string move)
        {
            if (!IsValidMove(move)) return this;
            Board nextBoard = board.Move(new FigureMoving(move));
            return new Chess(nextBoard);
        }

        /// <summary>
        /// Method for getting a figure on a square
        /// </summary>
        /// <param name="x">int coord of x</param>
        /// <param name="y">int coord of y</param>
        /// <returns>Char name of figure</returns>
        public char GetFigure(int x, int y)
        {
            Square square = new Square(x, y);
            Figure fig = board.GetFigure(square);
            return fig == Figure.Nothing ? '.' : (char)fig;
        }
        /// <summary>
        /// Method for getting a figure on a square
        /// </summary>
        /// <param name="square">coord of a square in ('a'-'h'+1-8) format. </param>
        /// <returns>Char name of figure</returns>
        public char GetFigure(string square)
        {
            Square sq = new Square(square);
            Figure fig = board.GetFigure(sq);
            return fig == Figure.Nothing ? '.' : (char)fig;
        }

        /// <summary>
        /// Private method to find all possible moves for all pieces.
        /// </summary>
        private void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            foreach (FigureOnSquare fs in board.YieldFigures())
                foreach (Square to in Square.YieldSquares())
                    foreach (Figure promotion in fs.Figure.YieldPromotion(to))
                    {
                        FigureMoving fm = new FigureMoving(fs, to, promotion);
                        if (moves.CanMove(fm) && !board.IsCheckAfterMove(fm)) allMoves.Add(fm);
                    }
        }

        /// <summary>
        /// Public method for getting all possible moves.
        /// </summary>
        /// <returns>Return List of string with all moves</returns>
        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach (FigureMoving fm in allMoves)
                list.Add(fm.ToString());
            return list;
        }

        public bool IsCheck()
        {
            return board.IsCheck();
        }

        public bool IsValidMove(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm) || board.IsCheckAfterMove(fm)) return false;
            return true;
        }

    }


}
