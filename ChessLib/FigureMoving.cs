﻿using System;

/// <summary>
/// The figure movement class implements the logic of figure movement.
/// 
/// Has two public constructors for generating movement both according to the given figure on the cell and destination Square
/// or according to the given string.
/// </summary>

namespace ChessLib
{
    class FigureMoving
    {
        private Figure figure;
        private Square from;
        private Square to;

        public Figure Promotion { get; private set; }
        internal Figure Figure { get => figure; set => figure = value; }
        internal Square To { get => to; set => to = value; }
        internal Square From { get => from; set => from = value; }

        public FigureMoving(FigureOnSquare fos, Square to, Figure promotion = Figure.Nothing)
        {
            Figure = fos.Figure;
            From = fos.Square;
            To = to;
            Promotion = promotion;
        }

        public FigureMoving(string move)
        {
            try
            {
                Figure = (Figure)move[0];
                From = new Square(move.ToLower().Substring(1, 2));
                To = new Square(move.ToLower().Substring(3, 2));
                if (move.Length == 6 && Figure.GetColor() == Color.White)
                {
                    var temp = move[5].ToString().ToUpper();
                    Promotion = (Figure)temp[0];
                }
                else if (move.Length == 6 && Figure.GetColor() == Color.Black)
                {
                    var temp = move[5].ToString().ToLower();
                    Promotion = (Figure)temp[0];
                }
                else Promotion = Figure.Nothing;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return;
            }
        }

        /// <summary>
        /// Some methods to make it easier to count the moves of pieces
        /// </summary>
        public int DeltaX { get => To.X - From.X; }
        public int DeltaY { get => To.Y - From.Y; }

        public int AbsDeltaX { get => Math.Abs(DeltaX); }
        public int AbsDeltaY { get => Math.Abs(DeltaY); }

        public int SignX { get => Math.Sign(DeltaX); }
        public int SignY { get => Math.Sign(DeltaY); }


        public override string ToString()
        {
            return (char)Figure + From.Name + To.Name + (Promotion == Figure.Nothing ? ' ' : (char)Promotion);
        }
    }
}
