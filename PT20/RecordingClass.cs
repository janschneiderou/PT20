using Accord.Video.FFMPEG;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Accord.Audio;
using Accord.Audio.Formats;
using Accord.DirectSound;
using System.Drawing;

namespace PT20
{
    public class RecordingClass 
    {

        VideoFileWriter vf;
        AudioCaptureDevice AudioSource;


        System.DateTime sartRecordingTime;
        System.DateTime lastRecordingVideoTime;
        System.DateTime lastRecordingSoundTime;




        private MemoryStream stream;


        private WaveEncoder encoder;

        Process process;

        private float[] current;

        private int frames;
        private int samples;
        private TimeSpan duration;

        string filename;
        string filenameAudio;
        string filenameCombined;
        int i;

        private Thread myCaptureThread;
        public bool isRecording = false;

        Bitmap bmpScreenShot;

        public RecordingClass()
        {
            filenameAudio = MainWindow.recordingPath + "\\" + MainWindow.recordingID + ".wav";
            filenameCombined = MainWindow.recordingPath + "\\" + MainWindow.recordingID + "c.mp4";
            filename = MainWindow.recordingPath + "\\" + MainWindow.recordingID + ".mp4";

            initalizeAudioStuff();

            vf = new VideoFileWriter();
        }

        #region audio
        private void initalizeAudioStuff()
        {

            try
            {
                AudioSource = new AudioCaptureDevice();
                AudioDeviceInfo info = null;
                var adc = new AudioDeviceCollection(AudioDeviceCategory.Capture);
                foreach (var ad in adc)
                {
                    string desc = ad.Description;
                    if (desc.IndexOf("Audio") > -1)
                    {
                        info = ad;
                    }
                }
                if (info == null)
                {
                    AudioSource = new AudioCaptureDevice();
                }
                else
                {
                    AudioSource = new AudioCaptureDevice(info);
                }

                //AudioCaptureDevice source = new AudioCaptureDevice();
                AudioSource.DesiredFrameSize = 4096;
                AudioSource.SampleRate = 44100;
                //int sampleRate = 44100;
                //int sampleRate = 22050;

                AudioSource.NewFrame += AudioSource_NewFrame;
                // AudioSource.Format = SampleFormat.Format64BitIeeeFloat;
                AudioSource.AudioSourceError += AudioSource_AudioSourceError;
                // AudioSource.Start();
                int x = 1;
            }
            catch
            {

            }

            // Create buffer for wavechart control
            current = new float[AudioSource.DesiredFrameSize];

            // Create stream to store file
            stream = new MemoryStream();
            encoder = new WaveEncoder(stream);

            frames = 0;
            samples = 0;



           

        }

        private void AudioSource_AudioSourceError(object sender, AudioSourceErrorEventArgs e)
        {
            int x = 1;
            x++;
        }

        private void AudioSource_NewFrame(object sender, Accord.Audio.NewFrameEventArgs e)
        {

            if (isRecording)
            {
                System.TimeSpan diff1 = DateTime.Now.Subtract(sartRecordingTime);
                if (diff1.Seconds >= 0.0)
                {
                    //writer.WriteAudioFrame(e.Signal.RawData);
                    e.Signal.CopyTo(current);

                    encoder.Encode(e.Signal);


                    duration += e.Signal.Duration;

                    samples += e.Signal.Samples;
                    frames += e.Signal.Length;
                }


            }
        }

        private void doAudioStop()
        {
            // Stops both cases
            if (AudioSource != null)
            {
                // If we were recording
                AudioSource.SignalToStop();
                AudioSource.WaitForStop();
            }


            var fileStream = File.Create(filenameAudio);
            stream.WriteTo(fileStream);
            fileStream.Close();
            fileStream.Dispose();
            // Also zero out the buffers and screen
            Array.Clear(current, 0, current.Length);
           // AudioSource.Dispose();

        }

        #endregion

        public void startRecording()
        {
            isRecording = true;
          
            

            int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth * 2;
            int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight * 2;
            bmpScreenShot = new Bitmap(screenWidth, screenHeight);

            vf.Open(filename, screenWidth, screenHeight, 25, VideoCodec.Default, 400000);

            myCaptureThread = new Thread(new ThreadStart(captureFunction));
            myCaptureThread.Start();

            // Start
            AudioSource.Start();

        }

        #region stop

        public void stopRecording()
        {
            isRecording = false;
            doAudioStop();
           

        }

        public void combineFiles()
        {
            try
            {
                // Process.Start("ffmpeg", "-i " + filename + " -i " + filenameAudio + " -c:v copy -c:a aac -strict experimental " + filenameCombined + "");

                string FFmpegFilename;
                string[] text = File.ReadAllLines(MainWindow.executingDirectory + "\\Config\\FFMPEGLocation.txt");
                FFmpegFilename = text[0];

                process = new Process();
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = FFmpegFilename;
                // process.StartInfo.FileName = @"C:\FFmpeg\bin\ffmpeg.exe";

                process.StartInfo.Arguments = "-i " + filename + " -i " + filenameAudio + " -c:v copy -c:a aac -strict experimental " + filenameCombined + " -shortest";


                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();
                vf.Dispose();
                AudioSource.Dispose();

            }
            catch (Exception xx)
            {
                int x = 0;
                x++;
            }

            
            

        }

        #endregion

        #region videoStuff
        private void captureFunction()
        {
            while (isRecording == true)
            {
                try
                {
                    int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth * 2;
                    int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight * 2;


                    //Bitmap bmpScreenShot = new Bitmap(screenWidth, screenHeight);
                    Graphics gfx = Graphics.FromImage((System.Drawing.Image)bmpScreenShot);
                    gfx.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));
                    System.TimeSpan diff1 = DateTime.Now.Subtract(sartRecordingTime);
                    vf.WriteVideoFrame(bmpScreenShot, diff1);
                }
                catch
                {

                }

                Thread.Sleep(40);
            }
            vf.Close();
            
        }

        #endregion
    }
}
