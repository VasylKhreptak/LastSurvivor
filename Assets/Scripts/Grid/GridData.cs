using UniRx;

namespace Grid
{
    public class GridData
    {
        public readonly IntReactiveProperty Rows;
        public readonly IntReactiveProperty Columns;
        public readonly IntReactiveProperty Count;
        public readonly IntReactiveProperty Capacity;

        public readonly IReadOnlyReactiveProperty<bool> IsFull;
        public readonly IReadOnlyReactiveProperty<bool> IsEmpty;

        public GridData()
        {
            Rows = new IntReactiveProperty(1);
            Columns = new IntReactiveProperty(1);
            Count = new IntReactiveProperty();
            Capacity = new IntReactiveProperty(1);
            IsFull = Count.Select(count => count >= Capacity.Value).ToReadOnlyReactiveProperty();
            IsEmpty = Count.Select(count => count == 0).ToReadOnlyReactiveProperty();
        }

        public GridData(int rows, int columns, int capacity)
        {
            Rows = new IntReactiveProperty(rows);
            Columns = new IntReactiveProperty(columns);
            Count = new IntReactiveProperty();
            Capacity = new IntReactiveProperty(capacity);
            IsFull = Count.Select(count => count >= Capacity.Value).ToReadOnlyReactiveProperty();
            IsEmpty = Count.Select(count => count == 0).ToReadOnlyReactiveProperty();
        }
    }
}