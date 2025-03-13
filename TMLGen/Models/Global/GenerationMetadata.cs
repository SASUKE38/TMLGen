using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Global
{
    public class GenerationMetadata
    {
        [XmlAttribute]
        public long FullGenerationTimestamp;
        [XmlAttribute]
        public long PartialGenerationTimestamp;

        public GenerationMetadata()
        {
            this.FullGenerationTimestamp = 0L;
            this.PartialGenerationTimestamp = 0L;
        }
    }
}
