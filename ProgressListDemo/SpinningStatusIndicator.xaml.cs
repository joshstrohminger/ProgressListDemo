using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProgressListDemo
{
    /// <summary>
    /// Interaction logic for SpinningStatusIndicator.xaml
    /// </summary>
    public partial class SpinningStatusIndicator : ISpinningStatusIndicator
    {
        #region Constants

        private const double MaxAnimationTime = 0.5;
        
        #endregion

        #region Dependency Properties

        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(ItemStatus),
            typeof(SpinningStatusIndicator),
            new PropertyMetadata(ItemStatus.Idle, (o, args) => ((SpinningStatusIndicator) o).UpdateAnimations()));

        public static DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(SpinningStatusIndicator),
                new PropertyMetadata(50d));

        #endregion

        #region Properties

        public ItemStatus Status
        {
            get => (ItemStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public double Size
        {
            get => (double) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        #endregion

        #region Construction

        public SpinningStatusIndicator()
        {
            InitializeComponent();
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            var fromColor = ((SolidColorBrush) Circle.Fill).Color;
            Color toColor;
            var fromAngle = Rotation.Angle;
            double toAngle;
            RepeatBehavior repeat;

            switch (Status)
            {
                case ItemStatus.Idle:
                    toColor = Colors.White;
                    toAngle = fromAngle + 90;
                    repeat = RepeatBehavior.Forever;
                    break;
                case ItemStatus.Running:
                    toColor = Colors.DeepSkyBlue;
                    toAngle = fromAngle + 90;
                    repeat = RepeatBehavior.Forever;
                    break;
                case ItemStatus.Done:
                    toColor = Colors.Green;
                    toAngle = Math.Ceiling(fromAngle / 90d) * 90;
                    repeat = new RepeatBehavior(1);
                    break;
                case ItemStatus.Failed:
                    toColor = Colors.Red;
                    toAngle = Math.Ceiling((fromAngle - 45) / 90d) * 90 + 45;
                    repeat = new RepeatBehavior(1);
                    break;
                default:
                    return;
            }

            var rotationTime = new Duration(TimeSpan.FromSeconds(MaxAnimationTime * ((toAngle - fromAngle) / 90)));

            Circle.Fill = new SolidColorBrush(fromColor);
            Circle.Fill.BeginAnimation(SolidColorBrush.ColorProperty,
                new ColorAnimation(fromColor, toColor, rotationTime),
                HandoffBehavior.SnapshotAndReplace);

            Rotation.BeginAnimation(RotateTransform.AngleProperty,
                new DoubleAnimation(fromAngle, toAngle, rotationTime, FillBehavior.HoldEnd) {RepeatBehavior = repeat},
                HandoffBehavior.SnapshotAndReplace);
        }

        #endregion
    }
}
