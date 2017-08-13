namespace ProgressListDemo
{
    public class SpinningStatusIndicatorDesignData : ISpinningStatusIndicator
    {
        public ItemStatus Status { get; set; }
        public double Size { get; set; }

        public SpinningStatusIndicatorDesignData()
        {
            Status = ItemStatus.Done;
            Size = 30;
        }
    }
}
