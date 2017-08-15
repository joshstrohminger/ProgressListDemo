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

        private const double MaxRotationSeconds = 0.3;
        private static readonly Duration ZoomTime = new Duration(TimeSpan.FromSeconds(0.25));
        private static readonly Duration ColorTime = new Duration(TimeSpan.FromSeconds(0.5));
        private static readonly RepeatBehavior Once = new RepeatBehavior(1);
        private static readonly IEasingFunction ZoomInEase =
            new BackEase {EasingMode = EasingMode.EaseOut, Amplitude = 0.5};

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
            var show = true;
            var fromScaleX = Scale.ScaleX;
            var fromScaleY = Scale.ScaleY;
            double toScale = 1;
            var fromColor = ((SolidColorBrush) Circle.Fill).Color;
            Color toColor;
            var fromAngle = Rotation.Angle;
            double toAngle;
            RepeatBehavior repeat;

            switch (Status)
            {
                case ItemStatus.Idle:
                    toScale = 0;
                    toColor = Colors.DeepSkyBlue;
                    toAngle = fromAngle + 90;
                    repeat = RepeatBehavior.Forever;
                    show = false;
                    break;
                case ItemStatus.Running:
                    toColor = Colors.DeepSkyBlue;
                    toAngle = fromAngle + 90;
                    repeat = RepeatBehavior.Forever;
                    break;
                case ItemStatus.Done:
                    toColor = Colors.Green;
                    toAngle = Math.Ceiling(fromAngle / 90d) * 90;
                    repeat = Once;
                    break;
                case ItemStatus.Failed:
                    toColor = Colors.Red;
                    toAngle = Math.Ceiling((fromAngle - 45) / 90d) * 90 + 45;
                    repeat = Once;
                    break;
                default:
                    return;
            }

            var rotationTime = new Duration(TimeSpan.FromSeconds(MaxRotationSeconds * ((toAngle - fromAngle) / 90)));
            
            var xAnim = new DoubleAnimation(fromScaleX, toScale, ZoomTime);
            var yAnim = new DoubleAnimation(fromScaleY, toScale, ZoomTime);
            if (show)
            {
                Indicator.Opacity = 1;
                xAnim.EasingFunction = ZoomInEase;
                yAnim.EasingFunction = ZoomInEase;
            }
            else
            {
                xAnim.Completed += (sender, args) => Indicator.Opacity = 0;
            }
            Scale.BeginAnimation(ScaleTransform.ScaleXProperty, xAnim, HandoffBehavior.SnapshotAndReplace);
            Scale.BeginAnimation(ScaleTransform.ScaleYProperty, yAnim, HandoffBehavior.SnapshotAndReplace);

            Circle.Fill = new SolidColorBrush(fromColor);
            Circle.Fill.BeginAnimation(SolidColorBrush.ColorProperty,
                new ColorAnimation(fromColor, toColor, ColorTime),
                HandoffBehavior.SnapshotAndReplace);

            Rotation.BeginAnimation(RotateTransform.AngleProperty,
                new DoubleAnimation(fromAngle, toAngle, rotationTime, FillBehavior.HoldEnd) {RepeatBehavior = repeat},
                HandoffBehavior.SnapshotAndReplace);
        }

        #endregion
    }
}
