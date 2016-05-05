using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Data.SqlClient;

namespace BionicFinalProject.Model
{
    class Login
    {
        Player current;
        List<Player> registered;
        string username;
        string dbSource;


        public Player Current
        {
            get { return current; }
            set { current = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        
        string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public Login()
        {
            current = new Player();
            username = "";
            password = "";
            dbSource = @"Data Source=SIRIUS-NOTE\SQLEXPRESS;Initial Catalog=FinalProject;Integrated Security=True";

            registered = GetUsers();
            
        }

        List<Player> GetUsers()
        {
            List<Player> result = new List<Player>();
            //using (SqlConnection conn = new SqlConnection(dbSource))
            //{
            //    conn.Open();
            //    string select = "SELECT * from [Users]";
            //    SqlCommand cmd = new SqlCommand(select, conn);
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        result.Add(new Player(dr[1].ToString(), dr[2].ToString(), Convert.ToInt32(dr[3]), Convert.ToInt32(dr[4])));
            //    }
                
            //    dr.Close();
            //}
            return result;
        }

        public bool TryLogin()
        {            
            bool result = false;
            foreach (Player item in registered)
            {
                if (item.Username == username)
                    if (item.Password == password)
                    {
                        MessageBox.Show(Properties.Resources.Welcome + " " + username);
                        current.Username = item.Username;
                        current.Password = item.Password;
                        current.GamePlayed = item.GamePlayed;
                        current.GameWon = item.GameWon;

                        result = true;
                    }
            }
            if (!result) MessageBox.Show(Properties.Resources.LoginFailed);
            return result;
        }
        public bool TryRegister()
        {
            bool result = true;
            foreach (Player item in registered)
            {
                if (item.Username == username)
                    
                        result = false;
                    
            }
            if (result)
            {
                //using (SqlConnection conn = new SqlConnection(dbSource))
                //{
                //    conn.Open();
                //    string select = @"INSERT INTO Users (username, password, GamePlayed, GameWon) VALUES ('" + username + @"', '" + password + @"', '0', '0')";
                //    SqlCommand cmd = new SqlCommand(select, conn);
                //    int rw = cmd.ExecuteNonQuery();
                   
                //}
                GetUsers();
            }

            return result;
        }
        public void Guest()
        {
            current.Username = Properties.Resources.Player1Guest;
            username = Properties.Resources.Player1Guest;
        }

    }
}
