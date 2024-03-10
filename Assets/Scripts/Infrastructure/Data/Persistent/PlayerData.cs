using System.Collections.Generic;
using Data.Persistent;
using Data.Persistent.Platforms;
using Quests.Main.Core;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public readonly Resources Resources = new Resources();
        public readonly PlatformsData PlatformsData = new PlatformsData();
        public readonly HashSet<QuestType> CompletedQuests = new HashSet<QuestType>();
        public readonly HashSet<int> ScheduledNotificationIDs = new HashSet<int>();
        public int CompletedLevelsCount;
    }
}