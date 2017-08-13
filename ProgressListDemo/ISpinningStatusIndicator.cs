using System.Windows.Controls;

namespace ProgressListDemo
{
    public interface ISpinningStatusIndicator
    {
        ItemStatus Status { get; set; }
        double Size { get; set; }
    }
}
