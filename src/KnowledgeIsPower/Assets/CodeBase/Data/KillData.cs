using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class KillData
    {
        public List<string> ClearedSpawners = new List<string>();
        public Dictionary<string, Vector3Data> SavedLoots = new Dictionary<string, Vector3Data>();
    }
}