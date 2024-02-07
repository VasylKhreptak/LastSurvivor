using Plugins.Animations.Adapters.Alpha.Core;
using UnityEngine;

namespace Plugins.Animations.Adapters.Alpha
{
    public class AdaptedCanvasGroupForAlpha : AlphaAdapter
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        #endregion

        public override float Value { get => _canvasGroup.alpha; set => _canvasGroup.alpha = value; }
    }
}