using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Main.Platforms.RecruitmentLogic
{
    public class HireAnimation : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;

        [Header("Scale Preferences")]
        [SerializeField] private AnimationCurve _scaleCurve;

        private EntityRecruiter _entityRecruiter;

        [Inject]
        private void Constructor(EntityRecruiter entityRecruiter)
        {
            _entityRecruiter = entityRecruiter;
        }

        #region MonoBehaviouir

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _entityRecruiter.OnHired += PlayAnimation;

        private void StopObserving() => _entityRecruiter.OnHired -= PlayAnimation;

        private void PlayAnimation(GameObject entity)
        {
            Vector3 targetScale = entity.transform.localScale;

            entity.transform.localScale = Vector3.zero;

            entity.transform
                .DOScale(targetScale, _duration)
                .SetEase(_scaleCurve)
                .Play();
        }
    }
}