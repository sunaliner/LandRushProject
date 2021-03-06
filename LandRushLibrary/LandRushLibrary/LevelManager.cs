﻿
namespace LandRushLibrary
{
    public class LevelManager
    {
        #region singleton
        private static LevelManager _instance;

        public static LevelManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LevelManager();
                return _instance;
            }
        }

        private LevelManager()
        {
        }
        #endregion

        public int GetNextExp(int newLevel, int currentMaxExp)
        {
            currentMaxExp += newLevel * 100;
            return currentMaxExp;
        }
    }
}
