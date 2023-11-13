using Plugins.Banks;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ClampedBanks
{
    public class ClampedBankValuesText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "{0}/{1}";

        private ClampedIntegerBank _bank;

        [Inject]
        private void Constructor(ClampedIntegerBank bank)
        {
            _bank = bank;
        }

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _bank.Value.Subscribe(_ => OnAnyValueChanged()).AddTo(_subscriptions);
            _bank.MaxValue.Subscribe(_ => OnAnyValueChanged()).AddTo(_subscriptions);
        }

        private void StopObserving() => _subscriptions?.Clear();

        private void OnAnyValueChanged() => _tmp.text = string.Format(_format, _bank.Value, _bank.MaxValue);
    }
}