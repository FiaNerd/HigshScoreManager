using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using static System.Console;

namespace HighScoreManager
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

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
            var response = httpClient.GetAsync("games")
                 .GetAwaiter()
                 .GetResult();

            if(response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .Result;

                var games = JsonConvert.DeserializeObject<IEnumerable<Game>>(jsonString);
                WriteLine("Id      Game");
                foreach (var game in games)
                {
                    WriteLine($"{game.Id}\t{game.Title}");
                }
            }

            ReadKey(true);
        }

        public class Game
        {
            public int Id { get; set; }

            public string Title { get; set; }
        }
    }
}
