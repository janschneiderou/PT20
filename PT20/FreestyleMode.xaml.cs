using Microsoft.Kinect;
using System;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Imaging;


namespace PT20
{
    /// <summary>
    /// Interaction logic for FreestyleMode.xaml
    /// </summary>
    public partial class FreestyleMode : UserControl
    {
        public bool allowsFeedback = true;
        public PresentationEvent previousFeedback;
        public MainWindow parent;
        public double animationWidth;
        public double animationTop;
        public double animationLeft;
        public enum currentState { stop, play }
        public currentState myState;
        InterruptFeedback interruptFeedback;
        PauseControl pauseControl;


       

        Ghost ghostPosture;
        Ghost ghostHandMovement;
        Ghost ghostHmmm;
        Ghost ghostHighVolume;
        Ghost ghostDancing;
        Ghost ghostLowVolume;
        Ghost ghostModuleVolume;
        Ghost ghostStopSpeaking;
        Ghost ghostStartSpeaking;
        Ghost ghostSmile;

        GhostMoving ghostPostureM;
        GhostMoving ghostHandMovementM;
        GhostMoving ghostHmmmM;
        GhostMoving ghostHighVolumeM;
        GhostMoving ghostDancingM;
        GhostMoving ghostLowVolumeM;
        GhostMoving ghostModuleVolumeM;
        GhostMoving ghostStopSpeakingM;
        GhostMoving ghostStartSpeakingM;
        GhostMoving ghostSmileM;

        FreestyleTextFeedback ghostPostureTF;
        FreestyleTextFeedback ghostHandMovementTF;
        FreestyleTextFeedback ghostHmmmTF;
        FreestyleTextFeedback ghostHighVolumeTF;
        FreestyleTextFeedback ghostDancingTF;
        FreestyleTextFeedback ghostLowVolumeTF;
        FreestyleTextFeedback ghostModuleVolumeTF;
        FreestyleTextFeedback ghostStopSpeakingTF;
        FreestyleTextFeedback ghostStartSpeakingTF;
        FreestyleTextFeedback ghostSmileTF;

        FreestyleOldText ghostPostureOT;
        FreestyleOldText ghostHandMovementOT;
        FreestyleOldText ghostHmmmOT;
        FreestyleOldText ghostHighVolumeOT;
        FreestyleOldText ghostDancingOT;
        FreestyleOldText ghostLowVolumeOT;
        FreestyleOldText ghostModuleVolumeOT;
        FreestyleOldText ghostStopSpeakingOT;
        FreestyleOldText ghostStartSpeakingOT;
        FreestyleOldText ghostSmileOT;

        //  SoundPlayer coinSound;

       


        public FreestyleMode()
        {
            InitializeComponent();
        }

        #region LoadingAndUnloading

        public void loaded()
        {

            backgroundImg.Width = parent.ActualWidth;
            backgroundImg.Height = parent.ActualHeight;
            Canvas.SetLeft(backgroundImg, 0);
            Canvas.SetTop(backgroundImg, 0);
            myBody.initialize(parent);
             myAudio.initialize(parent);
            mySkeleton.initialize(parent);
            focusLabel.Content = MainWindow.focusedString;

            loadGhosts();

            parent.rulesAnalyzerFIFO.feedBackEvent += rulesAnalyzerFIFO_feedBackEvent;
            parent.rulesAnalyzerFIFO.correctionEvent += rulesAnalyzerFIFO_correctionEvent;
            parent.rulesAnalyzerFIFO.myInterruptionEvent += rulesAnalyzerFIFO_myInterruptionEvent;

            myState = currentState.stop;

            coinSound.MediaEnded += coinSound_MediaEnded;
            countdown.startAnimation();
            countdown.countdownFinished += countdown_countdownFinished;

            countdownPause.countdownFinished += countdownPause_countdownFinished;

        }

        public void unload()
        {
            parent.rulesAnalyzerFIFO.feedBackEvent -= rulesAnalyzerFIFO_feedBackEvent;
            parent.rulesAnalyzerFIFO.correctionEvent -= rulesAnalyzerFIFO_correctionEvent;
            parent.rulesAnalyzerFIFO.myInterruptionEvent -= rulesAnalyzerFIFO_myInterruptionEvent;
            myBody.unload();
            mySkeleton.unload();
            myAudio.unload();
            
            parent.recordingClass = null;
            coinSound.MediaEnded -= coinSound_MediaEnded;

            countdown.countdownFinished -= countdown_countdownFinished;

            countdownPause.countdownFinished -= countdownPause_countdownFinished;

          //  pauseControl.GoBackButton.Click -= GoBackButtonPause_Click;
           // pauseControl.GoMainMenu.Click -= GoMainMenu1_Click;
           // interruptFeedback.GoBackButton.Click -= OrdinaryReturn_Click;
            //interruptFeedback.GoToExercises.Click -= GoToExercises_Click;
        }

        #endregion

        #region loadGhosts

        void loadGhosts()
        {
            ghostDancing = new Ghost();
            ghostHandMovement = new Ghost();
            ghostHighVolume = new Ghost();
            ghostHmmm = new Ghost();
            ghostLowVolume = new Ghost();
            ghostModuleVolume = new Ghost();
            ghostPosture = new Ghost();
            ghostStartSpeaking = new Ghost();
            ghostStopSpeaking = new Ghost();
            ghostSmile = new Ghost();

            ghostPostureM = new GhostMoving();
            ghostHandMovementM = new GhostMoving();
            ghostHmmmM = new GhostMoving();
            ghostHighVolumeM = new GhostMoving();
            ghostDancingM = new GhostMoving();
            ghostLowVolumeM = new GhostMoving();
            ghostModuleVolumeM = new GhostMoving();
            ghostStopSpeakingM = new GhostMoving();
            ghostStartSpeakingM = new GhostMoving();
            ghostSmileM = new GhostMoving();



            ghostPostureTF = new FreestyleTextFeedback();
            ghostHandMovementTF = new FreestyleTextFeedback();
            ghostHmmmTF = new FreestyleTextFeedback();
            ghostHighVolumeTF = new FreestyleTextFeedback();
            ghostDancingTF = new FreestyleTextFeedback();
            ghostLowVolumeTF = new FreestyleTextFeedback();
            ghostModuleVolumeTF = new FreestyleTextFeedback();
            ghostStopSpeakingTF = new FreestyleTextFeedback();
            ghostStartSpeakingTF = new FreestyleTextFeedback();
            ghostSmileTF = new FreestyleTextFeedback();

            ghostPostureOT = new FreestyleOldText();
            ghostHandMovementOT = new FreestyleOldText();
            ghostHmmmOT = new FreestyleOldText();
            ghostHighVolumeOT = new FreestyleOldText();
            ghostDancingOT = new FreestyleOldText();
            ghostLowVolumeOT = new FreestyleOldText();
            ghostModuleVolumeOT = new FreestyleOldText();
            ghostStopSpeakingOT = new FreestyleOldText();
            ghostStartSpeakingOT = new FreestyleOldText();
            ghostSmileOT = new FreestyleOldText();

            myCanvas.Children.Add(ghostDancing);
            myCanvas.Children.Add(ghostStopSpeaking);
            myCanvas.Children.Add(ghostHandMovement);
            myCanvas.Children.Add(ghostHighVolume);
            myCanvas.Children.Add(ghostHmmm);
            myCanvas.Children.Add(ghostLowVolume);
            myCanvas.Children.Add(ghostModuleVolume);
            myCanvas.Children.Add(ghostPosture);
            myCanvas.Children.Add(ghostStartSpeaking);
            myCanvas.Children.Add(ghostSmile);

            myCanvas.Children.Add(ghostDancingM);
            myCanvas.Children.Add(ghostStopSpeakingM);
            myCanvas.Children.Add(ghostHandMovementM);
            myCanvas.Children.Add(ghostHighVolumeM);
            myCanvas.Children.Add(ghostHmmmM);
            myCanvas.Children.Add(ghostLowVolumeM);
            myCanvas.Children.Add(ghostModuleVolumeM);
            myCanvas.Children.Add(ghostPostureM);
            myCanvas.Children.Add(ghostStartSpeakingM);
            myCanvas.Children.Add(ghostSmileM);

            myCanvas.Children.Add(ghostDancingTF);
            myCanvas.Children.Add(ghostStopSpeakingTF);
            myCanvas.Children.Add(ghostHandMovementTF);
            myCanvas.Children.Add(ghostHighVolumeTF);
            myCanvas.Children.Add(ghostHmmmTF);
            myCanvas.Children.Add(ghostLowVolumeTF);
            myCanvas.Children.Add(ghostModuleVolumeTF);
            myCanvas.Children.Add(ghostPostureTF);
            myCanvas.Children.Add(ghostStartSpeakingTF);
            myCanvas.Children.Add(ghostSmileTF);

            myCanvas.Children.Add(ghostDancingOT);
            myCanvas.Children.Add(ghostStopSpeakingOT);
            myCanvas.Children.Add(ghostHandMovementOT);
            myCanvas.Children.Add(ghostHighVolumeOT);
            myCanvas.Children.Add(ghostHmmmOT);
            myCanvas.Children.Add(ghostLowVolumeOT);
            myCanvas.Children.Add(ghostModuleVolumeOT);
            myCanvas.Children.Add(ghostPostureOT);
            myCanvas.Children.Add(ghostStartSpeakingOT);
            myCanvas.Children.Add(ghostSmileOT);

            Canvas.SetLeft(ghostDancingTF, 30);
            Canvas.SetLeft(ghostStopSpeakingTF, 30);
            Canvas.SetLeft(ghostHandMovementTF, 30);
            Canvas.SetLeft(ghostHighVolumeTF, 30);
            Canvas.SetLeft(ghostHmmmTF, 30);
            Canvas.SetLeft(ghostLowVolumeTF, 30);
            Canvas.SetLeft(ghostModuleVolumeTF, 30);
            Canvas.SetLeft(ghostPostureTF, 30);
            Canvas.SetLeft(ghostStartSpeakingTF, 30);
            Canvas.SetLeft(ghostSmileTF, 30);

            Canvas.SetLeft(ghostDancingOT, 30);
            Canvas.SetLeft(ghostStopSpeakingOT, 30);
            Canvas.SetLeft(ghostHandMovementOT, 30);
            Canvas.SetLeft(ghostHighVolumeOT, 30);
            Canvas.SetLeft(ghostHmmmOT, 30);
            Canvas.SetLeft(ghostLowVolumeOT, 30);
            Canvas.SetLeft(ghostModuleVolumeOT, 30);
            Canvas.SetLeft(ghostPostureOT, 30);
            Canvas.SetLeft(ghostStartSpeakingOT, 30);
            Canvas.SetLeft(ghostSmileOT, 30);

            Canvas.SetTop(ghostDancingTF, 354);
            Canvas.SetTop(ghostStopSpeakingTF, 354);
            Canvas.SetTop(ghostHandMovementTF, 354);
            Canvas.SetTop(ghostHighVolumeTF, 354);
            Canvas.SetTop(ghostHmmmTF, 354);
            Canvas.SetTop(ghostLowVolumeTF, 354);
            Canvas.SetTop(ghostModuleVolumeTF, 354);
            Canvas.SetTop(ghostPostureTF, 354);
            Canvas.SetTop(ghostStartSpeakingTF, 354);
            Canvas.SetTop(ghostSmileTF, 354);

            Canvas.SetTop(ghostDancingOT, 284);
            Canvas.SetTop(ghostStopSpeakingOT, 284);
            Canvas.SetTop(ghostHandMovementOT, 284);
            Canvas.SetTop(ghostHighVolumeOT, 284);
            Canvas.SetTop(ghostHmmmOT, 284);
            Canvas.SetTop(ghostLowVolumeOT, 284);
            Canvas.SetTop(ghostModuleVolumeOT, 284);
            Canvas.SetTop(ghostPostureOT, 284);
            Canvas.SetTop(ghostStartSpeakingOT, 284);
            Canvas.SetTop(ghostSmileOT, 284);

            Canvas.SetTop(ghostDancing, 80);
            Canvas.SetTop(ghostStopSpeaking, 80);
            Canvas.SetTop(ghostHandMovement, 80);
            Canvas.SetTop(ghostHighVolume, 80);
            Canvas.SetTop(ghostHmmm, 80);
            Canvas.SetTop(ghostLowVolume, 80);
            Canvas.SetTop(ghostModuleVolume, 80);
            Canvas.SetTop(ghostPosture, 80);
            Canvas.SetTop(ghostStartSpeaking, 80);
            Canvas.SetTop(ghostSmile, 80);

            ghostPostureM.Height = 650;
            ghostHandMovementM.Height = 650;
            ghostHmmmM.Height = 650;
            ghostHighVolumeM.Height = 650;
            ghostDancingM.Height = 650;
            ghostLowVolumeM.Height = 650;
            ghostModuleVolumeM.Height = 650;
            ghostStopSpeakingM.Height = 650;
            ghostStartSpeakingM.Height = 650;
            ghostSmile.Height = 650;


            setGhostInvisible();
            setGhostMovingInvisible();
            setFeedbackTextInvisible();
            setOldTextInvisible();

            var uriSource = new Uri(@"/PT20;component/Images/ghost_outline.png", UriKind.Relative);
            ghostPosture.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_hands.png", UriKind.Relative);
            ghostDancing.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_pause.png", UriKind.Relative);
            ghostStopSpeaking.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_gestures.png", UriKind.Relative);
            ghostHandMovement.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_lowerVolume.png", UriKind.Relative);
            ghostHighVolume.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_outline.png", UriKind.Relative);
            ghostHmmm.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_raiseVolume.png", UriKind.Relative);
            ghostLowVolume.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_moduleVolume.png", UriKind.Relative);
            ghostModuleVolume.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_startSpeaking.png", UriKind.Relative);
            ghostStartSpeaking.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_smile.png", UriKind.Relative);
            ghostSmile.ghostImg.Source = new BitmapImage(uriSource);
            ghostSmile.ghostImg.Height = 350;

            uriSource = new Uri(@"/PT20;component/Images/ghost_outline.png", UriKind.Relative);
            ghostPostureM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_hands.png", UriKind.Relative);
            ghostDancingM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_pause.png", UriKind.Relative);
            ghostStopSpeakingM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_gestures.png", UriKind.Relative);
            ghostHandMovementM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_lowerVolume.png", UriKind.Relative);
            ghostHighVolumeM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_outline.png", UriKind.Relative);
            ghostHmmmM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_raiseVolume.png", UriKind.Relative);
            ghostLowVolumeM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_moduleVolume.png", UriKind.Relative);
            ghostModuleVolumeM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_startSpeaking.png", UriKind.Relative);
            ghostStartSpeakingM.ghostImg.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_smile.png", UriKind.Relative);
            ghostSmileM.ghostImg.Source = new BitmapImage(uriSource);
            ghostSmileM.ghostImg.Height = 350;

            uriSource = new Uri(@"/PT20;component/Images/ghost_outline.png", UriKind.Relative);
            ghostPostureOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_hands.png", UriKind.Relative);
            ghostDancingOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_pause.png", UriKind.Relative);
            ghostStopSpeakingOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_gestures.png", UriKind.Relative);
            ghostHandMovementOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_lowerVolume.png", UriKind.Relative);
            ghostHighVolumeOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_outline.png", UriKind.Relative);
            ghostHmmmOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_raiseVolume.png", UriKind.Relative);
            ghostLowVolumeOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_moduleVolume.png", UriKind.Relative);
            ghostModuleVolumeOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_startSpeaking.png", UriKind.Relative);
            ghostStartSpeakingOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            uriSource = new Uri(@"/PT20;component/Images/ghost_smile.png", UriKind.Relative);
            ghostSmileOT.FeedbackIMG.Source = new BitmapImage(uriSource);
        }

        public void setGhostMovingInvisible()
        {
            ghostDancingM.Visibility = System.Windows.Visibility.Collapsed;
            ghostStopSpeakingM.Visibility = System.Windows.Visibility.Collapsed;
            ghostHandMovementM.Visibility = System.Windows.Visibility.Collapsed;
            ghostHighVolumeM.Visibility = System.Windows.Visibility.Collapsed;
            ghostHmmmM.Visibility = System.Windows.Visibility.Collapsed;
            ghostLowVolumeM.Visibility = System.Windows.Visibility.Collapsed;
            ghostModuleVolumeM.Visibility = System.Windows.Visibility.Collapsed;
            ghostPostureM.Visibility = System.Windows.Visibility.Collapsed;
            ghostStartSpeakingM.Visibility = System.Windows.Visibility.Collapsed;
            ghostSmileM.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void setFeedbackTextInvisible()
        {
            ghostDancingTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostStopSpeakingTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostHandMovementTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostHighVolumeTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostHmmmTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostLowVolumeTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostModuleVolumeTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostPostureTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostStartSpeakingTF.Visibility = System.Windows.Visibility.Collapsed;
            ghostSmileTF.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void setOldTextInvisible()
        {
            ghostDancingOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostStopSpeakingOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostHandMovementOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostHighVolumeOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostHmmmOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostLowVolumeOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostModuleVolumeOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostPostureOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostStartSpeakingOT.Visibility = System.Windows.Visibility.Collapsed;
            ghostSmileOT.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void setGhostInvisible()
        {
            ghostDancing.Visibility = System.Windows.Visibility.Collapsed;
            ghostStopSpeaking.Visibility = System.Windows.Visibility.Collapsed;
            ghostHandMovement.Visibility = System.Windows.Visibility.Collapsed;
            ghostHighVolume.Visibility = System.Windows.Visibility.Collapsed;
            ghostHmmm.Visibility = System.Windows.Visibility.Collapsed;
            ghostLowVolume.Visibility = System.Windows.Visibility.Collapsed;
            ghostModuleVolume.Visibility = System.Windows.Visibility.Collapsed;
            ghostPosture.Visibility = System.Windows.Visibility.Collapsed;
            ghostStartSpeaking.Visibility = System.Windows.Visibility.Collapsed;
            ghostSmile.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #region control

        void countdown_countdownFinished(object sender)
        {
            myState = currentState.play;
            if (MainWindow.presentationStarted == 0)
            {
                parent.setInitialStrings();
            }

            //start recordings
            parent.recordingClass = new RecordingClass();
            parent.recordingClass.startRecording();
            parent.storeClass.doStartStuff();

            parent.rulesAnalyzerFIFO.myJudgementMaker.myVoiceAndMovementObject.isSpeaking = false;
            parent.rulesAnalyzerFIFO.myJudgementMaker.lastSmile = DateTime.Now.TimeOfDay.TotalMilliseconds;
        }



        #endregion

        #region Pauses&Stop


        public void checkPause()
        {
            if (MainWindow.stopGesture == true && countdownPause.animationStarted == false)
            {
                countdownPause.startAnimation();
            }
        }

        void countdownPause_countdownFinished(object sender)
        {
            myState = currentState.stop;
            pauseControl = new PauseControl();
            myCanvas.Children.Add(pauseControl);
            Canvas.SetLeft(pauseControl, 20);
            Canvas.SetTop(pauseControl, 20);
            
            pauseControl.GoBackButton.Click += GoBackButtonPause_Click;
            pauseControl.GoMainMenu.Click += GoMainMenu1_Click;
           if(parent.recordingClass.isRecording==true)
            {
                parent.recordingClass.stopRecording();
                parent.storeClass.doStopStuff();
            }
            

        }
        public void pitchEnd()
        {
            myState = currentState.stop;
            pauseControl = new PauseControl();
            myCanvas.Children.Add(pauseControl);
            Canvas.SetLeft(pauseControl, 20);
            Canvas.SetTop(pauseControl, 20);
            pauseControl.GoBackButton.Click += GoBackButtonPause_Click;
            pauseControl.GoMainMenu.Click += GoMainMenu1_Click;


            if (parent.recordingClass.isRecording == true)
            {
                parent.recordingClass.stopRecording();
                parent.storeClass.doStopStuff();
            }
        }

        public void GoMainMenu1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.myState = MainWindow.States.menu;

            parent.recordingClass.combineFiles();
           
             doLoggingStuff(); //TODO 

            if (pauseControl != null)
            {
                pauseControl.GoBackButton.Click -= GoBackButtonPause_Click;
                pauseControl.GoMainMenu.Click -= GoMainMenu1_Click;
                myCanvas.Children.Remove(pauseControl);
                pauseControl = null;
            }
            try
            {


            }
            catch
            {

            }

           // myBody = null;
           // myAudio = null;
           // mySkeleton = null;
            parent.closeFreeStyleMode();
        }


        private void GoBackButtonPause_Click(object sender, RoutedEventArgs e)
        {


            if (pauseControl != null)
            {
                pauseControl.GoBackButton.Click -= GoBackButtonPause_Click;
                myCanvas.Children.Remove(pauseControl);
                pauseControl = null;
            }



            myState = currentState.play;
            setGhostMovingInvisible();
            setOldTextInvisible();
            setGhostInvisible();
            setFeedbackTextInvisible();
            //    textFeedback.FeedbackIMG.Visibility = Visibility.Visible;

            countdown.startAnimation();

            //parent.rulesAnalyzerFIFO.lastFeedbackTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
           // parent.rulesAnalyzerFIFO.resetAfterPause();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            myState = currentState.stop;

            if(parent.recordingClass.isRecording==true)
            {
                parent.recordingClass.stopRecording();
                parent.storeClass.doStopStuff();
            }

            
            pauseControl = new PauseControl();
            myCanvas.Children.Add(pauseControl);
            Canvas.SetLeft(pauseControl, 20);
            Canvas.SetTop(pauseControl, 20);
            pauseControl.GoBackButton.Click += GoBackButtonPause_Click;
            pauseControl.GoMainMenu.Click += GoMainMenu1_Click;

           
        }

        #endregion

        #region visualAndAudioEffects

   

        void coinSound_MediaEnded(object sender, RoutedEventArgs e)
        {
            coinSound.Stop();
        }



        #endregion

        #region feedbacks

        void rulesAnalyzerFIFO_feedBackEvent(object sender, PresentationAction x)
        {

           // ghost.hideFeedback();
            switch (x.myMistake)
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
                    badPostureFeedback();
                    break;
                case PresentationAction.MistakeType.DANCING:
                    dancingFeedback();
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handMovementFeedback();
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handMovementFeedback();
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumeFeedback();
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumeFeedback();
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    lowModulationFeedback();
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPauseFeedback();
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalkFeedback();
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmFeedback();
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    smileFeedback();
                    break;
            }
          

        }



        void badPostureFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostPosture, factor * xHead + displacement);
            ghostPosture.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_reset_posture.png", UriKind.Relative);
            ghostPostureTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostPostureTF.FeedbackIMG.Visibility = Visibility.Visible;
            ghostPostureTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;
            //textFeedback.CurrentFeedback.Content = leftPositionGhost + " Reset Posture " + xHead;
            ghostPostureTF.CurrentFeedback.Content = " Reset Posture ";
            ghostPosture.CurrentFeedback.Content = " Reset Posture ";
            //  parent.sendFeedback("Reset Posture");

            if (parent.bodyFrameHandler.bodyFramePreAnalysis.armsCrossed)
            {
                ghostPosture.leftHand.Visibility = Visibility.Visible;
                ghostPosture.rightHand.Visibility = Visibility.Visible;
                ghostPosture.leftArm.Visibility = Visibility.Visible;
                ghostPosture.rightArm.Visibility = Visibility.Visible;
            }
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.leftHandBehindBack
                || parent.bodyFrameHandler.bodyFramePreAnalysis.leftHandUnderHips)
            {
                ghostPosture.leftHand.Visibility = Visibility.Visible;
            }
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.rightHandBehindBack
                || parent.bodyFrameHandler.bodyFramePreAnalysis.rightHandUnderHips)
            {
                ghostPosture.rightHand.Visibility = Visibility.Visible;
            }
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.hunch)
            {
                ghostPosture.hunch.Visibility = Visibility.Visible;
            }
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.leftLean ||
                parent.bodyFrameHandler.bodyFramePreAnalysis.rightLean)
            {
                ghostPosture.back.Visibility = Visibility.Visible;
            }


        }

        private void handMovementFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostHandMovement, factor * xHead + displacement);
            ghostHandMovement.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_gesture.png", UriKind.Relative);
            ghostHandMovementTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostHandMovementTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostHandMovementTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;


            ghostHandMovementM.Visibility = Visibility.Collapsed;
            //textFeedback.CurrentFeedback.Content = leftPositionGhost + " Reset Posture " + xHead;
            ghostHandMovementTF.CurrentFeedback.Content = " Use Gestures ";
            ghostHandMovement.CurrentFeedback.Content = " Use Gestures ";
            //  parent.sendFeedback("Move your Hands");
        }

        private void highVolumeFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostHighVolume, factor * xHead + displacement);
            ghostHighVolume.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_lower_volume.png", UriKind.Relative);
            ghostHighVolumeTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostHighVolumeTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostHighVolumeTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            ghostHighVolumeTF.CurrentFeedback.Content = " Speak softer ";
            ghostHighVolume.CurrentFeedback.Content = " Speak softer ";
            //  parent.sendFeedback("Speak softer");
        }

        private void lowVolumeFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostLowVolume, factor * xHead + displacement);
            ghostLowVolume.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_raise_volume.png", UriKind.Relative);
            ghostLowVolumeTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostLowVolumeTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostLowVolumeTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            ghostLowVolumeTF.CurrentFeedback.Content = " Speak Louder ";
            ghostLowVolume.CurrentFeedback.Content = " Speak Louder ";
            //   parent.sendFeedback("Speak Louder");
        }

        private void lowModulationFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }


            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostModuleVolume, factor * xHead + displacement);
            ghostModuleVolume.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_raise_volume.png", UriKind.Relative);
            ghostModuleVolumeTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostModuleVolumeTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostModuleVolumeTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            ghostModuleVolumeTF.CurrentFeedback.Content = " Module Voice ";
            ghostModuleVolume.CurrentFeedback.Content = " Module Voice ";
        }

        private void longPauseFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostStartSpeaking, factor * xHead + displacement);
            ghostStartSpeaking.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_start_speaking.png", UriKind.Relative);
            ghostStartSpeakingTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostStartSpeakingTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostStartSpeakingTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            ghostStartSpeaking.Visibility = Visibility.Visible;
            ghostStartSpeakingTF.CurrentFeedback.Content = " Start Speaking ";
            ghostStartSpeaking.CurrentFeedback.Content = " Start Speaking ";
            // parent.sendFeedback("Start Speaking");
        }

        private void longTalkFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostStopSpeaking, factor * xHead + displacement);
            ghostStopSpeaking.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_pause_speaking.png", UriKind.Relative);
            ghostStopSpeakingTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostStopSpeakingTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostStopSpeaking.Visibility = Visibility.Visible;

            ghostStopSpeakingTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            ghostStopSpeakingTF.CurrentFeedback.Content = " Stop Speaking ";
            ghostStopSpeaking.CurrentFeedback.Content = " Stop Speaking ";
            //parent.sendFeedback("Stop Speaking");
        }

        private void hmmmFeedback()
        {

            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostHmmm, factor * xHead + displacement);
            ghostHmmm.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_start_speaking.png", UriKind.Relative);
            ghostHmmmTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostHmmmTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostHmmmTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            ghostHmmm.Visibility = Visibility.Visible;
            ghostHmmmTF.CurrentFeedback.Content = " Stop The hmmmms ";
            ghostHmmm.CurrentFeedback.Content = " Stop The hmmmms ";
            //  parent.sendFeedback("Stop The hmmmms");
            ghostHmmmTF.vanish();
            ghostHmmm.vanish();
        }

        private void smileFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostSmile, factor * xHead + displacement);
            ghostSmile.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_smile.png", UriKind.Relative);
            ghostSmileTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostSmileTF.FeedbackIMG.Visibility = Visibility.Visible;

            ghostSmileTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            ghostSmile.Visibility = Visibility.Visible;
            ghostSmileTF.CurrentFeedback.Content = " Smile ";
            ghostSmile.CurrentFeedback.Content = " Smile ";
            //  parent.sendFeedback("Smile");
            ghostSmileTF.vanish();
            ghostSmile.vanish();
        }


        private void dancingFeedback()
        {
            float factor = 345.45f;
            float displacement = 373;

            float xHead;
            if (parent.bodyFrameHandler.bodyFramePreAnalysis.body != null)
            {
                xHead = parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;
            }
            else
            {
                xHead = 0.1f;
            }

            float leftPositionGhost = factor * xHead + displacement;
            Canvas.SetLeft(ghostDancing, factor * xHead + displacement);
            ghostDancing.Visibility = Visibility.Visible;
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_move_more.png", UriKind.Relative);
            ghostDancingTF.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostDancingTF.FeedbackIMG.Visibility = Visibility.Visible;


            ghostDancingTF.Visibility = Visibility.Visible;
            textFeedback.Visibility = Visibility.Collapsed;

            //textFeedback.CurrentFeedback.Content = leftPositionGhost + " Reset Posture " + xHead;
            ghostDancingTF.CurrentFeedback.Content = " Stay Still ";
            ghostDancing.CurrentFeedback.Content = " Stay Still ";
            // parent.sendFeedback("Stay Still");
            ghostDancing.vanish();
            ghostDancingTF.vanish();

        }

        #endregion

        #region corrections

        void rulesAnalyzerFIFO_correctionEvent(object sender, PresentationAction x)
        {
           // animationWidth = ghost.ActualWidth;//TODO check
            // ghost.Visibility = Visibility.Collapsed;
            setGhostInvisible();
            handleCorrection(x);
            textFeedback.CurrentFeedback.Content = " :-D";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/thumbs_up.png", UriKind.Relative);
            textFeedback.FeedbackIMG.Source = new BitmapImage(uriSource);
            textFeedback.FeedbackIMG.Visibility = Visibility.Visible;

           


        }






        private void handleCorrection(PresentationAction x)
        {
            setFeedbackTextInvisible();
            textFeedback.Visibility = Visibility.Visible;

            switch (x.myMistake)
            {
                case PresentationAction.MistakeType.NOMISTAKE:
                   // ghost.Visibility = Visibility.Collapsed;
                    break;
                case PresentationAction.MistakeType.ARMSCROSSED:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEGSCROSSED:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.HUNCHBACK:
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    oldBadPostureFeedback();
                    break;
                case PresentationAction.MistakeType.DANCING:
                    oldDancingFeedback();
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    oldHandMovementFeedback();
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    oldHandMovementFeedback();
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    oldHighVolumeFeedback();
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    oldLowVolumeFeedback();
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    oldLowModulationFeedback();
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    oldLongPauseFeedback();
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    oldLongTalkFeedback();
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    oldHmmmFeedback();
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    oldSmileFeedback();
                    break;
            }

        }



        private void oldBadPostureFeedback()
        {
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_reset_posture.png", UriKind.Relative);

            // oldTextFeedback.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostPostureOT.Visibility = Visibility.Visible;

            //textFeedback.CurrentFeedback.Content = leftPositionGhost + " Reset Posture " + xHead;
            ghostPostureOT.CurrentFeedback.Content = " Reset Posture ";
            ghostPostureOT.startBanish();
            ghostPostureM.Visibility = Visibility.Visible;

            ghostPostureOT.vanish();

            ghostPostureM.CurrentFeedback.Content = " Reset Posture ";
            //  ghostAnimation(ghostPosture, ghostPostureM);

            ghostPostureM.ghostAnimation(ghostPosture);
        }





        private void oldHandMovementFeedback()
        {
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_gesture.png", UriKind.Relative);


            ghostHandMovementOT.Visibility = Visibility.Visible;

            //textFeedback.CurrentFeedback.Content = leftPositionGhost + " Reset Posture " + xHead;
            ghostHandMovementOT.CurrentFeedback.Content = " Use Gestures ";
            ghostHandMovementM.CurrentFeedback.Content = " Use Gestures ";
            ghostHandMovementOT.startBanish();
            ghostHandMovementM.Visibility = Visibility.Visible;
            ghostHandMovementOT.vanish();

            ghostHandMovementM.ghostAnimation(ghostHandMovement);
        }

        private void oldHighVolumeFeedback()
        {
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_lower_volume.png", UriKind.Relative);


            ghostHighVolumeOT.Visibility = Visibility.Visible;
            ghostHighVolumeOT.CurrentFeedback.Content = " Speak softer ";
            ghostHighVolumeM.CurrentFeedback.Content = " Speak softer ";
            ghostHighVolumeOT.vanish();
            ghostHighVolumeM.Visibility = Visibility.Visible;
            ghostHighVolumeM.ghostAnimation(ghostHighVolume);
        }

        private void oldLowVolumeFeedback()
        {

            ghostLowVolumeOT.Visibility = Visibility.Visible;
            ghostLowVolumeOT.CurrentFeedback.Content = " Speak Louder ";
            ghostLowVolumeM.CurrentFeedback.Content = " Speak Louder ";
            ghostLowVolumeM.Visibility = Visibility.Visible;
            ghostLowVolumeM.ghostAnimation(ghostLowVolume);
            ghostLowVolumeOT.vanish();
        }

        private void oldLowModulationFeedback()
        {

            ghostModuleVolumeOT.Visibility = Visibility.Visible;
            ghostModuleVolumeOT.CurrentFeedback.Content = " Module Volume ";
            ghostModuleVolumeM.CurrentFeedback.Content = " Module Volumer ";
            ghostModuleVolumeM.Visibility = Visibility.Visible;
            ghostModuleVolumeM.ghostAnimation(ghostModuleVolume);
            ghostModuleVolumeOT.vanish();
        }

        private void oldLongPauseFeedback()
        {


            ghostStartSpeakingOT.Visibility = Visibility.Visible;


            ghostStartSpeakingOT.CurrentFeedback.Content = " Start Speaking ";
            ghostStartSpeakingM.CurrentFeedback.Content = " Start Speaking ";
            ghostStartSpeakingM.Visibility = Visibility.Visible;
            ghostStartSpeakingM.ghostAnimation(ghostStartSpeaking);
            ghostStartSpeakingOT.vanish();
        }

        private void oldLongTalkFeedback()
        {

            ghostStopSpeakingOT.Visibility = Visibility.Visible;


            ghostStopSpeakingOT.CurrentFeedback.Content = " Stop Speaking ";
            ghostStopSpeakingM.CurrentFeedback.Content = " Stop Speaking ";
            ghostStopSpeakingM.Visibility = Visibility.Visible;
            ghostStopSpeakingM.ghostAnimation(ghostStopSpeaking);
            ghostStopSpeakingOT.vanish();
        }

        private void oldHmmmFeedback()
        {

            ghostHmmmOT.Visibility = Visibility.Visible;


            ghostHmmmOT.CurrentFeedback.Content = " Stop The hmmmms ";
            ghostHmmmM.CurrentFeedback.Content = " Stop The hmmmms ";
            //ghostHmmmM.Visibility = Visibility.Visible;
            //ghostHmmmM.ghostAnimation(ghostHmmm);
            ghostHmmmOT.vanish();


        }

        private void oldSmileFeedback()
        {

            ghostSmileOT.Visibility = Visibility.Visible;


            ghostSmileOT.CurrentFeedback.Content = " Smile ";
            ghostSmileOT.CurrentFeedback.Content = " Smile ";
            //ghostHmmmM.Visibility = Visibility.Visible;
            //ghostHmmmM.ghostAnimation(ghostHmmm);
            ghostSmileOT.vanish();


        }

        private void oldDancingFeedback()
        {

            ghostDancingOT.Visibility = Visibility.Visible;


            ghostDancingOT.CurrentFeedback.Content = " Stay Still ";
            ghostDancingM.CurrentFeedback.Content = " Stay Still ";
            //ghostDancingM.Visibility = Visibility.Visible;
            //ghostDancingM.ghostAnimation(ghostDancing);
            ghostDancingOT.vanish();
        }

        #endregion

        #region interruptions

        void rulesAnalyzerFIFO_myInterruptionEvent(object sender, PresentationAction[] x)
        {
            loadInterruption(x);
            //if (MainWindow.hapticPort != null)
            //{
            //    doHapticInterruption();
            //}

        }

        public void loadInterruption(PresentationAction[] x)
        {
            myState = currentState.stop;
            interruptFeedback = new InterruptFeedback();
            interruptFeedback.mistakes = x;
            myCanvas.Children.Add(interruptFeedback);
            Canvas.SetLeft(interruptFeedback, 20);
            Canvas.SetTop(interruptFeedback, 20);
            interruptFeedback.GoBackButton.Click += OrdinaryReturn_Click;
            interruptFeedback.GoToExercises.Click += GoToExercises_Click;
            

          //  stopButton.Click -= stopButton_Click;
         //   reportStopButton.Click -= reportStopButton_Click;
         //   logStopButton.Click -= logStopButton_Click;
        }

        void GoToExercises_Click(object sender, RoutedEventArgs e)
        {
            PresentationAction pa = interruptFeedback.mistakes[0].Clone();

         //   parent.loadIndividualSkills(pa);

            try
            {
                myCanvas.Children.Remove(interruptFeedback);
                interruptFeedback = null;
                // myState = currentState.play;
                setGhostMovingInvisible();
                setOldTextInvisible();
                setGhostInvisible();
                setFeedbackTextInvisible();
            }
            catch
            {

            }

        }

        public void OrdinaryReturn_Click(object sender, RoutedEventArgs e)
        {

          //  stopButton.Click += stopButton_Click;
          //  reportStopButton.Click += reportStopButton_Click;
         //   logStopButton.Click += logStopButton_Click;

            if (interruptFeedback != null)
            {
           //     interruptFeedback.GoBackButton.Click -= GoBackButton_Click;
                myCanvas.Children.Remove(interruptFeedback);
                interruptFeedback = null;
            }

           
            setGhostMovingInvisible();
            setOldTextInvisible();
            setGhostInvisible();
            setFeedbackTextInvisible();
            textFeedback.FeedbackIMG.Visibility = Visibility.Visible;

            countdownPause.animationStarted = false;
            countdown.startAnimation();

            try
            {
               // parent.rulesAnalyzerFIFO.lastFeedbackTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
             //   parent.rulesAnalyzerFIFO.resetAfterInterruption();
            }
            catch
            {

            }

            //if (parent.rulesAnalyzer.interruption)
            //{
            //    parent.rulesAnalyzer.interrupted = false;
            //    //parent.rulesAnalyzer.reset();
            //    parent.rulesAnalyzer.resetForMistake((Mistake)parent.rulesAnalyzer.feedBackList[parent.rulesAnalyzer.feedBackList.Count - 1]);
            //}
        }

        #endregion

        #region loggingStuff

        public void doLoggingStuff()
        {
            parent.setFinalStrings();

            parent.rulesAnalyzerFIFO.logRemainingMistakes();

            string logString = MainWindow.stringStartFinish;

            double ptm = (MainWindow.postureMistakeTime + MainWindow.volumeMistakeTime
                + ((MainWindow.gestureMistakeTime - 5000) * MainWindow.totalGesturesMistakes) +
                MainWindow.totalHmmmMistakes * 200 + MainWindow.totalDancingMistakes*4000+ ((MainWindow.seriousMistakeTime - 25000) * MainWindow.totalSeriousMistakes)
                - MainWindow.totalgoodiesTime) / MainWindow.presentationDuration;

            double posturePTM = MainWindow.postureMistakeTime / MainWindow.presentationDuration;
            double volumePTM = MainWindow.volumeMistakeTime / MainWindow.presentationDuration;
            double gesturePTM = ((MainWindow.gestureMistakeTime - 1000) * MainWindow.totalGesturesMistakes) / MainWindow.presentationDuration;
            double hmmmPTM = MainWindow.totalHmmmMistakes * 200 / MainWindow.presentationDuration;
            double blankFacePTM = ((MainWindow.seriousMistakeTime - 22000) * MainWindow.totalSeriousMistakes) / MainWindow.presentationDuration;
            double dancingPTM= MainWindow.totalDancingMistakes * 4000 / MainWindow.presentationDuration;
            double pausesPTM = ((MainWindow.cadenceMistakeTime - 12000) * MainWindow.totalCadenceMistakes) / MainWindow.presentationDuration;
            double totalPTM = posturePTM + volumePTM + gesturePTM + hmmmPTM + blankFacePTM + dancingPTM + pausesPTM;

            logString = logString + System.Environment.NewLine + "posturePTM " + posturePTM + "";
            logString = logString + System.Environment.NewLine + "volumePTM " + volumePTM + "";
            logString = logString + System.Environment.NewLine + "gesturePTM " + gesturePTM + "";
            logString = logString + System.Environment.NewLine + "hmmmPTM " + hmmmPTM + "";
            logString = logString + System.Environment.NewLine + "blankFacePTM " + blankFacePTM + "";
            logString = logString + System.Environment.NewLine + "dancingPTM " + dancingPTM + "";
            logString = logString + System.Environment.NewLine + "pausesPTM " + pausesPTM + "";
            logString = logString + System.Environment.NewLine + "totalPTM " + totalPTM + "";

            logString = logString + System.Environment.NewLine;
            logString = logString + System.Environment.NewLine;

            double PTG = MainWindow.totalgoodiesTime/ MainWindow.presentationDuration;
            logString = logString + System.Environment.NewLine + "PTG " + PTG + "";

            logString = logString + System.Environment.NewLine + "Posture Mistakes " + MainWindow.totalPostureMistakes;
            logString = logString + System.Environment.NewLine + "Volume Mistakes " + MainWindow.totalVolumeMistakes;
            logString = logString + System.Environment.NewLine + "Gesture Mistakes " + MainWindow.totalGesturesMistakes;
            logString = logString + System.Environment.NewLine + "Cadence Mistakes " + MainWindow.totalCadenceMistakes;
            logString = logString + System.Environment.NewLine + "Phonetic pauses Mistakes " + MainWindow.totalHmmmMistakes;
            logString = logString + System.Environment.NewLine + "Dancing Mistakes " + MainWindow.totalDancingMistakes;
            logString = logString + System.Environment.NewLine + "total Mistakes " + MainWindow.totalMistakes;

            logString = logString + System.Environment.NewLine;
            logString = logString + System.Environment.NewLine;

            logString = logString + System.Environment.NewLine + "<Posture Mistakes>" + System.Environment.NewLine + MainWindow.timePostureMistakes + "</Posture Mistakes>";
            logString = logString + System.Environment.NewLine + "<Gesture Mistakes> " + System.Environment.NewLine + MainWindow.timeGestureMistakes + "</Gesture Mistakes> ";
            logString = logString + System.Environment.NewLine + "<Volume Mistakes> " + System.Environment.NewLine + MainWindow.timeVolumeMistakes + "</Volume Mistakes> ";
            logString = logString + System.Environment.NewLine + "<Cadence Mistakes> " + System.Environment.NewLine + MainWindow.timeCadenceMistakes + "</Cadence Mistakes>";
            logString = logString + System.Environment.NewLine + "<Phonetic pauses Mistakes> " + System.Environment.NewLine + MainWindow.timeHmmmMistakes + "</Phonetic pauses Mistakes> ";
            logString = logString + System.Environment.NewLine + "<Dancing Mistakes> " + System.Environment.NewLine + MainWindow.timeDancingMistakes + "</Dancing Mistakes> ";
            logString = logString + System.Environment.NewLine + "<Blank Face Mistakes> " + System.Environment.NewLine + MainWindow.timeSeriousMistakes + "</Blank Face Mistakes> ";

            logString = logString + System.Environment.NewLine + MainWindow.stringGoodies;

            logString = logString + System.Environment.NewLine + MainWindow.stringSpeakingTime;
            logString = logString + System.Environment.NewLine + MainWindow.stringPausingTime;
            logString = logString + System.Environment.NewLine + MainWindow.stringMistakes;

            MainWindow.logString = logString;

 
        }

        #endregion

    }
}
