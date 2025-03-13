using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class SoundEvent
    {
        [XmlAttribute]
        public Guid Event;
        [XmlAttribute]
        public string SoundType;

        public SoundEvent()
        {
            this.SoundType = "SOUNDTYPE_Music";
        }
    }
}
