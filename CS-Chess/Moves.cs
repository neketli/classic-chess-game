﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Chess
{
    class Moves
    {
        FigureMoving fm;
        Board board;

        public Moves (Board board)
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

        bool CanMoveFrom()
        {
            return fm.From.OnBoard && fm.Figure.GetColor() == board.MoveColor;
        }

        bool CanMoveTo()
        {
            return fm.From != fm.To && fm.To.OnBoard && board.GetFigure(fm.To).GetColor() != board.MoveColor;
        }

        bool CanFigureMove()
        {
            switch (fm.Figure)
            {
                case Figure.WhiteKing:
                case Figure.BlackKing:
                    return CanKingMove();

                case Figure.WhiteQueen:
                case Figure.BlackQueen:
                    return CanStraightMove();

                case Figure.WhiteRook:
                case Figure.BlackRook:
                    return (fm.SignX == 0 || fm.SignY == 0) && CanStraightMove();
                    
                case Figure.WhiteBishop:
                case Figure.BlackBishop:
                    return (fm.SignX != 0 || fm.SignY != 0) && CanStraightMove();

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
                CanPawnJump(dy)  ||
                CanPawnEat(dy);
        }


        private bool CanPawnJump(int dy)
        {
            return board.GetFigure(fm.To) == Figure.Nothing &&
                fm.DeltaX == 0                              &&
                fm.DeltaY == 2 * dy                         &&
                (fm.From.Y == 1 || fm.From.Y == 6)          &&
                board.GetFigure(new Square(fm.From.X, fm.From.Y + dy)) == Figure.Nothing;
        }

        private bool CanPawnGo(int dy)
        {
            return board.GetFigure(fm.To) == Figure.Nothing &&
                fm.DeltaX == 0                              &&
                fm.DeltaY == dy;
        }

        private bool CanPawnEat(int dy)
        {
            return board.GetFigure(fm.To) != Figure.Nothing &&
                fm.AbsDeltaX == 1                           &&
                fm.DeltaY == dy;
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

        private bool CanKnightMove()
        {
            if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2) return true;
            if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1) return true;
            return false;
        }
    }
}
