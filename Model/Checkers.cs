using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.SqlClient;
using System.IO;


namespace BionicFinalProject.Model
{
    [Serializable]
    public class Checkers : INotifyPropertyChanged
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
        string upperPanelText;
        public string UpperPanelText
        {
            get
            {
                return upperPanelText;
            }
            set
            {
                upperPanelText = value;
                OnPropertyChanged("UpperPanelText");
            }
        }

        public Tile[] tiles { get; set; }
        public bool currentPlayer { get; set; } // true = 1 (white), false = 2 (black)
        bool JumpInd { get; set; }
        int clickedTile { get; set; }
        bool onceClicked { get; set; }
        Step currentStep { get; set; }
        public List<Step> progress { get; set; }
        string enemyName;
        public string EnemyName
        {
            get
            {
                return enemyName;
            }
            set
            {
                enemyName = value;
                OnPropertyChanged("EnemyName");
            }
        }
        bool isWinner { get; set; }
        string playerName;
        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
                OnPropertyChanged("PlayerName");
            }
        }
        bool Winner; // true = 1 (white), false = 2 (black)
        public bool yourCheckers; // true = 1 (white), false = 2 (black)
        public bool isTwoPlayers  { get; set; }
        public bool isNet { get; set; }
        public bool isAI { get; set; }
        public string Hostname { get; set; }
        public string Port { get; set; }
        string getTCPstring;


        string[] GoldWay = { "a1", "b2", "c3", "d4", "e5", "f6", "g7", "h8" }; // диагональ
        string[] DoubleWayG1A7 = { "g1", "f2", "e3", "d4", "c5", "b6", "a7" }; //Двойники
        string[] DoubleWayH2B8 = { "h2", "g3", "f4", "e5", "d6", "c7", "b8" };
        string[] TripleWayC1A3 = { "c1", "b2", "a3" }; //Тройники
        string[] TripleWayC1H6 = { "c1", "d2", "e3", "f4", "g5", "h6" };
        string[] TripleWayH6F8 = { "h6", "g7", "f8" };
        string[] TripleWayA3F8 = { "a3", "b4", "c5", "d6", "e7", "f8" };
        string[] UltraWayA5D8 = { "a5", "b6", "c7", "d8" }; //Косяки
        string[] UltraWayH4D8 = { "h4", "g5", "f6", "e7", "d8" };
        string[] UltraWayE1A5 = { "e1", "d2", "c3", "b4", "a5" };
        string[] UltraWayE1H4 = { "e1", "f2", "g3", "h4" };

        public Checkers()
        {
            tiles = new Tile[32];
            for (int i = 0; i < 32; i++)
            {
                tiles[i] = new Tile(i + 1);
            }
            JumpInd = false;
            currentPlayer = true;
            onceClicked = false;
            isTwoPlayers = true;
            isNet = false;
            isAI = false;
            progress = new List<Step>();
            currentStep = new Step();
            
            isWinner = false;
            Port = "2112";            
            enemyName = Properties.Resources.Player2;
            RefreshTileTexture();
        }

        public bool Click(int tile)
        {
            bool result = false;
            if (isTwoPlayers || (currentPlayer == yourCheckers))
            {
                tile--;

                if (onceClicked)
                {
                    result = Click2(tile, true);
                    if (result && !isTwoPlayers)
                    {
                        //ListenData();
                    }
                }
                else
                    result = Click1(tile, true);

                RefreshTileTexture();
            }
            else
            {
                MessageBox.Show(Properties.Resources.WaitYourTurn);
            }

            return result;
        } // добавить проверку и вывод выигрыша

        public bool Click1(int tile, bool mess)
        {
            for (int i = 0; i < 32; i++)
            {
                tiles[i].Border = false;
            }
            if ((currentPlayer && tiles[tile].Color == 1) || (!currentPlayer && tiles[tile].Color == 2))
            {
                int  beat = CheckBeat();
                if (JumpInd)
                {
                    if (!tiles[tile].CanBeat)
                    {
                        if (mess)
                        MessageBox.Show(Properties.Resources.MustAttack);

                        return false;
                    }
                    else
                    {

                        CheckBeatTile(tile);
                        clickedTile = tile;
                        onceClicked = true;
                        currentStep.Previous = tile;
                        return true;
                    }
                }
                else
                {
                    bool availMoves = CheckAvailableMoves(tile);
                    if (availMoves)
                    {
                        clickedTile = tile;
                        onceClicked = true;
                        currentStep.Previous = tile;
                        return true;
                    }
                    else
                    {
                        if (mess)
                        MessageBox.Show(Properties.Resources.ErrorHasNoMoves);
                        return false;
                    }
                }
                

            }
            else
            {
                if (mess)
                MessageBox.Show(Properties.Resources.ErrorNotYourCheckers);
                return false;
            }
        }
        public bool Click2(int tile, bool mess)
        {
            if (tiles[tile].Border)
            {
                currentStep.CurrentPlayer = currentPlayer;
                currentStep.Final = tile;
                ChangeTiles(clickedTile, tile);
                CheckQueen(tile);
                onceClicked = false;
                for (int i = 0; i < 32; i++)
                {
                    tiles[i].Border = false;
                }

                if (JumpInd)
                {
                    JumpInd = false;
                    int beaten = FindBeatenTile(clickedTile, tile);
                    currentStep.Kicked = beaten;
                    if (tiles[beaten].Queen)
                        currentStep.WasQueen = true;
                    else
                        currentStep.WasQueen = false;
                    tiles[beaten].Color = 0;
                    tiles[beaten].Queen = false;
                    int beat = CheckBeatTile(tile);
                    if (beat > 0)
                    {
                        JumpInd = true;
                        onceClicked = true;
                        clickedTile = tile;
                        RefreshTileTexture();
                        if (isNet) SendData(currentStep.Previous, currentStep.Final);
                        AddStep(progress, currentStep);
                        currentStep.Previous = tile;
                    }
                    else
                    {
                        currentPlayer = !currentPlayer;
                        if (isNet) SendData(currentStep.Previous, currentStep.Final);
                        AddStep(progress, currentStep);
                    }

                }
                else
                {
                    currentPlayer = !currentPlayer;
                    if (isNet) SendData(currentStep.Previous, currentStep.Final);
                    AddStep(progress, currentStep);
                }

                if ((isAI) && (currentPlayer != yourCheckers))
                {
                    
                    Computer tmp = new Computer((Checkers)Clone());
                    Move move = tmp.play();
                    if (move != null)
                        Move(move);
                    else MessageBox.Show(Properties.Resources.ICanNotMove);
                }
                else
                {
                    CheckWin();
                }
                
                return true;

            }
            else
            {
                if (((tiles[tile].Color == 1) && (currentPlayer)) || ((tiles[tile].Color == 2) && (!currentPlayer)))
                {
                    for (int i = 0; i < 32; i++)
                    {
                        tiles[i].Border = false;
                    }
                    Click1(tile, mess);
                    return true;
                }
                else
                {
                    if (mess)
                    MessageBox.Show(Properties.Resources.ErrorCanNotMove);
                    return false;
                }
            }
        }
        private void AddStep(List<Step> list, Step currentStep)
        {
            list.Add(new Step()
            {
                Previous = currentStep.Previous,
                Final = currentStep.Final,
                Kicked = currentStep.Kicked,
                CurrentPlayer = currentStep.CurrentPlayer,
                WasQueen = currentStep.WasQueen
            });
        }
        private int FindBeatenTile(int previousState, int currentState)
        {
            int result = 0;
            int resultNumber;
            char resultLetter;
            string resultName;
            char letter1 = tiles[previousState].Name[0];
            int number1 = Convert.ToInt32(tiles[previousState].Name.Substring(1));
            char letter2 = tiles[currentState].Name[0];
            int number2 = Convert.ToInt32(tiles[currentState].Name.Substring(1));
            if (number2 > number1)
                resultNumber = number2 - 1;
            else
                resultNumber = number2 + 1;
            if ((int)letter2 > (int)letter1)
                resultLetter = (char)((int)(letter2) - 1);
            else
                resultLetter = (char)((int)(letter2) + 1);
            resultName = resultLetter.ToString() + resultNumber.ToString();
            result = FindTile(resultName);
            return result;
        }

        private bool CheckAvailableMoves(int tile) // обозначим доступные ходы 
        {
            bool result = false;
            int moves = 0;
            char letter = tiles[tile].Name[0];
            int number = Convert.ToInt32(tiles[tile].Name.Substring(1));
            if (currentPlayer)
            {
                string possibleStep = "";
                if ((letter != 'h') && number < 8)
                {
                    possibleStep += (char)((int)letter + 1);
                    possibleStep += (number + 1);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                }
                possibleStep = "";
                if ((letter != 'a') && number < 8)
                {
                    possibleStep += (char)((int)letter - 1);
                    possibleStep += (number + 1);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                }

                

            }
            else
            {
                string possibleStep = "";
                if ((letter != 'h') && number > 1)
                {
                    possibleStep += (char)((int)letter + 1);
                    possibleStep += (number - 1);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                }
                possibleStep = "";
                if ((letter != 'a') && number > 1)
                {
                    possibleStep += (char)((int)letter - 1);
                    possibleStep += (number - 1);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                }
            }
            if (tiles[tile].Queen)
            {
                int k = 0;
                string possibleStep = "";
                while (letter != 'a' && number < 8)
                {
                    k++;
                    possibleStep = "";
                    possibleStep += (char)((int)letter - k);
                    possibleStep += (number + k);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                    else break;
                }
                k = 0;
                possibleStep = "";
                while (letter != 'a' && number > 1)
                {
                    k++;
                    possibleStep = "";
                    possibleStep += (char)((int)letter - k);
                    possibleStep += (number - k);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                    else break;
                }
                k = 0;
                possibleStep = "";
                while (letter != 'h' && number < 8)
                {
                    k++;
                    possibleStep = "";
                    possibleStep += (char)((int)letter + k);
                    possibleStep += (number + k);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                    else break;
                }
                k = 0;
                possibleStep = "";
                while (letter != 'h' && number > 1)
                {
                    k++;
                    possibleStep = "";
                    possibleStep += (char)((int)letter + k);
                    possibleStep += (number - k);

                    if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                        moves++;
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                    else break;
                }
            }
            if (moves == 0)
                result = false;
            else
                result = true;

            return result;
        }
        private int CheckBeatTile(int tile) // обозначим доступное направление атаки 
        {
            int result = 0;
            char letter = tiles[tile].Name[0];
            int number = Convert.ToInt32(tiles[tile].Name.Substring(1));
            if (tiles[tile].Queen)
            {
                int enemy;
                if (currentPlayer)
                { enemy = 2; }
                else
                { enemy = 1; }
                int k = 0;
                string possibleStep = "";
                char tmpLetter = letter;
                int tmpNumber = number;
                bool tmpcanBeat = true;
                while (tmpLetter != 'a' && tmpLetter != 'b' && tmpNumber < 7)
                {
                    k++;
                    tmpLetter = (char)((int)letter - k);
                    tmpNumber = number + k;
                    possibleStep = tmpLetter + tmpNumber.ToString();

                    if (tiles[FindTile(possibleStep)].Color == enemy)
                    {
                        possibleStep = "";
                        possibleStep += (char)((int)letter - (k + 1));
                        possibleStep += (number + (k + 1));
                        if (tiles[FindTile(possibleStep)].Color == 0)
                        tiles[FindTile(possibleStep)].Border = true;
                    }
                    else if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                    }
                    else
                    {
                        tmpcanBeat = false;
                        break;
                    }
                    
                }
                k = 0;
                possibleStep = "";
                tmpLetter = letter;
                tmpNumber = number;
                while (tmpLetter != 'a' && tmpLetter != 'b' && tmpNumber > 2)
                {
                    k++;
                    tmpLetter = (char)((int)letter - k);
                    tmpNumber = number - k;
                    possibleStep = tmpLetter + tmpNumber.ToString();

                    if (tiles[FindTile(possibleStep)].Color == enemy)
                    {
                        possibleStep = "";
                        possibleStep += (char)((int)letter - (k + 1));
                        possibleStep += (number - (k + 1));
                        if (tiles[FindTile(possibleStep)].Color == 0)
                            tiles[FindTile(possibleStep)].Border = true;
                    }
                    else if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                    }
                    else
                    {
                        tmpcanBeat = false;
                        break;
                    }
                }
                k = 0;
                possibleStep = "";
                tmpLetter = letter;
                tmpNumber = number;
                while (tmpLetter != 'h' && tmpLetter != 'g' && tmpNumber < 7)
                {
                    k++;
                    tmpLetter = (char)((int)letter + k);
                    tmpNumber = number + k;
                    possibleStep = tmpLetter + tmpNumber.ToString();

                    if (tiles[FindTile(possibleStep)].Color == enemy)
                    {
                        possibleStep = "";
                        possibleStep += (char)((int)letter + (k + 1));
                        possibleStep += (number + (k + 1));
                        if (tiles[FindTile(possibleStep)].Color == 0)
                            tiles[FindTile(possibleStep)].Border = true;
                    }
                    else if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                    }
                    else
                    {
                        tmpcanBeat = false;
                        break;
                    }
                }
                k = 0;
                possibleStep = "";
                tmpLetter = letter;
                tmpNumber = number;
                while (tmpLetter != 'h' && tmpLetter != 'g' && tmpNumber > 2)
                {
                    k++;
                    tmpLetter = (char)((int)letter + k);
                    tmpNumber = number - k;
                    possibleStep = tmpLetter + tmpNumber.ToString();

                    if (tiles[FindTile(possibleStep)].Color == enemy)
                    {
                        possibleStep = "";
                        possibleStep += (char)((int)letter + (k + 1));
                        possibleStep += (number - (k + 1));
                        if (tiles[FindTile(possibleStep)].Color == 0)
                            tiles[FindTile(possibleStep)].Border = true;
                    }
                    else if (tiles[FindTile(possibleStep)].Color == 0)
                    {
                    }
                    else
                    {
                        tmpcanBeat = false;
                        break;
                    }

                }
            }
            else
            {
                if (currentPlayer)
                {
                    string possibleStep = "";
                    if ((letter != 'g' && letter != 'h') && number < 7)
                    {
                        possibleStep += (char)((int)letter + 1);
                        possibleStep += (number + 1);

                        if (tiles[FindTile(possibleStep)].Color == 2)
                        {
                            possibleStep = "";
                            possibleStep += (char)((int)letter + 2);
                            possibleStep += (number + 2);
                            if (tiles[FindTile(possibleStep)].Color == 0)
                            {
                                tiles[FindTile(possibleStep)].Border = true;
                                result++;
                            }
                        }
                    }
                    possibleStep = "";
                    if ((letter != 'a' && letter != 'b') && number < 7)
                    {
                        possibleStep += (char)((int)letter - 1);
                        possibleStep += (number + 1);

                        if (tiles[FindTile(possibleStep)].Color == 2)
                        {
                            possibleStep = "";
                            possibleStep += (char)((int)letter - 2);
                            possibleStep += (number + 2);
                            if (tiles[FindTile(possibleStep)].Color == 0)
                            {
                                tiles[FindTile(possibleStep)].Border = true;
                                result++;
                            }
                        }
                    }
                    possibleStep = "";


                }
                else
                {
                    string possibleStep = "";
                    if ((letter != 'g' && letter != 'h') && number > 2)
                    {
                        possibleStep += (char)((int)letter + 1);
                        possibleStep += (number - 1);

                        if (tiles[FindTile(possibleStep)].Color == 1)
                        {
                            possibleStep = "";
                            possibleStep += (char)((int)letter + 2);
                            possibleStep += (number - 2);
                            if (tiles[FindTile(possibleStep)].Color == 0)
                            {
                                tiles[FindTile(possibleStep)].Border = true;
                                result++;
                            }
                        }
                    }
                    possibleStep = "";
                    if ((letter != 'a' && letter != 'b') && number > 2)
                    {
                        possibleStep += (char)((int)letter - 1);
                        possibleStep += (number - 1);

                        if (tiles[FindTile(possibleStep)].Color == 1)
                        {
                            possibleStep = "";
                            possibleStep += (char)((int)letter - 2);
                            possibleStep += (number - 2);
                            if (tiles[FindTile(possibleStep)].Color == 0)
                            {
                                tiles[FindTile(possibleStep)].Border = true;
                                result++;
                            }
                        }
                    }

                }
            }
            return result;
        }
        private int CheckBeat()  // общая проверка возможности атаки
        {
            int tmp = 0;
            bool tmp2 = false;
            tmp2 = CheckDiagon(GoldWay);
            if (tmp2) tmp = 1;
            tmp2 = false;
            tmp2 = CheckDiagon(DoubleWayG1A7);
            if (tmp2) tmp = 2;
            tmp2 = false;
            tmp2 = CheckDiagon(DoubleWayH2B8);
            if (tmp2) tmp = 3;
            tmp2 = false;
            tmp2 = CheckDiagon(TripleWayC1A3);
            if (tmp2) tmp = 4;
            tmp2 = false;
            tmp2 = CheckDiagon(TripleWayC1H6);
            if (tmp2) tmp = 5;
            tmp2 = false;
            tmp2 = CheckDiagon(TripleWayH6F8);
            if (tmp2) tmp = 6;
            tmp2 = false;
            tmp2 = CheckDiagon(TripleWayA3F8);
            if (tmp2) tmp = 7;
            tmp2 = false;
            tmp2 = CheckDiagon(UltraWayA5D8);
            if (tmp2) tmp = 8;
            tmp2 = false;
            tmp2 = CheckDiagon(UltraWayH4D8);
            if (tmp2) tmp = 9;
            tmp2 = false;
            tmp2 = CheckDiagon(UltraWayE1A5);
            if (tmp2) tmp = 10;
            tmp2 = false;
            tmp2 = CheckDiagon(UltraWayE1H4);
            if (tmp2) tmp = 11;
            return tmp;

        }
        private bool CheckWin()
        {
            bool result = false;
            int whiteCheckers = 0;
            int blackCheckers = 0;
            for (int i = 0; i < 32; i++)
			{
			    if (tiles[i].Color == 1)
                    whiteCheckers++;
                else if (tiles[i].Color == 2)
                    blackCheckers++;
			}
            if (whiteCheckers == 0)
            {
                Winner = false;
                isWinner = true;
                result = true;
                MessageBox.Show(Properties.Resources.BlackWon);
                NewGame();
            }
            if (blackCheckers == 0)
            {
                Winner = true;
                isWinner = true;
                result = true;
                MessageBox.Show(Properties.Resources.WhiteWon);
                NewGame();
            }
            


            return result;

        }
        private bool CheckDiagon(string[] diagon) //проверка диагонали на возможность атаки
        {
            bool result = false;            
            if (currentPlayer)
            {
                for (int i = 0; i < diagon.Length - 2; i++)
                {
                    if (tiles[FindTile(diagon[i])].Color == 1)
                    {

                        if (tiles[FindTile(diagon[i + 1])].Color == 2)
                        {
                            if (tiles[FindTile(diagon[i + 2])].Color == 0)
                            {
                                result = true;
                                tiles[FindTile(diagon[i])].CanBeat = true;
                                JumpInd = true;
                            }
                        }

                        
                    }
                }
                for (int i = 0; i < diagon.Length; i++)
                {
                    int tmpCounter = 0;
                    if (tiles[FindTile(diagon[i])].Color == 1 && tiles[FindTile(diagon[i])].Queen)
                    {
                        for (int j = 0; j < diagon.Length; j++)
                        {
                            if (tiles[FindTile(diagon[j])].Color == 2)
                            {
                                bool tmpCanBeat = true;
                                if (i > j) tmpCounter = -1;
                                if (j > i) tmpCounter = 1;
                                for (int k = i + tmpCounter; k != j; k = k + tmpCounter)
                                {
                                    if (tiles[FindTile(diagon[k])].Color != 0)
                                        tmpCanBeat = false;
                                }
                                if (tmpCanBeat)
                                {
                                    try
                                    {
                                        if (tiles[FindTile(diagon[j + tmpCounter])].Color == 0)
                                        {
                                            result = true;
                                            tiles[FindTile(diagon[i])].CanBeat = true;
                                            JumpInd = true;
                                        }
                                    }
                                    catch 
                                    {
                                        
                                    }
                                }
                            }
                        }
                    }
                }
			
            }
            else
            {
                for (int i = diagon.Length - 1; i > 1; i--)
                {
                    if (tiles[FindTile(diagon[i])].Color == 2)
                    {
                        if (tiles[FindTile(diagon[i - 1])].Color == 1)
                        {
                            if (tiles[FindTile(diagon[i - 2])].Color == 0)
                            {
                                result = true;
                                tiles[FindTile(diagon[i])].CanBeat = true;
                                JumpInd = true;
                            }
                        }

                    }
                }
                for (int i = 0; i < diagon.Length; i++)
                {
                    int tmpCounter = 0;
                    if (tiles[FindTile(diagon[i])].Color == 2 && tiles[FindTile(diagon[i])].Queen)
                    {
                        for (int j = 0; j < diagon.Length; j++)
                        {
                            if (tiles[FindTile(diagon[j])].Color == 1)
                            {
                                bool tmpCanBeat = true;
                                if (i>j) tmpCounter = -1;
                                if (j>i) tmpCounter = 1;
                                for (int k = i; k != j; k = k + tmpCounter)
                                {
                                    if (tiles[FindTile(diagon[k])].Color != 0)
                                        tmpCanBeat = false;
                                }
                                if (tmpCanBeat)
                                {
                                    try
                                    {
                                        if (tiles[FindTile(diagon[j + tmpCounter])].Color == 0)
                                        {
                                            result = true;
                                            tiles[FindTile(diagon[i])].CanBeat = true;
                                            JumpInd = true;
                                        }
                                    }
                                    catch 
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void ChangeTiles(int previousState, int currentState)
        {
            Tile tmp = new Tile(previousState);
            tmp.Queen = tiles[currentState].Queen;
            tmp.Color = tiles[currentState].Color;

            tiles[currentState].Color = tiles[previousState].Color;
            tiles[currentState].Queen = tiles[previousState].Queen;
            tiles[previousState].Color = tmp.Color;
            tiles[previousState].Queen = tmp.Queen;


        }
        private void CheckQueen(int tile)
        {
            if (currentPlayer)
            {
                if (tile > 27)
                {
                    tiles[tile].Queen = true;
                }
            }
            else
            {
                if (tile < 4)
                {
                    tiles[tile].Queen = true;
                }
            }
        }
        int FindTile(string name)
        {
            int k = 0;
            bool found = false;
            for (int i = 0; i < 32; i++)
            {
                if (tiles[i].Name == name)
                {
                    k = i;
                    found = true;
                    break;
                }
            }
            if (found) return k;
            else throw new Exception(name);
        }

        public void RefreshTileTexture()
        {
            for (int i = 0; i < 32; i++)
            {
                string txt = @"/BionicFinalProject;component/Media/";
                
                if (tiles[i].Color == 1)
                    txt += "White";
                else if (tiles[i].Color == 2)
                    txt += "Black";
                if (tiles[i].Queen)
                    txt += "Queen";

                if (tiles[i].Color == 0)
                    txt = @"/BionicFinalProject;component/Media/Empty";

                txt += ".gif";
                if (tiles[i].Color == 0 && tiles[i].Border)
                    txt = @"/BionicFinalProject;component/Media/Selected.png";

                tiles[i].TextureName = txt;


            }
            
            if (currentPlayer == yourCheckers)
                if (currentPlayer)
                {
                    UpperPanelText = Properties.Resources.WhiteTurn + playerName;
                }
                else
                {
                    UpperPanelText = Properties.Resources.BlackTurn + enemyName;
                }
            else
                if (currentPlayer)
                {
                    UpperPanelText = Properties.Resources.WhiteTurn + enemyName;
                }
                else
                {
                    UpperPanelText = Properties.Resources.BlackTurn + playerName;
                }
            
        }

        public bool SaveGame()
        {
            bool result = false;
            string filename = "";
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Saved game"; // Default file name
            dlg.DefaultExt = ".sch"; // Default file extension
            dlg.Filter = "Saved checkers game (.sch)|*.sch"; // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result1 = false;
            result1 = dlg.ShowDialog();

            // Process save file dialog box results 
            
                filename = dlg.FileName;
            
            if (filename != "")
            {
                SavingData sd = new SavingData();
                sd.progress = progress;
                sd.yourCheckers = yourCheckers;
                sd.isAI = isAI;
                sd.isNet = isNet;
                if (isNet)
                    MessageBox.Show(Properties.Resources.SavedGameIsOnline);
                sd.isTwoPlayers = isTwoPlayers;
                sd.currentPlayer = currentPlayer;
                sd.JumpInd = JumpInd;
                for (int i = 0; i < 32; i++)
                {
                    sd.color[i] = tiles[i].Color;
                    sd.queen[i] = tiles[i].Queen;
                }

                sd.Serialize(filename);

                result = true;
            }
            


            return result;
        }
        public bool LoadGame()
        {
            bool result = false;
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Saved game"; // Default file name
            dlg.DefaultExt = ".sch"; // Default file extension
            dlg.Filter = "Saved checkers game (.sch)|*.sch"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result1 = dlg.ShowDialog();

            // Process open file dialog box results 

            filename = dlg.FileName;


            SavingData sd = new SavingData();
            sd = sd.Deserialize(filename);
            try
            {
                progress = sd.progress;
                currentPlayer = sd.currentPlayer;
                JumpInd = sd.JumpInd;
                sd.yourCheckers = yourCheckers;
                isAI = sd.isAI;
                isNet = false;
                isTwoPlayers = sd.isTwoPlayers;
                if (sd.isNet)
                {
                    isTwoPlayers = true;
                    MessageBox.Show(Properties.Resources.SavedGameIsOnline);
                }
                for (int i = 0; i < 32; i++)
                {
                    tiles[i].Color = sd.color[i];
                    tiles[i].Queen = sd.queen[i];
                }

                RefreshTileTexture();
                result = true;
            }
            catch
            {
                MessageBox.Show("Error che");
            }

            return result;
        }

        public bool ReturnStep()
        {
            bool result = false;
            if (!isNet)
            {
                if (progress.Count > 0)
                {
                    Step prev = progress[progress.Count - 1];
                    ChangeTiles(prev.Previous, prev.Final);
                    if (prev.Kicked != 0)
                    {
                        if (prev.CurrentPlayer)
                            tiles[prev.Kicked].Color = 2;
                        else
                            tiles[prev.Kicked].Color = 1;
                        tiles[prev.Kicked].Queen = prev.WasQueen;
                    }
                    currentPlayer = prev.CurrentPlayer;
                    progress.RemoveAt(progress.Count - 1);
                    result = true;
                    RefreshTileTexture();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CanNotReturnStep);
                    result = false;
                }

            }
            else
            {
                MessageBox.Show("Нельзя отменить ход в сетевой игре");
            }
            return result;
        }

        public object Clone()
        {
            Checkers board = new Checkers();

            board.currentPlayer = currentPlayer;
            for (int i = 0; i < 32; i++)
                board.tiles[i] = new Tile(i + 1)
                {
                    Color = this.tiles[i].Color,
                    Queen = this.tiles[i].Queen,
                };
            board.RefreshTileTexture();

            return board;
        }
        public List<Move> LegalMoves()
        {
            List<Move> moves = new List<Move>();
           
            for (int i = 0; i < 32; i++)
            {
                if (Click1(i, false))
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (tiles[j].Border)
                        {
                            moves.Add(new Move(i,j));
                        }
                    }
                }
            }




            return moves;
        }
        public bool Move(Move move)
        {
            bool result = false;
            result = Click1(move.from, false);
            if (result)
                result = Click2(move.to, false);


            return result;
        }
        public byte GetPiece(int pos)
        {
            byte result = 0;
            if (pos < 0 || pos > 32)
                throw new Exception();
            result = (byte)(tiles[pos].Color * 2);
            if (result != 0 && tiles[pos].Queen) result++;
            return result;
        }
        
        public bool HasEnded()
        {
            int whitePieces = 0;
            int blackPieces = 0;
            for (int i = 0; i < 32; i++)
            {
                if (tiles[i].Color == 1)
                    whitePieces++;
                if (tiles[i].Color == 2)
                    blackPieces++;
            }
            return whitePieces == 0 || blackPieces == 0;
        }

        public void NewGame()
        {
            NewGame(true);
        }
        public void NewGame(bool currentPlayer)
        {
            for (int i = 0; i < 32; i++)
            {
                tiles[i].NewGame(i + 1);
            }
            JumpInd = false;
            yourCheckers = currentPlayer;
            onceClicked = false;
            RefreshTileTexture();
            progress = new List<Step>();
            currentStep = new Step();
            currentPlayer = true;
            isWinner = false;
            if ((isAI) && (currentPlayer != yourCheckers))
            {

                Computer tmp = new Computer((Checkers)Clone());
                Move move = tmp.play();
                if (move != null)
                    Move(move);
                else MessageBox.Show(Properties.Resources.ICanNotMove);
            }
        }

        public bool GetData()
        {
            bool result = false;
            string inputData = "";
            if (isNet)
            {
                inputData = GetDataTCP();
            }
            else if (isAI)
            {
            }
            string inputPrev = inputData.Substring(0, 2);
            string inputFin = inputData.Substring(3);
            if (FindTile(inputFin) > 0 && FindTile(inputFin) < 33)
            {
                Click1(FindTile(inputPrev), false);
                Click2(FindTile(inputFin), false);
            }
            else
            {
                MessageBox.Show("Ошибка синхронизации");
            }
            return result;
        }

        public string GetDataTCP() // РАЗОБРАТЬСЯ
        {
            getTCPstring = "";
            int k = 0;
            Thread thread = new Thread(new ThreadStart(ListenDataTCP));
            thread.Start();
            do
            {
                Thread.Sleep(1000);
                k++;
                if (k > 120)
                    MessageBox.Show("Возможно, пропала связь... Или противник заснул");

            }
            while (getTCPstring == "");
            return getTCPstring;
        }
        public void ListenDataTCP() // запускать в отдельном потоке
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            Int32 port = 2112;
            TcpListener tcpListener = new TcpListener(localAddr, port);
            tcpListener.Start();
            TcpClient tcpClient = tcpListener.AcceptTcpClient();
            NetworkStream ns = tcpClient.GetStream();
            StreamReader sr = new StreamReader(ns);
            string result = sr.ReadToEnd();
            tcpClient.Close();
            tcpListener.Stop();
            getTCPstring = result;
            //return result;
        }
        public bool SendData(int previous, int final)
        {
            bool result = false;
            try
            {
                TcpClient tcpclient = new TcpClient(Hostname, Int32.Parse(Port));
                NetworkStream ns = tcpclient.GetStream();
                string sendDataString = previous.ToString() + ";" + final.ToString();
                var sendData = Encoding.Default.GetBytes(sendDataString);
                ns.Write(sendData, 0, sendData.Length);
                ns.Close();
                result = true;
            }
            catch
            {
                result = false;
                ReturnStep();
                MessageBox.Show(Properties.Resources.ConnectionError);
            }
            return result;
        }

        public bool Connect(string ip)
        {
            bool result = false;
            Hostname = ip;
            try
            {
                TcpClient tcpclient = new TcpClient(Hostname, Int32.Parse(Port));
                NetworkStream ns = tcpclient.GetStream();
                string sendDataString = yourCheckers.ToString() + ";" + playerName;
                var sendData = Encoding.Default.GetBytes(sendDataString);
                ns.Write(sendData, 0, sendData.Length);
                ns.Close();
                result = true;

                
            }
            catch
            {
                result = false;                
            }
            if (!result)
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                Int32 port = 2112;
                TcpListener tcpListener = new TcpListener(localAddr, port);
                tcpListener.Start();
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                NetworkStream ns1 = tcpClient.GetStream();
                StreamReader sr = new StreamReader(ns1);
                string result1 = null;
                result1 = sr.ReadToEnd();
                tcpClient.Close();
                tcpListener.Stop();
                getTCPstring = result1;
                if (result1 != null)
                    result = true;
            }
            catch
            {
                result = false; 
            }

            return result;
        }
        
    }
}
