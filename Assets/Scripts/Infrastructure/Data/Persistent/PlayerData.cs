using Plugins.Banks.Data.Economy;

namespace Infrastructure.Data.Persistent
{
    public class PlayerData
    {
        public Banks Resources = new Banks();

        public bool FinishedTutorial;
    }
}