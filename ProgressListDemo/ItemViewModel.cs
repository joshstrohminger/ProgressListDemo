namespace ProgressListDemo
{
    public class ItemViewModel : ObservableObject
    {
        private ItemStatus _status;

        public RelayCommand<ItemStatus> SetStatus { get; }

        public ItemStatus Status
        {
            get => _status;
            private set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public ItemViewModel()
        {
            SetStatus = new RelayCommand<ItemStatus>(s => Status = s);
        }
    }
}
