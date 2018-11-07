using Microsoft.Kinect.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PT20
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainWindow parent;
        public MainMenu()
        {


            InitializeComponent();

            KinectRegion.SetKinectRegion(this, kinectRegion);


            App app = ((App)Application.Current);
            
            app.KinectRegion = kinectRegion;

            videoTutorial.Source = new System.Uri(MainWindow.executingDirectory+"\\Videos\\Tutorial.mp4");
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FreestyleButton.Content = "Pressed";
            MainWindow.pitchTime = int.Parse(pitchTime.Text);
            MainWindow.pitch = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.pitchTime = int.Parse(pitchTime.Text);
            MainWindow.pitch = true;
        }

        private void pitchTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
            int x = 1;
            x++;
        }
        private static bool IsTextAllowed(string text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
            delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            videoTutorial.Play();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            videoTutorial.Stop();
        }

        private void volumeCalibrationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void histogram_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
