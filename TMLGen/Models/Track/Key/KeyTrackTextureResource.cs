using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackTextureResource : KeyTrackBase
    {
        [XmlAttribute]
        public bool IsVirtual;

        public KeyTrackTextureResource()
        {
            Type = "KeyTrackTextureResource";
            IsVirtual = false;
        }
    }
}
