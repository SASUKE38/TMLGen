using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Key;

namespace TMLGen.Models.Sequences
{
    public class Channel
    {
        [XmlAttribute]
        public string Name;
        [XmlElement]
        public List<KeyBase> Key;

        public Channel()
        {
            Key = new List<KeyBase>();
        }
    }
}
