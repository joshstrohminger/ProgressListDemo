using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgressListDemo
{
    public class ItemViewModel : ObservableObject, IItemViewModel
    {
        private ItemStatus _status;
        private readonly Random _random = new Random();
        private bool _runningDemo;

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
                _runningDemo = true;
                foreach (var item in Tasks)
                {
                    item.Status = ItemStatus.Running;
                    await Task.Delay(_random.Next(100, 2000));
                    item.Status = item == Tasks.Last() ? ItemStatus.Failed : ItemStatus.Done;
                }
                await Task.Delay(2000);
                foreach (var item in Tasks)
                {
                    item.Status = ItemStatus.Idle;
                }
                _runningDemo = false;
                CommandManager.InvalidateRequerySuggested();
            }, () => !_runningDemo);
        }
    }
}
