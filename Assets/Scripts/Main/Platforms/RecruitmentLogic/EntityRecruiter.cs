using System;
using System.Linq;
using Extensions;
using Main.Platforms.Zones;
using Plugins.Banks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Main.Platforms.RecruitmentLogic
{
    public class EntityRecruiter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject[] _entities;

        private ReceiveZone _receiveZone;
        private ClampedIntegerBank _entitiesBank;

        [Inject]
        private void Constructor(ReceiveZone receiveZone, ClampedIntegerBank entitiesBank)
        {
            _receiveZone = receiveZone;
            _entitiesBank = entitiesBank;
        }

        public event Action<GameObject> OnHired;

        #region MonoBehaviour

        private void Awake() => PlaceHiredEntities();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        [Button]
        private void FindEntities() => _entities = transform.GetChildren().Select(x => x.gameObject).ToArray();

        private void StartObserving() => _receiveZone.OnReceivedAll += OnReceiveZoneFilled;

        private void StopObserving() => _receiveZone.OnReceivedAll -= OnReceiveZoneFilled;

        private void PlaceHiredEntities()
        {
            int entitiesToPlace = _entitiesBank.Value.Value;

            for (int i = 0; i < _entities.Length; i++)
            {
                GameObject entity = _entities[i];

                bool canBeEnabled = i < entitiesToPlace;

                entity.SetActive(canBeEnabled);
            }
        }

        private void OnReceiveZoneFilled()
        {
            if (TryHireEntity(out GameObject entity))
                OnHired?.Invoke(entity);
        }

        private bool TryHireEntity(out GameObject hiredEntity)
        {
            foreach (GameObject entity in _entities)
            {
                if (entity.activeSelf)
                    continue;

                entity.SetActive(true);

                _entitiesBank.Add(1);
                hiredEntity = entity;
                return true;
            }

            hiredEntity = null;
            return false;
        }
    }
}