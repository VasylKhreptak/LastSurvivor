using Infrastructure.Services.PersistentData.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Gameplay.Texts
{
    public class LevelNameText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "Level {0}";

        private IPersistentDataService _persistentDataService;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService) => _persistentDataService = persistentDataService;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void Awake() => _tmp.text = string.Format(_format, _persistentDataService.Data.PlayerData.CompletedLevelsCount + 1);

        #endregion
    }
}