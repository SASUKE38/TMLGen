using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentCameraExposure : ComponentBase
    {
        [XmlAttribute]
        public bool IsContinuous;

        [XmlIgnore]
        public static readonly string exposureEnabledPath = "Exposure.Enabled";
        [XmlIgnore]
        public static readonly string exposureDeltaCompensationPath = "Exposure.Delta Compensation";
        [XmlIgnore]
        public static readonly string exposureDeltaMinPath = "Exposure.Delta Min";
        [XmlIgnore]
        public static readonly string exposureDeltaMaxPath = "Exposure.Delta Max";

        public ComponentCameraExposure()
        {
            IsContinuous = false;
        }
    }
}
