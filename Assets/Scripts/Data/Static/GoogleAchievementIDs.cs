using System;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public class GoogleAchievementIDs
    {
        [SerializeField] private string _completedFirstLevel = "CgkIwPW18LsfEAIQBg";
        [SerializeField] private string _completedTenLevels = "CgkIwPW18LsfEAIQBQ";

        public string CompletedFirstLevel => _completedFirstLevel;
        public string CompletedTenLevels => _completedTenLevels;
    }
}