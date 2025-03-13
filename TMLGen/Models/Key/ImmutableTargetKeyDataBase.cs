using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class ImmutableTargetKeyDataBase
    {
        [XmlAttribute]
        public Guid TargetId;
    }
}
