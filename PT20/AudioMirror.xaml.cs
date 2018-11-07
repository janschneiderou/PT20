using Microsoft.Kinect;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace PT20
{
    /// <summary>
    /// Interaction logic for AudioMirror.xaml
    /// </summary>
    public partial class AudioMirror : UserControl
    {
        MainWindow parent;

        public AudioMirror()
        {
            InitializeComponent();
        }
        public void initialize(MainWindow parent)
        {
            this.parent = parent;

            if (parent.audioHandler.reader != null)
            {
                // Subscribe to new audio frame arrived events
                parent.audioHandler.reader.FrameArrived += Reader_FrameArrived;
            }


            //    CompositionTarget.Rendering += this.UpdateEnergy;

            //if (this.reader != null)
            //{
            //    // Subscribe to new audio frame arrived events
            //    this.reader.FrameArrived += this.Reader_FrameArrived;
            //}

        }
        public void unload()
        {
            parent.audioHandler.reader.FrameArrived -= Reader_FrameArrived;
        }

        public void Reader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            parent.audioHandler.Reader_FrameArrived(sender, e);
            if (MainWindow.myState == MainWindow.States.freestyle)
            {
                var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_audio-feedback-1.png", UriKind.Relative);

                float currentVolume = parent.audioHandler.energy[parent.audioHandler.energyIndex];
                if (Math.Abs(currentVolume) < 0.25)
                {
                    uriSource = new Uri(@"/PT20;component/Images/ic_audio-feedback-1.png", UriKind.Relative);
                }
                else if (Math.Abs(currentVolume) < 0.5)
                {
                    uriSource = new Uri(@"/PT20;component/Images/ic_audio-feedback-2.png", UriKind.Relative);
                }
                else if (Math.Abs(currentVolume) < 0.75)
                {
                    uriSource = new Uri(@"/PT20;component/Images/ic_audio-feedback-3.png", UriKind.Relative);
                }
                else
                {
                    uriSource = new Uri(@"/PT20;component/Images/ic_audio-feedback-4.png", UriKind.Relative);
                }


                MicroPhoneImage.Source = new BitmapImage(uriSource);
            }
            else
            {
                //  parent.audioHandler.UpdateEnergy(sender, null);
                kinectImage.Source = parent.audioHandler.energyBitmap;
            }



        }
    }
}
