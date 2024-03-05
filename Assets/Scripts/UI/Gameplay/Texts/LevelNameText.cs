using Gameplay.Levels;
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

        private LevelManager _levelManager;

        [Inject]
        private void Constructor(LevelManager levelManager) => _levelManager = levelManager;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable()
        {
            _localizedString.Arguments = new object[]
            {
                _levelManager.GetCurrentLevel()
            };

            _localizedString.StringChanged += UpdateText;
        }

        private void OnDisable() => _localizedString.StringChanged -= UpdateText;

        #endregion

        private void UpdateText(string text) => _tmp.text = text;
    }
}