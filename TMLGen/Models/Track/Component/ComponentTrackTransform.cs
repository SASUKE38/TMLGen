using System.Xml.Serialization;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackTransform : ComponentTrackBase
    {
        public ComponentTrackTransform()
        {
            Name = "Transform";
            Type = "ComponentTrackTransform";

            KeyTrackWrapper translateTrack = new() { Name = "Translate" };
            translateTrack.Tracks.Add(new KeyTrackFloat { Name = "X" });
            translateTrack.Tracks.Add(new KeyTrackFloat { Name = "Y" });
            translateTrack.Tracks.Add(new KeyTrackFloat { Name = "Z" });

            Tracks.Add(translateTrack);
            Tracks.Add(new KeyTrackRotation { Name = "Rotation" });
            Tracks.Add(new KeyTrackFloat { Name = "Scale", FallbackValue = 1f });
            Tracks.Add(new KeyTrackFrameOfReference { Name = "Parent Point" });
        }
    }
}
