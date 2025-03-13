using System;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackSplatter : ComponentTrackBase
    {
        public ComponentTrackSplatter()
        {
            Name = "Splatter";
            Type = "ComponentTrackSplatter";

            Tracks.Add(new KeyTrackSplatterType { Name = "Blood", SplatterTypeName = Enum.GetName(typeof(SplatterType), SplatterType.Blood) });
            Tracks.Add(new KeyTrackSplatterType { Name = "Dirt", SplatterTypeName = Enum.GetName(typeof(SplatterType), SplatterType.Dirt) });
            Tracks.Add(new KeyTrackSplatterType { Name = "Bruise", SplatterTypeName = Enum.GetName(typeof(SplatterType), SplatterType.Bruise) });
            Tracks.Add(new KeyTrackSplatterType { Name = "Sweat", SplatterTypeName = Enum.GetName(typeof(SplatterType), SplatterType.Sweat) });
        }
    }
}
