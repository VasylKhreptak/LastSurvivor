namespace Adapters.Core
{
    public interface IReadonlyAdapter<out T>
    {
        public T Value { get; }
    }
}