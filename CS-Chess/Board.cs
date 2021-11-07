using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Chess
{
    class Board
    {
        private string fen;
        Figure[,] figures;
        private Color moveColor;
        private int moveNum;

        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8,8];
            Init();
        }

        private void Init()
        {
            //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            string[] parts = Fen.Split();
            if (parts.Length != 6) return;
            InitFigures(parts[0]);
            MoveColor = parts[1] == "b" ? Color.Black : Color.White;
            MoveNum = int.Parse(parts[5]);
            SetFigure(new Square("e1"), Figure.WhiteKing);
            SetFigure(new Square("e8"), Figure.BlackKing);
            MoveColor = Color.White;
        }

        void InitFigures(string data)
        {
            for (int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = lines[7 - y][x] == '.'? Figure.Nothing : (Figure)lines[7 - y][x];

        }

        internal IEnumerable<FigureOnSquare> YieldFigures()
        {
            foreach (Square square in Square.YieldSquares())
                if (GetFigure(square).GetColor() == moveColor)
                    yield return new FigureOnSquare(GetFigure(square), square);
             
        }

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
       
        private void GenerateFen()
        {
            Fen = FenFigures() + " " +
                (MoveColor == Color.White ? "w" : "b") +
                " - - 0 " + moveNum.ToString();
        }
        public Figure GetFigure(Square square)
        {
            if (square.OnBoard) return figures[square.X, square.Y];
            return Figure.Nothing;
        }

        private void SetFigure(Square square, Figure figure)
        {
            if (square.OnBoard) figures[square.X, square.Y] = figure;
        }

        public Board Move(FigureMoving fm)
        {
            Board next = new Board(Fen);
            next.SetFigure(fm.From, Figure.Nothing);
            next.SetFigure(fm.To, (fm.Promotion == Figure.Nothing) ? fm.Figure : fm.Promotion);
            if (MoveColor == Color.Black) next.MoveNum++;
            next.MoveColor = ColorMethods.FlipColor(MoveColor);
            next.GenerateFen();
            return next;
        }

        public string Fen { get => fen; private set => fen = value; }
        public int MoveNum { get => moveNum; private set => moveNum = value; }
        internal Color MoveColor { get => moveColor; set => moveColor = value; }
    }
}
