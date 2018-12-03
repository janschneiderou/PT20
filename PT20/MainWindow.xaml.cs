using Microsoft.Kinect;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       
        public enum States { entryScreen, menu, freestyle, volumeCalibration, individual, histogram};
        public static States myState;

        #region kinectStuff
        private KinectSensor kinectSensor;
        public InfraredFrameReader frameReader = null;

        public VideoHandler videoHandler;
        public AudioHandler audioHandler;
        public FaceFrameHandler faceFrameHandler;
        public BodyFrameHandler bodyFrameHandler;
       

        #endregion

        #region GUIdefinitions

        EntryScreen entryScreen;
        MainMenu mainMenu;
        public FreestyleMode freestyleMode;
        public PauseReport pauseReport;
        public GestureReport gestureReport;
        public PostureReport postureReport;
        public ReportControl reportControl;
        public FutureImprovement futureImprovement;
        public VolumeCalibration volumeCalibrationMode;
        public History history;


        #endregion

        #region usedClasses

        public RecordingClass recordingClass;
        public RulesAnalyzerFIFO rulesAnalyzerFIFO;
        public StoreClass storeClass;

        #endregion

        #region logs
        public static string focusedString = "";
        public static string focusedPauses = "";
        public static string focusedGestures = "";
        public static string focusedPosture = "";

        public static string stringStartFinish = "";
        public static string stringGestures = "";
        public static string stringInterruptions = "";
        public static string stringFreestyleFeedbacks = "";
        public static string stringSpeakingTime = "";
        public static string stringPausingTime = "";
        public static string stringIndividualSkills = "";
        public static string stringIndividualFeedbacks = "";
        public static string stringMistakes = "";
        public static string stringReport = "";
        public static string stringGoodies = "";
        

        public static ArrayList speakTimes;
        public static ArrayList gestureTimes;
        public static ImageSource[] gestureImages;
        public static ImageSource[] postureImages;

        public static int totalPostureMistakes = 0;
        public static int totalGesturesMistakes = 0;
        public static int totalVolumeMistakes = 0;
        public static int totalCadenceMistakes = 0;
        public static int totalHmmmMistakes = 0;
        public static int totalDancingMistakes = 0;
        public static int totalSeriousMistakes = 0;
        public static int totalMistakes = 0;

        public static int totalSmiles = 0;


        public static string timePostureMistakes = "";
        public static string timeGestureMistakes = "";
        public static string timeVolumeMistakes = "";
        public static string timeCadenceMistakes = "";
        public static string timeHmmmMistakes = "";
        public static string timeDancingMistakes = "";
        public static string timeSeriousMistakes = "";

        public static string timeSmiles = "";

        public static double presentationDuration;
        public static double postureMistakeTime;
        public static double armsCrossedMistakeTime;
        public static double hunchMistakeTime;
        public static double handsUnderhipsMistakeTime;
        public static double handsNotVisibleMistakeTime;
        public static double volumeMistakeTime;
        public static double highVolumeMistakeTime;
        public static double lowVolumeMistakeTime;
        public static double gestureMistakeTime;
        public static double cadenceMistakeTime;
        public static double dancingMistakeTime;
        public static double hmmmMistakeTime;
        public static double seriousMistakeTime;

        public static double smileTime;
        public static double totalgoodiesTime;

        public static double presentationStarted = 0;
        public static double presentationFinished;

        public static string logString;
        #endregion

        #region staticStringsAndControlVariables

        public static bool freeGoal = false;
        public static string executingDirectory = "";
        public static string userDirectory = "";
        public static string usersPath = "";
        public static string recordingPath ="";
        public static string recordingID = "";
        public static double MaxRecordingTime = 90000;

        public static string ipAddress = "";
        public static int portNumber = 0;

        public static System.DateTime startPresentation;
        public static System.DateTime stopPresentation;

        #endregion

        public static bool stopGesture = false;
        public static bool pitch = false;
        public static int pitchTime = 70;
     



        public MainWindow()
        {
            InitializeComponent();
            initKinectStuff();
            initUsersStuff();


            storeClass = new StoreClass();
            myState = new States();
            myState = States.entryScreen;
          
        }

        #region initMethods

        private void initUsersStuff()
        {
            executingDirectory = Directory.GetCurrentDirectory();
           // usersPath = executingDirectory + "\\users"; 
            usersPath = System.IO.Path.Combine(executingDirectory, "users");
            bool exists = System.IO.Directory.Exists(usersPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(usersPath);
        }

        public void initKinectStuff()
        {
            this.kinectSensor = KinectSensor.GetDefault();
            this.kinectSensor.Open();
            this.frameReader = this.kinectSensor.InfraredFrameSource.OpenReader();

            videoHandler = new VideoHandler(this.kinectSensor);
            audioHandler = new AudioHandler(this.kinectSensor);
            bodyFrameHandler = new BodyFrameHandler(this.kinectSensor);
            faceFrameHandler = new FaceFrameHandler(this.kinectSensor);
        }

        #endregion

        #region controlStuff
        public void loadMode()
        {
            switch (myState)
            {
                case States.entryScreen:
                    loadEntryScreen();
                    break;
                case States.menu:
                    initFeedback();
                    loadMainMenu();
                    break;
                case States.freestyle:
                    loadFreestyle();
                    break;
                case States.volumeCalibration:
                    loadVolumeCalibration();

                    break;
                case States.histogram:
                    loadHistogram();
                    break;
               
            }
        }

       
        #region Freestyle
        private void loadFreestyle()
        {
            rulesAnalyzerFIFO = new RulesAnalyzerFIFO(this);

            if (freestyleMode == null)
            {
                freestyleMode = new FreestyleMode();
            }
            speakTimes = new ArrayList();
            gestureTimes = new ArrayList();
            gestureImages = new ImageSource[3];
            postureImages = new ImageSource[3];
            for (int i = 0; i < 3; i++)
            {
                gestureImages[i] = null;
                postureImages[i] = null;
            }
            freestyleMode.Height = this.ActualHeight;
            freestyleMode.Width = this.ActualWidth;
            MainCanvas.Children.Add(freestyleMode);
            Canvas.SetTop(freestyleMode, 0);
            Canvas.SetLeft(freestyleMode, 0);

            freestyleMode.Loaded += freeStyle_Loaded;
        }

        void freeStyle_Loaded(object sender, RoutedEventArgs e)
        {
            freestyleMode.parent = this;
            freestyleMode.loaded();
        }

        public void freeStyleMode_stopButton_Click(object sender, RoutedEventArgs e)
        {
            closeFreeStyleMode();
            myState = States.menu;
            loadMode();
            // we might have to unsubscribe (-=) to the click event of the pressed button
        }

        public void closeFreeStyleMode()
        {
            freestyleMode.setGhostMovingInvisible();
            freestyleMode.setOldTextInvisible();
            freestyleMode.setGhostInvisible();
            freestyleMode.setFeedbackTextInvisible();

            MainCanvas.Children.Remove(freestyleMode);
            //freestyleMode.Visibility = Visibility.Collapsed;
            freestyleMode.unload();


            freestyleMode = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
           // rulesAnalyzerFIFO = null;
            //TODO
             doPausesesReflection();


            

           
        }




        #endregion


        #region volumeCalibration

        private void loadVolumeCalibration()
        {
            volumeCalibrationMode = new VolumeCalibration();
            volumeCalibrationMode.Height = this.ActualHeight;
            volumeCalibrationMode.Width = this.ActualWidth;
            MainCanvas.Children.Add(volumeCalibrationMode);
            Canvas.SetTop(volumeCalibrationMode, 0);
            Canvas.SetLeft(volumeCalibrationMode, 0);
            volumeCalibrationMode.Loaded += volumeCalibrationMode_Loaded;
        }

        void volumeCalibrationMode_Loaded(object sender, RoutedEventArgs e)
        {
            volumeCalibrationMode.parent = this;
            volumeCalibrationMode.loaded();
        }

        void volumeCalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            closeMainMenu();
            myState = States.volumeCalibration;
            loadMode();
            volumeCalibrationMode.backButton.Click += volumeCalibrationMode_backButton_Click;
        }

        private void volumeCalibrationMode_backButton_Click(object sender, RoutedEventArgs e)
        {
            closeVolumeCalibration();
            myState = States.menu;
            loadMode();
            // we might have to unsubscribe (-=) to the click event of the pressed button
        }

        private void closeVolumeCalibration()
        {
            volumeCalibrationMode.backButton.Click -= volumeCalibrationMode_backButton_Click;
            MainCanvas.Children.Remove(volumeCalibrationMode);
        }

        #endregion

        #region Histogram

        public void loadHistogram()
        {
            history = new History();
            history.Height = this.ActualHeight;
            history.Width = this.ActualWidth;
            MainCanvas.Children.Add(history);
            Canvas.SetTop(history, 0);
            Canvas.SetLeft(history, 0);
            history.Loaded += History_Loaded;
        }

        private void History_Loaded(object sender, RoutedEventArgs e)
        {
            history.parent = this;
            history.loaded();
            history.returnButton.Click += ReturnButton_Click;
        }

        private void closeHistogram()
        {
            history.returnButton.Click -= Histogram_Click;
        
            MainCanvas.Children.Remove(history);
          
        }

        private void Histogram_Click(object sender, RoutedEventArgs e)
        {
            closeMainMenu();
            myState = States.histogram;
            loadMode();
           
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            closeHistogram();
            myState = States.menu;
            loadMode();
        }

        #endregion

        #region MainMenu
        private void loadMainMenu()
        {
            mainMenu = new MainMenu();
            mainMenu.Height = this.ActualHeight;
            mainMenu.Width = this.ActualWidth;
            MainCanvas.Children.Add(mainMenu);
            Canvas.SetTop(mainMenu, 0);
            Canvas.SetLeft(mainMenu, 0);
            mainMenu.Loaded += MainMenu_Loaded;
        }

        private void MainMenu_Loaded(object sender, RoutedEventArgs e)
        {
            mainMenu.parent = this;
            pitch = false;
            mainMenu.focusedLabel.Content = focusedString;
            mainMenu.FreestyleButton.Click += mainMenu_FreestyleClicked;
            mainMenu.volumeCalibrationButton.Click += volumeCalibrationButton_Click;
            mainMenu.histogram.Click += Histogram_Click;
            mainMenu.PitchButton.Click += mainMenu_FreestyleClicked;
        }

        

        private void mainMenu_FreestyleClicked(object sender, RoutedEventArgs e)
        {
            closeMainMenu();
            myState = States.freestyle;
            
            if (freestyleMode != null)
            {
                rulesAnalyzerFIFO.resetAll();
                freestyleMode.OrdinaryReturn_Click(null, null);
                freestyleMode.Visibility = Visibility.Visible;
            }
            else
            {
                loadMode();
            }

            
        }


        public void closeMainMenu()
        {
            MainCanvas.Children.Remove(mainMenu);
        }


        #endregion

        #region entryMenu
        private void loadEntryScreen()
        {
            entryScreen = new EntryScreen();
            entryScreen.Height = this.ActualHeight;
            entryScreen.Width = this.ActualWidth;
            MainCanvas.Children.Add(entryScreen);
            Canvas.SetTop(entryScreen, 0);
            Canvas.SetLeft(entryScreen, 0);
            entryScreen.Loaded += EntryScreen_Loaded;
        }

        private void EntryScreen_Loaded(object sender, RoutedEventArgs e)
        {
            entryScreen.parent = this;
            entryScreen.loaded();
            entryScreen.userSelected.Click += UserSelected_Click;
        }

        private void UserSelected_Click(object sender, RoutedEventArgs e)
        {
            if(entryScreen.freeGoal.IsChecked==true)
            {
                freeGoal = true;
            }
            MainCanvas.Children.Remove(entryScreen);
            entryScreen = null;
            try
            {

                string focusFile = System.IO.Path.Combine(userDirectory, "focused.txt");
                if (!File.Exists(focusFile))
                {
                    File.CreateText(focusFile);
                }
                    
                focusedString = System.IO.File.ReadAllText(focusFile);
            }
            catch
            {

            }
            
            myState = States.menu;
            loadMode();
            
        }
        #endregion

        #endregion

        #region selfreflection

        #region Pauses

        void doPausesesReflection()
        {
            pauseReport = new PauseReport();
            MainCanvas.Children.Add(pauseReport);


            Canvas.SetLeft(pauseReport, 20);
            Canvas.SetTop(pauseReport, 20);
            pauseReport.GoMainMenu.Click += GoMainMenu_Click_Pause;
        }

        private void GoMainMenu_Click_Pause(object sender, RoutedEventArgs e)
        {
            focusedPauses = pauseReport.answer1.Text;
            MainCanvas.Children.Remove(pauseReport);
            pauseReport = null;

            doGestureReflection();
        }
        #endregion

        #region Gestures

        void doGestureReflection()
        {

            gestureReport = new GestureReport();
            MainCanvas.Children.Add(gestureReport);


            Canvas.SetLeft(gestureReport, 20);
            Canvas.SetTop(gestureReport, 20);
            gestureReport.GoMainMenu.Click += GoMainMenu_Click_Gesture;
        }

        private void GoMainMenu_Click_Gesture(object sender, RoutedEventArgs e)
        {
            focusedGestures = gestureReport.answerQuestion.Text;
            MainCanvas.Children.Remove(gestureReport);
            gestureReport = null;

            doPostureReflection();
        }

        #endregion

        #region Posture

        void doPostureReflection()
        {
            postureReport = new PostureReport();
            MainCanvas.Children.Add(postureReport);


            Canvas.SetLeft(postureReport, 20);
            Canvas.SetTop(postureReport, 20);
            postureReport.GoMainMenu.Click += GoMainMenu_Click_Posture;
        }

        private void GoMainMenu_Click_Posture(object sender, RoutedEventArgs e)
        {
            focusedPosture = postureReport.answerQuestion.Text;
            MainCanvas.Children.Remove(postureReport);
            postureReport = null;
            doTimelineReflection();

        }

        #endregion

        #region TimeLine

        void doTimelineReflection()
        {
            reportControl = new ReportControl();
            MainCanvas.Children.Add(reportControl);
            reportControl.doInit();
            Canvas.SetLeft(reportControl, 20);
            Canvas.SetTop(reportControl, 20);

            reportControl.GoMainMenu.Click += GoReport_Click;
        }

        void GoReport_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Remove(reportControl);
            reportControl = null;

            doSelectionFutureReflection();

        }

        #region futureStuff

        void doSelectionFutureReflection()
        {
            futureImprovement = new FutureImprovement();
            MainCanvas.Children.Add(futureImprovement);

            Canvas.SetLeft(futureImprovement, 20);
            Canvas.SetTop(futureImprovement, 20);

            futureImprovement.selectionEvent += futureImprovement_selectionEvent;
        }

        void futureImprovement_selectionEvent()
        {
            MainCanvas.Children.Remove(futureImprovement);

            try
            {
                freestyleMode.unload();
                MainCanvas.Children.Remove(freestyleMode);
                freestyleMode = null;
            }
            catch
            {

            }

            
            string focusFile = System.IO.Path.Combine(userDirectory, "focused.txt");
            System.IO.File.WriteAllText(focusFile, focusedString);

            string focusFileGoals = System.IO.Path.Combine(recordingPath, "Goals.txt");
            string goals = focusedPauses + System.Environment.NewLine;
            goals =  goals + focusedGestures + System.Environment.NewLine;
            goals = goals + focusedPosture + System.Environment.NewLine;
            System.IO.File.WriteAllText(focusFileGoals, goals);

            doLogs();
            speakTimes = new ArrayList();
            gestureTimes = new ArrayList();
            myState = States.entryScreen;

            startAgain();
          //  loadMode();
        }

        #endregion

        #endregion




        #endregion


        #region doLogs

        public void doLogs()
        {
            //string filename = MainWindow.recordingPath + "\\Log.txt";
            string filename = System.IO.Path.Combine(MainWindow.recordingPath, "Log.txt");
            System.IO.File.WriteAllText(filename, logString);
            resetStrings();
        }

        public void resetStrings()
        {
            stringMistakes = "";
            timePostureMistakes = "";
            timeGestureMistakes = "";
            timeVolumeMistakes = "";
            timeCadenceMistakes = "";
            timeHmmmMistakes = "";
            timeDancingMistakes = "";
            timeSeriousMistakes = "";

            timeSmiles = "";

            stringStartFinish = "";
            stringGestures = "";
            stringInterruptions = "";
            stringFreestyleFeedbacks = "";
            stringSpeakingTime = "";
            stringPausingTime = "";
            stringIndividualSkills = "";
            stringIndividualFeedbacks = "";
            stringMistakes = "";
            stringReport = "";
            stringGoodies = "";
        }


        public void sendValues()
        {
            try
            {
                List<string> values = new List<string>();

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.apa.averageVolume + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.ffpa.smiling + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.AnkleRight].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.AnkleRight].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.AnkleRight].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.AnkleLeft].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.AnkleLeft].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.AnkleLeft].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ElbowRight].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ElbowRight].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ElbowRight].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ElbowLeft].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ElbowLeft].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ElbowLeft].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandRight].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandRight].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandRight].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandLeft].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandLeft].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandLeft].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandTipRight].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandTipRight].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandTipRight].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandTipLeft].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandTipLeft].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HandTipLeft].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.Head].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.Head].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.Head].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HipRight].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HipRight].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HipRight].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HipLeft].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HipLeft].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.HipLeft].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ShoulderRight].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ShoulderRight].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ShoulderRight].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ShoulderLeft].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ShoulderLeft].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.ShoulderLeft].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.SpineMid].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.SpineMid].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.SpineMid].Position.Z + "");

                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.SpineShoulder].Position.X + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.SpineShoulder].Position.Y + "");
                values.Add(rulesAnalyzerFIFO.myJudgementMaker.bfpa.body.Joints[JointType.SpineShoulder].Position.Z + "");

                storeClass.storeFrame(values);
            }
            catch
            {

            }
        }




        #endregion


        #region broadcastFeedback

        private Socket udpSendingSocket;
        private System.Net.IPEndPoint UDPendPoint;

        void initFeedback()
        {


            udpSendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

            IPAddress serverAddr = IPAddress.Parse(ipAddress);

            UDPendPoint = new IPEndPoint(serverAddr, portNumber);
        }

        public void sendFeedback(PresentationAction.MistakeType mistake)
        {
            

            string feedback = "good";
            switch (mistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEGSCROSSED:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.HUNCHBACK:
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    feedback = "Reset Posture";
                    break;
                case PresentationAction.MistakeType.DANCING:
                    feedback = "Stand Still";
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    feedback = "Move Hands";
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    feedback = "Move Hands";
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    feedback = "Speak Softer";
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    feedback = "Speak Louder";
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    feedback = "Module Voice";
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    feedback = "Start Speaking";
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    feedback = "Stop Speaking";
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    feedback = "Stop Hmmmm";
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    feedback = "Smile";
                    break;
                case PresentationAction.MistakeType.NOMISTAKE:
                    feedback = "Good!!!";
                    break;
            }

            FeedbackObject f = new FeedbackObject(startPresentation, feedback, "PT20");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(f, Formatting.Indented);

            try
            {
               // Socket udpSendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
              //  IPAddress serverAddr = IPAddress.Parse("192.168.178.23");
              //  IPEndPoint UDPendPoint = new IPEndPoint(serverAddr, 16002);
                byte[] send_buffer = Encoding.ASCII.GetBytes(json);
                udpSendingSocket.SendTo(send_buffer, UDPendPoint);
            }
            catch
            {

            }

            //byte[] send_buffer = Encoding.ASCII.GetBytes(json);
            //udpSendingSocket.SendTo(send_buffer, UDPendPoint);
        }

        #endregion

        #region loadingAndClosing

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadMode();
            if (this.frameReader != null)
            {
                this.frameReader.FrameArrived += frameReader_FrameArrived;
            }
        }


    
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
            //  audioHandlerControl.close();
            // bodyFrameHandlerControl.close();
            videoHandler.close();
        }

        public void startAgain()
        {
            
            
            //TODO
           // string pathString = executingDirectory + "\\restart.bat";
            string pathString = System.IO.Path.Combine(executingDirectory, "restart.bat");
          //  string pathApp = executingDirectory + "\\PT20.exe";
            string pathApp = System.IO.Path.Combine(executingDirectory, "PT20.exe");



            if (!System.IO.File.Exists(pathString))
            {
                StreamWriter w = new StreamWriter(pathString);
                w.WriteLine("timeout /t  2");
                w.WriteLine("Start " + "\"\" \"" + pathApp + "\"");
                w.Close();
            }
            else
            {
                StreamWriter w = new StreamWriter(pathString);
                w.WriteLine("timeout /t  2");
                w.WriteLine("Start " + "\"\" \"" + pathApp + "\"");
                w.Close();
            }

            //restart = true;
            Directory.SetCurrentDirectory(executingDirectory);
            System.Diagnostics.Process.Start(pathString);
            Application.Current.MainWindow.Close();

            //Application.Current.Shutdown();


        }

        #endregion

        #region KinectActions

        void frameReader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            //  rulesAnalyzer.AnalyseRules();

            switch (myState)
            {
                case States.freestyle:
                    rulesAnalyzerFIFO.AnalyseRules();
                    freestyleMode.checkPause();
                    break;
                case States.individual:
                    //if (individualSkills.ready)
                    //{
                    //    individualSkills.analyze();
                    //}
                    break;
       



                    break;
            }

        }

        #endregion

        #region doLogs

        public void setInitialStrings()
        {
            stringStartFinish = "<Start Time>" + DateTime.Now.TimeOfDay.TotalMilliseconds + "</Start Time>" + System.Environment.NewLine;
            presentationStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
            stringGestures = "<Gestures>" + System.Environment.NewLine;
            stringInterruptions = "<Interruptions>" + System.Environment.NewLine;
            stringFreestyleFeedbacks = "<Freestyle Feedbacks>" + System.Environment.NewLine;
            stringSpeakingTime = "<Speaking time> " + System.Environment.NewLine;
            stringPausingTime = "<Pausing time>" + System.Environment.NewLine;
            stringIndividualSkills = "<Individual Skills>" + System.Environment.NewLine;
            stringIndividualFeedbacks = "<Individual Skills Feedback>" + System.Environment.NewLine;
            stringMistakes = "<Mistakes>" + System.Environment.NewLine;

            startPresentation= DateTime.Now;
            recordingID = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day + "-";
            recordingID = recordingID + DateTime.Now.Hour.ToString();
            recordingID = recordingID + "H" + DateTime.Now.Minute.ToString() + "M" + DateTime.Now.Second.ToString() + "S" + DateTime.Now.Millisecond.ToString();

           // recordingPath = userDirectory + "\\"+recordingID;
            recordingPath = System.IO.Path.Combine(userDirectory, recordingID);
            bool exists = System.IO.Directory.Exists(recordingPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(recordingPath);
        }
        public void setFinalStrings()
        {
            presentationFinished = DateTime.Now.TimeOfDay.TotalMilliseconds;
            presentationDuration = DateTime.Now.TimeOfDay.TotalMilliseconds - presentationStarted;
            stringStartFinish = stringStartFinish + System.Environment.NewLine + "<Finish Time>" + DateTime.Now.TimeOfDay.TotalMilliseconds + "</FinishTime>" + System.Environment.NewLine;
            stringGestures = stringGestures + System.Environment.NewLine + "</Gestures>" + System.Environment.NewLine;
            stringInterruptions = stringInterruptions + System.Environment.NewLine + "</Interruptions>" + System.Environment.NewLine;
            stringFreestyleFeedbacks = stringFreestyleFeedbacks + System.Environment.NewLine + "</Freestyle Feedbacks>" + System.Environment.NewLine;
            stringSpeakingTime = stringSpeakingTime + System.Environment.NewLine + "</Speaking time>" + System.Environment.NewLine;
            stringPausingTime = stringPausingTime + System.Environment.NewLine + "</Pausing time>" + System.Environment.NewLine;
            stringIndividualSkills = stringIndividualSkills + System.Environment.NewLine + "</Individual Skills>" + System.Environment.NewLine;
            stringIndividualFeedbacks = stringIndividualFeedbacks + System.Environment.NewLine + "</Individual Skills Feedback>" + System.Environment.NewLine;
            stringMistakes = stringMistakes + System.Environment.NewLine + "</Mistakes>" + System.Environment.NewLine;
            stringGoodies = "<Goodies>" + stringGoodies + System.Environment.NewLine + "</Goodies>" + System.Environment.NewLine;
            stopPresentation = DateTime.Now;
        }
        #endregion
    }
}
