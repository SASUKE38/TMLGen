using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackCameraExposure : ComponentTrackBase
    {
        public ComponentTrackCameraExposure()
        {
            Name = "Exposure";
            Type = "ComponentTrackCameraExposure";

            Tracks.Add(new KeyTrackBoolean { Name = "Enabled" });
            Tracks.Add(new KeyTrackFloat { Name = "Delta Compensation" });
            Tracks.Add(new KeyTrackFloat { Name = "Delta Min" });
            Tracks.Add(new KeyTrackFloat { Name = "Delta Max" });
        }
    }
}
