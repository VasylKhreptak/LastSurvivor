using System;
using System.Linq;
using DG.Tweening;
using Gameplay.Entities.Health.Damages;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using UniRx;
using UnityEngine;
using Utilities.PhysicsUtilities.Trigger;
using Visitor;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Entities.Zombie
{
    public class ZombieAttacker : IFixedTickable, IDisposable
    {
        private readonly Animator _animator;
        private readonly Preferences _preferences;
        private readonly TriggerZone<IVisitable<ZombieDamage>> _damageZone;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IStaticDataService _staticDataService;

        public ZombieAttacker(Animator animator, Preferences preferences, IPersistentDataService persistentDataService,
            IStaticDataService staticDataService)
        {
            _animator = animator;
            _preferences = preferences;
            _persistentDataService = persistentDataService;
            _staticDataService = staticDataService;
            _damageZone = new TriggerZone<IVisitable<ZombieDamage>>(_preferences.AttackTrigger);
        }

        private IDisposable _damageZoneSubscription;
        private IDisposable _damageDelaySubscription;
        private IDisposable _attackIntervalSubscription;
        private IDisposable _layerFadeDelaySubscription;

        private Tween _layerTween;

        public void Start()
        {
            StartObserving();
            _damageZone.Initialize();
        }

        public void Stop()
        {
            StopObserving();
            _damageZone.Dispose();
            _damageDelaySubscription?.Dispose();
            _layerFadeDelaySubscription?.Dispose();
            StopAttacking();
            _layerTween?.Kill();
            _animator.SetLayerWeight(_preferences.AttackLayerIndex, 0f);
        }

        public void FixedTick() => _damageZone.FixedTick();

        public void Dispose() => Stop();

        private void StartObserving()
        {
            OnCountChanged(_damageZone.Triggers.Count);
            _damageZoneSubscription = _damageZone.Triggers
                .ObserveCountChanged()
                .Subscribe(OnCountChanged);
        }

        private void StopObserving() => _damageZoneSubscription?.Dispose();

        private void OnCountChanged(int count)
        {
            if (count > 0)
                StartAttacking();
            else
                StopAttacking();
        }

        private void StartAttacking()
        {
            _animator.ResetTrigger(_preferences.AttackTriggerName);

            _attackIntervalSubscription?.Dispose();
            _attackIntervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.AttackInterval))
                .DoOnSubscribe(PlayAttackAnimation)
                .Subscribe(_ => PlayAttackAnimation());
        }

        private void StopAttacking() => _attackIntervalSubscription?.Dispose();

        private void SetLayerWeightSmooth(float weight)
        {
            _layerTween?.Kill();
            _layerTween = DOTween
                .To(() => _animator.GetLayerWeight(_preferences.AttackLayerIndex),
                    w => _animator.SetLayerWeight(_preferences.AttackLayerIndex, w),
                    weight, _preferences.LayerSwitchDuration)
                .Play();
        }

        private void PlayAttackAnimation()
        {
            SetLayerWeightSmooth(1f);
            _damageDelaySubscription?.Dispose();
            _animator.SetTrigger(_preferences.AttackTriggerName);
            _damageDelaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(_preferences.DamageApplyDelay))
                .Subscribe(_ =>
                {
                    TryApplyDamage();

                    _layerFadeDelaySubscription?.Dispose();
                    _layerFadeDelaySubscription = Observable
                        .Timer(TimeSpan.FromSeconds(_preferences.LayerFadeDelay))
                        .Subscribe(_ => SetLayerWeightSmooth(0f));
                });
        }

        private void TryApplyDamage()
        {
            if (_damageZone.Triggers.Count == 0)
                return;

            IVisitable<ZombieDamage> damageable = _damageZone.Triggers.Last().Value;

            damageable.Accept(new ZombieDamage(GetDamage()));
        }

        private float GetDamage() =>
            _staticDataService.Balance.ZombieDamage.Get(_persistentDataService.PersistentData.PlayerData.Level);

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Collider _attackTrigger;
            [SerializeField] private float _attackInterval = 1f;
            [SerializeField] private float _damageApplyDelay = 0.5f;
            [SerializeField] private float _layerSwitchDuration = 0.3f;
            [SerializeField] private float _layerFadeDelay = 0.5f;
            [SerializeField] private string _attackTriggerName = "Attack";
            [SerializeField] private int _attackLayerIndex = 1;

            public Collider AttackTrigger => _attackTrigger;
            public float AttackInterval => _attackInterval;
            public float DamageApplyDelay => _damageApplyDelay;
            public float LayerSwitchDuration => _layerSwitchDuration;
            public float LayerFadeDelay => _layerFadeDelay;
            public string AttackTriggerName => _attackTriggerName;
            public int AttackLayerIndex => _attackLayerIndex;
        }
    }
}