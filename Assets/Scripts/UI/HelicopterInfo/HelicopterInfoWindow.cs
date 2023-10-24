using Infrastructure.Graphics.UI.Windows.Core;
using UnityEngine;

namespace UI.HelicopterInfo
{
    public class HelicopterInfoWindow : IWindow
    {
        private readonly GameObject _root;

        public HelicopterInfoWindow(HelicopterInfoWindowReferences references)
        {
            _root = references.Root;
        }

        public bool IsActive => _root.activeSelf;

        public void Show() => _root.SetActive(true);

        public void Hide() => _root.SetActive(false);
    }
}