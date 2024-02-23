using UniRx;

namespace Holders.Core
{
    public class ReactiveInstanceHolder<T>
    {
        public readonly ReactiveProperty<T> Instance = new ReactiveProperty<T>();
    }
}