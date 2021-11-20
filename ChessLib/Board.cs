using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// The Board class is a representation of a chessboard. 
/// It is immutable, and takes care of all the work with the FEN, the initialization of the board, the figures on it.
/// </summary>

namespace ChessLib
{
    class Board
    {
        private string fen; // Fen notation 
        private Figure[,] figures; // array of figures
        private Color moveColor; // the color that moves
        private int moveNum; // Move number
        public bool canCastleA1 { get; protected set; }
        public bool canCastleH1 { get; protected set; }
        public bool canCastleA8 { get; protected set; }
        public bool canCastleH8 { get; protected set; }

        public string Fen { get => fen; private set => fen = value; }
        public int MoveNum { get => moveNum; private set => moveNum = value; }
        internal Color MoveColor { get => moveColor; set => moveColor = value; }

        /// <summary>
        /// Constructor initializing board by FEN
        /// </summary>
        /// <param name="fen">String of FEN notation</param>
        public Board(string fen)
        {
            Fen = fen;
            figures = new Figure[8, 8];
            Init();
        }

        /// <summary>
        /// Method to initialize the board
        /// </summary>
        private void Init()
        {
            //FEN example: rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            string[] parts = Fen.Split();
            if (parts.Length != 6) return;
            InitFigures(parts[0]);
            MoveColor = parts[1] == "b" ? Color.Black : Color.White;
            InitFlags(parts[2]);
            MoveNum = int.Parse(parts[5]);
        }

        private void InitFlags(string flags)
        {
            canCastleA1 = flags.Contains("Q");
            canCastleA8 = flags.Contains("q");
            canCastleH1 = flags.Contains("K");
            canCastleH8 = flags.Contains("k");
        }

        /// <summary>
        /// Method initializing shapes
        /// </summary>
        /// <param name="data">Takes a list of shapes as a string from FEN notation</param>
        void InitFigures(string data)
        {
            for (int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = lines[7 - y][x] == '.' ? Figure.Nothing : (Figure)lines[7 - y][x];

        }

        /// <summary>
        /// The method required for iterating over figures on squares
        /// </summary>
        /// <returns>FigureOnSquare</returns>
        internal IEnumerable<FigureOnSquare> YieldFigures()
        {
            foreach (Square square in Square.YieldSquares())
                if (GetFigure(square).GetColor() == moveColor)
                    yield return new FigureOnSquare(GetFigure(square), square);

        }
        /// <summary>
        /// Method to build FEN by figures
        /// </summary>
        /// <returns>String of FEN</returns>
        private string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    sb.Append(figures[x, y] == Figure.Nothing ? '1' : (char)figures[x, y]);
                if (y > 0) sb.Append('/');
            }
            string t = "11111111";
            for (int j = 8; j >= 2; j--)
                sb.Replace(t.Substring(0, j), j.ToString());
            return sb.ToString();
        }

        private string FenCastleFlags()
        {
            string flags =
                (canCastleA1 ? "Q" : "") +
                (canCastleH1 ? "K" : "") +
                (canCastleA8 ? "q" : "") +
                (canCastleH8 ? "k" : "");
            flags = flags.Length != 0 ? flags : "-";
            return flags;

        }

        private void UpdateCastleFlags(FigureMoving fm)
        {
            switch (fm.Figure)
            {
                case Figure.WhiteKing:
                    canCastleA1 = false;
                    canCastleH1 = false;
                    return;
                case Figure.WhiteRook:
                    if (fm.From == new Square("a1"))
                    {
                        canCastleA1 = false;
                    }
                    if (fm.From == new Square("h1"))
                    {
                        canCastleH1 = false;
                    }
                    return;
                case Figure.BlackKing:
                    canCastleA8 = false;
                    canCastleH8 = false;
                    return;
                case Figure.BlackRook:
                    if (fm.From == new Square("a8"))
                    {
                        canCastleA1 = false;
                    }
                    if (fm.From == new Square("h8"))
                    {
                        canCastleH1 = false;
                    }
                    return;
                default: return;
            }
        }

        /// <summary>
        /// Method to update Fen prop by generated string from Board
        /// </summary>
        private void GenerateFen()
        {
            Fen = FenFigures() + " " +
                (MoveColor == Color.White ? "w" : "b") + " " +
                FenCastleFlags() +
                " - 0 " + moveNum.ToString();
        }

        /// <summary>
        /// A method that allows you to get a figure by square
        /// </summary>
        /// <param name="square"></param>
        /// <returns>Instance of Figure</returns>
        public Figure GetFigure(Square square)
        {
            if (square.OnBoard) return figures[square.X, square.Y];
            return Figure.Nothing;
        }
        /// <summary>
        /// A method that allows you to set a figure on square
        /// </summary>
        /// <param name="square"></param>
        /// <param name="figure"></param>
        private void SetFigure(Square square, Figure figure)
        {
            if (square.OnBoard) figures[square.X, square.Y] = figure;
        }

        /// <summary>
        /// Method for making a move
        /// </summary>
        /// <param name="fm">Takes an instance of FigureeMoving</param>
        /// <returns>Returns new Board with the executed move</returns>
        public Board Move(FigureMoving fm)
        {
            Board next = new Board(Fen);
            next.SetFigure(fm.From, Figure.Nothing);
            next.SetFigure(fm.To, (fm.Promotion == Figure.Nothing) ? fm.Figure : fm.Promotion);
            next.MoveCastleRook(fm);
            next.UpdateCastleFlags(fm);
            if (MoveColor == Color.Black) next.MoveNum++;
            next.MoveColor = MoveColor.FlipColor();
            next.GenerateFen();
            return next;
        }

        public bool IsCheck()
        {
            Board after = new Board(Fen)
            {
                MoveColor = MoveColor.FlipColor()
            };
            return after.CanEatKing();
        }
        public bool IsCheckAfterMove(FigureMoving fm)
        {
            Board after = Move(fm);
            return after.CanEatKing();
        }

        private void MoveCastleRook(FigureMoving fm)
        {
            if (fm.Figure == Figure.WhiteKing &&
                fm.From == new Square("e1") &&
                fm.To == new Square("g1"))
            {
                SetFigure(new Square("h1"), Figure.Nothing);
                SetFigure(new Square("f1"), Figure.WhiteRook);
                return;
            }
            if (fm.Figure == Figure.WhiteKing &&
                            fm.From == new Square("e1") &&
                            fm.To == new Square("c1"))
            {
                SetFigure(new Square("a1"), Figure.Nothing);
                SetFigure(new Square("d1"), Figure.WhiteRook);
                return;
            }
            if (fm.Figure == Figure.BlackKing &&
                fm.From == new Square("e8") &&
                fm.To == new Square("g8"))
            {
                SetFigure(new Square("h8"), Figure.Nothing);
                SetFigure(new Square("f8"), Figure.BlackRook);
                return;
            }
            if (fm.Figure == Figure.BlackKing &&
                            fm.From == new Square("e8") &&
                            fm.To == new Square("c8"))
            {
                SetFigure(new Square("a8"), Figure.Nothing);
                SetFigure(new Square("d8"), Figure.BlackRook);
                return;
            }

        }

        private bool CanEatKing()
        {
            Square target = FindTarget();
            Moves moves = new Moves(this);
            foreach (FigureOnSquare fs in YieldFigures())
            {
                FigureMoving fm = new FigureMoving(fs, target);
                if (moves.CanMove(fm)) return true;
            }
            return false;
        }

        private Square FindTarget()
        {
            Figure target = moveColor == Color.White ? Figure.BlackKing : Figure.WhiteKing;
            foreach (Square square in Square.YieldSquares())
                if (GetFigure(square) == target) return square;
            return Square.Nothing;
        }
    }
}
