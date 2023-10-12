using Infrastructure.Services.Input.Main.Core;
using UnityEngine;

namespace Infrastructure.Services.Input.Main
{
    public class MainInputService : MonoBehaviour, IMainInputService
    {        
        public float Horizontal
        {
            get;
        }
        
        public float Vertical
        {
            get;
        }
        
        public void Enable()
        {
            
        }
        
        public void Disable()
        {
        
        }
    }
}
