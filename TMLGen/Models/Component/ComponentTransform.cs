using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentTransform : ComponentBase
    {
        [XmlAttribute]
        public bool IsContinuous;

        [XmlIgnore]
        public static readonly string translateXPath = "Transform.Translate.X";
        [XmlIgnore]
        public static readonly string translateYPath = "Transform.Translate.Y";
        [XmlIgnore]
        public static readonly string translateZPath = "Transform.Translate.Z";
        [XmlIgnore]
        public static readonly string rotatePath = "Transform.Rotation";
        [XmlIgnore]
        public static readonly string scalePath = "Transform.Scale";
        [XmlIgnore]
        public static readonly string frameOfReferencePath = "Transform.Parent Point";

        public ComponentTransform()
        {
            IsContinuous = false;
        }
    }
}
