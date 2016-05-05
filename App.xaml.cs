using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BionicFinalProject.View;
using BionicFinalProject.ViewModel;

namespace BionicFinalProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            
            var mw = new MainWindowView
            {
                DataContext = new MainViewModel()                
            };

            mw.Show();
        }
    }
}
