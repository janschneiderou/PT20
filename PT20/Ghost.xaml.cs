using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace PT20
{
    /// <summary>
    /// Interaction logic for Ghost.xaml
    /// </summary>
    public partial class Ghost : UserControl
    {
        public Ghost()
        {
            InitializeComponent();
        }
        public void hideFeedback()
        {
            hunch.Visibility = Visibility.Collapsed;
            leftHand.Visibility = Visibility.Collapsed;
            rightHand.Visibility = Visibility.Collapsed;
            back.Visibility = Visibility.Collapsed;
            leftArm.Visibility = Visibility.Collapsed;
            rightArm.Visibility = Visibility.Collapsed;
        }

        public void vanish()
        {

            var animation1Opacity = new DoubleAnimation();
            animation1Opacity.From = 1.0;
            animation1Opacity.To = 0;
            animation1Opacity.Duration = new Duration(TimeSpan.FromMilliseconds(3000));

            Storyboard.SetTarget(animation1Opacity, this);
            Storyboard.SetTargetProperty(animation1Opacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animating1 = new Storyboard();
            animating1.Children.Add(animation1Opacity);
            // animating1.Completed += animating1_Completed;
            animating1.Begin();
        }

    }
}
