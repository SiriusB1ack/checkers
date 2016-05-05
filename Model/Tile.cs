using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace BionicFinalProject.Model
{
    
    public class Tile : INotifyPropertyChanged
    {
        #region Implement INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        string name;        
        public string Name 
        { 
            get
            {
                return name;
            }
            set
            {
                name = value;

            }
        }
        string textureName;
        public string TextureName
        {
            get
            {
                return textureName;
            }
            set
            {
                textureName = value;
                OnPropertyChanged("TextureName");
            }
        }

        int number;
         //1 - white, 2 - black, 0 - empty
        int color;
        public int Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }
        bool queen;
        public bool Queen
        {
            get
            {
                return queen;
            }
            set
            {
                queen = value;
                OnPropertyChanged("Queen");
            }
        }
        bool border;
        public bool Border
        {
            get
            {
                return border;
            }
            set
            {
                border = value;
                OnPropertyChanged("Border");
            }
        }
        public bool goldWay { get; set; }
        public bool doubleWay { get; set; }
        public bool tripleWay { get; set; }
        public bool ultraWay { get; set; } 
        public bool CanBeat { get; set; }

        public Tile(int number)
        {
            goldWay = false;
            doubleWay = false;
            tripleWay = false;
            ultraWay = false;
            this.number = number;
            name = ConvertNumberToName(number);
            if (number < 13) Color = 1;
            else if (number > 20) Color = 2;
            else Color = 0;

            Queen = false;
            Border = false;
            CanBeat = false;

        }
        public void NewGame(int number, bool firstPlayer)
        {            
            this.number = number;
            int upper = 2;
            int lower = 1;
            if (!firstPlayer)
            {
                upper = 1;
                lower = 2;
            }
            name = ConvertNumberToName(number);
            
            if (number < 13) Color = lower;
            else if (number > 20) Color = upper;
            else Color = 0;

            Queen = false;
            Border = false;
            CanBeat = false;
        }
        public void NewGame(int number)
        {
            NewGame(number, true);
        }

        
        private string ConvertNumberToName(int number)
        {
            switch (number)
            {
                case 1:
                    {
                        goldWay = true;
                        return "a1";
                    }
                case 2:
                    {
                        tripleWay = true;
                        return "c1";
                    }
                case 3:
                    {
                        ultraWay = true;
                        return "e1";
                    }
                case 4:
                    {
                        doubleWay = true;
                        return "g1";
                    }
                case 5:
                    {
                        goldWay = true;
                        tripleWay = true;
                        return "b2";
                    }
                case 6:
                    {
                        tripleWay = true;
                        ultraWay = true;
                        return "d2";
                    }
                case 7:
                    {
                        ultraWay = true;
                        doubleWay = true;
                        return "f2";
                    }
                case 8:
                    {
                        doubleWay = true;
                        return "h2";
                    }
                case 9:
                    {
                        tripleWay = true;
                        return "a3";
                    }
                case 10:
                    {
                        ultraWay = true;
                        goldWay = true;
                        return "c3";
                    }
                case 11:
                    {
                        tripleWay = true;
                        doubleWay = true;
                        return "e3";
                    }
                case 12:
                    {
                        ultraWay = true;
                        doubleWay = true;
                        return "g3";
                    }
                case 13:
                    {
                        ultraWay = true;
                        tripleWay = true;
                        return "b4";
                    }
                case 14:
                    {
                        doubleWay = true;
                        goldWay = true;
                        return "d4";
                    }
                case 15:
                    {
                        tripleWay = true;
                        doubleWay = true;
                        return "f4";
                    }
                case 16:
                    {
                        ultraWay = true;
                        return "h4";
                    }
                case 17:
                    {
                        ultraWay = true;
                        return "a5";
                    }
                case 18:
                    {
                        tripleWay = true;
                        doubleWay = true;
                        return "c5";
                    }
                case 19:
                    {
                        doubleWay = true;
                        goldWay = true;
                        return "e5";
                    }
                case 20:
                    {
                        ultraWay = true;
                        tripleWay = true;
                        return "g5";
                    }
                case 21:
                    {
                        ultraWay = true;
                        doubleWay = true;
                        return "b6";
                    }
                case 22:
                    {
                        tripleWay = true;
                        doubleWay = true;
                        return "d6";
                    }
                case 23:
                    {
                        ultraWay = true;
                        goldWay = true;
                        return "f6";
                    }
                case 24:
                    {
                        tripleWay = true;
                        return "h6";
                    }
                case 25:
                    {
                        doubleWay = true;
                        return "a7";
                    }
                case 26:
                    {
                        ultraWay = true;
                        doubleWay = true;
                        return "c7";
                    }
                case 27:
                    {
                        ultraWay = true;
                        tripleWay = true;
                        return "e7";
                    }
                case 28:
                    {
                        tripleWay = true;
                        goldWay = true;
                        return "g7";
                    }
                case 29:
                    {
                        doubleWay = true;
                        return "b8";
                    }
                case 30:
                    {
                        ultraWay = true;
                        return "d8";
                    }
                case 31:
                    {
                        tripleWay = true;
                        return "f8";
                    }
                case 32:
                    {
                        goldWay = true;
                        return "h8";
                    }
                default:
                    {
                        return null;
                    }
            }
        }

    }
}
