using System.Collections.Generic;

namespace ProgressListDemo
{
    public class ItemViewModelDesignData : IItemViewModel
    {
        public RelayCommand<ItemStatus> SetStatus { get; } = new RelayCommand<ItemStatus>(x => { });
        public RelayCommand RunDemo { get; } = new RelayCommand(() => { });
        public ItemStatus Status { get; set; } = ItemStatus.Running;
        public List<StartItem> Tasks { get; } = new List<StartItem>
        {
            new StartItem("The first thing to do", ItemStatus.Done),
            new StartItem("Now we're doing the second thing", ItemStatus.Done),
            new StartItem("And here we are now", ItemStatus.Running),
            new StartItem("doing some more stuff"),
            new StartItem("just because we can")
        };
    }
}
