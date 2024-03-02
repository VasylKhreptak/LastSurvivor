using UnityEngine;
using UnityEngine.UI;

namespace UI.SwitchMenu
{
    public class SwitchNextButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SwitchMenu _switchMenu;
        [SerializeField] private Button _button;

        #region MonoBehaviour

        private void OnValidate() => _button ??= GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(_switchMenu.SwitchNext);

        private void OnDisable() => _button.onClick.RemoveListener(_switchMenu.SwitchNext);

        #endregion
    }
}