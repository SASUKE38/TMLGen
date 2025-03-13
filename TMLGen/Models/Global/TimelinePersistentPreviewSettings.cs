using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Global
{
    public class TimelinePersistentPreviewSettings
    {
        [XmlAttribute]
        public bool UnsheatheWeapons;

        public TimelinePersistentPreviewSettings()
        {
            this.UnsheatheWeapons = false;
        }
    }
}
