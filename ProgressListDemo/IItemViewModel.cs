using System.Collections.Generic;

namespace ProgressListDemo
{
    public interface IItemViewModel
    {
        RelayCommand<ItemStatus> SetStatus { get; }
        RelayCommand RunDemo { get; }
        ItemStatus Status { get; set; }
        List<StartItem> Tasks { get; }
    }
}