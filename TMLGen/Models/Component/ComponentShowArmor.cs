using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentShowArmor : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelPrefix = "Show Clothing (All.All.";
    }
}
