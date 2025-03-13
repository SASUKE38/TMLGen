using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class TextureResourceKeyData
    {
        [XmlAttribute]
        public Guid TextureResourceId;

        public TextureResourceKeyData()
        {
            TextureResourceId = Guid.Empty;
        }
    }
}
