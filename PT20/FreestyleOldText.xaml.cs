using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace PT20
{
    /// <summary>
    /// Interaction logic for FreestyleOldText.xaml
    /// </summary>
    public partial class FreestyleOldText : UserControl
    {
        public FreestyleOldText()
        {
            InitializeComponent();
        }

        internal void startBanish()
        {

            Storyboard s;
            try
            {
                s = (Storyboard)this.FindResource("MyStoryboard") as Storyboard;
                Storyboard.SetTarget(s, myGrid);
                s.Begin();
            }
            catch (Exception e)
            {
                int x = 0;
                x++;
            }


        }
        public void vanish()
        {

            var animationGoOpacity = new DoubleAnimation();
            animationGoOpacity.From = 1.0;
            animationGoOpacity.To = 0;
            animationGoOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

            Storyboard.SetTarget(animationGoOpacity, this);
            Storyboard.SetTargetProperty(animationGoOpacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animatingGo = new Storyboard();
            animatingGo.Children.Add(animationGoOpacity);

            animatingGo.Begin();
        }
    }
}
