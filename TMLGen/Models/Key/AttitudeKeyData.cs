using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class AttitudeKeyData
    {
        [XmlAttribute]
        public Guid Pose;
        [XmlAttribute]
        public Guid Transition;

        public AttitudeKeyData()
        {
            Pose = Guid.Empty;
            Transition = Guid.Empty;
        }
    }
}
