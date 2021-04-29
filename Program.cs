using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using static System.Console;

namespace HighScoreManager
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            CursorVisible = true;

            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

            MainMenu();
        }


        private static void MainMenu()
        {
            CursorVisible = false;

            bool programRunning = true;

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
                    case ConsoleKey.NumPad1:
                        RegisterHighScore();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ListGame();
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        AddGame();
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        DeleteGame();
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:

                        programRunning = false;

                        break;
                }

                Clear();

            } while (programRunning);
        }


        private static void RegisterHighScore()
        {
            CursorVisible = true;
            bool programRunning = true;

            do
            {
                WriteLine("Game: ");
                WriteLine("Player: ");
                WriteLine("Date: ");
                WriteLine("Score: ");

                try
                {
                    SetCursorPosition(10, 0);
                    string addGameTitle = ReadLine().Trim();

                    SetCursorPosition(10, 1);
                    string addPlayer = ReadLine().Trim().ToUpper();

                    SetCursorPosition(10, 2);
                    string addDate = ReadLine().Trim();

                    SetCursorPosition(10, 3);
                    int addScore = int.Parse(ReadLine().Trim());

                    SetCursorPosition(5, 8);
                    WriteLine("Is this correct [Y]es / [N]o ?  [Esc] Back to main");
                    var userInput = ReadKey(true).Key;

                    while (userInput != ConsoleKey.Y && userInput != ConsoleKey.N && userInput != ConsoleKey.Escape)
                    {
                        userInput = ReadKey(true).Key;
                    }
                    Clear();


                    if (userInput == ConsoleKey.Y)
                    {
                        PostGame(addGameTitle, addPlayer, addDate, addScore);

                        programRunning = false;
                    }

                    if (userInput == ConsoleKey.N)
                    {
                        programRunning = true;
                        Clear();
                    }
                    else if (userInput == ConsoleKey.Escape)
                    {
                        Clear();
                        programRunning = false;
                        MainMenu();
                        Clear();
                    }
                }
                catch (Exception e)
                {
                    Clear();
                    SetCursorPosition(6, 4);
                    WriteLine(e.Message);
                    Thread.Sleep(2000);
                    Clear();
                }

            } while (programRunning);
        }


        private static void PostGame(string addGameTitle, string addPlayer, string addDate, int addScore)
        {

            HighScore highScores = new HighScore()
            {
                GTitle = addGameTitle,
                Player = addPlayer,
                Date = addDate,
                Score = addScore,
            };

            string json = JsonConvert.SerializeObject(highScores);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("https://localhost:5001/api/highscores/", data)
                 .Result;

            response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Clear();
                WriteLine($"Highscore {highScores.Score} registerd");
                Thread.Sleep(2000);
                Clear();
            }
            else
            {
                Clear();
                WriteLine($"The Game dosn't exist, try agin!");
                Thread.Sleep(2000);
                Clear();
            }
        }


        private static void ListGame()
        {
            bool programRunning = true;

            do
            {
                var response = httpClient.GetAsync("games")
                     .GetAwaiter()
                     .GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync()
                        .Result;

                    var games = JsonConvert.DeserializeObject<IEnumerable<Game>>(jsonString);

                    WriteLine(" Id     Game");
                    WriteLine("==============");

                    foreach (var game in games)
                    {
                        WriteLine($" {game.Id}\t{game.Title}");
                    }
                }

                WriteLine("[Esc] Back to main");
                var userInput = ReadKey(true).Key;

                while (userInput == ConsoleKey.Escape)
                {
                    Clear();
                    programRunning = false;
                    MainMenu();
                    Clear();
                }

                Clear();

            } while (programRunning);
        }


        private static void AddGame()
        {
            CursorVisible = true;

            bool programRunning = true;

            do
            {
                WriteLine("Title: ");
                WriteLine("Description: ");
                WriteLine("Genre: ");
                WriteLine("Release Year: ");
                WriteLine("Image URL: ");

                try
                {
                    SetCursorPosition(15, 0);
                    string addTitle = ReadLine().Trim();

                    SetCursorPosition(15, 1);
                    string addDescription = ReadLine().Trim();

                    SetCursorPosition(15, 2);
                    string addGenre = ReadLine().Trim();

                    SetCursorPosition(15, 3);
                    string addRealeaseYear = ReadLine().Trim();

                    SetCursorPosition(15, 4);
                    string addImageUrl = ReadLine().Trim();

                    SetCursorPosition(5, 8);
                    WriteLine("Is this correct [Y]es / [N]o ?  [Esc] Back to main");
                    var userInput = ReadKey(true).Key;

                    while (userInput != ConsoleKey.Y && userInput != ConsoleKey.N && userInput != ConsoleKey.Escape)
                    {
                        userInput = ReadKey(true).Key;
                    }

                    Clear();

                    if (userInput == ConsoleKey.Y)
                    {
                        PostGame(addTitle, addDescription, addGenre, addRealeaseYear, addImageUrl);

                        programRunning = false;
                    }

                    if (userInput == ConsoleKey.N)
                    {
                        programRunning = true;
                        Clear();
                    }
                    else if (userInput == ConsoleKey.Escape)
                    {
                        Clear();
                        programRunning = false;
                        MainMenu();
                        Clear();
                    }
                }
                catch (Exception e)
                {
                    Clear();
                    SetCursorPosition(6, 4);
                    WriteLine(e.Message);
                    Thread.Sleep(2000);
                    Clear();
                }

            } while (programRunning);
        }


        private static void PostGame(string addTitle, string addDescription, string addGenre, string addRealeaseYear, string addImageUrl)
        {
            Game games = new Game()
            {
                Title = addTitle,
                Description = addDescription,
                Genre = addGenre,
                ReleaseYear = addRealeaseYear,
                ImageUrl = addImageUrl
            };

            string json = JsonConvert.SerializeObject(games);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("https://localhost:5001/api/games/", data)
                 .Result;

            response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                WriteLine($"Game {games.Title} registerd");
                Thread.Sleep(2000);
                Clear();
            }
            else if (!response.IsSuccessStatusCode)
            {
                WriteLine($"This game {games.Title} is occupied or Empty");
                Thread.Sleep(2000);
                Clear();
            }
        }


        private static void DeleteGame()
        {
            CursorVisible = true;

            bool programRunning = true;

            do
            {
                try
                {

                    Write("ID: ");
                    int deletId = int.Parse(ReadLine().Trim());

                    Game game = new Game()
                    {
                        Id = deletId
                    };

                    var response = httpClient.DeleteAsync("https://localhost:5001/api/games/" + game.Id)
                        .Result;

                    response.Content.ReadAsStringAsync();

                    SetCursorPosition(5, 4);
                    WriteLine("Are you sure [Y]es / [N]o ?  [Esc] Back to main");
                    var userInput = ReadKey(true).Key;

                    while (userInput != ConsoleKey.Y && userInput != ConsoleKey.N && userInput != ConsoleKey.Escape)
                    {
                        userInput = ReadKey(true).Key;
                    }

                    Clear();

                    if (userInput == ConsoleKey.Y)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            Clear();
                            WriteLine($"{game.Id} Succcesfull delete!");
                            Thread.Sleep(2000);
                            Clear();
                        }
                        else
                        {
                            Clear();
                            WriteLine($"ID {game.Id} dosn't exist, NOT deleted!");
                            Thread.Sleep(2000);
                            Clear();
                        }

                        programRunning = false;
                    }

                    if (userInput == ConsoleKey.N)
                    {
                        programRunning = true;
                        Clear();
                    }
                    else if (userInput == ConsoleKey.Escape)
                    {
                        Clear();
                        programRunning = false;
                        //MainMenu();
                        Clear();
                    }
                }
                catch (Exception e)
                {
                    Clear();
                    SetCursorPosition(6, 4);
                    WriteLine(e.Message);
                    Thread.Sleep(2000);
                    Clear();
                }

            } while (programRunning);

        }
    }
}
