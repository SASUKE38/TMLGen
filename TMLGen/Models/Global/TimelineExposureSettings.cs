using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Global
{
    public class TimelineExposureSettings
    {
        [XmlAttribute]
        public float DeltaCompensation;
        [XmlAttribute]
        public float DeltaMin;
        [XmlAttribute]
        public float DeltaMax;

        public TimelineExposureSettings()
        {
            this.DeltaCompensation = 0.0f;
            this.DeltaMin = 0.0f;
            this.DeltaMax = 0.0f;
        }
    }
}
