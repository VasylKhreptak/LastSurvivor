using UnityEngine;

namespace Main.Entities.Player
{
    public class TutorialArrow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _arrow;

        [HideInInspector] public Transform Target;

        #region MonoBehaviour

        private void Update()
        {
            if (Target == null)
            {
                _arrow.gameObject.SetActive(false);
                return;
            }

            _arrow.gameObject.SetActive(true);
            _arrow.rotation = Quaternion.LookRotation(Target.position - transform.position);
        }

        #endregion
    }
}