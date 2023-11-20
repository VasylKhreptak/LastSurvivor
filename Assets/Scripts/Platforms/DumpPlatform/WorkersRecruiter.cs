using System;
using System.Collections.Generic;
using Data.Persistent.Platforms;
using DG.Tweening;
using Platforms.Zones;
using Plugins.Banks;
using UnityEngine;
using Zenject;

namespace Platforms.DumpPlatform
{
    public class WorkersRecruiter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<GameObject> _workers;

        [Header("Scale Preferences")]
        [SerializeField] private float _scaleDuration = 0.5f;
        [SerializeField] private AnimationCurve _scaleCurve;

        private ReceiveZone _receiveZone;
        private DumpPlatformData _platformData;
        private ClampedIntegerBank _workersBank;

        [Inject]
        private void Constructor(ReceiveZone receiveZone, DumpPlatformData platformData)
        {
            _receiveZone = receiveZone;
            _platformData = platformData;
            _workersBank = _platformData.WorkersBank;
        }

        public event Action OnHiredWorker;

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
            if (TryHireWorker())
                OnHiredWorker?.Invoke();
        }

        private bool TryHireWorker()
        {
            foreach (GameObject worker in _workers)
            {
                if (worker.activeSelf)
                    continue;

                worker.SetActive(true);

                Vector3 targetScale = worker.transform.localScale;
                worker.transform.localScale = Vector3.zero;
                worker.transform
                    .DOScale(targetScale, _scaleDuration)
                    .SetEase(_scaleCurve)
                    .Play();

                _workersBank.Add(1);
                return true;
            }

            return false;
        }
    }
}