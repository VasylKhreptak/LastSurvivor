namespace Adapters.Core
{
    public interface IAdapter<T> : IReadonlyAdapter<T>
    {
        public new T Value { get; set; }
    }
}