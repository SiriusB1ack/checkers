using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicFinalProject.Model
{
    class LocData
    {
        public string GameMenu { get; set; }
        public string LoadGame { get; set; }
        public string SaveGame { get; set; }
        public string LoginButton { get; set; }
        public string Password { get; set; }
        public string Quit { get; set; }
        public string RegisterButton { get; set; }
        public string Username { get; set; }
        public string NewGame { get; set; }
        public string ReturnStep { get; set; }
        public string ExitToMainMenu { get; set; }
        public string ProgressList { get; set; }
        public string LocalGame { get; set; }
        public string OnlineGame { get; set; }
        public string AIGame { get; set; }
        public string CheckWhite { get; set; }
        public string CheckBlack { get; set; }
        public string CheckMode { get; set; }
        public string CheckYourCheckers { get; set; }
        public string CheckIP { get; set; }
        public string CheckStartGame { get; set; }
        public string CheckLogOut { get; set; }
        public string RegisterBetter { get; set; }
        public string MenuStatistics { get; set; }
        public string GamePlayed { get; set; }
        public string GameWon { get; set; }
        public string MenuPlayer { get; set; }


        public LocData()
        {
            GameMenu = Properties.Resources.GameMenu;
            LoadGame = Properties.Resources.LoadGame;
            SaveGame = Properties.Resources.SaveGame;
            LoginButton = Properties.Resources.LoginButton;
            Password = Properties.Resources.Password;
            Quit = Properties.Resources.Quit;
            RegisterButton = Properties.Resources.RegisterButton;
            Username = Properties.Resources.Username;
            NewGame = Properties.Resources.NewGame;
            ReturnStep = Properties.Resources.ReturnStep;
            ExitToMainMenu = Properties.Resources.ExitToMainMenu;
            ProgressList = Properties.Resources.ProgressList;
            LocalGame = Properties.Resources.LocalGame;
            OnlineGame = Properties.Resources.OnlineGame;
            AIGame = Properties.Resources.AIGame;
            CheckWhite = Properties.Resources.CheckWhite;
            CheckBlack = Properties.Resources.CheckBlack;
            CheckMode = Properties.Resources.CheckMode;
            CheckYourCheckers = Properties.Resources.CheckYourCheckers;
            CheckIP = Properties.Resources.CheckIP;
            CheckStartGame = Properties.Resources.CheckStartGame;
            CheckLogOut = Properties.Resources.CheckLogOut;
            ProgressList = Properties.Resources.ProgressList;
            RegisterBetter = Properties.Resources.RegisterBetter;
            MenuStatistics = Properties.Resources.MenuStatistics;
            GamePlayed = Properties.Resources.GamePlayed;
            GameWon = Properties.Resources.GameWon;
            MenuPlayer = Properties.Resources.MenuPlayer;
        }
    }
}
