using Infrastructure.Services.PersistentData.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace UI.Gameplay.Texts
{
    public class LevelNameText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private LocalizedString _localizedString;

        private IPersistentDataService _persistentDataService;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService) => _persistentDataService = persistentDataService;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable()
        {
            _localizedString.Arguments = new object[]
            {
                _persistentDataService.Data.PlayerData.CompletedLevelsCount + 1
            };

            _localizedString.StringChanged += UpdateText;
        }

        private void OnDisable() => _localizedString.StringChanged -= UpdateText;

        #endregion

        private void UpdateText(string text) => _tmp.text = text;
    }
}