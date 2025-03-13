using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackSplatterType : KeyTrackBase
    {
        [XmlAttribute("SplatterType")]
        public string SplatterTypeName;

        public KeyTrackSplatterType()
        {
            Type = "KeyTrackSplatterType";
            SplatterTypeName = Enum.GetName(typeof(SplatterType), SplatterType.Blood);
        }
    }
}
