using System;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public class GoogleLeaderboardIDs
    {
        [SerializeField] private string _level = "CgkIwPW18LsfEAIQBA";

        public string Level => _level;
    }
}