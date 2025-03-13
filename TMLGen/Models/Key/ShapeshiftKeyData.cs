using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class ShapeshiftKeyData
    {
        [XmlAttribute]
        public Guid TemplateId;

        public ShapeshiftKeyData()
        {
            TemplateId = Guid.Empty;
        }
    }
}
