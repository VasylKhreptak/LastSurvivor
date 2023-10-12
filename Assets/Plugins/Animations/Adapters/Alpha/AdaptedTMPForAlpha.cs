using Plugins.Animations.Adapters.Alpha.Core;
using Plugins.Animations.Extensions;
using TMPro;
using UnityEngine;

namespace Plugins.Animations.Adapters.Alpha
{
    public class AdaptedTMPForAlpha : AlphaAdapter
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
        }

        #endregion

        public override float Value
        {
            get => _tmp.color.a;
            set => _tmp.color.WithAlpha(value);
        }
    }
}
