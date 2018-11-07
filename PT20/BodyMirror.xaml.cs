﻿
using System.Windows.Controls;


namespace PT20
{
    /// <summary>
    /// Interaction logic for BodyMirror.xaml
    /// </summary>
    public partial class BodyMirror : UserControl
    {
        MainWindow parent;
        // FileSink filesink;
        //    VideoFileWriter writer;

        public BodyMirror()
        {
            InitializeComponent();
        }
        public void initialize(MainWindow parent)
        {
            this.parent = parent;

            // create instance of video writer
            //    writer = new VideoFileWriter();
            // create new video file
            //      writer.Open("test.avi", myImage.ActualWidth, myImage.ActualHeight, 25, VideoCodec.MPEG4);
            // create a bitmap to save into the video file

            parent.videoHandler.multiFrameSourceReader.MultiSourceFrameArrived += multiFrameSourceReader_MultiSourceFrameArrived;

        }
        public void unload()
        {
            parent.videoHandler.multiFrameSourceReader.MultiSourceFrameArrived -= multiFrameSourceReader_MultiSourceFrameArrived;
        }

        public void initializeB(MainWindow parent)
        {
            this.parent = parent;
            parent.videoHandler.multiFrameSourceReader.MultiSourceFrameArrived += multiFrameSourceReader_MultiSourceFrameArrivedB;
        }

        void multiFrameSourceReader_MultiSourceFrameArrivedB(object sender, Microsoft.Kinect.MultiSourceFrameArrivedEventArgs e)
        {
            parent.videoHandler.Reader_MultiSourceFrameArrived(sender, e);
            myImage.Source = parent.videoHandler.kinectImage.Source;
            //  parent.heroMode.heroAvatar.Source = parent.videoHandler.kinectImage.Source;

            //     writer.WriteVideoFrame(myImage);

            //      writer.Close();

            if (parent.videoHandler.kinectImage.Source != null)
            {
                int x = 1;
                x++;
                //     parent.heroMode.heroAvatar.Source = parent.videoHandler.kinectImage.Source;
            }
        }

        void multiFrameSourceReader_MultiSourceFrameArrived(object sender, Microsoft.Kinect.MultiSourceFrameArrivedEventArgs e)
        {
            parent.videoHandler.Reader_MultiSourceFrameArrived(sender, e);
            myImage.Source = parent.videoHandler.kinectImage.Source;


            //     writer.WriteVideoFrame(myImage);

            //      writer.Close();

        }
        public void close()
        {

        }
    }
}
