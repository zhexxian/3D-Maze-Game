using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class DataControl
    {
        public static GameData mGameData = new GameData();
        private static string path = Application.persistentDataPath;
        private static string fileName = "/savedGames.gd";
        public static void Save()
        {
            //File.Create(path + fileName).Dispose();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            mGameData.unlockedLevel = GlobalVariable.UnlockedLevel;
            if (File.Exists(path + fileName))
            {
                Debug.Log("rewrite savean");
                file = File.Open(path + fileName, FileMode.Open);
            }
            else
            {
                Debug.Log("create savean");
                file = File.Create(path + fileName);        
            }
            bf.Serialize(file, mGameData);
            file.Close();
        }

        public static void Load()
        {
            if (File.Exists(path + fileName))
            {
                Debug.Log("Load savean");
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path + fileName, FileMode.Open);
                mGameData = (GameData)bf.Deserialize(file);
                file.Close();
                GlobalVariable.UnlockedLevel = mGameData.unlockedLevel;
            }
            else {
                Debug.Log("Load Initial");
                GlobalVariable.UnlockedLevel = 1;
            }
        }

    }
}
