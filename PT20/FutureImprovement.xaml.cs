
using System.Windows;
using System.Windows.Controls;


namespace PT20
{
    /// <summary>
    /// Interaction logic for FutureImprovement.xaml
    /// </summary>
    public partial class FutureImprovement : UserControl
    {
        public delegate void SelectionEvent();
        public event SelectionEvent selectionEvent;

        string nextImprovement = "";

        public FutureImprovement()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonPosture":
                    nextImprovement = "Next Posture";
                    MainWindow.focusedString = MainWindow.focusedPosture;
                    break;
                case "buttonVolume":
                    nextImprovement = "Next Volume";
                    MainWindow.focusedString = "";
                    break;
                case "buttonGesture":
                    nextImprovement = "Next Gesture";
                    MainWindow.focusedString = MainWindow.focusedGestures;
                    break;
                case "buttonPauses":
                    nextImprovement = "Next Pauses";
                    MainWindow.focusedString = MainWindow.focusedPauses;
                    break;
                case "buttonFace":
                    nextImprovement = "Next Face";
                    MainWindow.focusedString = "";
                    break;

            }


            MainWindow.logString = MainWindow.logString + System.Environment.NewLine + nextImprovement;
            selectionEvent();
        }
    }
}