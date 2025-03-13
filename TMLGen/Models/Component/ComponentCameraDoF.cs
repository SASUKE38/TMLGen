using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentCameraDoF : ComponentBase
    {
        [XmlIgnore]
        public static readonly string doFFocalDistancePath = "Depth of Field.Focal Distance";
        [XmlIgnore]
        public static readonly string doFAperturePath = "Depth of Field.Aperture";
        [XmlIgnore]
        public static readonly string doFEnabledPath = "Depth of Field.Enabled";
        [XmlIgnore]
        public static readonly string doFAutoFocusEnabledPath = "Depth of Field.Auto Focus";
        [XmlIgnore]
        public static readonly string doFNearSharpnessPath = "Depth of Field.Near Sharpness Offset";
        [XmlIgnore]
        public static readonly string doFFarSharpnessPath = "Depth of Field.Far Sharpness Offset";
        [XmlIgnore]
        public static readonly string doFAreaPath = "Depth of Field.DoF Area";
    }
}
