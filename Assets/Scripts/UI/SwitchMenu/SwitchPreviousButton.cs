using UnityEngine;
using UnityEngine.UI;

namespace UI.SwitchMenu
{
    public class SwitchPreviousButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SwitchMenu _switchMenu;
        [SerializeField] private Button _button;

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(_switchMenu.SwitchPrevious);

        private void OnDisable() => _button.onClick.RemoveListener(_switchMenu.SwitchPrevious);

        #endregion
    }
}