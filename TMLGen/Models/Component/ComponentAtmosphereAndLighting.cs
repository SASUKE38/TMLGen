using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentAtmosphereAndLighting : ComponentBase
    {
        [XmlIgnore]
        public static readonly string atmospherePath = "Atmosphere and Lighting.Atmosphere";
        [XmlIgnore]
        public static readonly string lightingPath = "Atmosphere and Lighting.Lighting";
    }
}
