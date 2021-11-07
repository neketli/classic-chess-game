﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Chess
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
