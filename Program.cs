﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using static HighScoreManager.Program;
using static System.Console;

namespace HighScoreManager
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

            CursorVisible = true;

            MainMenu();

        }

        private static void MainMenu()
        {

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

                        applicationRunning = false;

                        break;
                }

                Clear();

            } while (applicationRunning);

        }




        private static void RegisterHighScore()
        {
            bool programRuning = true;

            do
            {
                WriteLine("Game:");
                WriteLine("Player:");
                WriteLine("Date:");
                WriteLine("Score:");

                try
                {
                    SetCursorPosition(10, 0);
                    int addGame = Convert.ToInt32(ReadLine());

                    SetCursorPosition(10, 1);
                    string addPlayer = ReadLine().Trim().ToUpper();

                    SetCursorPosition(10, 2);
                    string addDate = ReadLine().Trim().ToUpper();

                    SetCursorPosition(10, 3);
                    int addScore = int.Parse(ReadLine().Trim());

                    SetCursorPosition(5, 8);
                    WriteLine("Is this correct [Y]es / [N]o?  [Esc] Back to main");
                    var userInput = ReadKey(true).Key;

                    while (userInput != ConsoleKey.Y && userInput != ConsoleKey.N && userInput != ConsoleKey.Escape)
                    {
                        userInput = ReadKey(true).Key;
                    }
                    Clear();


                    if (userInput == ConsoleKey.Y)
                    {
                        HighScore highScores = new HighScore()
                        {
                            Player = addPlayer,
                            Date = addDate,
                            Score = addScore,
                            GameId = addGame
                        };

                        string json = JsonConvert.SerializeObject(highScores);

                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = httpClient.PostAsync("https://localhost:5001/api/highscores/", data)
                             .Result;

                        response.Content.ReadAsStringAsync();


                        //if (!highScore.CategoryName.Any(HighScore))

                        //{
                        //InsertCategoryIntoDataBase(highScore);
                        //Console.WriteLine($"Highscore {highScore.Player} registerd");
                        //Thread.Sleep(2000);
                        //Console.Clear();
                        //}
                        //        else
                        //        {

                        //            Console.WriteLine($"This category {userCategoryName} is occupied");
                        //            Thread.Sleep(2000);
                        //            Console.Clear();
                        //            programRuning = false;
                        //            programRuning = false;

                        //        }


                        programRuning = false;

                        if (userInput == ConsoleKey.N)
                        {
                            programRuning = true;
                            Clear();
                        }
                        else if (userInput == ConsoleKey.Escape)
                        {
                            Clear();
                            programRuning = false;
                            MainMenu();
                            Clear();
                        }
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

            } while (programRuning);
        }


        private static void ListGame()
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

            ReadKey(true);
        }


        private static void AddGame()
        {
            bool programRuning = true;

            do
            {
                WriteLine("Title:");
                WriteLine("Description:");
                WriteLine("Release Year:");
                WriteLine("Genre:");
                WriteLine("Image URL");

                try
                {
                    SetCursorPosition(15, 0);
                    string addTitle = ReadLine().Trim().ToUpper();

                    SetCursorPosition(15, 1);
                    string addDescription = ReadLine().Trim().ToUpper();

                    SetCursorPosition(15, 2);
                    string addRealeaseYear = ReadLine().Trim().ToUpper();

                    SetCursorPosition(15, 3);
                    string addGenre = ReadLine().Trim().ToUpper();

                    SetCursorPosition(15, 4);
                    string addImageUrl = ReadLine().Trim();

                    SetCursorPosition(5, 8);
                    WriteLine("Is this correct [Y]es / [N]o?  [Esc] Back to main");
                    var userInput = ReadKey(true).Key;

                    while (userInput != ConsoleKey.Y && userInput != ConsoleKey.N && userInput != ConsoleKey.Escape)
                    {
                        userInput = ReadKey(true).Key;
                    }

                    Clear();

                    if (userInput == ConsoleKey.Y)
                    {
                        PostGame(addTitle, addDescription, addRealeaseYear, addGenre, addImageUrl);

                        programRuning = false;

                    }

                    if (userInput == ConsoleKey.N)
                    {
                        programRuning = true;
                        Clear();
                    }
                    else if (userInput == ConsoleKey.Escape)
                    {
                        Clear();
                        programRuning = false;
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

            } while (programRuning);

        }

        private static void PostGame(string addTitle, string addDescription, string addRealeaseYear, string addGenre, string addImageUrl)
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

            WriteLine($"Game {games.Title} registerd");
            Thread.Sleep(2000);
            Clear();
            //}
            //else
            //{

            //    Console.WriteLine($"This game {games.Title} is occupied");
            //    Thread.Sleep(2000);
            //    Console.Clear();
            //    programRuning = false;
            //    programRuning = false;

            //}
        }

        private static void DeleteGame()
        {
            Write("ID: ");
            int deletId = int.Parse(ReadLine().Trim());

            Game gameId = new Game()
            {
                Id = deletId
            };

            var response = httpClient.DeleteAsync("https://localhost:5001/api/games/" + gameId.Id).Result;

            response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Clear();
                WriteLine("Succcesfull delete");
                Thread.Sleep(2000);
                Clear();
            }
            else
            {
                Clear();
                Console.WriteLine("Someting went wrong, hotel not deleted");
                Thread.Sleep(2000);
                Clear();
            }
        }
    }

    public class Game
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public string ReleaseYear { get; set; }

        public string ImageUrl { get; set; }
    }

    public class HighScore
    {
        public int Id { get; set; }

        public string Player { get; set; }

        public string Date { get; set; }

        public int Score { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
