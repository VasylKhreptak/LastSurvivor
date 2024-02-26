namespace Quests.Core
{
    public interface IQuestCallbackHandler
    {
        public void OnBecameActive(bool isActive);
    }
}