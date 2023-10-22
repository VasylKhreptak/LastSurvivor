using Data.Persistent;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public Resources Resources = new Resources();
        public HelicopterData HelicopterData = new HelicopterData();

        public bool FinishedTutorial;
    }
}