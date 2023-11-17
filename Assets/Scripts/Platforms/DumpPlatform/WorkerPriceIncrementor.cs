using System;
using Plugins.Banks;
using Zenject;

namespace Platforms.DumpPlatform
{
    public class WorkerPriceIncrementor : IInitializable, IDisposable
    {
        private const int IncrementValue = 25;

        private readonly WorkersRecruiter _workersRecruiter;
        private readonly ClampedIntegerBank _hireWorkerZoneBank;

        public WorkerPriceIncrementor(WorkersRecruiter workersRecruiter, ClampedIntegerBank hireWorkerZoneBank)
        {
            _workersRecruiter = workersRecruiter;
            _hireWorkerZoneBank = hireWorkerZoneBank;
        }

        public void Initialize() => _workersRecruiter.OnHiredWorker += IncrementPrice;

        public void Dispose() => _workersRecruiter.OnHiredWorker -= IncrementPrice;

        private void IncrementPrice()
        {
            _hireWorkerZoneBank.SetMaxValue(_hireWorkerZoneBank.MaxValue.Value + IncrementValue);
        }
    }
}