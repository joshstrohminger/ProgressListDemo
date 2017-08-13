namespace ProgressListDemo
{
    public class StartItem : ObservableObject
    {
        private ItemStatus _status;

        public string Text { get; set; }

        public ItemStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public StartItem(string text, ItemStatus status = ItemStatus.Idle)
        {
            Text = text;
            Status = status;
        }
    }
}