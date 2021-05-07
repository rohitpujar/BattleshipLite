using BattleshipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (var letter in letters)
            {
                foreach (var number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel();
            spot.SpotLetter = letter;
            spot.SpotNumber = number;
            spot.Status = Enums.GridSpotStatus.Empty;

            model.ShotGrid.Add(spot);
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            // Iterate over all the ship locations. If we find 5 ships in Sunk state, then the game is over.
            bool isActive = false;

            foreach (var ship in player.ShipLocations)
            {
                if(ship.Status != Enums.GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }
        }

        // Ship location accepts any random location, ex B34. Fix it.
        // May 05 1:10 am  
        public static bool PlaceShip(PlayerInfoModel model, string location)
        {
            (string row, int column) = SplitShotIntoRowAndColumn(location);

            foreach (var gridSpot in model.ShipLocations)
            {
                if (gridSpot.SpotLetter == row && gridSpot.SpotNumber == column)
                {
                    return false;
                }
            }

            GridSpotModel newShip = new GridSpotModel();
            newShip.SpotLetter = row;
            newShip.SpotNumber = column;
            model.ShipLocations.Add(newShip);

            return true;
        }

        public static int GetShotCount(PlayerInfoModel winner)
        {
            throw new NotImplementedException();
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string row = shot[0].ToString();
            int column = int.Parse(shot[1].ToString());

            return (row, column);
        }

        public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int column)
        {
            // Check players shotgrid. If the spot is Empty, it was a valid shot.
            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter == row && gridSpot.SpotNumber == column)
                {
                    if (gridSpot.Status == Enums.GridSpotStatus.Empty || gridSpot.Status == Enums.GridSpotStatus.Ship)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            foreach (var gridSpot in opponent.ShipLocations)
            {
                if (gridSpot.SpotLetter == row && gridSpot.SpotNumber == column)
                {
                    if (gridSpot.Status == Enums.GridSpotStatus.Ship)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int column, bool isAHit)
        {
            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter == row && gridSpot.SpotNumber == column)
                {
                    if (isAHit)
                    {
                        gridSpot.Status = Enums.GridSpotStatus.Hit;
                    }
                    else
                    {
                        gridSpot.Status = Enums.GridSpotStatus.Miss;
                    }
                }
            }
        }
    }
}
