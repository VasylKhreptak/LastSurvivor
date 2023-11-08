using Flexalon;
using UniRx;
using UnityEngine;
using Zenject;

namespace Grid
{
    public class GridSizeUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FlexalonGridLayout _grid;

        private GridData _gridData;

        [Inject]
        private void Constructor(GridData gridData)
        {
            _gridData = gridData;
        }

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate() => _grid ??= GetComponent<FlexalonGridLayout>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _gridData.Rows.Skip(1).Subscribe(_ => OnAnyGridSizeChanged()).AddTo(_subscriptions);
            _gridData.Columns.Skip(1).Subscribe(_ => OnAnyGridSizeChanged()).AddTo(_subscriptions);
        }

        private void StopObserving() => _subscriptions.Clear();

        private void OnAnyGridSizeChanged()
        {
            _grid.Rows = (uint)_gridData.Rows.Value;
            _grid.Columns = (uint)_gridData.Rows.Value;
        }
    }
}