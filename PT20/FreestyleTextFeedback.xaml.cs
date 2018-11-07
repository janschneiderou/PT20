using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace PT20
{
    /// <summary>
    /// Interaction logic for FreestyleTextFeedback.xaml
    /// </summary>
    public partial class FreestyleTextFeedback : UserControl
    {
        public FreestyleTextFeedback()
        {
            InitializeComponent();
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
