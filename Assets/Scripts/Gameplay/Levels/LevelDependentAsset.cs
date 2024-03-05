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

        private LevelManager _levelManager;

        [Inject]
        private void Constructor(LevelManager levelManage) => _levelManager = levelManage;

        #region MonoBehaviour

        private void OnValidate() => _asset = transform.GetChild(0).gameObject;

        private void Awake()
        {
            if (_levelManager.GetCurrentLevel() < _requiredLevel)
                Destroy(gameObject);
            else
                _asset.SetActive(true);
        }

        #endregion
    }
}