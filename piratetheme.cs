using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class piratetheme
    {
        private string[] _theme = { 
            "\\images\\pirate\\", "\\sounds\\standard\\", //This is the paths to files 
            };

        private string[] _string = {
            "Place Ships onto Board, when done click ready",//0
            "Not all ships have been placed. Try again",//1
            "Attack",//2
            "Drag a peg onto board then click Attack",//3
            "The enemy hit your ",//4
            "Aircraft Carrier",//5
            "Battleship",//6
            "Destroyer",//7
            "Submarine",//8
            "Patrol Boat",//9
            "The enemy hit nothing but the open blue!",//10
            "You hit nothing but the open blue",//11
            "You the the enemy's ",//12
            "Remove peg",//13
            "Ready",//14
            "Does not fit"//15
            };

        public string[] Theme
        {
            get { return _theme; }
        }

        public string[] String
        {
            get { return _string; }
        }
    }
}
