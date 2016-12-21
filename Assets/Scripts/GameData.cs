using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    [Serializable]
    public class GameData
    {
        public int unlockedLevel;

        public GameData() {
            unlockedLevel = 1;
        }

    }
}
