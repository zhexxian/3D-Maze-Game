using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Assets.Scripts
{
    public static class DataControl
    {
        public static GameData mGameData = new GameData();
        private static string path = "/";
        private static string fileName = "savedGames.gd";
        public static void Save()
        {
            //savedGames.Add(Game.current);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path+fileName);
            bf.Serialize(file, mGameData);
            file.Close();
        }

        public static void Load()
        {
            if (File.Exists("/savedGames.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path + fileName, FileMode.Open);
                mGameData = (GameData)bf.Deserialize(file);
                file.Close();
                GlobalVariable.UnlockedLevel = mGameData.unlockedLevel;
            }
        }

    }
}
