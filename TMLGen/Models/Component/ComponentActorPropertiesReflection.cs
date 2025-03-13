using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentActorPropertiesReflection : ComponentBase
    {
        [XmlIgnore]
        public static readonly string parameterDimXName = "X";
        [XmlIgnore]
        public static readonly string parameterDimYName = "Y";
        [XmlIgnore]
        public static readonly string parameterDimZName = "Z";
        [XmlIgnore]
        public static readonly string parameterDimWName = "W";
    }
}
