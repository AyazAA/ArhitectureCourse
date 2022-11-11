using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public Stats HeroStats;
        public WorldData WorldData;
        public State HeroState;
        public KillData KillData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
        }
    }
}