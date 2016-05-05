using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicFinalProject.Model
{
    [Serializable]
    public class Step
    {
        public int Previous { get; set; }
        public int Final { get; set; }
        public int Kicked { get; set; }
        public bool CurrentPlayer { get; set; }
        public bool WasQueen { get; set; }
        public string ToString()
        {
            string tmp = "";
            Tile PreviousTile = new Tile(Previous + 1);
            Tile FinalTile = new Tile(Final + 1);
            if (CurrentPlayer)
                tmp = "1: ";
            else tmp = "2: ";
            return (tmp + PreviousTile.Name + " - " + FinalTile.Name);
        }
        
    }
}
