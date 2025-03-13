using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Global
{
    public class TimelineWorldCinematicSettings
    {
        [XmlAttribute]
        public int PlayCount;
        [XmlAttribute]
        public int MinPlayCountBound;
        [XmlAttribute]
        public int MaxPlayCountBound;
        [XmlAttribute]
        public float LoopDelay;
        [XmlAttribute]
        public bool IsInfinite;

        public TimelineWorldCinematicSettings()
        {
            this.PlayCount = 1;
            this.MinPlayCountBound = 0;
            this.MaxPlayCountBound = 0;
            this.LoopDelay = 0;
            this.IsInfinite = false;
        }
    }
}
