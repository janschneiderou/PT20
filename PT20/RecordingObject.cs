﻿
using System.Collections.Generic;


namespace PT20
{
    public class RecordingObject
    {
        public string recordingID { get; set; }
        public string applicationName { get; set; }

        public List<FrameObject> frames { get; set; }

        public RecordingObject()
        {
            frames = new List<FrameObject>();
        }
    }
}
