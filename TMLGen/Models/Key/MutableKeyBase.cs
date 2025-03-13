using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class MutableKeyBase<T> : KeyBase
    {
        [XmlAttribute]
        public T Value;
    }
}
