using Plugins.Animations.Adapters.Color.Core;
using UnityEngine;

namespace Plugins.Animations.Adapters.Color
{
    public class AdaptedSpriteRendererForColor : ColorAdapter
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #region MonoBehaviour

        private void OnValidate()
        {
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
        }

        #endregion

        public override UnityEngine.Color Value { get => _spriteRenderer.color; set => _spriteRenderer.color = value; }
    }
}