using Plugins.Animations.Adapters.Color.Core;
using TMPro;
using UnityEngine;

namespace Plugins.Animations.Adapters.Color
{
    public class AdaptedTMPForColor : ColorAdapter
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
        }

        #endregion

        public override UnityEngine.Color Value { get => _tmp.color; set => _tmp.color = value; }
    }
}