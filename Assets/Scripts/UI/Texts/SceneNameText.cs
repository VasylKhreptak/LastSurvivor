using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Texts
{
    public class SceneNameText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void Awake() => _tmp.text = SceneManager.GetActiveScene().name;

        #endregion
    }
}