using System.Collections.Generic;
using Data.Persistent.Platforms;
using Plugins.Banks;
using UnityEngine;
using Zenject;

namespace Platforms.DumpPlatform
{
    public class WorkersRecruiter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<GameObject> _workers;

        [Header("Preferences")]
        [SerializeField] private int _workerPriceIncrement = 25;

        private ReceiveZone _receiveZone;
        private DumpPlatformData _platformData;
        private ClampedIntegerBank _hireZone;
        private ClampedIntegerBank _workersBank;

        [Inject]
        private void Constructor(ReceiveZone receiveZone, DumpPlatformData platformData, ClampedIntegerBank hireZone)
        {
            _receiveZone = receiveZone;
            _platformData = platformData;
            _hireZone = hireZone;
            _workersBank = _platformData.WorkersBank;
        }

        #region MonoBehaviour

        private void Awake() => PlaceHiredWorkers();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _receiveZone.OnReceivedAll += OnReceiveZoneFilled;

        private void StopObserving() => _receiveZone.OnReceivedAll -= OnReceiveZoneFilled;

        private void PlaceHiredWorkers()
        {
            int workersToPlace = _platformData.WorkersBank.Value.Value;

            for (int i = 0; i < _workers.Count; i++)
            {
                GameObject worker = _workers[i];

                bool canBeEnabled = i < workersToPlace;

                worker.SetActive(canBeEnabled);
            }
        }

        private void OnReceiveZoneFilled()
        {
            if (TryHireWorker() == false)
                return;

            IncreaseWorkerPrice();
        }

        private bool TryHireWorker()
        {
            foreach (var worker in _workers)
            {
                if (worker.activeSelf)
                    continue;

                worker.SetActive(true);
                _workersBank.Add(1);
                return true;
            }

            return false;
        }

        private void IncreaseWorkerPrice() => _hireZone.SetMaxValue(_hireZone.MaxValue.Value + _workerPriceIncrement);

        private void OnHiredAllWorkers() { }
    }
}