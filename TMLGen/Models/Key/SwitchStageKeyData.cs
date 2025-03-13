using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class SwitchStageKeyData
    {
        [XmlAttribute]
        public Guid Event;
        [XmlAttribute]
        public bool ForceTransformUpdate;
        [XmlAttribute]
        public bool ForceUpdateCameraBehavior;

        public SwitchStageKeyData()
        {
            Event = Guid.Empty;
            ForceTransformUpdate = false;
            ForceUpdateCameraBehavior = true;
        }
    }
}
