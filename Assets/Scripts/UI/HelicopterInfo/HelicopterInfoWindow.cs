using Infrastructure.Graphics.UI.Windows.Core;
using UnityEngine;
using Zenject;

namespace UI.HelicopterInfo
{
    public class HelicopterInfoWindow : MonoBehaviour, IWindow
    {
        private GameObject _root;

        [Inject]
        private void Constructor(HelicopterInfoWindowReferences references)
        {
            _root = references.Root;
        }

        public bool IsActive => _root.activeSelf;

        public void Show()
        {
            _root.SetActive(true);
        }

        public void Hide()
        {
            _root.SetActive(false);
        }
    }
}