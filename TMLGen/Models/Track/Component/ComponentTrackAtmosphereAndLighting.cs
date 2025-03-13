using System.Xml.Serialization;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackAtmosphereAndLighting : ComponentTrackBase
    {
        public ComponentTrackAtmosphereAndLighting()
        {
            Name = "Atmosphere and Lighting";
            Type = "ComponentTrackAtmosphereAndLighting";

            Tracks.Add(new KeyTrackAtmosphere { Name = "Atmosphere" });
            Tracks.Add(new KeyTrackLighting { Name = "Lighting" });
        }
    }
}
