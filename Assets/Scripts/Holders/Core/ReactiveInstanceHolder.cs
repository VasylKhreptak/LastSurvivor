using UniRx;

namespace Holders.Core
{
    public class ReactiveInstanceHolder<T>
    {
        public ReactiveProperty<T> Instance { get; private set; } = new ReactiveProperty<T>();
    }
}