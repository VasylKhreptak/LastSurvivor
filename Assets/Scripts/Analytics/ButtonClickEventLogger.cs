using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

namespace Analytics
{
    public class ButtonClickEventLogger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        [Header("Preferences")]
        [SerializeField] private string _name;

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(LogEvent);

        private void OnDisable() => _button.onClick.RemoveListener(LogEvent);

        #endregion

        private void LogEvent() =>
            FirebaseAnalytics.LogEvent(AnalyticEvents.ClickedButton, new Parameter(AnalyticParameters.Name, _name));
    }
}