using Infrastructure.Services.PersistentData.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Levels
{
    public class LevelDependentAsset : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _asset;

        [Header("Preferences")]
        [SerializeField] private int _requiredLevel = 5;

        private IPersistentDataService _persistentDataService;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService) => _persistentDataService = persistentDataService;

        #region MonoBehaviour

        private void OnValidate() => _asset = transform.GetChild(0).gameObject;

        private void Awake()
        {
            if (_persistentDataService.Data.PlayerData.CompletedLevelsCount + 1 < _requiredLevel)
                Destroy(gameObject);
            else
                _asset.SetActive(true);
        }

        #endregion
    }
}