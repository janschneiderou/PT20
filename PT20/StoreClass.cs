using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT20
{
    public class StoreClass
    {
        
        System.DateTime startRecordingTime;
        public string recordingID;
        string applicationName;
        private RecordingObject myRecordingObject;
        List<string> valuesNameDefinition;
        List<FrameObject> frames;
      

        public StoreClass()
        {
            valuesNameDefinition = new List<string>();
            frames = new List<FrameObject>();
            setValuesNames();
        }

        public void doStartStuff()
        {
            startRecordingTime = DateTime.Now;
            //"<START RECORDING>recordinID,ApplicationID</START RECORDING>"
           
            myRecordingObject = new RecordingObject();
            myRecordingObject.recordingID = MainWindow.recordingID;
            myRecordingObject.applicationName = "PT20";
            
        }

        public void doStopStuff()
        {
            int x = frames.Count;
           
            try
            {
                string json = JsonConvert.SerializeObject(myRecordingObject, Formatting.Indented);
                
                string fileName = MainWindow.recordingPath + "\\" + myRecordingObject.recordingID + myRecordingObject.applicationName + ".json";
                File.WriteAllText(fileName, json);
                
                x++;
            }
            catch (Exception e)
            {
                int z = 0;
                z++;
            }
        }

        public void storeFrame(List<string> frameValues)
        {
            try
            {
                FrameObject f = new FrameObject(startRecordingTime, valuesNameDefinition, frameValues);
                myRecordingObject.frames.Add(f);
            }
            catch
            {

            }

        }

        public  void setValuesNames()
        {
            
            string temp = "AverageVolume";
            valuesNameDefinition.Add(temp);
            temp = "FaceExpression";
            valuesNameDefinition.Add(temp);
            temp = "AnkleRightX";
            valuesNameDefinition.Add(temp);
            temp = "AnkleRightY";
            valuesNameDefinition.Add(temp);
            temp = "AnkleRightZ";
            valuesNameDefinition.Add(temp);
            temp = "AnkleLeftX";
            valuesNameDefinition.Add(temp);
            temp = "AnkleLeftY";
            valuesNameDefinition.Add(temp);
            temp = "AnkleLeftZ";
            valuesNameDefinition.Add(temp);
            temp = "ElbowRightX";
            valuesNameDefinition.Add(temp);
            temp = "ElbowRightY";
            valuesNameDefinition.Add(temp);
            temp = "ElbowRightZ";
            valuesNameDefinition.Add(temp);
            temp = "ElbowLeftX";
            valuesNameDefinition.Add(temp);
            temp = "ElbowLeftY";
            valuesNameDefinition.Add(temp);
            temp = "ElbowLeftZ";
            valuesNameDefinition.Add(temp);
            temp = "HandRightX";
            valuesNameDefinition.Add(temp);
            temp = "HandRightY";
            valuesNameDefinition.Add(temp);
            temp = "HandRightZ";
            valuesNameDefinition.Add(temp);
            temp = "HandLeftX";
            valuesNameDefinition.Add(temp);
            temp = "HandLeftY";
            valuesNameDefinition.Add(temp);
            temp = "HandLeftZ";
            valuesNameDefinition.Add(temp);
            temp = "HandRightTipX";
            valuesNameDefinition.Add(temp);
            temp = "HandRightTipY";
            valuesNameDefinition.Add(temp);
            temp = "HandRightTipZ";
            valuesNameDefinition.Add(temp);
            temp = "HandLeftTipX";
            valuesNameDefinition.Add(temp);
            temp = "HandLeftTipY";
            valuesNameDefinition.Add(temp);
            temp = "HandLeftTipZ";
            valuesNameDefinition.Add(temp);
            temp = "HeadX";
            valuesNameDefinition.Add(temp);
            temp = "HeadY";
            valuesNameDefinition.Add(temp);
            temp = "HeadZ";
            valuesNameDefinition.Add(temp);
            temp = "HipRightX";
            valuesNameDefinition.Add(temp);
            temp = "HipRightY";
            valuesNameDefinition.Add(temp);
            temp = "HipRightZ";
            valuesNameDefinition.Add(temp);
            temp = "HipLeftX";
            valuesNameDefinition.Add(temp);
            temp = "HipLeftY";
            valuesNameDefinition.Add(temp);
            temp = "HipLeftZ";
            valuesNameDefinition.Add(temp);
            temp = "ShoulderRightX";
            valuesNameDefinition.Add(temp);
            temp = "ShoulderRightY";
            valuesNameDefinition.Add(temp);
            temp = "ShoulderRightZ";
            valuesNameDefinition.Add(temp);
            temp = "ShoulderLeftX";
            valuesNameDefinition.Add(temp);
            temp = "ShoulderLeftY";
            valuesNameDefinition.Add(temp);
            temp = "ShoulderLeftZ";
            valuesNameDefinition.Add(temp);
            temp = "SpineMidX";
            valuesNameDefinition.Add(temp);
            temp = "SpineMidY";
            valuesNameDefinition.Add(temp);
            temp = "SpineMidZ";
            valuesNameDefinition.Add(temp);
            temp = "SpineShoulderX";
            valuesNameDefinition.Add(temp);
            temp = "SpineShoulderY";
            valuesNameDefinition.Add(temp);
            temp = "SpineShoulderZ";
            valuesNameDefinition.Add(temp);


            
        }


    }
}
