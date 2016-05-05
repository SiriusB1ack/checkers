using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicFinalProject.Model
{
    class Player
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int GamePlayed { get; set; }
        public int GameWon { get; set; }
        public Player(string name, string pass)
        {
            Username = name;
            Password = pass;
            GamePlayed = 0;
            GameWon = 0;
        }
        public Player(string name, string pass, int gp, int gw)
        {
            Username = name;
            Password = pass;
            GamePlayed = gp;
            GameWon = gw;
        }
        public Player()
        {
            Username = Properties.Resources.Player1Guest;
            Password = "";
            GamePlayed = 0;
            GameWon = 0;
        }
    }
}
