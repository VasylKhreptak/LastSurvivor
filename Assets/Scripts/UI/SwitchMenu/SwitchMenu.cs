using System;
using UnityEngine;

namespace UI.SwitchMenu
{
    public class SwitchMenu : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private int _maxIndex = 3;
        [SerializeField] private bool _loop = true;

        public int CurrentIndex { get; private set; }

        public event Action<int> OnIndexChanged;

        public void SwitchNext()
        {
            if (CurrentIndex == _maxIndex)
            {
                if (_loop)
                    SetIndex(0);

                return;
            }

            SetIndex(CurrentIndex + 1);
        }

        public void SwitchPrevious()
        {
            if (CurrentIndex == 0)
            {
                if (_loop)
                    SetIndex(_maxIndex);

                return;
            }

            SetIndex(CurrentIndex - 1);
        }

        public void SetIndex(int index)
        {
            index = Mathf.Clamp(index, 0, _maxIndex);

            if (index == CurrentIndex)
                return;

            CurrentIndex = index;
            OnIndexChanged?.Invoke(CurrentIndex);
        }
    }
}