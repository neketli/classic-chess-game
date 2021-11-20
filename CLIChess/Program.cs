using ChessLib;
using System;

namespace CLIChess
{
    class Program
    {
        static void Main(string[] args)
        {
            Chess chess = new();
            while (true)
            {
                Console.WriteLine(chess.Fen);
                Print(ChessToAscii(chess));
                if (chess.IsCheck()) Console.WriteLine("check");
                if (chess.GetAllMoves().Count == 0)
                {
                    if (chess.IsCheck()) Console.WriteLine("mat");
                    else Console.WriteLine("stalemate");

                    Console.WriteLine("Reload? y/n");
                    string answer = Console.ReadLine().ToLower();
                    if (answer[0] == 'y')
                    {
                        chess = new();
                        Print(ChessToAscii(chess));
                    }
                    else break;

                }
                foreach (string Move in chess.GetAllMoves())
                    Console.Write(Move + '\t');
                Console.Write("\n >>");
                string move = Console.ReadLine();
                if (move == "") break;
                chess = chess.Move(move);
            }
        }

        static string ChessToAscii(Chess chess)
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
