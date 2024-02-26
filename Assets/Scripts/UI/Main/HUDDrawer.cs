using System.Collections;
using UI.Main.Windows;
using UnityEngine;
using Zenject;

namespace UI.Main
{
    public class HUDDrawer : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _delay = 1f;

        private HUD _hud;

        [Inject]
        private void Constructor(HUD hud) => _hud = hud;

        #region MonoBehaivour

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_delay);

            _hud.Show();
        }

        #endregion
    }
}