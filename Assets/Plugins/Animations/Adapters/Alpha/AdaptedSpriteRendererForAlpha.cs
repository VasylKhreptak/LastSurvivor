using Plugins.Animations.Adapters.Alpha.Core;
using Plugins.Animations.Extensions;
using UnityEngine;

namespace Plugins.Animations.Adapters.Alpha
{
    public class AdaptedSpriteRendererForAlpha : AlphaAdapter
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #region MonoBehaviour

        private void OnValidate()
        {
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
        }

        #endregion

        public override float Value
        {
            get => _spriteRenderer.color.a;
            set => _spriteRenderer.color.WithAlpha(value);
        }
    }
}
