/// <summary>
/// Class Moves implements all the logic of the movements of all specified figures.
/// Has a public CanMove method accepting an instance of FigureMoving and returns whether the Figure (of FigureMoving) can move.
/// </summary>

namespace ChessLib
{
    class Moves
    {
        FigureMoving fm;
        Board board;

        public Moves(Board board)
        {
            this.board = board;
        }

        public bool CanMove(FigureMoving fm)
        {
            this.fm = fm;
            return CanMoveFrom() &&
                CanMoveTo() &&
                CanFigureMove();
        }

        private bool CanMoveFrom()
        {
            return fm.From.OnBoard && fm.Figure.GetColor() == board.MoveColor;
        }

        private bool CanMoveTo()
        {
            return fm.From != fm.To && fm.To.OnBoard && board.GetFigure(fm.To).GetColor() != board.MoveColor;
        }

        private bool CanFigureMove()
        {
            switch (fm.Figure)
            {
                case Figure.WhiteKing:
                case Figure.BlackKing:
                    return CanKingMove() || CanKingCastle();

                case Figure.WhiteQueen:
                case Figure.BlackQueen:
                    return CanStraightMove();

                case Figure.WhiteRook:
                case Figure.BlackRook:
                    return (fm.SignX == 0 || fm.SignY == 0) && CanStraightMove();

                case Figure.WhiteBishop:
                case Figure.BlackBishop:
                    return (fm.SignX != 0 && fm.SignY != 0) && CanStraightMove();

                case Figure.WhiteKnight:
                case Figure.BlackKnight:
                    return CanKnightMove();

                case Figure.WhitePawn:
                case Figure.BlackPawn:
                    return CanPawnMove();

                default:
                    return false;
            }
        }

        private bool CanPawnMove()
        {
            if (fm.From.Y < 1 || fm.From.Y > 6) return false;
            int dy = fm.Figure.GetColor() == Color.White ? 1 : -1;
            return CanPawnGo(dy) ||
                CanPawnJump(dy) ||
                CanPawnEat(dy) ||
                CanPawnEnpassant(dy);
        }


        private bool CanPawnJump(int dy)
        {
            return board.GetFigure(fm.To) == Figure.Nothing &&
                fm.DeltaX == 0 &&
                fm.DeltaY == 2 * dy &&
                (fm.From.Y == 1 || fm.From.Y == 6) &&
                board.GetFigure(new Square(fm.From.X, fm.From.Y + dy)) == Figure.Nothing;
        }

        private bool CanPawnGo(int dy)
        {
            return board.GetFigure(fm.To) == Figure.Nothing &&
                fm.DeltaX == 0 &&
                fm.DeltaY == dy &&
                !((fm.To.Y == 0 || fm.To.Y == 7) && fm.Promotion == Figure.Nothing);
        }

        private bool CanPawnEat(int dy)
        {
            return board.GetFigure(fm.To) != Figure.Nothing &&
                fm.AbsDeltaX == 1 &&
                fm.DeltaY == dy &&
                !((fm.To.Y == 0 || fm.To.Y == 7) && fm.Promotion == Figure.Nothing);
        }

        private bool CanPawnEnpassant(int dy)
        {
            if (fm.To == board.Enpassant &&
                fm.DeltaY == dy &&
                fm.AbsDeltaX == 1 &&
                (dy == 1 && fm.From.Y == 4 || dy == -1 && fm.From.Y == 3)) return true;
            return false;   
        }

        private bool CanStraightMove()
        {
            Square at = fm.From;
            do
            {
                at = new Square(at.X + fm.SignX, at.Y + fm.SignY);
                if (at == fm.To) return true;
            } while (at.OnBoard && board.GetFigure(at) == Figure.Nothing);
            return false;
        }

        private bool CanKingMove()
        {
            if (fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1) return true;
            return false;
        }

        private bool CanKingCastle()
        {
            if (fm.Figure.GetColor() == Color.White)
            {
                if (fm.From == new Square("e1") &&
                    fm.To == new Square("g1") &&
                    board.GetFigure(new Square("f1")) == Figure.Nothing &&
                    board.GetFigure(new Square("g1")) == Figure.Nothing &&
                    board.CanCastleH1 &&
                    !board.IsCheck() &&
                    !board.IsCheckAfterMove(new FigureMoving("Ke1f1")) &&
                    !board.IsCheckAfterMove(fm)) return true;
                if (fm.From == new Square("e1") &&
                    fm.To == new Square("c1") &&
                    board.GetFigure(new Square("b1")) == Figure.Nothing &&
                    board.GetFigure(new Square("c1")) == Figure.Nothing &&
                    board.GetFigure(new Square("d1")) == Figure.Nothing &&
                    board.CanCastleA1 &&
                    !board.IsCheck() &&
                    !board.IsCheckAfterMove(new FigureMoving("Ke1c1")) &&
                    !board.IsCheckAfterMove(fm)) return true;
                return false;
            }
            if (fm.Figure.GetColor() == Color.Black)
            {
                if (fm.From == new Square("e8") &&
                    fm.To == new Square("g8") &&
                    board.GetFigure(new Square("f8")) == Figure.Nothing &&
                    board.GetFigure(new Square("g8")) == Figure.Nothing &&
                    board.CanCastleH8 &&
                    !board.IsCheck() &&
                    !board.IsCheckAfterMove(new FigureMoving("ke8f8")) &&
                    !board.IsCheckAfterMove(fm)) return true;
                if (fm.From == new Square("e8") &&
                    fm.To == new Square("c8") &&
                    board.GetFigure(new Square("b8")) == Figure.Nothing &&
                    board.GetFigure(new Square("c8")) == Figure.Nothing &&
                    board.GetFigure(new Square("d8")) == Figure.Nothing &&
                    board.CanCastleA8 &&
                    !board.IsCheck() &&
                    !board.IsCheckAfterMove(new FigureMoving("ke8c8")) &&
                    !board.IsCheckAfterMove(fm)) return true;
                return false;
            }
            return false;
        }

        private bool CanKnightMove()
        {
            if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2) return true;
            if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1) return true;
            return false;
        }
    }
}
