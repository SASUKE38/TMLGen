using System.Collections.Generic;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackActorPropertiesReflection : ComponentTrackBase
    {
        [XmlIgnore]
        public HashSet<string> subTrackTypes;
        [XmlIgnore]
        public static readonly HashSet<string> booleanParameters = new()
        {
            "IsEnabled",
            "RenderShadow",
            "UseTemperature",
            "RenderVolumetricShadow",
            "FillLight",
            "CastShadow"
        };

        public ComponentTrackActorPropertiesReflection()
        {
            Name = "Properties";
            Type = "ComponentTrackActorPropertiesReflection";
            subTrackTypes = [];
        }
    }
}
