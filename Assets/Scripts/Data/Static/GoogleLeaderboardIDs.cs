using System;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public class GoogleLeaderboardIDs
    {
        [SerializeField] private string _levelID = "CgkIwPW18LsfEAIQBA";

        public string LevelID => _levelID;
    }
}