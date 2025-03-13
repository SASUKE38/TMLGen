using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class ImmutableKeyBase<T> : KeyBase
    {
        [XmlElement]
        public T Value;
    }
}
