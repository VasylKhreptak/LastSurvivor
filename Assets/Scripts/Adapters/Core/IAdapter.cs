using Infrastructure.Providers.Core;

namespace Adapters.Core
{
    public interface IAdapter<T> : IProvider<T>
    {
        public new T Value { get; set; }
    }
}