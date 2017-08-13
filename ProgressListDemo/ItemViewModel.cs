using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProgressListDemo
{
    public class ItemViewModel : ObservableObject
    {
        private ItemStatus _status;
        private string _text;
        private Random _random = new Random();

        public RelayCommand<ItemStatus> SetStatus { get; }
        public RelayCommand RunDemo { get; }

        public ItemStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public List<StartItem> Tasks { get; }

        public ItemViewModel()
        {
            SetStatus = new RelayCommand<ItemStatus>(s => Status = s);

            Tasks = new List<StartItem>
            {
                new StartItem("The first thing to do"),
                new StartItem("Now we're doing the second thing"),
                new StartItem("And here we are now"),
                new StartItem("doing some more stuff"),
                new StartItem("just because we can")
            };

            RunDemo = new RelayCommand(async () =>
            {
                foreach (var item in Tasks)
                {
                    item.Status = ItemStatus.Idle;
                }
                await Task.Delay(500);
                foreach (var item in Tasks)
                {
                    item.Status = ItemStatus.Running;
                    await Task.Delay(2000);
                    item.Status = _random.Next(2) == 1 ? ItemStatus.Done : ItemStatus.Failed;
                }
            });
        }
    }

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

        public StartItem(string text)
        {
            Text = text;
        }
    }
}
