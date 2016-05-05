using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows;

namespace BionicFinalProject.Model
{
    [Serializable]
    class SavingData
    {
        public bool currentPlayer { get; set; } // true = 1 (white), false = 2 (black)
        public bool JumpInd { get; set; }
        public List<Step> progress { get; set; }        
        public bool[] queen { get; set; }
        public int[] color { get; set; }
        public bool yourCheckers { get; set; } // true = 1 (white), false = 2 (black)
        public bool isTwoPlayers { get; set; }
        public bool isNet { get; set; }
        public bool isAI { get; set; }

        public SavingData()
        {
            queen = new bool[32];
            color = new int[32];
        }
        public void Serialize(string filename)
        {
            //try
            //{
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                bf.Serialize(fs, this);
                fs.Flush();
                fs.Close();
            //}
            //catch
            //{
            //}
        }
        public SavingData Deserialize(string filename)
        {
            //SavingData deserialized = new SavingData();
            //try
            //{
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read);

            SavingData deserialized = (SavingData)bf.Deserialize(fs);
            fs.Flush();
            fs.Close();
            //}
            //catch
            //{
            //    MessageBox.Show("Error des");
            //}
            return deserialized;
        }

    }


}
