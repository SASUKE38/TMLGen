using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackSoundDevice : ComponentTrackBase
    {
        [XmlAttribute("SoundObjectIndex")]
        public string soundObjectIndex;
        [XmlAttribute("SoundType")]
        public string soundType;
        [XmlIgnore]
        public static readonly string soundDeviceName = "Sound";

        public ComponentTrackSoundDevice()
        {
            Name = "Sound Device";
            Type = "ComponentTrackSoundDevice";
            soundType = Enum.GetName(typeof(SoundType), SoundType.SOUNDTYPE_Default);
            soundObjectIndex = Enum.GetName(typeof(SoundObjectIndex), SoundObjectIndex.Root);
        }
    }
}
