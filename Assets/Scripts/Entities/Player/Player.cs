using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Context _context;

        public DiContainer Container => _context.Container;

        #region MonoBehaviour

        private void OnValidate()
        {
            _context ??= GetComponent<Context>();
        }

        #endregion
    }
}