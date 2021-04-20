using System;
using static System.Console;

namespace HighScoreManager
{
    class Program
    {
        static void Main(string[] args)
        {

            CursorVisible = false;

            bool applicationRunning = true;

            do
            {
                WriteLine("1. Register highscore");
                WriteLine("2. List games");
                WriteLine("3. Add game");
                WriteLine("4. Delete game");
                WriteLine("5. Exit");

                ConsoleKeyInfo input = ReadKey(true);

                Clear();

                switch (input.Key)
                {
                    case ConsoleKey.D1:

                        break;

                    case ConsoleKey.D2:
                        ListGame();
                        break;

                    case ConsoleKey.D3:

                        break;
                    case ConsoleKey.D4:

                        break;

                    case ConsoleKey.D5:

                        applicationRunning = false;

                        break;
                }

                Clear();

            } while (applicationRunning);

        }

        private static void ListGame()
        {
            WriteLine("List products...");
            ReadKey(true);
        }
    }
}
