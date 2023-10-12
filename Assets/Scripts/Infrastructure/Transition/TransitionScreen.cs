using System;
using Infrastructure.Transition.Core;
using UnityEngine;

namespace Infrastructure.Transition
{
    public class TransitionScreen : MonoBehaviour, ITransitionScreen
    {
        public event Action OnHidden;
        
        public void Show()
        {
            
        }
        
        public void Hide()
        {
            
        }
    }
}
