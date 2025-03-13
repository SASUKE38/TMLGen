using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentMaterial : ComponentBase
    {
        [XmlAttribute]
        public bool IsContinuous;

        [XmlIgnore]
        public static readonly string visibilityName = "Material Visibility";
        [XmlIgnore]
        public static readonly string parameterDimXName = "X";
        [XmlIgnore]
        public static readonly string parameterDimYName = "Y";
        [XmlIgnore]
        public static readonly string parameterDimZName = "Z";
        [XmlIgnore]
        public static readonly string parameterDimWName = "W";

        public ComponentMaterial()
        {
            IsContinuous = false;
        }
    }
}
