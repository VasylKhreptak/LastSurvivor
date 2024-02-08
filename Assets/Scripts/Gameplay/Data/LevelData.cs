using UniRx;

namespace Gameplay.Data
{
    public class LevelData
    {
        public readonly IntReactiveProperty CollectedGears = new IntReactiveProperty();
        public readonly IntReactiveProperty CollectedMoney = new IntReactiveProperty();
    }
}