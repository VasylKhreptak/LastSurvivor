using Plugins.Animations.Adapters.Color.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Animations.Adapters.Color
{
    public class AdaptedImageForColor : ColorAdapter
    {
        [Header("References")]
        [SerializeField] private Image _image;

        #region MonoBehaviour

        private void OnValidate()
        {
            _image ??= GetComponent<Image>();
        }

        #endregion

        public override UnityEngine.Color Value
        {
            get => _image.color;
            set => _image.color = value;
        }
    }
}
