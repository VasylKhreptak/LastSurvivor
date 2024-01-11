using System.Collections.Generic;
using Plugins.AudioPoolSystem.Facade.Core;

namespace Plugins.AudioPoolSystem.Core
{
    public interface IAudioPool
    {
        public IAudio Get();
        
        public IEnumerable<IAudio> GetAllActive();
    }
}