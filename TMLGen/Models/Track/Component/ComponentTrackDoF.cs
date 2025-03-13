using System;
using System.Xml.Serialization;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackDoF : ComponentTrackBase
    {
        [XmlIgnore]
        public static readonly float fallbackFocalDistance = 10f;
        [XmlIgnore]
        public static readonly float fallbackAperture = 2.8f;
        [XmlIgnore]
        public static readonly bool fallbackEnabled = false;
        [XmlIgnore]
        public static readonly bool fallbackAutoFocus = false;
        [XmlIgnore]
        public static readonly string fallbackDoFArea = Enum.GetName(typeof(DOFArea), DOFArea.Mode_Both);

        public ComponentTrackDoF()
        {
            Name = "Depth of Field";
            Type = "ComponentTrackDoF";

            Tracks.Add(new KeyTrackFloat { FallbackValue = fallbackFocalDistance, Name = "Focal Distance" });
            Tracks.Add(new KeyTrackAperture { FallbackValue = 2.8f, Name = "Aperture" });
            Tracks.Add(new KeyTrackBoolean { Name = "Enabled" });
            Tracks.Add(new KeyTrackBoolean { Name = "Auto Focus" });
            Tracks.Add(new KeyTrackFloat { Name = "Near Sharpness Offset" });
            Tracks.Add(new KeyTrackFloat { Name = "Far Sharpness Offset" });
            Tracks.Add(new KeyTrackDoFArea { Name = "DoF Area", FallbackValue = fallbackDoFArea });
        }
    }
}
