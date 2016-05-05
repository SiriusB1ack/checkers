using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BionicFinalProject.Model;
using BionicFinalProject.View;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace BionicFinalProject.ViewModel
{
    class MainViewModel
    {
        public ICollectionView Languages { get; private set; }
        public Checkers CheckersGame { get; set; }
        public Login LoginGame { get; set; }
        public LocData LocalizData { get; set; }
        public ICommand ClickCommand1 { get; set; }
        public ICommand ClickCommand2 { get; set; }
        public ICommand ClickCommand3 { get; set; }
        public ICommand ClickCommand4 { get; set; }
        public ICommand ClickCommand5 { get; set; }
        public ICommand ClickCommand6 { get; set; }
        public ICommand ClickCommand7 { get; set; }
        public ICommand ClickCommand8 { get; set; }
        public ICommand ClickCommand9 { get; set; }
        public ICommand ClickCommand10 { get; set; }
        public ICommand ClickCommand11 { get; set; }
        public ICommand ClickCommand12 { get; set; }
        public ICommand ClickCommand13 { get; set; }
        public ICommand ClickCommand14 { get; set; }
        public ICommand ClickCommand15 { get; set; }
        public ICommand ClickCommand16 { get; set; }
        public ICommand ClickCommand17 { get; set; }
        public ICommand ClickCommand18 { get; set; }
        public ICommand ClickCommand19 { get; set; }
        public ICommand ClickCommand20 { get; set; }
        public ICommand ClickCommand21 { get; set; }
        public ICommand ClickCommand22 { get; set; }
        public ICommand ClickCommand23 { get; set; }
        public ICommand ClickCommand24 { get; set; }
        public ICommand ClickCommand25 { get; set; }
        public ICommand ClickCommand26 { get; set; }
        public ICommand ClickCommand27 { get; set; }
        public ICommand ClickCommand28 { get; set; }
        public ICommand ClickCommand29 { get; set; }
        public ICommand ClickCommand30 { get; set; }
        public ICommand ClickCommand31 { get; set; }
        public ICommand ClickCommand32 { get; set; }
        public ICommand StartCommand { get; set; }
        public ICommand StatCommand { get; set; }
        public ICommand OKCommand { get; set; }
        public ICommand LogOut { get; set; }
        public ICommand Save { get; set; }
        public ICommand Load { get; set; }
        public ICommand NewCommand { get; set; }
        public ICommand Exit { get; set; }
        public ICommand ClickLogin { get; set; }
        public ICommand ClickRegister { get; set; }
        public ICommand ClickGuest { get; set; }
        public ICommand ClickEng { get; set; }
        public ICommand ClickRus { get; set; }
        public ICommand ReturnStepCom { get; set; }
        UserControl1 uc1 { get; set; }
        Options2 op2 { get; set; }
        Options op1 { get; set; }

        #region Constructor
        public MainViewModel()
        {
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ko-KR");

            uc1 = new UserControl1();
            op2 = new Options2();
            op1 = new Options();
            //stat1 = new Stat();
            LocalizData = new LocData();
            ReturnStepCom = new Command(arg => ReturnStepMet());
            LogOut = new Command(arg => LogOutMethod());
            OKCommand = new Command(arg => OKMethod());
            StatCommand = new Command(arg => StatMethod());
            StartCommand = new Command(arg => StartMethod());
            ClickCommand1 = new Command(arg => ClickMethod1());
            ClickCommand2 = new Command(arg => ClickMethod2());
            ClickCommand3 = new Command(arg => ClickMethod3());
            ClickCommand4 = new Command(arg => ClickMethod4());
            ClickCommand5 = new Command(arg => ClickMethod5());
            ClickCommand6 = new Command(arg => ClickMethod6());
            ClickCommand7 = new Command(arg => ClickMethod7());
            ClickCommand8 = new Command(arg => ClickMethod8());
            ClickCommand9 = new Command(arg => ClickMethod9());
            ClickCommand10 = new Command(arg => ClickMethod10());
            ClickCommand11 = new Command(arg => ClickMethod11());
            ClickCommand12 = new Command(arg => ClickMethod12());
            ClickCommand13 = new Command(arg => ClickMethod13());
            ClickCommand14 = new Command(arg => ClickMethod14());
            ClickCommand15 = new Command(arg => ClickMethod15());
            ClickCommand16 = new Command(arg => ClickMethod16());
            ClickCommand17 = new Command(arg => ClickMethod17());
            ClickCommand18 = new Command(arg => ClickMethod18());
            ClickCommand19 = new Command(arg => ClickMethod19());
            ClickCommand20 = new Command(arg => ClickMethod20());
            ClickCommand21 = new Command(arg => ClickMethod21());
            ClickCommand22 = new Command(arg => ClickMethod22());
            ClickCommand23 = new Command(arg => ClickMethod23());
            ClickCommand24 = new Command(arg => ClickMethod24());
            ClickCommand25 = new Command(arg => ClickMethod25());
            ClickCommand26 = new Command(arg => ClickMethod26());
            ClickCommand27 = new Command(arg => ClickMethod27());
            ClickCommand28 = new Command(arg => ClickMethod28());
            ClickCommand29 = new Command(arg => ClickMethod29());
            ClickCommand30 = new Command(arg => ClickMethod30());
            ClickCommand31 = new Command(arg => ClickMethod31());
            ClickCommand32 = new Command(arg => ClickMethod32());
            Save = new Command(arg => SaveClick());
            Load = new Command(arg => LoadClick());
            NewCommand = new Command(arg => NewClick());
            Exit = new Command(arg => ExitClick());
            ClickLogin = new Command(arg => ClickLoggin());
            ClickRegister = new Command(arg => ClickReggista());
            ClickGuest = new Command(arg => ClickGuesto());
            ClickEng = new Command(arg => EngClick());
            ClickRus = new Command(arg => RusClick());
            App.Current.MainWindow.Content = op1.Content;
            CheckersGame = new Checkers();
            LoginGame = new Login();
            
           
        }
        #endregion

        #region Methods

        private void OKMethod()
        {
            //stat1.Close();
        }

        private void StatMethod()
        {
            if (LoginGame.Current.Username == Properties.Resources.Player1Guest)
                MessageBox.Show("Вы не зашли в свою учетную запись!");
            else
            {
                var thread = new Thread(() => DisplayFormNewThread1(""));
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start();
                //stat1.Show();
            }
        }
        private void StartMethod()
        {
            string tmp = "";            
            if (App.Current.MainWindow.Content == op1.Content)
            {
                CheckersGame.PlayerName = Properties.Resources.Player1Guest;
                tmp = op1.ip.Text;

                if (op1.white.IsChecked == true)
                    CheckersGame.yourCheckers = true;
                else CheckersGame.yourCheckers = false;
                if (op1.local.IsChecked == true)
                    CheckersGame.isTwoPlayers = true;
                else CheckersGame.isTwoPlayers = false;
                if (op1.net.IsChecked == true)
                {
                    CheckersGame.Hostname = tmp;
                    var thread = new Thread(() => DisplayFormNewThread(tmp));
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.IsBackground = true;
                    thread.Start();

                }
                else CheckersGame.isNet = false;
                if (op1.ai.IsChecked == true)
                {
                    CheckersGame.isAI = true;
                }
                else CheckersGame.isAI = false;
            }
            else if (App.Current.MainWindow.Content == op2.Content)
            {
                tmp = op2.ip.Text;

                if (op2.white.IsChecked == true)
                    CheckersGame.yourCheckers = true;
                else CheckersGame.yourCheckers = false;
                if (op2.local.IsChecked == true)
                    CheckersGame.isTwoPlayers = true;
                else CheckersGame.isTwoPlayers = false;
                if (op2.net.IsChecked == true)
                {
                    CheckersGame.Hostname = tmp;
                    var thread = new Thread(() => DisplayFormNewThread(tmp));
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.IsBackground = true;
                    thread.Start();

                }
                else CheckersGame.isNet = false;
                if (op2.ai.IsChecked == true)
                {
                    CheckersGame.isAI = true;
                }
                else CheckersGame.isAI = false;
            }

            CheckersGame.currentPlayer = true;
            if (CheckersGame.isAI || CheckersGame.isNet || CheckersGame.isTwoPlayers )
            App.Current.MainWindow.Content = uc1.Content;
            CheckersGame.NewGame(CheckersGame.yourCheckers);
        }
        private void DisplayFormNewThread1(string ip)
        {
            try
            {
                Stat stat2 = new Stat();
                stat2.Show();                
                stat2.Closed += (s, e) => System.Windows.Threading.Dispatcher.ExitAllFrames();
                System.Windows.Threading.Dispatcher.Run();

                stat2.Close();
            }
            catch
            {
                
            }
        }
        private void DisplayFormNewThread(string ip)
        {
            try
            {
                ConnectWindow connectWindow = new ConnectWindow();
                connectWindow.Show();
                
                bool tmp = CheckersGame.Connect(ip);
                if (tmp)
                {
                    CheckersGame.isNet = true; 
                    
                }
                else
                {
                    MessageBox.Show(Properties.Resources.ConnectionError);
                    CheckersGame.isNet = false;
                }
                connectWindow.Closed += (s, e) => System.Windows.Threading.Dispatcher.ExitAllFrames();
                System.Windows.Threading.Dispatcher.Run();

                MessageBox.Show(Properties.Resources.ConnectionError);
                connectWindow.Close();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.ConnectionError);
            }
        }
        private void LogOutMethod()
        {
            LoginGame.Guest();
            if (CheckersGame.PlayerName != Properties.Resources.Player1Guest)
                App.Current.MainWindow.Content = op2.Content;
            else
                App.Current.MainWindow.Content = op1.Content;
        }

        private void ReturnStepMet()
        {
            CheckersGame.ReturnStep();
            RefreshProgress();
        }

        private void ClickLoggin()
        {
            LoginGame.Username = op1.loginbox1.Text;
            LoginGame.Password = op1.passwordbox1.Password;
            if (LoginGame.TryLogin())
            {
                CheckersGame.PlayerName = LoginGame.Username;
                App.Current.MainWindow.Content = op2.Content;
                CheckersGame.RefreshTileTexture();
            }
            
        }
        private void ClickReggista()
        {
            LoginGame.Username = op1.loginbox1.Text;
            LoginGame.Password = op1.passwordbox1.Password;
            if (LoginGame.TryRegister())
            {
                CheckersGame.PlayerName = LoginGame.Username;
                App.Current.MainWindow.Content = op2.Content;
                CheckersGame.RefreshTileTexture();
            }
            
        }
        private void ClickGuesto()
        {
            LoginGame.Guest();
            CheckersGame.PlayerName = LoginGame.Username;
            App.Current.MainWindow.Content = op1.Content;
            CheckersGame.RefreshTileTexture();
        }
        private void EngClick()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            LocalizData = new LocData();
            op1 = new Options();
            op2 = new Options2();
            uc1 = new UserControl1();
        }
        private void RusClick()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
           
        }
        private void LoadClick()
        {
            CheckersGame.LoadGame();
            RefreshProgress();
        }
        private void SaveClick()
        {
            CheckersGame.SaveGame();
        }
        private void NewClick()
        {            
            CheckersGame.NewGame();
            RefreshProgress();
        }
        private void ExitClick()
        {
            App.Current.Shutdown();
        }

        private void ClickMethod1()
        {
            CheckersGame.Click(1);
            RefreshProgress();
        }
        private void ClickMethod2()
        {
            CheckersGame.Click(2);
            RefreshProgress();
        }
        private void ClickMethod3()
        {
            CheckersGame.Click(3);
            RefreshProgress();
        }
        private void ClickMethod4()
        {
            CheckersGame.Click(4);
            RefreshProgress();
        }
        private void ClickMethod5()
        {
            CheckersGame.Click(5);
            RefreshProgress();
        }
        private void ClickMethod6()
        {
            CheckersGame.Click(6);
            RefreshProgress();
        }
        private void ClickMethod7()
        {
            CheckersGame.Click(7);
            RefreshProgress();
        }
        private void ClickMethod8()
        {
            CheckersGame.Click(8);
            RefreshProgress();
        }
        private void ClickMethod9()
        {
            CheckersGame.Click(9);
            RefreshProgress();
        }
        private void ClickMethod10()
        {
            CheckersGame.Click(10);
            RefreshProgress();
        }
        private void ClickMethod11()
        {
            CheckersGame.Click(11);
            RefreshProgress();
        }
        private void ClickMethod12()
        {
            CheckersGame.Click(12);
            RefreshProgress();
        }
        private void ClickMethod13()
        {
            CheckersGame.Click(13);
            RefreshProgress();
        }
        private void ClickMethod14()
        {
            CheckersGame.Click(14);
            RefreshProgress();
        }
        private void ClickMethod15()
        {
            CheckersGame.Click(15);
            RefreshProgress();
        }
        private void ClickMethod16()
        {
            CheckersGame.Click(16);
            RefreshProgress();
        }
        private void ClickMethod17()
        {
            CheckersGame.Click(17);
            RefreshProgress();
        }
        private void ClickMethod18()
        {
            CheckersGame.Click(18);
            RefreshProgress();
        }
        private void ClickMethod19()
        {
            CheckersGame.Click(19);
            RefreshProgress();
        }
        private void ClickMethod20()
        {
            CheckersGame.Click(20);
            RefreshProgress();
        }
        private void ClickMethod21()
        {
            CheckersGame.Click(21);
            RefreshProgress();
        }
        private void ClickMethod22()
        {
            CheckersGame.Click(22);
            RefreshProgress();
        }
        private void ClickMethod23()
        {
            CheckersGame.Click(23);
            RefreshProgress();
        }
        private void ClickMethod24()
        {
            CheckersGame.Click(24);
            RefreshProgress();
        }
        private void ClickMethod25()
        {
            CheckersGame.Click(25);
            RefreshProgress();
        }
        private void ClickMethod26()
        {
            CheckersGame.Click(26);
            RefreshProgress();
        }
        private void ClickMethod27()
        {
            CheckersGame.Click(27);
            RefreshProgress();
        }
        private void ClickMethod28()
        {
            CheckersGame.Click(28);
            RefreshProgress();
        }
        private void ClickMethod29()
        {
            CheckersGame.Click(29);
            RefreshProgress();
        }
        private void ClickMethod30()
        {
            CheckersGame.Click(30);
            RefreshProgress();
        }
        private void ClickMethod31()
        {
            CheckersGame.Click(31);
            RefreshProgress();
        }
        private void ClickMethod32()
        {
            CheckersGame.Click(32);
            RefreshProgress();
        }
        private void RefreshProgress()
        {
            uc1.List.Items.Clear();
            foreach (var item in CheckersGame.progress)
            {
                uc1.List.Items.Add(item.ToString());
            }
        }
        #endregion

    }
}
