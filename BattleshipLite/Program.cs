using BattleshipLibrary.Models;
using BattleshipLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayWelcomeMessage();


            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                DisplayShotGrid(activePlayer);

                RecordPlayerShot(activePlayer, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                if (doesGameContinue == true)
                {
                    // Swap positions
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

                Console.WriteLine();

            } while (winner == null);

            IdentifyWinner(winner);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UserName} for winning!");
            Console.WriteLine($"{winner.UserName} took {GameLogic.GetShotCount(winner)} shots.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskforShot(activePlayer);
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {
                    isValidShot = false;
                }

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location. Please try again");
                }

            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

            DisplayShotResult(row, column, isAHit);
        }

        private static void DisplayShotResult(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{column} was a Hit!");
            }
            else
            {
                Console.WriteLine($"{row}{column} was a miss.");
            }
        }

        private static string AskforShot(PlayerInfoModel activePlayer)
        {
            Console.WriteLine();
            Console.Write($"{activePlayer.UserName}, please enter your shot selection: ");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == Enums.GridSpotStatus.Empty)
                {
                    Console.Write($"{gridSpot.SpotLetter}{ gridSpot.SpotNumber} ");
                }
                else if (gridSpot.Status == Enums.GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                }
                else if (gridSpot.Status == Enums.GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.Write(" ? ");
                }
            }

            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");

            output.UserName = GetUsersName($"Enter {playerTitle} name: ");

            GameLogic.InitializeGrid(output);

            PlaceShips(output);

            Console.Clear();

            return output;
        }

        private static void DisplayWelcomeMessage()
        {
            Console.WriteLine("### Welcome to Battleship Lite ### \n");
        }

        private static string GetUsersName(string message)
        {
            Console.Write(message);
            string name = Console.ReadLine();
            return name;
        }

        public static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}: ");

                string location = Console.ReadLine();
                bool isValidLocation;

                try
                {
                    isValidLocation = GameLogic.PlaceShip(model, location);
                }
                catch (Exception ex)
                {
                    isValidLocation = false;
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again");
                }
            } while (model.ShipLocations.Count < 5);
        }
    }
}
