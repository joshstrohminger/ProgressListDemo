using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProgressListDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const double MaxAnimationTime = 0.5;

        public MainWindow()
        {
            this.DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var oldVm = dependencyPropertyChangedEventArgs.OldValue as ItemViewModel;
            if (oldVm != null)
            {
                oldVm.PropertyChanged -= VmOnPropertyChanged;
            }

            var vm = dependencyPropertyChangedEventArgs.NewValue as ItemViewModel;
            if (vm != null)
            {
                vm.PropertyChanged += VmOnPropertyChanged;
            }
        }

        private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var vm = (ItemViewModel) sender;

            if (propertyChangedEventArgs.PropertyName == nameof(vm.Status))
            {
                var fromColor = ((SolidColorBrush) Circle.Fill).Color;
                Color toColor;
                var fromAngle = Rotation.Angle;
                double toAngle;
                RepeatBehavior repeat;

                switch (vm.Status)
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
                Circle.Fill.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(fromColor, toColor, rotationTime),
                    HandoffBehavior.SnapshotAndReplace);

                Rotation.BeginAnimation(RotateTransform.AngleProperty,
                    new DoubleAnimation(fromAngle, toAngle, rotationTime, FillBehavior.HoldEnd){RepeatBehavior = repeat},
                    HandoffBehavior.SnapshotAndReplace);
            }
        }
    }
}
