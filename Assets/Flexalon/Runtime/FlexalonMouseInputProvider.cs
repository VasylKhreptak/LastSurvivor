using UnityEngine;

namespace Flexalon
{
    /// <summary> Simple input provider that uses the mouse for input. </summary>
    public class FlexalonMouseInputProvider : InputProvider
    {
        public bool Activated => Input.GetMouseButtonDown(0);
        public bool Active => Input.GetMouseButton(0);
        public Ray Ray => Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}