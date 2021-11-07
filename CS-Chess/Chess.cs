using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Chess
{
    public class Chess
    {
        public string Fen { get; private set; }
        
        private Board board;
        private Moves moves;

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            Fen = fen;
            board = new Board(fen);
            moves = new Moves(board);
        }

        private Chess (Board board)
        {
            this.board = board;
            Fen = board.Fen;
            moves = new Moves(board);
        }
        
        public Chess Move(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm)) return this;
            Board nextBoard = board.Move(fm);
            return new Chess(nextBoard);
        }

        public char GetFigure(int x, int y)
        {
            Square square = new Square(x, y);
            Figure fig = board.GetFigure(square);
            return fig == Figure.Nothing ? '.' : (char)fig;
        }

       
    }


}
