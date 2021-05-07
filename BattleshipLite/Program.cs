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
            string row;
            int column;

            do
            {
                string shot = AskforShot();
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location. Please try again");
                }

            } while (isValidShot == false);


            // Determine if it is a valid shot. Iterate over opponent's Shiplocations and check if
            // the list contains the object with this letter and number. If it's a match, check the status.
            // If the status is Empty or Ship, it's a valid shot. OR
            // Check if the value is one of the item in activePlayer's gridshot.
            // Go back to the beginning if not a valid shot

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
        }

        private static string AskforShot()
        {
            Console.Write("Please enter your shot selection: ");
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
                    Console.Write($" {gridSpot.SpotLetter}{ gridSpot.SpotNumber} ");
                }
                else if (gridSpot.Status == Enums.GridSpotStatus.Hit)
                {
                    Console.Write("X");
                }
                else if (gridSpot.Status == Enums.GridSpotStatus.Miss)
                {
                    Console.Write("O");
                }
                else
                {
                    Console.Write("?");
                }
            }
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");

            // Ask the user for their name
            output.UserName = GetUsersName($"Enter {playerTitle} name");

            // Load up the shot grid
            GameLogic.InitializeGrid(output);

            // Ask the user for their 5 ship placements
            PlaceShips(output);

            // Clear the screen
            Console.Clear();

            return output;
        }

        // Implemented by Rohit -- commenting out for now
        /* private static void DisplayGrid(PlayerInfoModel playerInfo)
         {
             Console.WriteLine("Ship locations");
             foreach (var item in playerInfo.ShipLocations)
             {
                 Console.Write(item.SpotLetter + item.SpotNumber);
                 Console.Write(" ");
             }
             Console.WriteLine();
             Console.WriteLine();

             char[] letters = { 'X', 'A', 'B', 'C', 'D', 'E' };

             for (int i = 1; i < 6; i++)
             {
                 //Console.Write($"   {i}");
                 string t = i.ToString();

                 int padVal = 4;
                 if (t.Equals("1"))
                 {
                     padVal = 5;
                 }
                 Console.Write(t.PadLeft(padVal));
             }

             Console.WriteLine();


             for (int i = 1; i < 6; i++)
             {
                 Console.Write(letters[i].ToString());

                 int prevSpotNum = 0;
                 foreach (var item in playerInfo.ShipLocations)
                 {
                     if (letters[i].ToString().Equals(item.SpotLetter.ToString()))
                     {
                         if (item.SpotNumber < prevSpotNum)
                         {
                             Console.Write("x".PadRight(Math.Abs((item.SpotNumber * 4) - (prevSpotNum * 4))));
                         }
                         else
                         {
                             Console.Write("x".PadLeft(Math.Abs((item.SpotNumber * 4) - (prevSpotNum * 4))));
                             prevSpotNum = item.SpotNumber;
                         }
                     }
                 }
                 Console.WriteLine();
             }

         } */

        private static void DisplayWelcomeMessage()
        {
            Console.WriteLine("### Welcome to Battleship Lite ### \n");
        }

        private static string GetUsersName(string message)
        {
            Console.WriteLine(message);
            string name = Console.ReadLine();
            return name;
        }

        public static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}: ");

                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.PlaceShip(model, location);

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again");
                }
            } while (model.ShipLocations.Count < 6);
        }

        /* private static void GetShipLocations(List<GridSpotModel> shipLocations)
         {
             Console.WriteLine("Enter your ship locations, (example: A5)");

             int i = 1;
             do
             {
                 Console.WriteLine($"Enter ship { i } location");
                 string shipLoc = Console.ReadLine();
                 if (IsvalidSpot(shipLoc, shipLocations))
                 {
                     GridSpotModel shipPlacement = new GridSpotModel();
                     shipPlacement.SpotLetter = shipLoc[0].ToString();
                     shipPlacement.SpotNumber = int.Parse(shipLoc[1].ToString());
                     shipLocations.Add(shipPlacement);
                     Console.Clear();
                     i++;
                 }
                 else
                 {
                     Console.WriteLine($"Spot {shipLoc} is taken, please enter a different location");
                 }
             } while (i < 6);

         } */

        /* private static bool IsvalidSpot(string spot, List<GridSpotModel> shipLocations)
         {
             string spotLetter = spot[0].ToString();
             int spotNumber = int.Parse(spot[1].ToString());

             foreach (var item in shipLocations)
             {
                 if (item.SpotLetter == spotLetter && item.SpotNumber == spotNumber)
                 {
                     return false;
                 }
             }
             return true;
         } 
        */

        private static void DisplayShotGrid()
        {

        }
    }
}
