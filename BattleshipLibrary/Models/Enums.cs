using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary.Models
{
    public class Enums
    {
        public enum GridSpotStatus
        {
            Empty,
            Ship,
            Hit,
            Miss,
            Sunk
        }
    }
}
