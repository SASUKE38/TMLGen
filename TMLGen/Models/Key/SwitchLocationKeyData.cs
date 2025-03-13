using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class SwitchLocationKeyData
    {
        [XmlAttribute]
        public Guid Event;
        [XmlAttribute]
        public Guid LevelTemplateId;

        public SwitchLocationKeyData()
        {
            Event = Guid.Empty;
            LevelTemplateId = Guid.Empty;
        }
    }
}
