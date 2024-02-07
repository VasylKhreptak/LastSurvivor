using Plugins.Animations.Adapters.Alpha.Core;
using Plugins.Animations.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Animations.Adapters.Alpha
{
    public class AdaptedImageForAlpha : AlphaAdapter
    {
        [Header("References")]
        [SerializeField] private Image _image;

        #region MonoBehaviour

        private void OnValidate()
        {
            _image ??= GetComponent<Image>();
        }

        #endregion

        public override float Value { get => _image.color.a; set => _image.color.WithAlpha(value); }
    }
}