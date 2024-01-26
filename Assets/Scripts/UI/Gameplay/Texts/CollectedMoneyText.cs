using Gameplay.Data;
using TMPro;
using UI.Texts.Core;
using UnityEngine;
using Zenject;

namespace UI.Gameplay.Texts
{
    public class CollectedMoneyText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private LevelData _levelData;

        [Inject]
        private void Constructor(LevelData levelData) => _levelData = levelData;

        private PropertyTextConverter<int> _propertyTextConverter;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void Awake() => _propertyTextConverter = new PropertyTextConverter<int>(_tmp, _levelData.CollectedMoney);

        private void OnEnable() => _propertyTextConverter.Initialize();

        private void OnDisable() => _propertyTextConverter.Dispose();

        #endregion
    }
}