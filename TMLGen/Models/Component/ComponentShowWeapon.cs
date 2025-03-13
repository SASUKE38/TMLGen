using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentShowWeapon : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Show Weapon";
    }
}
