using System;

namespace DemoChess
{
    class Program
    {
        static void Main(string[] args)
        {
            CS_Chess.Chess chess = new();
            while (true)
            {
                Console.WriteLine(chess.Fen);
                Print(ChessToAscii(chess));
                string move = Console.ReadLine();
                if (move == "") break;
                chess = chess.Move(move);
            }
        }

        static string ChessToAscii(CS_Chess.Chess chess)
        {
            string text = "#-----------------#\n";
            for (int y = 7; y >= 0; y--)
            {
                text += y + 1;
                text += "|";
                for (int x = 0; x < 8; x++)
                {
                    text += chess.GetFigure(x, y) + " ";
                }
                text += "|\n";
            }
            text += "#-----------------#\n";
            text += "  a b c d e f g h\n";
            return text;
        }

        static void Print(string text)
        {
            ConsoleColor oldForegroundColor = Console.ForegroundColor;
            foreach (char ch in text)
            {
                if (ch >= 'a' && ch <= 'z') Console.ForegroundColor = ConsoleColor.Red;
                else if (ch >= 'A' && ch <= 'Z') Console.ForegroundColor = ConsoleColor.Blue;
                else Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(ch);
            }
            Console.ForegroundColor = oldForegroundColor;
        }
    }
}
