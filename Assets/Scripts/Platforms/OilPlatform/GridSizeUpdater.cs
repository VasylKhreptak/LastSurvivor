using Data.Persistent;
using Flexalon;
using UniRx;
using UnityEngine;
using Zenject;

namespace Platforms.OilPlatform
{
    public class GridSizeUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FlexalonGridLayout _grid;

        private OilPlatformData _platformData;

        [Inject]
        private void Constructor(OilPlatformData platformData)
        {
            _platformData = platformData;
        }

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate() => _grid ??= GetComponent<FlexalonGridLayout>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _platformData.GridRows.Skip(1).Subscribe(_ => OnAnyGridSizeChanged()).AddTo(_subscriptions);
            _platformData.GridColumns.Skip(1).Subscribe(_ => OnAnyGridSizeChanged()).AddTo(_subscriptions);
            OnAnyGridSizeChanged();
        }

        private void StopObserving() { }

        private void OnAnyGridSizeChanged()
        {
            _grid.Rows = (uint)_platformData.GridRows.Value;
            _grid.Columns = (uint)_platformData.GridColumns.Value;
            _grid.ForceUpdate();
        }
    }
}