﻿using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : UserControl
    {
        List<string> myDirectories;
        double[] totalPtm;
        double[] posturePtm;
        double[] gesturesPtm;
        double[] volumePtm;
        double[] dancingPtm;
        double[] hmmmPtm;
        double[] blankPtm;
        double[] pausesPtm;
        double maxValue;
        public MainWindow parent;
        string currentDirectoryString;
        int currentDirectoryInt;
        string currentVideoName;
        
       
        public History()
        {
            totalPtm = new double[5];
            posturePtm = new double[5];
            gesturesPtm = new double[5];
            volumePtm = new double[5];
            dancingPtm = new double[5];
            hmmmPtm = new double[5];
            blankPtm = new double[5];
            pausesPtm = new double[5];
            InitializeComponent();
            myDirectories = GetDirectories();
            getLastFiveDirectories();
            currentDirectoryInt = myDirectories.Count - 1;
            getValues();
            placeValues();
            getDirectoryNames();
            
        }

        public void loaded()
        {

        }

        

        private void getDirectoryNames()
        {
            string tempString = myDirectories[currentDirectoryInt];
            tempString = tempString.Substring(tempString.LastIndexOf("\\")+1);
            currentDirectoryString = tempString;

            currentTraining.Content = "Practice Session: "+ currentDirectoryString;

            if (File.Exists(MainWindow.userDirectory + "\\"+ currentDirectoryString +"\\"+ currentDirectoryString+"c.mp4"))
            {
               
                myVideo.Source = new System.Uri(MainWindow.userDirectory +"\\" + currentDirectoryString + "\\" + currentDirectoryString + "c.mp4");
              //  myVideo.Stop();
            }
            else if(File.Exists(MainWindow.userDirectory + "\\" + currentDirectoryString + "\\" + currentDirectoryString + ".mp4"))
            {
                myVideo.Source = new System.Uri(MainWindow.userDirectory + "\\" + currentDirectoryString + "\\" + currentDirectoryString + ".mp4");
               // myVideo.Stop();
            }


            if (currentDirectoryInt+1==myDirectories.Count)
            {
                buttonNext.IsEnabled = false;
            }
            else
            {
                buttonNext.IsEnabled = true;
            }
            if(currentDirectoryInt==0)
            {
                buttonPrevious.IsEnabled = false;
            }
            else
            {
                buttonPrevious.IsEnabled = true;
            }
        }

        private void placeValues()
        {
            try
            {
                first.Children.Clear();
                for (int i = 0; i < myDirectories.Count; i++)
                {
                    double ellipseSize ;
                    if(i==currentDirectoryInt)
                    {
                        ellipseSize = 20;
                    }
                    else
                    {
                        ellipseSize = 10;
                    }

                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.Black;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * totalPtm[i]);

                    ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.Red;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * posturePtm[i]);

                    ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.RoyalBlue;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * gesturesPtm[i]);

                    ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.LawnGreen;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * volumePtm[i]);

                    ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.Violet;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * blankPtm[i]);

                    ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.Chocolate;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * dancingPtm[i]);

                    ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.OliveDrab;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * hmmmPtm[i]);

                    ellipse = new Ellipse();
                    ellipse.Height = ellipseSize;
                    ellipse.Width = ellipseSize;
                    ellipse.Fill = Brushes.Orange;
                    first.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, 50 + i * 50);
                    Canvas.SetTop(ellipse, 240 - 240 / maxValue * pausesPtm[i]);


                   
                   
                    
                }
                showSelfEvaluation();
            }
            catch
            {

            }
            
            
        }

       private void showSelfEvaluation()
       {
            int i = 0;
            foreach(string s in myDirectories)
            {

                string path = s + "\\selfEvaluation.txt";
                if (File.Exists(path))
                {
                    string fileString = File.ReadAllText(path);
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(MainWindow.executingDirectory + "\\Images\\" + fileString));
                    img.Width = 30;
                    first.Children.Add(img);
                    Canvas.SetLeft(img, 50 + i * 50-5);
                    Canvas.SetTop(img, 260);
                }
                i++;
            }
            
            
        }
        private void getValues()
        {
            int i = 0;
            foreach(string s in myDirectories)
            {
                extractValuesFromFile(s,i);
                i++;
            }
            findBiggestValue();
        }

        private void findBiggestValue()
        {
            maxValue = 0;
            for(int i=0;i<5;i++)
            {
                if(maxValue<totalPtm[i])
                {
                    maxValue = totalPtm[i];
                }
            }
        }

        private void extractValuesFromFile(string filePath, int i)
        {
            try
            {
                string fileString = File.ReadAllText(filePath + "\\Log.txt");
                int start = fileString.IndexOf("totalPTM") + 9;
                int finish = fileString.IndexOf(Environment.NewLine, start);
                int length = finish - start;
                string tempString = fileString.Substring(start, length);
                totalPtm[i] = double.Parse(tempString);

                start = fileString.IndexOf("posturePTM") + 11;
                finish = fileString.IndexOf(Environment.NewLine, start);
                length = finish - start;
                tempString = fileString.Substring(start, length);
                posturePtm[i] = double.Parse(tempString);

                start = fileString.IndexOf("gesturePTM") + 11;
                finish = fileString.IndexOf(Environment.NewLine, start);
                length = finish - start;
                tempString = fileString.Substring(start, length);
                gesturesPtm[i] = double.Parse(tempString);

                start = fileString.IndexOf("volumePTM") + 10;
                finish = fileString.IndexOf(Environment.NewLine, start);
                length = finish - start;
                tempString = fileString.Substring(start, length);
                volumePtm[i] = double.Parse(tempString);

                start = fileString.IndexOf("dancingPTM") + 11;
                finish = fileString.IndexOf(Environment.NewLine, start);
                length = finish - start;
                tempString = fileString.Substring(start, length);
                dancingPtm[i] = double.Parse(tempString);

                start = fileString.IndexOf("hmmmPTM") + 8;
                finish = fileString.IndexOf(Environment.NewLine, start);
                length = finish - start;
                tempString = fileString.Substring(start, length);
                hmmmPtm[i] = double.Parse(tempString);

                start = fileString.IndexOf("blankFacePTM") + 13;
                finish = fileString.IndexOf(Environment.NewLine, start);
                length = finish - start;
                tempString = fileString.Substring(start, length);
                blankPtm[i] = double.Parse(tempString);

                start = fileString.IndexOf("pausesPTM") + 10;
                finish = fileString.IndexOf(Environment.NewLine, start);
                length = finish - start;
                tempString = fileString.Substring(start, length);
                pausesPtm[i] = double.Parse(tempString);
            }
            catch
            {

            }

            

            
        }

        private void getLastFiveDirectories()
        {
            if(myDirectories.Count>5)
            {
                myDirectories.RemoveAt(0);
                getLastFiveDirectories();
            }
        }

        private static List<string> GetDirectories()
        {
            try
            {
                return Directory.GetDirectories(MainWindow.userDirectory).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(currentDirectoryInt>0)
            {
                currentDirectoryInt--;
                getDirectoryNames();
                placeValues();
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           // 
            if (currentDirectoryInt < myDirectories.Count-1)
            {
                currentDirectoryInt++;
                getDirectoryNames();
                placeValues();
            }

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myVideo.Play();
            }
            catch
            {

            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myVideo.Stop();
            }
            catch
            {

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.userDirectory + "\\" + currentDirectoryString + "\\selfEvaluation.txt";
            System.IO.File.WriteAllText(path, "Feel1.png");
            placeValues();
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.userDirectory + "\\" + currentDirectoryString + "\\selfEvaluation.txt";
           
            System.IO.File.WriteAllText(path, "Feel2.png");
            placeValues();

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.userDirectory + "\\" + currentDirectoryString + "\\selfEvaluation.txt";
            
            System.IO.File.WriteAllText(path, "Feel3.png");
            placeValues();

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.userDirectory + "\\" + currentDirectoryString + "\\selfEvaluation.txt";
            System.IO.File.WriteAllText(path, "Feel4.png");
            placeValues();

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string path = MainWindow.userDirectory + "\\" + currentDirectoryString + "\\selfEvaluation.txt";
            System.IO.File.WriteAllText(path, "Feel5.png");
            placeValues();

        }
    }
}
